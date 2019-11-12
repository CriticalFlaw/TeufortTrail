using System.Text;
using TeufortTrail.Entities;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel
{
    /// <summary>
    /// Displays the travel progress as the player moves along the trail.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class ContinueTrail : Form<TravelInfo>
    {
        /// <summary>
        /// Animated sway bar moving back and fourth, stepping at every tick.
        /// </summary>
        private MarqueeBar _marqueeBar;

        /// <summary>
        /// Stores the text related to animated sway bar, each tick of simulation steps it.
        /// </summary>
        private string _swayBarText;

        /// <summary>
        /// Determines if user input is allowed on this screen.
        /// </summary>
        public override bool InputFillsBuffer => false;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueTrail" /> class.
        /// </summary>
        public ContinueTrail(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            // Initialize the game instance and marquee bar.
            base.OnFormPostCreate();
            _marqueeBar = new MarqueeBar();
            _swayBarText = _marqueeBar.Step();

            // Mark the previous location as Departed as the party is now moving towards the next location.
            if ((GameCore.Instance.Trail.NextLocationDistance > 0) && (GameCore.Instance.Trail.CurrentLocation.Status == LocationStatus.Arrived))
                GameCore.Instance.Trail.CurrentLocation.Status = LocationStatus.Departed;
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        public override void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick vehicle at an interval.
            base.OnTick(systemTick, skipDay);
            if (systemTick) return;

            // Check the status of the vehicle at the this tick.
            GameCore.Instance.Vehicle.CheckStatus();

            // Continue on the trail if the vehicle is already in motion.
            if (GameCore.Instance.Vehicle.Status == VehicleStatus.Moving)
            {
                _swayBarText = _marqueeBar.Step();
                GameCore.Instance.TakeTurn();
                GameCore.Instance.Trail.OnTick(false, skipDay);
            }
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
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
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            // Display the marquee bar and current party status
            var _continueTrail = new StringBuilder();
            _continueTrail.AppendLine(TravelInfo.TravelStatus);
            _continueTrail.AppendLine($"  {_swayBarText}");
            _continueTrail.AppendLine(TravelInfo.PartyStatus);
            return _continueTrail.ToString();
        }
    }
}