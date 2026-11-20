using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;
using TeufortTrail.Events.Prefab;

namespace TeufortTrail.Events.Vehicle
{
    /// <summary>
    /// The camper van gets into an accident, losing supplies and maybe hurting the party.
    /// </summary>
    [DirectorEvent(EventCategory.Vehicle)]
    public sealed class CrashedVehicle : ItemDestroyer
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "Your camper van got into an accident.";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            return TryHurtPassengers(20, 20, 80, "crushed");
        }
    }
}
