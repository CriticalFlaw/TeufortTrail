using System.Text;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel
{
    [ParentWindow(typeof(Travel))]
    public sealed class ContinueTrail : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _continueTrail;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.ContinueTrail" /> class.
        /// </summary>
        public ContinueTrail(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        public override void OnInputBufferReturned(string input)
        {
            // Can only stop the simulation if it is actually running.
            if (!string.IsNullOrEmpty(input))
                return;

            // Stop ticks and close this state.
            if (GameCore.Instance.Vehicle.Status == VehicleStatus.Moving)
                GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;

            // Remove the this form.
            ClearForm();
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            _continueTrail = new StringBuilder();
            _continueTrail.Clear();
            _continueTrail.AppendLine(TravelInfo.PartyStatus);
            return _continueTrail.ToString();
        }
    }
}