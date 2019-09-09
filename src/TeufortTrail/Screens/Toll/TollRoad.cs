using System;
using System.Text;
using TeufortTrail.Entities.Location;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Toll
{
    [ParentWindow(typeof(Travel))]
    public sealed class TollRoad : InputForm<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _tollRoad;
        private bool CannotAfford => (GameCore.Instance.Vehicle.Inventory[Entities.Item.Types.Money].TotalValue <= UserData.Toll.Cost) ? true : false;
        protected override DialogType DialogType => (CannotAfford) ? DialogType.Prompt : DialogType.YesNo;
        public override bool InputFillsBuffer => !CannotAfford;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Toll.TollRoad" /> class.
        /// </summary>
        public TollRoad(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            _tollRoad = new StringBuilder();
            var game = GameCore.Instance;

            // First portion of the message changes based on varying conditions.
            _tollRoad.Append($"{Environment.NewLine}You must pay the toll {UserData.Toll.Cost:C0} to travel to ");
            if (UserData.Toll.Road != null)
                _tollRoad.AppendLine($"{UserData.Toll.Road.Name}.");
            else if (game.Trail.CurrentLocation != null)
                _tollRoad.AppendLine($"{game.Trail.CurrentLocation.Name}.");
            else if (game.Trail.NextLocation != null)
                _tollRoad.AppendLine($"{game.Trail.NextLocation.Name}.");
            else
                _tollRoad.AppendLine($"the indefinable road.");

            // Check if the player has enough money to pay for the toll road.
            if (game.Vehicle.Inventory[Entities.Item.Types.Money].TotalValue >= UserData.Toll.Cost)
                _tollRoad.AppendLine($"{Environment.NewLine}Are you willing to do this? Y/N");
            else
                _tollRoad.AppendLine($"{Environment.NewLine}You don't have enough cash for the toll road.");
            return _tollRoad.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Check if the player has enough monies to pay for the toll road.
            if (!CannotAfford)
            {
                SetForm(typeof(ForkRoad));
                return;
            }

            // Depending on player response we will subtract money or continue on trail.
            switch (reponse)
            {
                case DialogResponse.Yes:
                    // Remove monies for the cost of the trip on toll road.
                    GameCore.Instance.Vehicle.Inventory[Entities.Item.Types.Money].SubtractQuantity(UserData.Toll.Cost);

                    // Only insert the location if there is one to actually insert.
                    //if (UserData.Toll.Road != null) GameCore.Instance.Trail.InsertLocation(UserData.Toll.Road);

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
                    throw new ArgumentOutOfRangeException(nameof(reponse), reponse, null);
            }
        }
    }

    public sealed class TollGenerator
    {
        #region VARIABLES

        public TollInRoad Road { get; }
        public int Cost { get; }

        #endregion VARIABLES

        public TollGenerator(TollInRoad tollRoad)
        {
            Road = tollRoad;
            Cost = GameCore.Instance.Random.Next(5, 15);
        }
    }
}