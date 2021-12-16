using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeufortTrail.Entities;
using WolfCurses.Utility;

namespace TeufortTrail.Events.Director
{
    /// <summary>
    /// Factory pattern for creating director event instances from event type references.
    /// </summary>
    public sealed class EventFactory
    {
        /// <summary>
        /// References all of the events that have been triggered, in the order they occurred.
        /// </summary>
        private Dictionary<EventKey, Type> EventReference { get; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFactory" /> class.
        /// </summary>
        public EventFactory()
        {
            // Create a dictionary for storing event reference keys and types.
            EventReference = new Dictionary<EventKey, Type>();

            // Collect and loop through all of the event types.
            foreach (var eventObject in AttributeExtensions.GetTypesWith<DirectorEventAttribute>(true))
            {
                // Check if the class is abstract base class, we don't want to add that.
                if (eventObject.GetTypeInfo().IsAbstract) continue;

                // Check the attribute itself from the event we are working on, which gives us the event type enumerable.
                var eventAttribute = eventObject.GetTypeInfo().GetAttributes<DirectorEventAttribute>(true).First();

                // Initialize the execution history dictionary with every event type.
                foreach (var modeType in Enum.GetValues(typeof(EventCategory)))
                {
                    // Create key for the event execution counter.
                    var eventKey = new EventKey((EventCategory)modeType, eventObject.Name, eventAttribute.EventExecutionType);
                    if (!EventReference.ContainsKey(eventKey)) EventReference.Add(eventKey, eventObject);
                }
            }
        }

        /// <summary>
        /// Creates a new event instance of a given type, which will be kept track of in event reference dictionary.
        /// </summary>
        /// <param name="eventType">The type of event which we should create an instance of.</param>
        internal EventProduct CreateInstance(Type eventType)
        {
            // Retrieve an event reference from the dictionary that matches the event inputted type.
            var eventReference = EventReference.FirstOrDefault(x => x.Value == eventType).Value;

            // Check if the class is abstract base class, we don't want to add that.
            if (eventReference.GetTypeInfo().IsAbstract) return null;

            // Create an event product (instance), then execute and return it.
            if (FactoryExtensions.New<EventProduct>.GetUninitializedObject(eventReference) is not EventProduct eventInstance) return null;
            eventInstance.OnEventCreate();
            return eventInstance;
        }

        /// <summary>
        /// Creates a new event instance of a given type, which will be kept track of in event reference dictionary.
        /// </summary>
        /// <param name="eventCategory">Enumerable value of the event type to create</param>
        public EventProduct CreateRandomByType(EventCategory eventCategory)
        {
            // Loop through all the event types to find matching enumeration values.
            var eventList = (from type in EventReference where type.Key.Category.Equals(eventCategory) && (type.Key.ExecutionType == EventExecution.RandomOrManual) select type.Value).ToList();

            // Create a random event instance of a given type if there is at least one event type that has been returned.
            return eventList.Count <= 0 ? null : CreateInstance(eventList[GameCore.Instance.Random.Next(eventList.Count)]);
        }
    }
}