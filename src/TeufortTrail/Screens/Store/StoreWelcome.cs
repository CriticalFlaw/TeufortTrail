using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Store
{
    /// <summary>
    /// Displays a welcome message when the player visits the store for the first time.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class StoreWelcome : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreWelcome" /> class.
        /// </summary>
        public StoreWelcome(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Display the welcome message when the player enters the store.
            var _storeWelcome = new StringBuilder();
            _storeWelcome.Clear();
            _storeWelcome.Append($"{Environment.NewLine}Welcome to the Mann Co. Store! You fellas are heading to Teufort?");
            _storeWelcome.Append($"{Environment.NewLine}We can fix you right up with these fine products.");
            _storeWelcome.AppendLine($"{Environment.NewLine}Your satisfaction is guaranteed or we'll beat it into you!");
            _storeWelcome.Append($"{Environment.NewLine}So what do you need?");
            _storeWelcome.Append($"{Environment.NewLine} - Sandviches and other food for the trip.");
            _storeWelcome.Append($"{Environment.NewLine} - Hats and clothing for both summer and winter.");
            _storeWelcome.Append($"{Environment.NewLine} - Ammunition to fight off the robots.");
            _storeWelcome.AppendLine(Environment.NewLine);
            return _storeWelcome.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            SetForm(typeof(Store));
        }
    }
}