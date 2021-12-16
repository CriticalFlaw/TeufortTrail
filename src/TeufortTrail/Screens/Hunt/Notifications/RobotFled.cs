using System;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Hunt.Notifications
{
    /// <summary>
    /// Displayed when the target robot has retreated.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class RobotFled : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RobotFled" /> class.
        /// </summary>
        public RobotFled(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            return $"{Environment.NewLine}The {UserData.Hunt.LastEscaped.Entity.Name.ToLowerInvariant()} robot detects danger and escapes.{Environment.NewLine}";
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