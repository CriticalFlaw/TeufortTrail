using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// Party leader has died! This will end the entire simulation since the others cannot go on without the leader.
    /// </summary>
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class PlayerDeath : EventProduct
    {
        private StringBuilder _deathPlayer;

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
            // Check to make sure this player is the leader (aka the player).
            var sourcePerson = eventExecutor.SourceEntity as Entities.Person.Person;
            if (sourcePerson == null) throw new ArgumentNullException(nameof(eventExecutor), "Could not cast source entity as player.");
            if (!sourcePerson.Leader) throw new ArgumentException("Cannot kill this person because it is not the player!");
            _deathPlayer = new StringBuilder();
            _deathPlayer.AppendLine("You have died.");
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            return _deathPlayer.ToString();
        }
    }
}