﻿using System;
using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Prefab;

namespace TeufortTrail.Events.Special
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
            return $"The Pyro set your vehicle on fire,{Environment.NewLine}resulting in the loss of:";
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