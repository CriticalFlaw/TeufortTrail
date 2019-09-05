namespace TeufortTrail.Events.Director
{
    /// <summary>
    /// Represents an event that can be triggered by the event director while the vehicle is traveling along the trail.
    /// </summary>
    public abstract class EventProduct
    {
        /// <summary>
        /// Called after the event has been created by the event factory, but before it is executed.
        /// </summary>
        public virtual void OnEventCreate()
        {
        }

        /// <summary>
        /// Called when the event handler will be triggering an event.
        /// </summary>
        public abstract void Execute(EventInfo eventExecutor);

        /// <summary>
        /// Called when the the event needs to be rendered onto the user interface.
        /// </summary>
        public string Render(EventInfo userData)
        {
            return OnRender(userData);
        }

        /// <summary>
        /// Called when the the event needs to be rendered onto the user interface.
        /// </summary>
        protected abstract string OnRender(EventInfo userData);

        /// <summary>
        /// Called right before an event is closed to perform some kind of task.
        /// </summary>
        public virtual void OnEventClose(EventInfo userData)
        {
            // Nothing to see here, move along...
        }
    }

    /// <summary>
    /// Defines the event type (Vehicle, Party, Weather etc.).
    /// </summary>
    public enum EventCategory
    {
        Person
    }

    /// <summary>
    /// Defines whether the event is called manually or randomly.
    /// </summary>
    public enum EventExecution
    {
        RandomOrManual = 0,
        ManualOnly = 1
    }

    /// <summary>
    /// Used as a unique identifier for each event that it to be registered in the system as it is being triggered.
    /// </summary>
    public sealed class EventKey
    {
        #region VARIABLES

        public EventCategory Category { get; }
        public string Name { get; }
        public EventExecution ExecutionType { get; }

        #endregion VARIABLES

        public EventKey(EventCategory category, string name, EventExecution executionType)
        {
            Category = category;
            Name = name;
            ExecutionType = executionType;
        }
    }
}