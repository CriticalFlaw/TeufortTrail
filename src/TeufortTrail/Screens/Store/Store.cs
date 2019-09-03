using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Location;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.Store
{
    [ParentWindow(typeof(Travel))]
    public sealed class Store : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _store;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Store.Store" /> class.
        /// </summary>
        public Store(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // TODO: Add current time
            _store = new StringBuilder();
            _store.Clear();
            _store.AppendLine(GameCore.Instance.Trail.CurrentLocation?.Name + " -  Mann Co. Store");
            _store.AppendLine("------------------------------------------");

            // Display the available items at the store, also add the option to leave.
            var storeItems = new List<Types>(Enum.GetValues(typeof(Types)).Cast<Types>());
            for (var index = 0; index < storeItems.Count; index++)
            {
                var storeItem = storeItems[index];
                // Get the selected item, check if its of a type that can be sold at the store.
                if ((storeItem == Types.Vehicle) || (storeItem == Types.Person) || (storeItem == Types.Money) || (storeItem == Types.Location)) continue;

                // Format the price tag for every item type that is sold at the store.
                var storeTag = storeItem.ToDescriptionAttribute() + "              " + UserData.Store.Transactions[storeItem].Value.ToString("C2");
                _store.AppendLine($"  {(int)storeItem}. {storeTag}");
                if (index == storeItems.Count - 5)
                    _store.AppendLine($"  {storeItems.Count - 3}. Leave store");
            }

            // Display the player's current money balance and pending transaction cost.
            _store.AppendLine("--------------------------------");
            var totalBill = UserData.Store.TotalTransactionCost;
            var playerBalance = GameCore.Instance.Vehicle.Balance - totalBill;
            _store.Append($"Total bill:            {totalBill:C2}" + $"{Environment.NewLine}Amount you have:       {playerBalance:C2}");
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is not empty.
            if (string.IsNullOrWhiteSpace(input)) return;

            // Check that the user input is a valid enumerable.
            Enum.TryParse(input, out Types selectedItem);

            // Depending on the item category selected, proceed to confirm the purchase.
            switch (selectedItem)
            {
                case Types.Food:
                    UserData.Store.SelectedItem = Resources.Food;
                    SetForm(typeof(StorePurchase));
                    break;

                case Types.Hats:
                    UserData.Store.SelectedItem = Resources.Hats;
                    SetForm(typeof(StorePurchase));
                    break;

                case Types.Ammo:
                    UserData.Store.SelectedItem = Resources.Ammunition;
                    SetForm(typeof(StorePurchase));
                    break;

                default:
                    LeaveStore();
                    break;
            }
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            return _store.ToString();
        }

        /// <summary>
        /// Called when the player has chosen to leave the store.
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
        #region VARIABLES

        /// <summary>
        /// Defines the list of currently active transactions.
        /// </summary>
        public IDictionary<Types, Item> Transactions;

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

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Store.StoreGenerator" /> class.
        /// </summary>
        public StoreGenerator()
        {
            Transactions = new Dictionary<Types, Item>(Vehicle.DefaultInventory);
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
            foreach (var transaction in new Dictionary<Types, Item>(Transactions))
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
            Transactions = new Dictionary<Types, Item>(Vehicle.DefaultInventory);
        }
    }
}