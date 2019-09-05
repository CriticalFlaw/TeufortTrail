using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Location
{
    [ParentWindow(typeof(Travel))]
    public sealed class StopToRest : InputForm<TravelInfo>
    {
        #region VARIABLES

        /// <summary>
        /// The number of days the party will rest for at the location.
        /// </summary>
        /// <remarks>TODO: Let the player choose how many days to rest.</remarks>
        private int daysToRest = 3;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Location.StopToRest" /> class.
        /// </summary>
        public StopToRest(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        /// <remarks>TODO: Consume resources and trigger events after staying in town for a significant amount of time.</remarks>
        protected override string OnDialogPrompt()
        {
            // Simulate the days to rest in time and event system, this will trigger random event game Windows if required. R
            for (var x = 0; x < daysToRest; x++)
                GameCore.Instance.TakeTurn(false);
            return $"{Environment.NewLine}Your party has rested for {daysToRest} days.{Environment.NewLine}{Environment.NewLine}";
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ClearForm();
        }
    }
}