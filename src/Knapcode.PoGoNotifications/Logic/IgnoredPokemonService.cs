using System.Collections.Generic;
using System.Linq;
using Knapcode.PoGoNotifications.Models;
using Microsoft.Extensions.Options;

namespace Knapcode.PoGoNotifications.Logic
{
    public class IgnoredPokemonService : IIgnoredPokemonService
    {
        private readonly IOptions<NotificationOptions> _options;
        private readonly List<NotificationArea> _areas;

        public IgnoredPokemonService(IOptions<NotificationOptions> options)
        {
            _options = options;
            _areas = BuildNotificationAreas(options);
        }

        public bool IsIgnored(PokemonEncounter encounter)
        {
            var point = new Point(encounter.Longitude, encounter.Latitude);
            var ignored = false;

            foreach (var area in _areas)
            {
                if (area.Polygon.ContainsPoint(point))
                {
                    ignored = area.IgnoredPokemon.Contains(encounter.PokemonId);
                }
            }

            return ignored;
        }

        private static List<NotificationArea> BuildNotificationAreas(IOptions<NotificationOptions> options)
        {
            var ignored = new HashSet<int>();
            var areas = new List<NotificationArea>();

            foreach (var areaOptions in options.Value.NotificationAreas)
            {
                // Build the set of ignored pokemon IDs
                foreach (var pokemon in areaOptions.Pokemon)
                {
                    if (pokemon.Ignore)
                    {
                        ignored.Add(pokemon.Id);
                    }
                    else
                    {
                        ignored.Remove(pokemon.Id);
                    }
                }

                // Build the polygon
                var points = areaOptions
                    .Polygon
                    .Select(x => new Point(x.Longitude, x.Latitude))
                    .ToArray();
                var polygon = new Polygon(points);

                var area = new NotificationArea
                {
                    IgnoredPokemon = new HashSet<int>(ignored),
                    Polygon = polygon
                };

                areas.Add(area);
            }

            return areas;
        }

        private class NotificationArea
        {
            public Polygon Polygon { get; set; }
            public HashSet<int> IgnoredPokemon { get; set; }
        }
    }
}
