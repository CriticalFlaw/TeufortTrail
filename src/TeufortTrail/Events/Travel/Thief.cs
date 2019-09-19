using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Enemy spy sneaks in the middle of the night and steals from the player. There is a chance some of your party members may get backstabbed.
    /// </summary>
    [DirectorEvent(EventCategory.Wild)]
    public sealed class Thief : ItemDestroyer
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "An enemy spy snuck in the middle of the night, resulting in ";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            // Randomly generate the amount of ammo used to kill the spy.
            GameCore.Instance.Vehicle.Inventory[ItemTypes.Ammo].SubtractQuantity(GameCore.Instance.Random.Next(1, 5));

            // Change event text depending on the outcome.
            return (destroyedItems.Count > 0) ? TryKillPassengers("backstabbed") : "nothing because you've caught him before he was able to do anything.";
        }
    }
}