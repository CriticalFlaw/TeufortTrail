﻿using TeufortTrail.Entities;
using TeufortTrail.Events.Director;

namespace TeufortTrail.Events
{
    /// <summary>
    /// One of the members of the vehicle passenger manifest is sick.
    /// </summary>
    [DirectorEvent(EventCategory.Person)]
    public sealed class Infected : PersonInfect
    {
        /// <summary>
        /// Called after the event has been executed, updates target entity.
        /// </summary>
        protected override string OnPostInfection(Entities.Person.Person person)
        {
            return $"The {person.Class} is sick.";
        }
    }
}