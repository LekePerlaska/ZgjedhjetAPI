using ZgjedhjetApi.Models.Entities;

namespace ZgjedhjetApi.Models.DTOs
{
    public class ZgjedhjetElasticDto
    {
        public int Id { get; set; }
        public string Kategoria { get; set; }
        public string Komuna { get; set; }
        public string? Qendra_e_votimit { get; set; }
        public string? Vendvotimi { get; set; }

        public PartiaVotes Partia { get; set; } = null!;
    }
}
