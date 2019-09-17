using System.Collections.Generic;
using TeufortTrail.Entities;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Fire in the vehicle occurs, there is a chance that some of the inventory items or people were burned to death.
    /// </summary>
    public sealed class VehicleFire : ItemDestroyer
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "The Pyro set your vehicle on fire, resulting in ";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            // Change event text depending on if items were destroyed or not.
            return (destroyedItems.Count > 0) ? TryKillPassengers("burned") : "no loss of items.";
        }
    }
}