using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Events;
using WolfCurses.Core;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.River
{
    /// <summary>
    /// Displays the progress of crossing the river.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class Crossing : Form<TravelInfo>
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

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Crossing" /> class.
        /// </summary>
        public Crossing(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            // Advance the progress bar, step it to next phase.
            base.OnFormPostCreate();
            _marqueeBar = new MarqueeBar();
            _swayBarText = _marqueeBar.Step();

            // Park the vehicle if it's not already.
            GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;

            // Double check that the player can pay to cross the river, subtract the payment from the inventory.
            if ((GameCore.Instance.Vehicle.Inventory[ItemTypes.Money].TotalValue > UserData.River.FerryCost) && (UserData.River.FerryCost > 0))
            {
                GameCore.Instance.Vehicle.Inventory[ItemTypes.Money].SubtractQuantity((int)UserData.River.FerryCost);
                UserData.River.FerryCost = 0;
            }
            else if ((GameCore.Instance.Vehicle.Inventory[ItemTypes.Clothing].Quantity > UserData.River.HelpCost) && (UserData.River.HelpCost > 0))
            {
                GameCore.Instance.Vehicle.Inventory[ItemTypes.Clothing].SubtractQuantity(UserData.River.HelpCost);
                UserData.River.HelpCost = 0;
            }
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        /// <remarks>TODO: Trigger events when the player is crossing the river via ferry or help.</remarks>
        public override void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick vehicle at an interval.
            base.OnTick(systemTick, skipDay);
            if (systemTick || DoneRiverCrossing) return;

            // Advance the progress bar, step it to next phase.
            _swayBarText = _marqueeBar.Step();

            // Increment the distance the party has traveled over the river.
            RiverWidthCrossed += GameCore.Instance.Random.Next(1, UserData.River.RiverWidth / 4);

            // Check to see if we will finish crossing river before crossing more.
            if (RiverWidthCrossed >= UserData.River.RiverWidth)
            {
                RiverWidthCrossed = UserData.River.RiverWidth;
                DoneRiverCrossing = true;
                return;
            }

            // River crossing will allow ticking of people, vehicle, and other important events but others like consuming food are disabled.
            GameCore.Instance.TakeTurn(true);

            // Trigger a random event when crossing the river.
            switch (UserData.River.CrossingType)
            {
                case RiverOptions.Float:
                    if (GameCore.Instance.Random.NextBool() &&
                        !UserData.River.DisasterHappened &&
                        (RiverWidthCrossed >= UserData.River.RiverWidth / 2))
                    {
                        UserData.River.DisasterHappened = true;
                        GameCore.Instance.EventDirector.TriggerEvent(GameCore.Instance.Vehicle, typeof(VehicleFloods));
                    }
                    break;

                case RiverOptions.Ferry:
                case RiverOptions.Help:
                    break;

                case RiverOptions.None:
                    throw new InvalidOperationException($"Invalid river crossing result choice {UserData.River.CrossingType}.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Skip if we are still crossing the river.
            if (RiverWidthCrossed < UserData.River.RiverWidth) return;
            SetForm(typeof(CrossingResult));
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            // Display the river crossing progress. Wait for user input.
            var _crossing = new StringBuilder();
            _crossing.AppendLine($"Crossing the {GameCore.Instance.Trail.CurrentLocation.Name}");
            _crossing.AppendLine($"River crossed: {RiverWidthCrossed:N0} out of {UserData.River.RiverWidth:N0} feet");
            _crossing.AppendLine("------------------------------------------");
            _crossing.AppendLine($"{Environment.NewLine}  {_swayBarText}");
            if (DoneRiverCrossing) _crossing.AppendLine(InputManager.PRESSENTER);
            return _crossing.ToString();
        }
    }
}