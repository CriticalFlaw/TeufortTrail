using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Event prefab used when a person in the party needs to injured.
    /// </summary>
    public abstract class PersonInjure : EventProduct
    {
        /// <summary>
        /// Called when the event directory triggers the event action.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // Set the injury flag on the source entity.
            var person = eventExecutor.SourceEntity as Entities.Person.Person;
            person?.Injure();
        }

        /// <summary>
        /// Called when the event director needs to render the event text to the user interface.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            // Skip this step if the source entity is not of correct type.
            var person = userData.SourceEntity as Entities.Person.Person;
            return (person == null) ? "There was an injury scare, but everyone is okay." : OnPostInjury(person);
        }

        /// <summary>
        /// Called after the event has been executed, updates target entity.
        /// </summary>
        protected abstract string OnPostInjury(Entities.Person.Person person);
    }
}