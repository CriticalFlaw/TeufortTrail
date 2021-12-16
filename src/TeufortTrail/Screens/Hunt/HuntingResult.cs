using System;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Hunt
{
    /// <summary>
    /// Displays the results of the hunting session after it's over. The player is awarded the cash amount collected from defeated robots.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class HuntingResult : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HuntingResult" /> class.
        /// </summary>
        public HuntingResult(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            GameCore.Instance.TakeTurn();
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Display the message letting the player know they didn't collect any money.
            if (UserData.Hunt.MoneyCollected <= 0) return $"{Environment.NewLine}You were unable to destroy any robots!{Environment.NewLine}";

            // Increment the player's money balance with the money collected from defeated robots.
            GameCore.Instance.Vehicle.Inventory[Entities.ItemTypes.Money].AddQuantity(Convert.ToInt32(UserData.Hunt.MoneyCollected));

            // Display the message letting the player know how much money they've collected.
            return $"{Environment.NewLine}From the robots you've destroyed, you collected ${UserData.Hunt.MoneyCollected:N0}.{Environment.NewLine}";
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Clean up the hunting mini-game resources.
            UserData.DestroyHunt();
            ClearForm();
        }
    }
}