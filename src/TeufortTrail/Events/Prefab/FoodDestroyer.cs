using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Event prefab used when a random amount of food needs to removed from the player inventory.
    /// </summary>
    public abstract class FoodDestroyer : EventProduct
    {
        /// <summary>
        /// Called when the event directory triggers the event action.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // Skip this step if the source entity is not of correct type.
            var vehicle = eventExecutor.SourceEntity as Entities.Vehicle.Vehicle;
            if (vehicle == null) return;

            // Check that the player has enough food to remove.
            if (vehicle.Inventory[Entities.ItemTypes.Food].Quantity < 4) return;

            // Remove a random amount of food from the player's inventory.
            var spoiledFood = vehicle.Inventory[Entities.ItemTypes.Food].Quantity / 4;
            vehicle.Inventory[Entities.ItemTypes.Food].SubtractQuantity(GameCore.Instance.Random.Next(3, spoiledFood));
        }

        /// <summary>
        /// Called when the event director needs to render the event text to the user interface.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            return OnFoodSpoilReason();
        }

        /// <summary>
        /// Called after the event has been executed, display the food loss explanation.
        /// </summary>
        protected abstract string OnFoodSpoilReason();
    }
}