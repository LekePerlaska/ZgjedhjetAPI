using Microsoft.EntityFrameworkCore;
using ZgjedhjetApi.Enums;

namespace ZgjedhjetApi.Models.Entities
{
    [Owned]
    public class PartiaVotes
    {
        public int Partia111 { get; set; }
        public int Partia112 { get; set; }
        public int Partia113 { get; set; }
        public int Partia114 { get; set; }
        public int Partia115 { get; set; }
        public int Partia116 { get; set; }
        public int Partia117 { get; set; }
        public int Partia118 { get; set; }
        public int Partia119 { get; set; }
        public int Partia120 { get; set; }
        public int Partia121 { get; set; }
        public int Partia122 { get; set; }
        public int Partia123 { get; set; }
        public int Partia124 { get; set; }
        public int Partia125 { get; set; }
        public int Partia126 { get; set; }
        public int Partia127 { get; set; }
        public int Partia128 { get; set; }
        public int Partia129 { get; set; }
        public int Partia130 { get; set; }
        public int Partia131 { get; set; }
        public int Partia132 { get; set; }
        public int Partia133 { get; set; }
        public int Partia134 { get; set; }
        public int Partia135 { get; set; }
        public int Partia136 { get; set; }
        public int Partia137 { get; set; }
        public int Partia138 { get; set; }

        public int GetVotes(Partia partia)
        {
            return partia switch
            {
                Partia.Partia111 => Partia111,
                Partia.Partia112 => Partia112,
                Partia.Partia113 => Partia113,
                Partia.Partia114 => Partia114,
                Partia.Partia115 => Partia115,
                Partia.Partia116 => Partia116,
                Partia.Partia117 => Partia117,
                Partia.Partia118 => Partia118,
                Partia.Partia119 => Partia119,
                Partia.Partia120 => Partia120,
                Partia.Partia121 => Partia121,
                Partia.Partia122 => Partia122,
                Partia.Partia123 => Partia123,
                Partia.Partia124 => Partia124,
                Partia.Partia125 => Partia125,
                Partia.Partia126 => Partia126,
                Partia.Partia127 => Partia127,
                Partia.Partia128 => Partia128,
                Partia.Partia129 => Partia129,
                Partia.Partia130 => Partia130,
                Partia.Partia131 => Partia131,
                Partia.Partia132 => Partia132,
                Partia.Partia133 => Partia133,
                Partia.Partia134 => Partia134,
                Partia.Partia135 => Partia135,
                Partia.Partia136 => Partia136,
                Partia.Partia137 => Partia137,
                Partia.Partia138 => Partia138,
                _ => 0,
            };
        }
    }
}
