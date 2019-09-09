using System;
using System.Text;
using TeufortTrail.Entities.Location;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Core;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.River
{
    [ParentWindow(typeof(Travel))]
    public sealed class Crossing : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _crossing;

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

        /// <summary>
        /// Flags the current river crossing attempt as completed.
        /// </summary>
        public bool DoneRiverCrossing;

        /// <summary>
        /// Total distance that has crossed by the player on the river.
        /// </summary>
        private int RiverWidthCrossed;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.River.Crossing" /> class.
        /// </summary>
        public Crossing(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            // Initialize the game instance and marquee bar.
            base.OnFormPostCreate();
            var game = GameCore.Instance;
            _crossing = new StringBuilder();
            _marqueeBar = new MarqueeBar();

            // Advance the progress bar, step it to next phase.
            _swayBarText = _marqueeBar.Step();

            // Park the vehicle if it is not somehow by now.
            game.Vehicle.Status = VehicleStatus.Stopped;

            // Double check that the player can pay the toll, then subtract it from the player's total.
            if ((game.Vehicle.Inventory[Entities.Item.Types.Money].TotalValue > UserData.River.FerryCost) && (UserData.River.FerryCost > 0))
            {
                game.Vehicle.Inventory[Entities.Item.Types.Money].SubtractQuantity((int)UserData.River.FerryCost);
                UserData.River.FerryCost = 0;
            }

            if ((game.Vehicle.Inventory[Entities.Item.Types.Hats].Quantity > UserData.River.HelpCost) && (UserData.River.HelpCost > 0))
            {
                game.Vehicle.Inventory[Entities.Item.Types.Hats].SubtractQuantity(UserData.River.HelpCost);
                UserData.River.HelpCost = 0;
            }
        }

        /// <summary>
        /// Called when the simulation is ticked.
        /// </summary>
        /// <remarks>TODO: Trigger events when the player is crossing the river.</remarks>
        public override void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick vehicle at an inverval.
            base.OnTick(systemTick, skipDay);
            if (systemTick || DoneRiverCrossing) return;
            var game = GameCore.Instance;

            // Advance the progress bar, step it to next phase.
            _swayBarText = _marqueeBar.Step();

            // Increment the amount we have floated over the river.
            RiverWidthCrossed += game.Random.Next(1, UserData.River.RiverWidth / 4);

            // Check to see if we will finish crossing river before crossing more.
            if (RiverWidthCrossed >= UserData.River.RiverWidth)
            {
                RiverWidthCrossed = UserData.River.RiverWidth;
                DoneRiverCrossing = true;
                return;
            }

            // River crossing will allow ticking of people, vehicle, and other important events but others like consuming food are disabled.
            GameCore.Instance.TakeTurn(true);

            // Attempt to throw a random event related to some failure happening with river crossing.
            switch (UserData.River.CrossingType)
            {
                case RiverCrossChoice.Float:
                case RiverCrossChoice.Ferry:
                case RiverCrossChoice.Help:
                    break;

                case RiverCrossChoice.None:
                    throw new InvalidOperationException($"Invalid river crossing result choice {UserData.River.CrossingType}.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Skip if we are still crossing the river.
            if (RiverWidthCrossed < UserData.River.RiverWidth)
                return;
            SetForm(typeof(CrossingResult));
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            // Show the total river crossing progress.
            _crossing.Clear();
            _crossing.AppendLine($"{Environment.NewLine}{_swayBarText}");
            _crossing.AppendLine("------------------------------------------");
            _crossing.AppendLine($"Crossing the {GameCore.Instance.Trail.CurrentLocation.Name}");
            _crossing.AppendLine($"River crossed: {RiverWidthCrossed:N0}/{UserData.River.RiverWidth:N0} feet");
            _crossing.AppendLine("------------------------------------------");

            // Wait for user input...
            if (DoneRiverCrossing)
                _crossing.AppendLine(InputManager.PRESSENTER);
            return _crossing.ToString();
        }
    }
}