using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.River
{
    /// <summary>
    /// Displays information on crossing the river by asking civilians for help.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class Help : InputForm<TravelInfo>
    {
        /// <summary>
        /// Flags the player as being unable to trade resources for help crossing the river.
        /// </summary>
        private bool CannotTrade => GameCore.Instance.Vehicle.Inventory[ItemTypes.Clothing].Quantity <= UserData.River.HelpCost;

        /// <summary>
        /// Sets the kind of prompt response the player can give. Could be Yes, No or Press Any.
        /// </summary>
        protected override DialogType DialogType => (CannotTrade) ? DialogType.Prompt : DialogType.YesNo;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Help" /> class.
        /// </summary>
        public Help(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            var help = new StringBuilder();
            help.AppendLine($"{Environment.NewLine}The local folk will help you float your camper");
            help.AppendLine($"van across the river in exchange for {UserData.River.HelpCost:N0} hats.{Environment.NewLine}");
            help.Append(CannotTrade
                ? "You do not have enough hats to be helped."
                : "Do you accept this offer? Y/N");
            return help.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            switch (response)
            {
                case DialogResponse.Yes:
                    UserData.River.CrossingType = RiverOptions.Help;
                    SetForm(typeof(Crossing));
                    break;
                case DialogResponse.No:
                case DialogResponse.Custom:
                    UserData.River.CrossingType = RiverOptions.None;
                    SetForm(typeof(River));
                    break;
            }
        }
    }
}