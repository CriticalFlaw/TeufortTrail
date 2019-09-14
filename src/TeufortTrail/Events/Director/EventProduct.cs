using TeufortTrail.Entities;

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
        /// Called when the event handler is triggering an event.
        /// </summary>
        public abstract void Execute(EventInfo eventExecutor);

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public string Render(EventInfo userData)
        {
            return OnRender(userData);
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected abstract string OnRender(EventInfo userData);

        /// <summary>
        /// Called right before an event is closed to perform some kind of task.
        /// </summary>
        public virtual void OnEventClose(EventInfo userData)
        {
            // Nothing to see here, move along...
        }

        /// <summary>
        /// Called after an event is executed and allows event prefabs to do post-event execution.
        /// </summary>
        internal virtual bool OnPostExecute(EventExecutor eventExecutor)
        {
            return false;
        }
    }

    /// <summary>
    /// Used as a unique identifier for each event that it to be registered in the system as it is being triggered.
    /// </summary>
    public sealed class EventKey
    {
        public EventCategory Category { get; }
        public string Name { get; }
        public EventExecution ExecutionType { get; }

        //-------------------------------------------------------------------------------------------------

        public EventKey(EventCategory category, string name, EventExecution executionType)
        {
            Category = category;
            Name = name;
            ExecutionType = executionType;
        }
    }
}