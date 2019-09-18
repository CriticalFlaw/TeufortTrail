﻿using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
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
            // We are going to inflict enough damage to probably kill the person.
            var person = eventExecutor.SourceEntity as Entities.Person.Person;
            person?.Damage(100);
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            // Skip this step if the source entity is not a person.
            var person = userData.SourceEntity as Entities.Person.Person;
            return (person == null) ? "Nobody has taken a turn for the worse." : $"{person.Class} has taken a turn for the worse.";
        }
    }
}