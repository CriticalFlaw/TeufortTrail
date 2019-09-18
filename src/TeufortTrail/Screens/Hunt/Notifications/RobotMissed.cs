using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Hunt
{
    /// <summary>
    /// Displayed when the player misses their shot on the target.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class RobotMissed : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RobotMissed" /> class.
        /// </summary>
        public RobotMissed(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            return $"{Environment.NewLine}You missed the shot, and the {UserData.Hunt.LastEscaped.Entity.Name.ToLowerInvariant()} robot got away!{Environment.NewLine}";
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(Hunting));
        }
    }
}