using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Commands
{
    [ParentWindow(typeof(Travel))]
    public sealed class StopToRest : InputForm<TravelInfo>
    {
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
            for (var x = 0; x < UserData.DaysToRest; x++)
                GameCore.Instance.TakeTurn(false);
            return $"{Environment.NewLine}Your party has rested for {UserData.DaysToRest} days.{Environment.NewLine}{Environment.NewLine}";
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