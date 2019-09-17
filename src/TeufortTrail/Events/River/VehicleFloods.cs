using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Player forded the river and it was to deep, they have been washed out by the current and some items destroyed.
    /// </summary>
    [DirectorEvent(EventCategory.River, EventExecution.ManualOnly)]
    public sealed class VehicleFloods : ItemDestroyer
    {
        /// <summary>
        /// Called when the event directory triggers the event action.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            base.Execute(eventExecutor);
        }

        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "Your vehicle got flooded when attempting to cross the river, resulting in ";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            return (destroyedItems.Count > 0) ? TryKillPassengers("drowned") : "no loss of items.";
        }
    }
}