using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using StackExchange.Redis;
using ZgjedhjetApi.Data;
using ZgjedhjetApi.Enums;
using ZgjedhjetApi.Models.DTOs;
using ZgjedhjetApi.Models.Entities;

namespace ZgjedhjetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZgjedhjetElasticSearchController : ControllerBase
    {
        readonly ILogger<ZgjedhjetElasticSearchController> _logger;
        readonly LifeDbContext _dbcontext;
        readonly IElasticClient _elastic;
        readonly IConnectionMultiplexer _redis;
        readonly IDatabase _db;

        public ZgjedhjetElasticSearchController(
            LifeDbContext dbcontext,
            IElasticClient elastic,
            IConnectionMultiplexer redis,
            ILogger<ZgjedhjetElasticSearchController> logger
        )
        {
            _dbcontext = dbcontext;
            _logger = logger;
            _elastic = elastic;
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        [HttpPost("migrate")]
        public async Task<IActionResult> Migrate()
        {
            const int batchSize = 500;
            int totalMigrated = 0;

            var totalCount = await _dbcontext.Zgjedhjet.CountAsync();

            _logger.LogInformation("Starting migration of {TotalCount} records", totalCount);

            for (int i = 0; i < totalCount; i += batchSize)
            {
                var batch = await _dbcontext
                    .Zgjedhjet.OrderBy(x => x.Id)
                    .Skip(i)
                    .Take(batchSize)
                    .ToListAsync();

                if (!batch.Any())
                    continue;

                var batchToIndex = batch
                    .Select(x => new
                    {
                        x.Id,
                        Partia = x.Partia.ToString(),
                        Kategoria = x.Kategoria.ToString(),
                        Komuna = x.Komuna,
                        KomunaStr = x.Komuna.ToString(),
                        x.Qendra_e_votimit,
                        x.Vendvotimi,
                    })
                    .ToList();

                var response = await _elastic.BulkAsync(b =>
                    b.IndexMany(
                            batchToIndex,
                            (descriptor, doc) =>
                            {
                                if (doc.Id == 0) // or null if nullable
                                    throw new InvalidOperationException(
                                        "Document Id cannot be null or 0"
                                    );
                                return descriptor.Id(doc.Id);
                            }
                        )
                        .Refresh(Elasticsearch.Net.Refresh.WaitFor)
                );

                if (response.Errors)
                {
                    foreach (var item in response.ItemsWithErrors)
                        _logger.LogError($"Failed document {item.Id}: {item.Error.Reason}");
                }
                totalMigrated += batch.Count;
                _logger.LogInformation("Migrated {Count} / {Total}", totalMigrated, totalCount);
            }

            return Ok(
                new { Message = "Migration completed successfully", TotalMigrated = totalMigrated }
            );
        }

        [HttpGet("elastic")]
        public async Task<ActionResult<ZgjedhjetAggregatedResponse>> GetZgjedhjetElastic(
            [FromQuery] Kategoria? kategoria = null,
            [FromQuery] Komuna? komuna = null,
            [FromQuery] string? qendra_e_votimit = null,
            [FromQuery] string? vendvotimi = null,
            [FromQuery] Partia? partia = null
        )
        {
            var mustQueries = new List<Func<QueryContainerDescriptor<Zgjedhjet>, QueryContainer>>();

            if (kategoria.HasValue)
                mustQueries.Add(q => q.Term(t => t.Field(f => f.Kategoria).Value(kategoria.Value)));

            if (komuna.HasValue)
                mustQueries.Add(q => q.Term(t => t.Field(f => f.Komuna).Value(komuna.Value)));

            if (!string.IsNullOrWhiteSpace(qendra_e_votimit))
                mustQueries.Add(q =>
                    q.Term(t => t.Field(f => f.Qendra_e_votimit).Value(qendra_e_votimit))
                );

            if (!string.IsNullOrWhiteSpace(vendvotimi))
                mustQueries.Add(q => q.Term(t => t.Field(f => f.Vendvotimi).Value(vendvotimi)));

            var searchResponse = await _elastic.SearchAsync<Zgjedhjet>(s =>
                s.Index("zgjedhjet").Size(10000).Query(q => q.Bool(b => b.Must(mustQueries)))
            );

            if (!searchResponse.IsValid)
                return StatusCode(500, searchResponse.DebugInformation);

            var data = searchResponse.Documents.ToList();

            if (
                (
                    !string.IsNullOrWhiteSpace(qendra_e_votimit)
                    || !string.IsNullOrWhiteSpace(vendvotimi)
                ) && !data.Any()
            )
            {
                return NotFound("Qendra_e_Votimit or VendVotimi not found.");
            }

            var results = new List<PartiaVotesResponse>();

            foreach (Partia p in Enum.GetValues(typeof(Partia)))
            {
                if (partia.HasValue && p != partia.Value)
                    continue;

                var totalVotes = data.Sum(x => x.Partia.GetVotes(p));

                results.Add(
                    new PartiaVotesResponse { Partia = p.ToString(), TotalVota = totalVotes }
                );
            }

            var response = new ZgjedhjetAggregatedResponse { Results = results };

            return Ok(response);
        }

        [HttpGet("elastic/komuna-suggestions")]
        public async Task<IActionResult> GetKomunaSuggestions([FromQuery] string? search)
        {
            var response = await _elastic.SearchAsync<ZgjedhjetElasticDto>(s =>
                s.Index("zgjedhjet")
                    .Size(0)
                    .Query(q =>
                    {
                        if (string.IsNullOrWhiteSpace(search))
                            return null;

                        return q.Bool(b =>
                            b.Should(
                                    sh => sh.Prefix(p => p.Field(f => f.KomunaStr).Value(search)),
                                    sh =>
                                        sh.Wildcard(w =>
                                            w.Field(f => f.KomunaStr).Value($"*{search}*")
                                        )
                                )
                                .MinimumShouldMatch(1)
                        );
                    })
                    .Aggregations(a =>
                        a.Terms(
                            "komuna_suggestions",
                            t =>
                                t.Field(f => f.KomunaStr.Suffix("keyword"))
                                    .Size(20)
                                    .Order(o => o.Ascending("_key"))
                        )
                    )
            );

            if (!response.IsValid)
                return StatusCode(500, response.DebugInformation);

            var suggestions = response
                .Aggregations.Terms("komuna_suggestions")
                .Buckets.Select(b => b.Key)
                .ToList();

            return Ok(suggestions);
        }

        [HttpGet("komuna-suggestion-stats")]
        public async Task<IActionResult> GetKomunaSuggestionStats([FromQuery] int? top = null)
        {
            var entries = await _db.HashGetAllAsync("komuna:suggestion:count");

            var results = entries
                .Select(e => new { Komuna = e.Name.ToString(), nrISugjerimeve = (int)e.Value })
                .OrderByDescending(x => x.nrISugjerimeve);

            var finalRes =
                (top.HasValue && top.Value > 0)
                    ? results.Take(top.Value).ToList()
                    : results.ToList();

            return Ok(finalRes);
        }
    }
}
