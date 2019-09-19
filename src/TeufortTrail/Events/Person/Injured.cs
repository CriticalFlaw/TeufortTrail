using System;
using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// One of the members of the vehicle passenger manifest is injured.
    /// </summary>
    [DirectorEvent(EventCategory.Person)]
    public sealed class Injured : PersonInjure
    {
        /// <summary>
        /// Called after the event has been executed, updates target entity.
        /// </summary>
        protected override string OnPostInjury(Entities.Person.Person person)
        {
            return $"The {person.Class} is injured.{Environment.NewLine}";
        }
    }
}