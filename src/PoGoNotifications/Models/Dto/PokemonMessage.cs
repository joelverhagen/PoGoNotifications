using Newtonsoft.Json;

namespace Knapcode.PoGoNotifications.Models.WebHook
{
    public class PokemonMessage
    {
        [JsonProperty("disappear_time")]
        public double DisappearTime { get; set; }

        [JsonProperty("pokemon_id")]
        public int PokemonId { get; set; }

        [JsonProperty("spawnpoint_id")]
        public string SpawnpointId { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("encounter_id")]
        public string EncounterId { get; set; }

        [JsonProperty("is_lured")]
        public bool IsLured { get; set; }
    }
}
