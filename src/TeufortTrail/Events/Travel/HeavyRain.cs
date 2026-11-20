using System;
using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;
using TeufortTrail.Events.Prefab;

namespace TeufortTrail.Events.Travel
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
            return "Heavy rain soaks the camper van.";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            return string.Empty;
        }
    }
}