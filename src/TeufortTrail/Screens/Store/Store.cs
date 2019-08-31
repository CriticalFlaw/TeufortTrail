using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Location;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.Store
{
    [ParentWindow(typeof(Travel))]
    public sealed class Store : Form<TravelInfo>
    {
        private StringBuilder _store;

        public Store(IWindow window) : base(window)
        {
        }

        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            _store = new StringBuilder();

            // TODO: Add current location and time
            _store.Clear();
            _store.AppendLine("--------------------------------");
            _store.AppendLine("Mann Co. Store");
            _store.AppendLine("--------------------------------");

            var storeItems = new List<Categories>(Enum.GetValues(typeof(Categories)).Cast<Categories>());
            for (var index = 0; index < storeItems.Count; index++)
            {
                var storeItem = storeItems[index];

                if ((storeItem == Categories.Money) || (storeItem == Categories.Person) || (storeItem == Categories.Location))
                    continue;

                _store.AppendLine($"  {(int)storeItem}. {storeItem.ToDescriptionAttribute()}");
                if (index == storeItems.Count - 4)
                    _store.AppendLine($"  {storeItems.Count - 2}. Leave store");
            }

            _store.AppendLine("--------------------------------");
        }

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

        public override string OnRenderForm()
        {
            return _store.ToString();
        }

        private void LeaveStore()
        {
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
        public Item SelectedItem { get; set; }

        public StoreGenerator()
        {
        }
    }
}