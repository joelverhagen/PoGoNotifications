using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knapcode.PoGoNotifications.Models;
using Knapcode.PoGoNotifications.Models.Pokedex;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Knapcode.PoGoNotifications.Logic
{
    public class NotificationBuilder : INotificationBuilder
    {
        private readonly IOptions<NotificationOptions> _options;
        private readonly pokedexContext _pokedexContext;

        public NotificationBuilder(pokedexContext context, IOptions<NotificationOptions> options)
        {
            _pokedexContext = context;
            _options = options;
        }

        public async Task<Notification> BuildNotificationAsync(PokemonEncounter encounter)
        {
            var nameRecord = await _pokedexContext
                 .PokemonSpeciesNames
                 .Where(x => x.PokemonSpecies.Id == encounter.PokemonId)
                 .Where(x => x.LocalLanguage.Identifier == "en")
                 .FirstAsync();
            var name = nameRecord.Name;

            var disappearsIn = encounter.DisappearTime - DateTimeOffset.Now;
            var disappearTime = encounter.DisappearTime.ToLocalTime().ToString("h:mm tt");
            var minutes = (int)disappearsIn.TotalMinutes;
            var text = $"A wild {name} has appeared! It disappears at {disappearTime} ({minutes} minute{(minutes != 1 ? "s" : string.Empty)} from now).";

            var mapUrl = GetMapUrl(encounter.PokemonId, encounter.Latitude, encounter.Longitude);

            var notification = new Notification
            {
                Text = text
            };

            if (_options.Value.UseNotificationImage)
            {
                notification.Image = new ImageAttachment
                {
                    Url = mapUrl
                };
            }

            if (_options.Value.UseNotificationLocation)
            {
                notification.Location = new LocationAttachment
                {
                    Name = $"Location of the {name}",
                    Latitude = encounter.Latitude,
                    Longitude = encounter.Longitude
                };
            }

            return notification;
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
