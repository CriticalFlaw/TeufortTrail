using System;
using System.Text;
using TeufortTrail.Entities.Location;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel
{
    [ParentWindow(typeof(Travel))]
    public sealed class ContinueTrail : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _continueTrail;
        private MarqueeBar _marqueeBar;
        private string _swayBarText;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.ContinueTrail" /> class.
        /// </summary>
        public ContinueTrail(IWindow window) : base(window)
        {
        }

        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            var game = GameCore.Instance;
            _continueTrail = new StringBuilder();
            _marqueeBar = new MarqueeBar();
            _swayBarText = _marqueeBar.Step();

            if ((game.Trail.NextLocationDistance > 0) && (game.Trail.CurrentLocation.Status == LocationStatus.Arrived))
                game.Trail.CurrentLocation.Status = LocationStatus.Departed;
        }

        public override void OnTick(bool systemTick, bool skipDay)
        {
            base.OnTick(systemTick, skipDay);
            if (systemTick) return;
            var game = GameCore.Instance;
            game.Vehicle.CheckStatus();

            switch (game.Vehicle.Status)
            {
                case VehicleStatus.Stopped:
                case VehicleStatus.Disabled:    // TODO: Let the player know they are unable to continue
                    break;

                case VehicleStatus.Moving:
                    _swayBarText = _marqueeBar.Step();
                    game.TakeTurn(false);
                    GameCore.Instance.Trail.OnTick(false, skipDay);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        public override void OnInputBufferReturned(string input)
        {
            if (!string.IsNullOrEmpty(input)) return;
            if (GameCore.Instance.Vehicle.Status == VehicleStatus.Moving)
                GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;
            ClearForm();
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            _continueTrail = new StringBuilder();
            _continueTrail.Clear();
            _continueTrail.AppendLine($"{Environment.NewLine}{_swayBarText}");
            _continueTrail.AppendLine(TravelInfo.PartyStatus);
            return _continueTrail.ToString();
        }
    }
}