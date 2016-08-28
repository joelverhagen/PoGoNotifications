using System.Collections.Generic;
using System.Threading.Tasks;
using Knapcode.PoGoNotifications.Models;

namespace Knapcode.PoGoNotifications.Logic
{
    public interface IPokemonEncounterService
    {
        Task<IEnumerable<PokemonEncounter>> GetEncountersAsync(int skip, int take, bool ascending);
        Task AddEncounterAsync(PokemonEncounter encounter);
    }
}