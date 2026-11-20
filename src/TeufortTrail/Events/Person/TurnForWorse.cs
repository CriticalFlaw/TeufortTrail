using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events.Person
{
    /// <summary>
    /// To start to get worse. It appeared that person was going to get well; then, unfortunately, they took a turn for the worse.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class TurnForWorse : EventProduct
    {
        /// <summary>
        /// Called when the event handler is triggering an event.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // A serious setback that can kill someone who is already weak.
            var person = eventExecutor.SourceEntity as Entities.Person.Person;
            person?.Damage(GameBalance.TurnForWorseDamage);
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            // Skip this step if the source entity is not a person.
            if (userData.SourceEntity is not Entities.Person.Person person) return "There was a health scare, but everyone is okay.";
            return person.HealthState == HealthStatus.Dead
                ? $"The {person.Class}'s health took a turn for the worse. The {person.Class} has died."
                : $"The {person.Class}'s health has taken a turn for the worse.";
        }
    }
}