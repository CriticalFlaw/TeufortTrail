using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Food in the inventory is lost due to spoilage or improper storage.
    /// </summary>
    [DirectorEvent(EventCategory.Wild)]
    public sealed class FoodSpoiled : FoodDestroyer
    {
        /// <summary>
        /// Called after the event has been executed, display the food loss explanation.
        /// </summary>
        protected override string OnFoodSpoilReason()
        {
            return "Food spoiled.";
        }
    }
}