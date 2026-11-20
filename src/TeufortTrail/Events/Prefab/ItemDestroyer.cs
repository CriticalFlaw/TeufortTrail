using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events.Prefab
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

            // Get a list of schema items of random type and quantity, and list them.
            var destroyedItems = GameCore.Instance.Vehicle.DestroyRandomItems(CanLoseMoney);
            if (destroyedItems.Count > 0)
            {
                _itemDestroyer.AppendLine("You lost:");
                foreach (var (key, value) in destroyedItems)
                    _itemDestroyer.AppendLine(key == ItemTypes.Money ? $"  - ${value:N0}" : $"  - {value:N0} {ItemName(key, value)}");
            }
            else
                _itemDestroyer.AppendLine("Luckily, no supplies were lost.");

            // Append the post-destroy message if it exists.
            var postDestroyText = OnPostDestroyItems(destroyedItems);
            if (!string.IsNullOrEmpty(postDestroyText))
                _itemDestroyer.Append(postDestroyText);
        }

        /// <summary>
        /// TRUE if this event can take the party's money.
        /// </summary>
        protected virtual bool CanLoseMoney => false;

        /// <summary>
        /// Name of an item type as shown to the player.
        /// </summary>
        internal static string ItemName(ItemTypes itemType, int count = 2) => itemType switch
        {
            ItemTypes.Clothing => count == 1 ? "hat" : "hats",
            ItemTypes.Ammo => "ammo",
            _ => itemType.ToString().ToLowerInvariant()
        };

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
        /// Rolls the dice for each passenger to see if they get hurt. Anyone hurt badly enough dies.
        /// </summary>
        /// <param name="percentChance">Chance out of 100 that each passenger is hurt.</param>
        /// <param name="minDamage">Smallest amount of health lost.</param>
        /// <param name="maxDamage">Largest amount of health lost.</param>
        /// <param name="killVerb">Action verb that describes how the person died such as burned, frozen, drowned, etc.</param>
        /// <returns>Formatted string that can be displayed on render for event item destruction.</returns>
        internal static string TryHurtPassengers(int percentChance, int minDamage, int maxDamage, string killVerb)
        {
            var output = new StringBuilder();
            foreach (var person in GameCore.Instance.Vehicle.TryHurt(percentChance, minDamage, maxDamage))
                output.AppendLine(person.HealthState == HealthStatus.Dead
                    ? $"  - The {person.Class} was {killVerb}."
                    : $"  - The {person.Class} was injured.");
            return output.Length > 0 ? Environment.NewLine + output : string.Empty;
        }
    }
}