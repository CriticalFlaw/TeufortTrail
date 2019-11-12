using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Event prefab used when a person in the party needs to infected.
    /// </summary>
    public abstract class PersonInfect : EventProduct
    {
        /// <summary>
        /// Called when the event directory triggers the event action.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // Set the infect flag on the source entity.
            var person = eventExecutor.SourceEntity as Entities.Person.Person;
            person?.Infect();
        }

        /// <summary>
        /// Called when the event director needs to render the event text to the user interface.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            // Skip this step if the source entity is not of correct type.
            var person = userData.SourceEntity as Entities.Person.Person;
            return (person == null) ? "There was an illness scare, but everyone is okay." : OnPostInfection(person);
        }

        /// <summary>
        /// Called after the event has been executed, updates target entity.
        /// </summary>
        protected abstract string OnPostInfection(Entities.Person.Person person);
    }
}