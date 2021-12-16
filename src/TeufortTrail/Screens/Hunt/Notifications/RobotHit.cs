using System;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Hunt.Notifications
{
    /// <summary>
    /// Displayed when the player hits their shot on the target.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class RobotHit : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RobotHit" /> class.
        /// </summary>
        public RobotHit(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            return $"{Environment.NewLine}You shot a{((UserData.Hunt.LastDestroyed.Entity.TotalValue > 100) ? " giant " : " ")}robot {UserData.Hunt.LastDestroyed.Entity.Name.ToLowerInvariant()}.{Environment.NewLine}";
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