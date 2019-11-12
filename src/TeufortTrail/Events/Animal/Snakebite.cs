using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Processes an attack of snake biting one of the passengers in the vehicle at random. Depending on the outcome of the event we might kill the player if they actually get bit, otherwise the event will say they killed it.
    /// </summary>
    [DirectorEvent(EventCategory.Animal)]
    public sealed class Snakebite : EventProduct
    {
        /// <summary>
        /// Called when the event handler is triggering an event.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // Skip this step if the source entity is not a person.
            var person = eventExecutor.SourceEntity as Entities.Person.Person;
            if (person == null) return;

            // Remove the player ammo used to fight off the snake.
            GameCore.Instance.Vehicle.Inventory[ItemTypes.Ammo].SubtractQuantity(5);

            // Infect the person was bit by the snake.
            if (GameCore.Instance.Random.NextBool())
            {
                person.Infect();
                person.Damage(150);
            }
            else
                person.Damage(10);
        }

        /// <summary>
        /// Called when the event director needs to render the event text to the user interface.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            return "You've killed a poisonous snake, but only after it bit you.";
        }
    }
}