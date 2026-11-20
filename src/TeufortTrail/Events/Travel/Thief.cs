using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;
using TeufortTrail.Events.Prefab;

namespace TeufortTrail.Events.Travel
{
    /// <summary>
    /// An enemy spy steals supplies and money in the night.
    /// </summary>
    [DirectorEvent(EventCategory.Wild)]
    public sealed class Thief : ItemDestroyer
    {
        /// <summary>
        /// Spies go for the wallet too.
        /// </summary>
        protected override bool CanLoseMoney => true;

        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            GameCore.Instance.Vehicle.Inventory[ItemTypes.Ammo].SubtractQuantity(GameCore.Instance.Random.Next(1, 5));
            return "An enemy spy sneaked into camp in the middle of the night.";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            return TryHurtPassengers(10, 20, 60, "backstabbed");
        }
    }
}
