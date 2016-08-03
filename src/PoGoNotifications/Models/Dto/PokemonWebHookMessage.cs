using Newtonsoft.Json;

namespace Knapcode.PoGoNotifications.Models.WebHook
{
    public class PokemonWebHookMessage
    {
        [JsonProperty("type")]
        public WebHookMessageType Type { get; set; }

        [JsonProperty("message")]
        public PokemonMessage Message { get; set; }
    }
}
