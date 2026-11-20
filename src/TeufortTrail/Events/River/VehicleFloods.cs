using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;
using TeufortTrail.Events.Prefab;

namespace TeufortTrail.Events.River
{
    /// <summary>
    /// The camper van floods while fording a river, washing away supplies and nearly drowning the party.
    /// </summary>
    [DirectorEvent(EventCategory.River, EventExecution.ManualOnly)]
    public sealed class VehicleFloods : ItemDestroyer
    {
        /// <summary>
        /// Money can wash away too.
        /// </summary>
        protected override bool CanLoseMoney => true;

        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "Your camper van flooded while fording the river!";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            return TryHurtPassengers(30, 60, 180, "drowned");
        }
    }
}
