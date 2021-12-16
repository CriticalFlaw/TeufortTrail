using System;
using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Store
{
    /// <summary>
    /// Displays a confirmation message, asking the player how much of the item they want to buy.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class StorePurchase : Form<TravelInfo>
    {
        private StringBuilder _storePurchase;

        /// <summary>
        /// Defines the store item that the player has chosen to purchase.
        /// </summary>
        private Item _purchaseItem;

        /// <summary>
        /// Defines the limit on how much of this item the player can have.
        /// </summary>
        private int _purchaseLimit;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="StorePurchase" /> class.
        /// </summary>
        public StorePurchase(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            // Determine the current player balance and the limit on their purchase.
            base.OnFormPostCreate();
            _purchaseLimit = (int)((GameCore.Instance.Vehicle.Balance - UserData.Store.TotalTransactionCost) / UserData.Store.SelectedItem.Value);

            // Check that the limit is within range.
            if (_purchaseLimit < 0) _purchaseLimit = 0;
            if (_purchaseLimit > UserData.Store.SelectedItem.MaxQuantity)
                _purchaseLimit = UserData.Store.SelectedItem.MaxQuantity;

            // Display a message indicating how many of this item the player can buy.
            _storePurchase = new StringBuilder();
            _storePurchase.AppendLine($"{Environment.NewLine}Your maximum capacity is {_purchaseLimit} {UserData.Store.SelectedItem.Name.ToLowerInvariant()}.");
            _storePurchase.Append("How many do you want to buy?");
            _purchaseItem = UserData.Store.SelectedItem;
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is a valid integer.
            if (!int.TryParse(input, out var userInput)) return;

            // Check that the user input is not zero, within the purchasing limit, does not exceed the set maximum and can actually afford the item. If all checks pass, then add the item to the player's inventory.
            if (userInput <= 0 || userInput > _purchaseLimit || userInput > _purchaseItem.MaxQuantity || GameCore.Instance.Vehicle.Balance < _purchaseItem.TotalValue * userInput)
                UserData.Store.RemoveItem(_purchaseItem);
            else
                UserData.Store.AddItem(_purchaseItem, userInput);

            // Deselect the item and return to the store.
            UserData.Store.SelectedItem = null;
            SetForm(typeof(Store));
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            return _storePurchase.ToString();
        }
    }
}