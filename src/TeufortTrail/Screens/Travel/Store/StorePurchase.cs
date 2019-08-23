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
        private StringBuilder _purchaseText;
        private Item PurchaseText;

        public StorePurchase(IWindow window) : base(window)
        {
        }

        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            _purchaseText = new StringBuilder();
            _purchaseText.AppendLine($"{Environment.NewLine}You can afford 0 {UserData.Store.SelectedItem.Name.ToLowerInvariant()}.");
            _purchaseText.Append($"How many {UserData.Store.SelectedItem.Name.ToLowerInvariant()} to buy?");
            PurchaseText = UserData.Store.SelectedItem;
        }

        public override void OnInputBufferReturned(string input)
        {
            if (!int.TryParse(input, out var userInput)) return;

            // TODO: Purchase the items

            UserData.Store.SelectedItem = null;
            SetForm(typeof(Store));
        }

        public override string OnRenderForm()
        {
            return _purchaseText.ToString();
        }
    }
}