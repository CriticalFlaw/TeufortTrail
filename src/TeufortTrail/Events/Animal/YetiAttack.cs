﻿using System;
using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;
using TeufortTrail.Events.Prefab;

namespace TeufortTrail.Events.Animal
{
    /// <summary>
    /// The Yeti attacks the player.
    /// </summary>
    [DirectorEvent(EventCategory.Animal)]
    public sealed class YetiAttack : ItemDestroyer
    {
        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            return $"A wild yeti attack you in the night,{Environment.NewLine}resulting in the loss of:";
        }

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected override string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems)
        {
            // Change event text depending on if items were destroyed or not.
            return (destroyedItems.Count > 0) ? TryKillPassengers("mauled") : "no loss of items.";
        }
    }
}