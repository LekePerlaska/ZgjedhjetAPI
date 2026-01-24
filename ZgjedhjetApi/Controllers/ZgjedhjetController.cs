using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZgjedhjetApi.Data;
using ZgjedhjetApi.Enums;
using ZgjedhjetApi.Models.DTOs;
using ZgjedhjetApi.Models.Entities;

namespace ZgjedhjetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZgjedhjetController : ControllerBase
    {
        // YOUR CODE HERE
        // IT IS UP TO YOU TO DECIDE IF YOU WILL USE DB CONTEXT HERE OR THROUGH SERVICES/REPOSITORIES
        private readonly ILogger<ZgjedhjetController> _logger;
        private readonly LifeDbContext _dbcontext;

        public ZgjedhjetController(ILogger<ZgjedhjetController> logger, LifeDbContext dbContext)
        {
            _logger = logger;
            _dbcontext = dbContext;
        }

        /// <summary>
        /// POST endpoint to import CSV file
        /// </summary>
        [HttpPost("import")]
        public async Task<ActionResult<CsvImportResponse>> MigrateData(IFormFile file)
        {
            // YOUR CODE HERE
            var response = new CsvImportResponse();

            if (file == null || file.Length == 0)
            {
                response.Success = false;
                response.Message = "CSV file is missing.";
                return BadRequest(response);
            }

            try
            {
                using var reader = new StreamReader(file.OpenReadStream());

                var headerLine = await reader.ReadLineAsync();
                if (headerLine == null)
                    return BadRequest("CSV has no header.");

                var headers = headerLine.Split(',');

                var entities = new List<Zgjedhjet>();

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var values = line.Split(',');

                    var entity = new Zgjedhjet
                    {
                        Kategoria = Enum.Parse<Kategoria>(values[0]),
                        Komuna = Enum.Parse<Komuna>(values[1]),
                        Qendra_e_votimit = values[2],
                        Vendvotimi = values[3],
                        Partia = new PartiaVotes
                        {
                            Partia111 = int.Parse(values[4]),
                            Partia112 = int.Parse(values[5]),
                            Partia113 = int.Parse(values[6]),
                            Partia114 = int.Parse(values[7]),
                            Partia115 = int.Parse(values[8]),
                            Partia116 = int.Parse(values[9]),
                            Partia117 = int.Parse(values[10]),
                            Partia118 = int.Parse(values[11]),
                            Partia119 = int.Parse(values[12]),
                            Partia120 = int.Parse(values[13]),
                            Partia121 = int.Parse(values[14]),
                            Partia122 = int.Parse(values[15]),
                            Partia123 = int.Parse(values[16]),
                            Partia124 = int.Parse(values[17]),
                            Partia125 = int.Parse(values[18]),
                            Partia126 = int.Parse(values[19]),
                            Partia127 = int.Parse(values[20]),
                            Partia128 = int.Parse(values[21]),
                            Partia129 = int.Parse(values[22]),
                            Partia130 = int.Parse(values[23]),
                            Partia131 = int.Parse(values[24]),
                            Partia132 = int.Parse(values[25]),
                            Partia133 = int.Parse(values[26]),
                            Partia134 = int.Parse(values[27]),
                            Partia135 = int.Parse(values[28]),
                            Partia136 = int.Parse(values[29]),
                            Partia137 = int.Parse(values[30]),
                            Partia138 = int.Parse(values[31]),
                        },
                    };

                    entities.Add(entity);
                }

                await _dbcontext.Zgjedhjet.AddRangeAsync(entities);
                await _dbcontext.SaveChangesAsync();

                response.Success = true;
                response.Message = $"Successfully imported {entities.Count}.";
                response.RecordsImported = entities.Count;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Import failed.";
                response.Errors.Add(ex.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// GET endpoint to retrieve and filter electoral data
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ZgjedhjetAggregatedResponse>> GetZgjedhjet(
            [FromQuery] Kategoria? kategoria = null,
            [FromQuery] Komuna? komuna = null,
            [FromQuery] string? qendra_e_votimit = null,
            [FromQuery] string? vendvotimi = null,
            [FromQuery] Partia? partia = null
        )
        {
            // YOUR CODE HERE
            var query = _dbcontext.Zgjedhjet.AsQueryable();

            if (kategoria.HasValue)
                query = query.Where(x => x.Kategoria == kategoria.Value);

            if (komuna.HasValue)
                query = query.Where(x => x.Komuna == komuna.Value);

            if (!string.IsNullOrWhiteSpace(qendra_e_votimit))
                query = query.Where(x => x.Qendra_e_votimit == qendra_e_votimit);

            if (!string.IsNullOrWhiteSpace(vendvotimi))
                query = query.Where(x => x.Vendvotimi == vendvotimi);

            if (
                !string.IsNullOrWhiteSpace(qendra_e_votimit)
                || !string.IsNullOrWhiteSpace(vendvotimi)
            )
            {
                var exists = await query.AnyAsync();

                if (!exists)
                    return NotFound("Qendra_e_Votimit or VendVotimi not found.");
            }

            var data = await query.ToListAsync();

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
    }
}
