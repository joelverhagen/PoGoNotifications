using System.Threading.Tasks;
using Knapcode.PoGoNotifications.Models;

namespace Knapcode.PoGoNotifications.Logic
{
    public interface INotificationBuilder
    {
        Task<Notification> BuildNotificationAsync(PokemonEncounter encounter);
    }
}