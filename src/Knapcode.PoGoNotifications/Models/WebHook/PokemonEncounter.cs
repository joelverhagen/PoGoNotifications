using System;

namespace Knapcode.PoGoNotifications.Models
{
    public class PokemonEncounter
    {
        public string EncounterId { get; set; }
        public string SpawnpointId { get; set; }
        public int PokemonId { get; set; }
        public DateTimeOffset DisappearTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsLured { get; set; }
    }
}
