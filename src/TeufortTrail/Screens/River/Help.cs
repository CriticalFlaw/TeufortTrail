using System;
using System.Text;
using TeufortTrail.Entities.Location;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.River
{
    [ParentWindow(typeof(Travel))]
    public sealed class Help : InputForm<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _help;
        private bool CannotTrade => GameCore.Instance.Vehicle.Inventory[Entities.Item.Types.Hats].Quantity <= UserData.River.HelpCost;
        protected override DialogType DialogType => (CannotTrade) ? DialogType.Prompt : DialogType.YesNo;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.River.Help" /> class.
        /// </summary>
        public Help(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            _help = new StringBuilder();
            _help.AppendLine($"{Environment.NewLine}The local folk will help you float your camper");
            _help.AppendLine($"van across the river in exchange for {UserData.River.HelpCost:N0} hats.{Environment.NewLine}");

            // Change the last message depending on whether or not the player has enough required resource to pay the toll.
            if (CannotTrade)
                _help.AppendLine($"You do not have enough hats to be helped.{Environment.NewLine}");
            else
                _help.AppendLine("Do you accept this offer? Y/N");
            return _help.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            switch (response)
            {
                case DialogResponse.Yes:
                    UserData.River.CrossingType = RiverCrossChoice.Help;
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