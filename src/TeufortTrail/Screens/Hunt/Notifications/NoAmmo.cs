using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Hunt
{
    /// <summary>
    /// Displayed when the player wants to hunt but doesn't have any ammo.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class NoAmmo : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoAmmo" /> class.
        /// </summary>
        public NoAmmo(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            return $"{Environment.NewLine}You need more bullets to go hunting robots.{Environment.NewLine}";
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            ClearForm();
        }
    }
}