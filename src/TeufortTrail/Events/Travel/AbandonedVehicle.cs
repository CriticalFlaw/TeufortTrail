using System;
using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// The party discovers an abandoned vehicle on the side of the road with supplies inside for the player to take.
    /// </summary>
    [DirectorEvent(EventCategory.Wild)]
    public sealed class AbandonedVehicle : ItemCreator
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreCreateItems()
        {
            return "You find an abandoned Mann Co. Supply Truck, ";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostCreateItems(IDictionary<ItemTypes, int> createdItems)
        {
            return (createdItems.Count > 0) ? $"and find:{Environment.NewLine}" : "but it's empty.";
        }
    }
}