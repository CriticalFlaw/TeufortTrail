using System;
using TeufortTrail.Entities;

namespace TeufortTrail.Events.Director
{
    public sealed class EventDirector : WolfCurses.Module.Module
    {
        private EventFactory _eventFactory;

        /// <summary>
        /// Called when an event has been triggered by the event director.
        /// </summary>
        public event EventTriggered OnEventTriggered;

        /// <summary>
        /// Called when an event has been triggered by the event director.
        /// </summary>
        public delegate void EventTriggered(IEntity gameEntity, EventProduct eventProduct);

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="EventDirector" /> class.
        /// </summary>
        public EventDirector()
        {
            _eventFactory = new EventFactory();
        }

        /// <summary>
        /// Called when the game is closing and needs to clean up data remanents.
        /// </summary>
        public override void Destroy()
        {
            _eventFactory = null;
        }

        /// <summary>
        /// Called when an event of a given type needs to be triggered.
        /// </summary>
        /// <param name="gameEntity">Game entity that will be effected by the event.</param>
        /// <param name="eventType">Event type to trigger.</param>
        public void TriggerEvent(IEntity gameEntity, Type eventType)
        {
            // Create an instance of the triggered event, then execute it.
            var eventProduct = _eventFactory.CreateInstance(eventType);
            ExecuteEvent(gameEntity, eventProduct);
        }

        /// <summary>
        /// Attaches the event to the screen and executes its function.
        /// </summary>
        /// <param name="gameEntity">Game entity that will be effected by the event.</param>
        /// <param name="directorEvent">Instance of an event to execute.</param>
        private void ExecuteEvent(IEntity gameEntity, EventProduct eventProduct)
        {
            // Attach the base event screen then triggering the event.
            GameCore.Instance.WindowManager.Add(typeof(Event));
            OnEventTriggered?.Invoke(gameEntity, eventProduct);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class DirectorEventAttribute : Attribute
    {
        /// <summary>
        /// Defines the event type (Vehicle, Party, Weather etc.).
        /// </summary>
        public EventCategory EventCategory { get; }

        /// <summary>
        /// Defines whether the event is called manually or randomly.
        /// </summary>
        public EventExecution EventExecutionType { get; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectorEventAttribute" /> class.
        /// </summary>
        public DirectorEventAttribute(EventCategory eventCategory, EventExecution eventExecutionType = EventExecution.RandomOrManual)
        {
            EventCategory = eventCategory;
            EventExecutionType = eventExecutionType;
        }
    }
}