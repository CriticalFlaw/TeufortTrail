using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Entities.Location;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Toll
{
    /// <summary>
    /// Displays the toll road and the amount the player needs to pay to pass.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class TollRoad : InputForm<TravelInfo>
    {
        /// <summary>
        /// Flags the player as being able to pay the toll.
        /// </summary>
        private bool CannotAfford => (GameCore.Instance.Vehicle.Inventory[ItemTypes.Money].TotalValue <= UserData.Toll.Cost) ? true : false;

        /// <summary>
        /// Sets the kind of prompt response the player can give. Could be Yes, No or Press Any.
        /// </summary>
        protected override DialogType DialogType => (CannotAfford) ? DialogType.Prompt : DialogType.YesNo;

        /// <summary>
        /// Determines if user input is allowed on this screen.
        /// </summary>
        public override bool InputFillsBuffer => !CannotAfford;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="TollRoad" /> class.
        /// </summary>
        public TollRoad(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Display the toll payment prompt message to the player.
            var _tollRoad = new StringBuilder();
            _tollRoad.Append($"{Environment.NewLine}You must pay the toll {UserData.Toll.Cost:C0} to travel to ");
            if (UserData.Toll.Road != null)
                _tollRoad.AppendLine($"{UserData.Toll.Road.Name}.");
            else if (GameCore.Instance.Trail.CurrentLocation != null)
                _tollRoad.AppendLine($"{GameCore.Instance.Trail.CurrentLocation.Name}.");
            else if (GameCore.Instance.Trail.NextLocation != null)
                _tollRoad.AppendLine($"{GameCore.Instance.Trail.NextLocation.Name}.");
            else
                _tollRoad.AppendLine($"the indefinable road.");
            _tollRoad.Append(Environment.NewLine);
            // Display the prompt based on whether or not the player can afford the toll.
            _tollRoad.Append((GameCore.Instance.Vehicle.Inventory[ItemTypes.Money].TotalValue >= UserData.Toll.Cost)
                ? $"Are you willing to do this? Y/N"
                : $"You don't have enough cash for the toll road.");
            return _tollRoad.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            // Check if the player has enough monies to pay for the toll road.
            if (CannotAfford)
            {
                SetForm(typeof(ForkRoad));
                return;
            }

            // Depending on player response we will subtract money or continue on trail.
            switch (response)
            {
                case DialogResponse.Yes:
                    // Remove monies for the cost of the trip on toll road.
                    GameCore.Instance.Vehicle.Inventory[ItemTypes.Money].SubtractQuantity(UserData.Toll.Cost);

                    // Only insert the location if there is one to actually insert.
                    if (UserData.Toll.Road != null) GameCore.Instance.Trail.InsertLocation(UserData.Toll.Road);

                    // Destroy the toll road data now that we are done with it.
                    UserData.DestroyToll();

                    // Onward to the next location!
                    SetForm(typeof(ContinueTrail));
                    break;

                case DialogResponse.No:
                case DialogResponse.Custom:
                    if (GameCore.Instance.Trail.CurrentLocation is ForkInRoad)
                        SetForm(typeof(ForkRoad));
                    else
                        ClearForm();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(response), response, null);
            }
        }
    }

    /// <summary>
    /// Generates the toll location with the cost of passing.
    /// </summary>
    public sealed class TollGenerator
    {
        public TollInRoad Road { get; }
        public int Cost { get; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="TollGenerator" /> class.
        /// </summary>
        public TollGenerator(TollInRoad tollRoad)
        {
            Road = tollRoad;
            Cost = GameCore.Instance.Random.Next(5, 15);
        }
    }
}