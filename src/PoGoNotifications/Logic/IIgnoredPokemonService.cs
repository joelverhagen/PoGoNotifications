using Knapcode.PoGoNotifications.Models;

namespace Knapcode.PoGoNotifications.Logic
{
    public interface IIgnoredPokemonService
    {
        bool IsIgnored(PokemonEncounter encounter);
    }
}