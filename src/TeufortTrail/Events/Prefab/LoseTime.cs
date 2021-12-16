using TeufortTrail.Events.Director;
using TeufortTrail.Screens.Travel;

namespace TeufortTrail.Events.Prefab
{
    /// <summary>
    /// Event prefab used when the time needs to be advanced.
    /// </summary>
    public abstract class LoseTime : EventProduct
    {
        /// <summary>
        /// Defines the number of days that should be skipped the result of a given event.
        /// </summary>
        protected abstract int DaysToSkip();

        /// <summary>
        /// Defines the explanation as to why the time skip has taken place.
        /// </summary>
        protected abstract string OnLostTimeReason();

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Called when the event directory triggers the event action.
        /// </summary>
        public override void Execute(EventInfo eventExecutor)
        {
            // Increment the days to skip total.
            eventExecutor.DaysToSkip += DaysToSkip();
        }

        /// <summary>
        /// Called when the event director needs to render the event text to the user interface.
        /// </summary>
        protected override string OnRender(EventInfo userData)
        {
            return OnLostTimeReason();
        }

        /// <summary>
        /// Called after the event has been executed, display the time skip explanation.
        /// </summary>
        internal override bool OnPostExecute(EventExecutor eventExecutor)
        {
            base.OnPostExecute(eventExecutor);

            // Skip this step if there's no time skip.
            if (eventExecutor.UserData.DaysToSkip > 0) return false;

            // Attach the screen explaining the time skip event.
            eventExecutor.SetForm(typeof(EventSkipDay));
            return true;
        }
    }
}