using System;
using System.Text;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class PassengerDeath : EventProduct
    {
        #region VARIABLES

        private StringBuilder _deathCompanion;

        #endregion VARIABLES

        /// <summary>
        /// Called after the event has been created by the event factory, but before it is executed.
        /// </summary>
        public override void OnEventCreate()
        {
            base.OnEventCreate();
            _deathCompanion = new StringBuilder();
        }

        /// <summary>
        /// Called when the event handler will be triggering an event.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // Cast the game entity as a party member/vehicle passenger.
            var passenger = eventExecutor.SourceEntity as Entities.Person.Person;

            // Check that the passenger entity was cast correctly.
            if (passenger == null) throw new ArgumentNullException(nameof(eventExecutor), "Could not cast source entity as passenger of vehicle.");

            // Create the event notification message to be displayed to the player.
            _deathCompanion.AppendLine($"{passenger.Class} has died.");
        }

        /// <summary>
        /// Called when the the event needs to be rendered onto the user interface.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            return _deathCompanion.ToString();
        }
    }
}