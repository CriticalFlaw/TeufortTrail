using System;
using System.Text;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Store
{
    /// <summary>
    /// Displays a welcome message when the player visits the store for the first time.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
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
            var storeWelcome = new StringBuilder();
            storeWelcome.Clear();
            storeWelcome.Append($"{Environment.NewLine}Welcome to the Mann Co. Store! You fellas are heading to Teufort?");
            storeWelcome.Append($"{Environment.NewLine}We can fix you right up with these fine products.");
            storeWelcome.AppendLine($"{Environment.NewLine}Your satisfaction is guaranteed or we'll beat it into you!");
            storeWelcome.Append($"{Environment.NewLine}So what do you need?");
            storeWelcome.Append($"{Environment.NewLine} - Sandviches and other food. A team of four eats about {4 * Entities.GameBalance.FoodPerPerson(Entities.RationLevel.Filling)} pounds");
            storeWelcome.Append($"{Environment.NewLine}   a day on filling rations, and the next store is in {TravelInfo.NextStoreName}.");
            storeWelcome.Append($"{Environment.NewLine} - Hats. Everyone needs {Entities.GameBalance.HatsPerPerson} to stay warm and healthy.");
            storeWelcome.Append($"{Environment.NewLine} - Ammunition to hunt robots for cash and fight off ambushes.");
            storeWelcome.AppendLine(Environment.NewLine);
            return storeWelcome.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponseEnum response)
        {
            SetForm(typeof(Store));
        }
    }
}