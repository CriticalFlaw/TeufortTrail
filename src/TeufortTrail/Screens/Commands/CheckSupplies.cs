using System;
using System.Text;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Commands
{
    /// <summary>
    /// Displays the party's current resource supply and individual member status.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class CheckSupplies : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckSupplies" /> class.
        /// </summary>
        public CheckSupplies(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Increment the turn counter without advancing time.
            GameCore.Instance.TakeTurn(true);

            // Generate a table of resources and party status.
            var checkSupplies = new StringBuilder();
            checkSupplies.AppendLine($"{Environment.NewLine} Your Party:");
            checkSupplies.AppendLine($"{Environment.NewLine}{TravelInfo.PartyStatus}");
            checkSupplies.AppendLine($"{Environment.NewLine} Your Supplies:");
            checkSupplies.AppendLine($"{Environment.NewLine}{TravelInfo.PartySupplies}");
            return checkSupplies.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ClearForm();
        }
    }
}