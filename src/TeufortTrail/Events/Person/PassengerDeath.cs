using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// One of the party members dies.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class PassengerDeath : EventProduct
    {
        private StringBuilder _deathCompanion;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Called after the event has been created by the event factory, but before it is executed.
        /// </summary>
        public override void OnEventCreate()
        {
            base.OnEventCreate();
        }

        /// <summary>
        /// Called when the event handler is triggering an event.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // Cast the game entity as a party member/vehicle passenger. Check that the passenger entity was cast correctly.
            var passenger = eventExecutor.SourceEntity as Entities.Person.Person;
            if (passenger == null) throw new ArgumentNullException(nameof(eventExecutor), "Could not cast source entity as passenger of vehicle.");

            // Create the event notification message to be displayed to the player.
            _deathCompanion = new StringBuilder();
            _deathCompanion.AppendLine($"{passenger.Class} has died.");
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            return _deathCompanion.ToString();
        }
    }
}