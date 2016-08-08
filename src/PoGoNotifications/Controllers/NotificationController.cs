using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knapcode.PoGoNotifications.Models;
using Knapcode.PoGoNotifications.Models.Pokedex;
using Knapcode.PoGoNotifications.Models.WebHook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Knapcode.PoGoNotifications.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly NotificationContext _notificationContext;
        private readonly ILogger<NotificationController> _logger;
        private readonly pokedexContext _pokedexContext;
        private readonly IOptions<NotificationOptions> _options;

        public NotificationController(
            ILogger<NotificationController> logger,
            pokedexContext pokedexContext,
            NotificationContext notificationContext,
            IOptions<NotificationOptions> options)
        {
            _logger = logger;
            _pokedexContext = pokedexContext;
            _notificationContext = notificationContext;
            _options = options;
        }

        [HttpGet]
        public async Task<IEnumerable<PokemonEncounter>> Get(int take = 10, int skip = 0, bool asc = false)
        {
            var result = _notificationContext.PokemonEncounters;
            IOrderedQueryable<PokemonEncounter> orderedResult;

            if (asc)
            {
                orderedResult = result.OrderBy(x => x.DisappearTime);
            }
            else
            {
                orderedResult = result.OrderByDescending(x => x.DisappearTime);
            }

            return await orderedResult.Skip(skip).Take(take).ToListAsync();
        }

        [HttpPost]
        public async Task Post([FromBody]PokemonWebHookMessage body)
        {
            if (!ModelState.IsValid)
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

            var existing = _notificationContext.PokemonEncounters.FirstOrDefault(x =>
                x.EncounterId == encounter.EncounterId &&
                x.DisappearTime == encounter.DisappearTime);

            if (existing == null)
            {
                _logger.LogInformation("New encounter {encounterId}.", encounter.EncounterId);

                try
                {
                    _notificationContext.PokemonEncounters.Add(encounter);
                    await _notificationContext.SaveChangesAsync();
                }
                catch (DbUpdateException exception) when (IsDuplicateException(exception))
                {
                    _logger.LogInformation("Duplicate encounter {encounterId} found after inserting.", encounter.EncounterId);
                    return;
                }

                var name = _pokedexContext
                     .PokemonSpeciesNames
                     .Where(x => x.PokemonSpecies.Id == body.Message.PokemonId)
                     .Where(x => x.LocalLanguage.Identifier == "en")
                     .First();

                var disappearsIn = encounter.DisappearTime - DateTimeOffset.Now;
                var mapUrl = GetMapUrl(encounter.PokemonId, encounter.Latitude, encounter.Longitude);

                _logger.LogInformation(
                    $"Pokemon Go:{Environment.NewLine}{{name}} has appeared! It disappears in {{minutes}} minutes.{Environment.NewLine}{{mapUrl}}",
                    name.Name,
                    (int)disappearsIn.TotalMinutes,
                    mapUrl);
            }
            else
            {
                _logger.LogInformation("Duplicate encounter {encounterId} found before inserting.", encounter.EncounterId);
            }
        }

        private bool IsDuplicateException(DbUpdateException exception)
        {
            var postgresException = exception.InnerException as PostgresException;
            if (postgresException == null)
            {
                return false;
            }

            return postgresException.SqlState == "23505";
        }

        private string GetMapUrl(int pokemonId, double latitude, double longitude)
        {
            var baseUrl = "https://maps.googleapis.com/maps/api/staticmap";
            var markers = new StringBuilder();
            markers.Append("icon:");
            markers.AppendFormat(_options.Value.PokemonIconUrlFormat, pokemonId);
            markers.Append("|");
            markers.Append(latitude);
            markers.Append(",");
            markers.Append(longitude);

            return QueryHelpers.AddQueryString(
                baseUrl,
                new Dictionary<string, string>
                {
                    { "size", "600x600" },
                    { "key", _options.Value.GoogleMapsApiKey },
                    { "markers", markers.ToString() }
                });
        }
    }
}
