using System;
using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Severe weather will destroy supplies and waste your time, at least nobody will get killed.
    /// </summary>
    [DirectorEvent(EventCategory.Wild, EventExecution.ManualOnly)]
    public sealed class HeavyRain : ItemDestroyer
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "Heavy rain, resulting in the loss of:";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            return (destroyedItems.Count > 0) ? $"supplies lost:{Environment.NewLine}" : "no items lost.";
        }
    }
}