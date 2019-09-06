using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Commands
{
    [ParentWindow(typeof(Travel))]
    public sealed class CheckSupplies : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Location.CheckSupplies" /> class.
        /// </summary>
        public CheckSupplies(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Build up representation of supplies once in constructor and then reference when asked for render.
            GameCore.Instance.TakeTurn(true);
            var _checkSupplies = new StringBuilder();
            // Return the generated tables.
            _checkSupplies.AppendLine($"{Environment.NewLine}Your Party:{Environment.NewLine}");
            _checkSupplies.AppendLine(TravelInfo.PartyStatus);
            _checkSupplies.AppendLine($"Your Supplies:{Environment.NewLine}");
            _checkSupplies.AppendLine(TravelInfo.PartySupplies);
            return _checkSupplies.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ClearForm();
        }
    }
}