using System;
using System.Text;
using TeufortTrail.Entities.Item;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.Store
{
    [ParentWindow(typeof(Travel))]
    public sealed class StorePurchase : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _storePurchase;

        /// <summary>
        /// Defines the store item that the player has chosen to purchase.
        /// </summary>
        private Item PurchaseItem;

        /// <summary>
        /// Defines the limit on how many of this item the player can purchase.
        /// </summary>
        private int PurchaseLimit;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Store.StorePurchase" /> class.
        /// </summary>
        public StorePurchase(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Determine the current player balance and the limit on their purchasing.
            var currentBalance = (int)(GameCore.Instance.Vehicle.Balance - UserData.Store.TotalTransactionCost);
            PurchaseLimit = (int)(currentBalance / UserData.Store.SelectedItem.Value);

            // Check that the limist are within range.
            if (PurchaseLimit < 0)
                PurchaseLimit = 0;
            if (PurchaseLimit > UserData.Store.SelectedItem.MaxQuantity)
                PurchaseLimit = UserData.Store.SelectedItem.MaxQuantity;

            // Display a message indicating how much of this item the player can afford.
            _storePurchase = new StringBuilder();
            _storePurchase.AppendLine($"{Environment.NewLine}You can afford {PurchaseLimit} {UserData.Store.SelectedItem.Name.ToLowerInvariant()}.");
            _storePurchase.Append("How many do you want to buy?");
            PurchaseItem = UserData.Store.SelectedItem;
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is a valid intenger.
            if (!int.TryParse(input, out var userInput)) return;

            // Check that the user input is not zero, within the purchasing limit, does not exceed the set maximum and can actually afford the item. If all checks pass, then add the item to the player's inventory.
            if (userInput <= 0 || userInput > PurchaseLimit || userInput > PurchaseItem.MaxQuantity || GameCore.Instance.Vehicle.Balance < PurchaseItem.TotalValue * userInput)
                UserData.Store.RemoveItem(PurchaseItem);
            else
                UserData.Store.AddItem(PurchaseItem, userInput);

            // Deselect the item and return to the store.
            UserData.Store.SelectedItem = null;
            SetForm(typeof(Store));
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            return _storePurchase.ToString();
        }
    }
}