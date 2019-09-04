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

        /// <summary>
        /// Animated sway bar that prints out as text, ping-pongs back and fourth between left and right side, moved by stepping it with tick.
        /// </summary>
        private MarqueeBar _marqueeBar;

        /// <summary>
        /// Holds the text related to animated sway bar, each tick of simulation steps it.
        /// </summary>
        private string _swayBarText;

        /// <summary>
        /// Sets the visibility of the prompt for the player to provide input.
        /// </summary>
        public override bool InputFillsBuffer => false;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.ContinueTrail" /> class.
        /// </summary>
        public ContinueTrail(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Initialize the game instance and marquee bar.
            var game = GameCore.Instance;
            _continueTrail = new StringBuilder();
            _marqueeBar = new MarqueeBar();
            _swayBarText = _marqueeBar.Step();

            // Mark the previous location as Departed as the vehicle is now moving onto the next location.
            if ((game.Trail.NextLocationDistance > 0) && (game.Trail.CurrentLocation.Status == LocationStatus.Arrived))
                game.Trail.CurrentLocation.Status = LocationStatus.Departed;
        }

        /// <summary>
        /// Called when the simulation is ticked.
        /// </summary>
        /// <remarks>TODO: Let the player know they are unable to continue</remarks>
        public override void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick vehicle at an inverval.
            base.OnTick(systemTick, skipDay);
            if (systemTick) return;
            var game = GameCore.Instance;

            // Check the status of the vehicle at the this tick.
            game.Vehicle.CheckStatus();

            switch (game.Vehicle.Status)
            {
                case VehicleStatus.Stopped:
                case VehicleStatus.Disabled:
                    break;

                case VehicleStatus.Moving:
                    // Continue on the trail if the vehicle is already in motion.
                    _swayBarText = _marqueeBar.Step();
                    game.TakeTurn();
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
            // Check that the user input is not empty.
            if (!string.IsNullOrEmpty(input)) return;

            // Stop the vehicle and clear the screen if it's already in motion.
            if (GameCore.Instance.Vehicle.Status == VehicleStatus.Moving)
                GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;
            ClearForm();
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            // Disply the marquee bar and current party status
            _continueTrail = new StringBuilder();
            _continueTrail.Clear();
            _continueTrail.AppendLine($"{Environment.NewLine}{_swayBarText}");
            _continueTrail.AppendLine(TravelInfo.TrailStatus);
            _continueTrail.AppendLine(TravelInfo.PartyStatus);
            return _continueTrail.ToString();
        }
    }
}