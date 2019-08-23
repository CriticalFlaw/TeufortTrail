using System.Text;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel
{
    [ParentWindow(typeof(Travel))]
    public sealed class ContinueTrail : Form<TravelInfo>
    {
        private StringBuilder _continueTrail;

        public ContinueTrail(IWindow window) : base(window)
        {
        }

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

        public override string OnRenderForm()
        {
            _continueTrail = new StringBuilder();
            _continueTrail.Clear();
            _continueTrail.AppendLine(TravelInfo.PartyStatus);
            return _continueTrail.ToString();
        }
    }
}