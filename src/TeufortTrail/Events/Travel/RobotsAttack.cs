using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;
using TeufortTrail.Events.Prefab;

namespace TeufortTrail.Events.Travel
{
    /// <summary>
    /// Robots ambush the party. Having enough ammo to fight back keeps injuries down.
    /// </summary>
    [DirectorEvent(EventCategory.Wild)]
    public sealed class RobotsAttack : ItemDestroyer
    {
        private bool _couldFightBack;

        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            var ammo = GameCore.Instance.Vehicle.Inventory[ItemTypes.Ammo];
            _couldFightBack = ammo.Quantity >= GameBalance.AmmoToRepelRobots;
            ammo.SubtractQuantity(GameCore.Instance.Random.Next(5, 16));
            return _couldFightBack
                ? "Robots ambush your party! You fight them off."
                : "Robots ambush your party! Without enough ammo, you can barely fight back.";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            return _couldFightBack
                ? TryHurtPassengers(15, 30, 90, "killed")
                : TryHurtPassengers(40, 60, 160, "killed");
        }
    }
}
