namespace ZgjedhjetApi.Models.DTOs
{
    public class ZgjedhjetElasticDto
    {
        public int Id { get; set; }
        public string Partia { get; set; } = null!;
        public string Kategoria { get; set; } = null!;
        public int Komuna { get; set; }
        public string KomunaStr => Komuna.ToString();
        public string? Qendra_e_votimit { get; set; }
        public string? Vendvotimi { get; set; }
    }
}
