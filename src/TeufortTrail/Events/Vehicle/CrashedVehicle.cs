﻿using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Vehicle gets into an accident, supplies could be destroyed and passengers can be crushed to death.
    /// </summary>
    [DirectorEvent(EventCategory.Vehicle)]
    public sealed class CrashedVehicle : ItemDestroyer
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return "Your vehicle got into an accident, resulting in ";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            // Change event text depending on if items were destroyed or not.
            return (destroyedItems.Count > 0) ? TryKillPassengers("crushed") : "no loss of items.";
        }
    }
}