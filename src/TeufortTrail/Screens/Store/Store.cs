using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.Store
{
    /// <summary>
    /// Displays the in-game store the player can purchase items from.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class Store : Form<TravelInfo>
    {
        private StringBuilder _store;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Store" /> class.
        /// </summary>
        public Store(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            _store = new StringBuilder();
            _store.AppendLine(GameCore.Instance.Trail.CurrentLocation?.Name + " -  Mann Co. Store");
            _store.AppendLine("------------------------------------------");

            // Display the available items at the store, add the option to leave.
            var storeItems = new List<ItemTypes>(Enum.GetValues(typeof(ItemTypes)).Cast<ItemTypes>());
            for (var index = 0; index < storeItems.Count; index++)
            {
                // Get the selected item, check if its of a type that can be sold at the store.
                var storeItem = storeItems[index];
                if (storeItem == ItemTypes.Money) continue;

                // Format the price tag for every item type that is sold at the store.
                var storeTag = storeItem.ToDescriptionAttribute() + "              " + (UserData.Store.Transactions[storeItem].Quantity * UserData.Store.Transactions[storeItem].Value).ToString("C2") + " (" + UserData.Store.Transactions[storeItem].Value.ToString("C2") + ")";
                _store.AppendLine($"  {(int)storeItem}. {storeTag}");

                // Add the option to leave the store
                if (index == storeItems.Count - 2) _store.AppendLine($"  {storeItems.Count}. Leave store");
            }

            // Display the player's current money balance and pending transaction cost.
            _store.AppendLine("------------------------------------------");
            var totalBill = UserData.Store.TotalTransactionCost;
            var playerBalance = GameCore.Instance.Vehicle.Balance - totalBill;
            _store.Append($"Total bill:            {totalBill:C2}" + $"{Environment.NewLine}Amount you have:       {playerBalance:C2}");
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is not empty.
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) return;

            // Check that the user input is a valid enumerable.
            Enum.TryParse(input, out ItemTypes userInput);

            // Depending on the item category selected, proceed to confirm the purchase.
            switch (userInput)
            {
                case ItemTypes.Food:
                    UserData.Store.SelectedItem = Resources.Food;
                    SetForm(typeof(StorePurchase));
                    break;

                case ItemTypes.Clothing:
                    UserData.Store.SelectedItem = Resources.Clothing;
                    SetForm(typeof(StorePurchase));
                    break;

                case ItemTypes.Ammo:
                    UserData.Store.SelectedItem = Resources.Ammo;
                    SetForm(typeof(StorePurchase));
                    break;

                default:
                    LeaveStore();
                    break;
            }
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            return _store.ToString();
        }

        /// <summary>
        /// Called when the player wants to leave the store.
        /// </summary>
        private void LeaveStore()
        {
            // Purchase the items in queue.
            UserData.Store.PurchaseItems();

            // Show the apropriate dialog screen if this is the first trail location.
            if (GameCore.Instance.Trail.IsFirstLocation && (GameCore.Instance.Trail.CurrentLocation?.Status == LocationStatus.Unreached))
            {
                GameCore.Instance.Trail.ArriveAtLocation();
                SetForm(typeof(LocationArrived));
            }
            else
                ClearForm();
        }
    }

    public sealed class StoreGenerator
    {
        /// <summary>
        /// Defines the list of currently active transactions.
        /// </summary>
        public IDictionary<ItemTypes, Item> Transactions;

        /// <summary>
        /// Defines the store item the player has chosen to purchase.
        /// </summary>
        public Item SelectedItem { get; set; }

        /// <summary>
        /// Calculate the current transaction cost.
        /// </summary>
        public float TotalTransactionCost
        {
            get
            {
                // Loop through all the transactions and multiply the quantity by value.
                float totalCost = 0;
                foreach (var item in Transactions)
                    totalCost += item.Value.Quantity * item.Value.Value;
                return totalCost;
            }
        }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreGenerator" /> class.
        /// </summary>
        public StoreGenerator()
        {
            Transactions = new Dictionary<ItemTypes, Item>(Vehicle.DefaultInventory);
        }

        /// <summary>
        /// Add the item to the on-going transaction.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void AddItem(Item item, int amount)
        {
            Transactions[item.Category] = new Item(item, amount);
        }

        /// <summary>
        /// Remove the item from the on-going transaction.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            // Loop through ever transaction
            foreach (var transaction in new Dictionary<ItemTypes, Item>(Transactions))
            {
                // Reset the quantity value of the item, removing it from the player's inventory.
                if (!transaction.Key.Equals(item.Category)) continue;
                Transactions[item.Category].ResetQuantity();
                break;
            }
        }

        /// <summary>
        /// Purchase the items in the transaction. Adding them to the player inventory.
        /// </summary>
        public void PurchaseItems()
        {
            // Check that the player can afford the items they want to purhcase.
            if (GameCore.Instance.Vehicle.Balance < TotalTransactionCost)
                throw new InvalidOperationException("Attempted to purchase items the player does not have enough monies for!");

            // Process each purchased item in the transaction.
            foreach (var transaction in Transactions)
                GameCore.Instance.Vehicle.PurchaseItem(transaction.Value);
            Transactions = new Dictionary<ItemTypes, Item>(Vehicle.DefaultInventory);
        }
    }
}