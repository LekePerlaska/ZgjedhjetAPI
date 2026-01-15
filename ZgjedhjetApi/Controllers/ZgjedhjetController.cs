using Microsoft.AspNetCore.Mvc;
using ZgjedhjetApi.Enums;
using ZgjedhjetApi.Models.DTOs;

namespace ZgjedhjetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZgjedhjetController : ControllerBase
    {
        // YOUR CODE HERE
        // IT IS UP TO YOU TO DECIDE IF YOU WILL USE DB CONTEXT HERE OR THROUGH SERVICES/REPOSITORIES
        private readonly ILogger<ZgjedhjetController> _logger;

        public ZgjedhjetController(ILogger<ZgjedhjetController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// POST endpoint to import CSV file
        /// </summary>
        [HttpPost("import")]
        public async Task<ActionResult<CsvImportResponse>> MigrateData(IFormFile file)
        {
            // YOUR CODE HERE
            var response = new CsvImportResponse();
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
            [FromQuery] Partia? partia = null)
        {
            // YOUR CODE HERE
            var response = new ZgjedhjetAggregatedResponse();
            return Ok(response);
        }
    }
}
