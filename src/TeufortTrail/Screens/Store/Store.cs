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
            _store = new StringBuilder();

            // TODO: Add current time
            _store.Clear();
            _store.AppendLine("--------------------------------");
            _store.AppendLine(GameCore.Instance.Trail.CurrentLocation?.Name + " Mann Co. Store");
            _store.AppendLine("--------------------------------");

            var storeItems = new List<Categories>(Enum.GetValues(typeof(Categories)).Cast<Categories>());
            for (var index = 0; index < storeItems.Count; index++)
            {
                var storeItem = storeItems[index];

                if ((storeItem == Categories.Vehicle) || (storeItem == Categories.Person) || (storeItem == Categories.Money) || (storeItem == Categories.Location)) continue;

                var storeTag = storeItem.ToDescriptionAttribute() + "              " + UserData.Store.Transactions[storeItem].Value.ToString("C2");
                _store.AppendLine($"  {(int)storeItem}. {storeTag}");
                if (index == storeItems.Count - 5)
                    _store.AppendLine($"  {storeItems.Count - 2}. Leave store");
            }

            _store.AppendLine("--------------------------------");
            var totalBill = UserData.Store.TotalTransactionCost;
            var playerBalance = GameCore.Instance.Vehicle.Balance - totalBill;

            _store.Append(GameCore.Instance.Trail.IsFirstLocation && (GameCore.Instance.Trail.CurrentLocation?.Status == LocationStatus.Unreached)
                ? $"Total bill:            {totalBill:C2}" + $"{Environment.NewLine}Amount you have:       {playerBalance:C2}"
                : $"You have {GameCore.Instance.Vehicle.Balance:C2} to spend.");
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        public override void OnInputBufferReturned(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            Enum.TryParse(input, out Categories selectedItem);

            switch (selectedItem)
            {
                case Categories.Food:
                    UserData.Store.SelectedItem = Resources.Food;
                    SetForm(typeof(StorePurchase));
                    break;

                case Categories.Hats:
                    UserData.Store.SelectedItem = Resources.Hats;
                    SetForm(typeof(StorePurchase));
                    break;

                case Categories.Weapons:
                    UserData.Store.SelectedItem = Resources.Weapons;
                    SetForm(typeof(StorePurchase));
                    break;

                case Categories.Ammo:
                    UserData.Store.SelectedItem = Resources.Ammunition;
                    SetForm(typeof(StorePurchase));
                    break;

                case Categories.Vehicle:
                    UserData.Store.SelectedItem = Resources.Parts;
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
            if (GameCore.Instance.Trail.IsFirstLocation && (GameCore.Instance.Trail.CurrentLocation?.Status == LocationStatus.Unreached))
            {
                UserData.Store.PurchaseItems();
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
        public IDictionary<Categories, Item> Transactions;

        /// <summary>
        /// Defines the store item the player has chosen to purchase.
        /// </summary>
        public Item SelectedItem { get; set; }

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Store.StoreGenerator" /> class.
        /// </summary>
        public StoreGenerator()
        {
            Transactions = new Dictionary<Categories, Item>(Vehicle.DefaultInventory);
        }

        /// <summary>
        /// Calculate the current transaction cost.
        /// </summary>
        public float TotalTransactionCost
        {
            get
            {
                float totalCost = 0;
                foreach (var item in Transactions)
                    totalCost += item.Value.Quantity * item.Value.Value;
                return totalCost;
            }
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
            foreach (var transaction in new Dictionary<Categories, Item>(Transactions))
            {
                if (!transaction.Key.Equals(item.Category))
                    continue;
                Transactions[item.Category].ResetQuantity();
                break;
            }
        }

        /// <summary>
        /// Purchase the items in the transaction. Adding them to the player inventory.
        /// </summary>
        public void PurchaseItems()
        {
            if (GameCore.Instance.Vehicle.Balance < TotalTransactionCost)
                throw new InvalidOperationException("Attempted to purchase items the player does not have enough monies for!");
            foreach (var transaction in Transactions)
                GameCore.Instance.Vehicle.PurchaseItem(transaction.Value);
            Transactions = new Dictionary<Categories, Item>(Vehicle.DefaultInventory);
        }
    }
}