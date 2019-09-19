using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Makes the person whom the event was fired on no loner afflicted by any illness.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class WellAgain : EventProduct
    {
        /// <summary>
        /// Called when the event handler is triggering an event.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // Removes all infections, injuries, and heals the person in full.
            var person = eventExecutor.SourceEntity as Entities.Person.Person;
            person?.HealFully();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            // Skip if the source entity is not a person.
            var person = userData.SourceEntity as Entities.Person.Person;
            return (person == null) ? "nobody got healed." : $"{person.Class} has fully healed.";
        }
    }
}