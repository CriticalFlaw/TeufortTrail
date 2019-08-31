using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel
{
    [ParentWindow(typeof(Travel))]
    public sealed class DepartLocation : InputForm<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _departLocation;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.DepartLocation" /> class.
        /// </summary>
        public DepartLocation(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Display the next location on the trail and how far it is.
            _departLocation = new StringBuilder();
            _departLocation.AppendLine($"{Environment.NewLine}From {GameCore.Instance.Trail.CurrentLocation.Name} it is {GameCore.Instance.Trail.NextLocationDistance} miles to {GameCore.Instance.Trail.NextLocation.Name}{Environment.NewLine}");
            return _departLocation.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(ContinueTrail));
        }
    }
}