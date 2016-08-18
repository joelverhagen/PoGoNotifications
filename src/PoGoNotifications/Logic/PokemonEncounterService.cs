using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Knapcode.PoGoNotifications.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Knapcode.PoGoNotifications.Logic
{
    public class PokemonEncounterService : IPokemonEncounterService
    {
        private readonly IOptions<NotificationOptions> _options;
        private readonly NotificationContext _notificationContext;
        private readonly ILogger<PokemonEncounterService> _logger;
        private readonly Polygon _polygon;
        private readonly HashSet<int> _ignoredPokemonIds;
        private readonly INotificationBuilder _notificationBuilder;
        private readonly INotificationService _notificationService;

        public PokemonEncounterService(
            ILogger<PokemonEncounterService> logger,
            IOptions<NotificationOptions> options,
            INotificationBuilder notificationBuilder,
            INotificationService notificationService,
            NotificationContext notificationContext)
        {
            _logger = logger;
            _options = options;
            _notificationBuilder = notificationBuilder;
            _notificationService = notificationService;
            _notificationContext = notificationContext;

            var points = _options
                .Value
                .NotifyPolygon
                .Select(x => new Point(x.Longitude, x.Latitude))
                .ToArray();
            _polygon = new Polygon(points);

            var ignoredPokemonIds = _options
                .Value
                .IgnoredPokemon?
                .Select(x => x.Id) ?? Enumerable.Empty<int>();
            _ignoredPokemonIds = new HashSet<int>(ignoredPokemonIds);
        }

        public async Task<IEnumerable<PokemonEncounter>> GetEncountersAsync(int skip, int take, bool ascending)
        {
            var result = _notificationContext.PokemonEncounters;
            IOrderedQueryable<PokemonEncounter> orderedResult;

            if (ascending)
            {
                orderedResult = result.OrderBy(x => x.DisappearTime);
            }
            else
            {
                orderedResult = result.OrderByDescending(x => x.DisappearTime);
            }

            return await orderedResult.Skip(skip).Take(take).ToListAsync();
        }

        public async Task AddEncounterAsync(PokemonEncounter encounter)
        {
            if (!IsInPolygon(encounter))
            {
                _logger.LogInformation("Encounter {encounterId} outside of the notify polygon.", encounter.EncounterId);
                return;
            }

            if (IsIgnored(encounter))
            {
                _logger.LogInformation("Encounter {encounterId} (pokemon {pokemonId}) is ignored.", encounter.EncounterId, encounter.PokemonId);
                return;
            }

            var disappearsIn = encounter.DisappearTime - DateTimeOffset.Now;
            if (disappearsIn < TimeSpan.FromMinutes(1))
            {
                _logger.LogInformation("Encounter {encounterId} is too soon and therefore ignored.", encounter.EncounterId, encounter.PokemonId);
                return;
            }

            var existing = _notificationContext.PokemonEncounters.FirstOrDefault(x =>
                x.EncounterId == encounter.EncounterId &&
                x.DisappearTime == encounter.DisappearTime);

            if (existing != null)
            {
                _logger.LogInformation("Duplicate encounter {encounterId} found before inserting.", encounter.EncounterId);
                return;
            }

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

            // Build the notification.
            var notification = await _notificationBuilder.BuildNotificationAsync(encounter);

            // Send the notification.
            await _notificationService.SendNotificationAsync(notification);
        }

        private bool IsIgnored(PokemonEncounter encounter)
        {
            return _ignoredPokemonIds.Contains(encounter.PokemonId);
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

        private bool IsInPolygon(PokemonEncounter encounter)
        {
            return _polygon.ContainsPoint(new Point(encounter.Longitude, encounter.Latitude));
        }
    }
}
