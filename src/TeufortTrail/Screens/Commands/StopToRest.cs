using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Commands
{
    /// <summary>
    /// Displays the number of days the party has rested at the current location.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class StopToRest : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopToRest" /> class.
        /// </summary>
        public StopToRest(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        /// <remarks>TODO: Consume resources and trigger events after resting.</remarks>
        protected override string OnDialogPrompt()
        {
            // Increment the turn counter depending on the number of days the party has rested.
            for (var x = 0; x < UserData.DaysToRest; x++)
                GameCore.Instance.TakeTurn(false);
            return $"{Environment.NewLine}Your party has rested for {UserData.DaysToRest} days.{Environment.NewLine}{Environment.NewLine}";
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ClearForm();
        }
    }
}