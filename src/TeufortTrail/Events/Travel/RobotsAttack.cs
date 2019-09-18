using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Robots attack the player. There is a chance some of your party members may get murdered.
    /// </summary>
    [DirectorEvent(EventCategory.Wild)]
    public sealed class RobotsAttack : ItemDestroyer
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "Robots ambush your party, resulting in ";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            // Randomly generate the amount of ammo used to kill the robots.
            GameCore.Instance.Vehicle.Inventory[ItemTypes.Ammo].SubtractQuantity(GameCore.Instance.Random.Next(5, 15));

            // Change event text depending on the outcome.
            return (destroyedItems.Count > 0) ? TryKillPassengers("murdered") : "no loss of items.";
        }
    }
}