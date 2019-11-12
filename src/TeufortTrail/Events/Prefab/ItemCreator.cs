using System.Collections.Generic;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Event prefab used when a random amount of items needs to added to the player inventory.
    /// </summary>
    public abstract class ItemCreator : EventProduct
    {
        private StringBuilder _itemCreator;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Called when the event has been created by the event director, and before it is executed.
        /// </summary>
        public override void OnEventCreate()
        {
            base.OnEventCreate();
            _itemCreator = new StringBuilder();
        }

        /// <summary>
        /// Called when the event directory triggers the event action.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            _itemCreator.Clear();

            // Append the pre-create message if it exists.
            var preCreateText = OnPreCreateItems();
            if (!string.IsNullOrEmpty(preCreateText))
                _itemCreator.AppendLine(preCreateText);

            // Get a list of schema items of random type and quantity.
            var createdItems = GameCore.Instance.Vehicle.CreateRandomItems();

            // Append the post-create message if it exists.
            var postCreateText = OnPostCreateItems(createdItems);
            if (!string.IsNullOrEmpty(postCreateText))
                _itemCreator.Append(postCreateText);

            // Skip this step if no items were created.
            if (!(createdItems?.Count > 0)) return;

            // Loop through the generated items and add them to output string.
            foreach (var createdItem in createdItems)
                _itemCreator.AppendLine($"  + {createdItem.Value:N0} {createdItem.Key}");
        }

        /// <summary>
        /// Called when the event director needs to render the event text to the user interface.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            return _itemCreator.ToString();
        }

        /// <summary>
        /// Called before the event has been executed.
        /// </summary>
        protected abstract string OnPreCreateItems();

        /// <summary>
        /// Called after the event has been executed.
        /// </summary>
        protected abstract string OnPostCreateItems(IDictionary<ItemTypes, int> createdItems);
    }
}