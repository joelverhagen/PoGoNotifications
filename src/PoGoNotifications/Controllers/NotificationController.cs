using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knapcode.PoGoNotifications.Logic;
using Knapcode.PoGoNotifications.Models;
using Knapcode.PoGoNotifications.Models.WebHook;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Knapcode.PoGoNotifications.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly NotificationContext _notificationContext;
        private readonly IPokemonEncounterService _notifier;

        public NotificationController(IPokemonEncounterService notifier, NotificationContext notificationContext)
        {
            _notifier = notifier;
            _notificationContext = notificationContext;
        }

        [HttpGet]
        public async Task<IEnumerable<PokemonEncounter>> Get(int skip = 0, int take = 10, bool asc = false)
        {
            return await _notifier.GetEncountersAsync(skip, take, asc);
        }

        [HttpPost]
        public async Task Post([FromBody]PokemonWebHookMessage body)
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            if (body.Type != WebHookMessageType.Pokemon)
            {
                return;
            }

            var encounter = new PokemonEncounter
            {
                EncounterId = body.Message.EncounterId,
                DisappearTime = DateTimeOffset.FromUnixTimeSeconds((long)body.Message.DisappearTime),
                SpawnpointId = body.Message.SpawnpointId,
                PokemonId = body.Message.PokemonId,
                Latitude = body.Message.Latitude,
                Longitude = body.Message.Longitude,
                IsLured = body.Message.IsLured
            };

            await _notifier.AddEncounterAsync(encounter);
        }
    }
}
