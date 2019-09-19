using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Event prefab used when a random amount of items needs to removed from the player inventory.
    /// </summary>
    public abstract class ItemDestroyer : EventProduct
    {
        private StringBuilder _itemDestroyer;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Called when the event has been created by the event director, and before it is executed.
        /// </summary>
        public override void OnEventCreate()
        {
            base.OnEventCreate();
            _itemDestroyer = new StringBuilder();
        }

        /// <summary>
        /// Called when the event directory triggers the event action.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            _itemDestroyer.Clear();

            // Append the pre-destroy message if it exists.
            var preDestroyText = OnPreDestroyItems();
            if (!string.IsNullOrEmpty(preDestroyText))
                _itemDestroyer.AppendLine(preDestroyText);

            // Get a list of schema items of random type and quantity.
            var destroyedItems = GameCore.Instance.Vehicle.DestroyRandomItems();

            // Append the post-destroy message if it exists.
            var postDestroyText = OnPostDestroyItems(destroyedItems);
            if (!string.IsNullOrEmpty(postDestroyText))
                _itemDestroyer.AppendLine(postDestroyText);

            // Skip this step if no items were destroyed.
            if (!(destroyedItems?.Count > 0)) return;

            // Loop through the generated items and add them to output string.
            foreach (var destroyedItem in destroyedItems)
                _itemDestroyer.AppendLine($"- {destroyedItem.Value:N0} {destroyedItem.Key}");
        }

        /// <summary>
        /// Called when the event director needs to render the event text to the user interface.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            return _itemDestroyer.ToString();
        }

        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected abstract string OnPreDestroyItems();

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected abstract string OnPostDestroyItems(IDictionary<ItemTypes, int> destroyedItems);

        /// <summary>
        ///     Rolls the dice and attempts to kill the passengers of the vehicle. If that happens then the killing verb will be
        ///     applied next to their name.
        /// </summary>
        /// <param name="killVerb">Action verb that describes how the person died such as burned, frozen, drowned, etc.</param>
        /// <returns>Formatted string that can be displayed on render for event item destruction.</returns>
        internal static string TryKillPassengers(string killVerb)
        {
            // Get a list of vehicle passengers killed in the event.
            var _postDestroy = new StringBuilder();
            _postDestroy.AppendLine("The loss of:");
            var passengersKilled = GameCore.Instance.Vehicle.TryKill();
            var passengers = (passengersKilled as IList<Entities.Person.Person>) ?? passengersKilled.ToList();
            foreach (var person in passengers)
            {
                // Skip this step if the passenger isn't dead.
                if (person.HealthState != HealthStatus.Dead) continue;
                _postDestroy.AppendLine($"- {person.Class} ({killVerb})");
            }
            return _postDestroy.ToString();
        }
    }
}