using System;
using System.Text;
using TeufortTrail.Entities.Location;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.River
{
    [ParentWindow(typeof(Travel))]
    public sealed class Ferry : InputForm<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _ferry;
        private bool CannotAfford => (UserData.River.FerryCost >= GameCore.Instance.Vehicle.Inventory[Entities.Item.Types.Money].TotalValue) ? true : false;
        protected override DialogType DialogType => (CannotAfford) ? DialogType.Prompt : DialogType.YesNo;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.River.Ferry" /> class.
        /// </summary>
        public Ferry(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            _ferry = new StringBuilder();
            _ferry.AppendLine($"{Environment.NewLine}To use a ferry means to put your camper van on");
            _ferry.AppendLine($"on top of a flat boat and float it across. The");
            _ferry.AppendLine($"owner of the ferry will bring it across for {UserData.River.FerryCost:C2}.{Environment.NewLine}");

            // Change the last message depending on whether or not the player has enough required resource to pay the toll.
            if (CannotAfford)
                _ferry.AppendLine($"You do not have enough monies to take the ferry.{Environment.NewLine}");
            else
                _ferry.AppendLine("Do you accept this offer? Y/N");
            return _ferry.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        /// <remarks>TODO: Add a delay (in days) for using the ferry.</remarks>
        protected override void OnDialogResponse(DialogResponse response)
        {
            switch (response)
            {
                case DialogResponse.Yes:
                    GameCore.Instance.Vehicle.Status = Entities.Vehicle.VehicleStatus.Stopped;
                    SetForm(typeof(Crossing));
                    break;

                case DialogResponse.No:
                case DialogResponse.Custom:
                    UserData.River.CrossingType = RiverCrossChoice.None;
                    SetForm(typeof(River));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(response), response, null);
            }
        }
    }
}