using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZgjedhjetApi.Enums;

namespace ZgjedhjetApi.Models.Entities
{
    public class Zgjedhjet
    {
        // YOUR CODE HERE
        [Key]
        public int Id { get; set; }

        [Column("Kategoria")]
        public Kategoria Kategoria { get; set; }

        [Column("Komuna")]
        public Komuna Komuna { get; set; }

        [Column("Qendra_e_votimit")]
        public string Qendra_e_votimit { get; set; }

        [Column("Vendvotimi")]
        public string Vendvotimi { get; set; }

        public PartiaVotes Partia { get; set; }
        }
}
