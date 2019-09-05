using TeufortTrail.Entities;
using TeufortTrail.Events.Director;
using WolfCurses.Window;

namespace TeufortTrail.Events.Director
{
    /// <summary>
    /// Base event screen that gets attached to the user interface by the event director, then delegates the execution to another event.
    /// </summary>
    public sealed class Event : Window<EventCommands, EventInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Events.Director.Event" /> class.
        /// </summary>
        public Event(GameCore game) : base(game)
        {
        }

        /// <summary>
        /// Called when the screen has been activated.
        /// </summary>
        public override void OnWindowPostCreate()
        {
            // Event director has event to know when events are triggered.
            GameCore.Instance.EventDirector.OnEventTriggered += Director_OnEventTriggered;
        }

        /// <summary>
        /// Called when the event screen has been removed from the user interface. Prevents it from being triggered multiple times.
        /// </summary>
        protected override void OnModeRemoved()
        {
            base.OnModeRemoved();
            if (GameCore.Instance.EventDirector != null)
                GameCore.Instance.EventDirector.OnEventTriggered -= Director_OnEventTriggered;
        }

        /// <summary>
        /// Called when the event director has triggered an event either by random or on purpose.
        /// </summary>
        private void Director_OnEventTriggered(IEntity simEntity, EventProduct directorEvent)
        {
            UserData.DirectorEvent = directorEvent;
            UserData.SourceEntity = simEntity;
            SetForm(typeof(EventExecutor));
        }
    }

    public enum EventCommands
    {
        // Nothing to see here... move along.
    }

    public sealed class EventInfo : WindowData
    {
        public EventProduct DirectorEvent { get; set; }
        public IEntity SourceEntity { get; set; }
        public string EventText { get; set; }
    }
}