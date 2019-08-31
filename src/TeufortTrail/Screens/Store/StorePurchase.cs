﻿using System;
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
            _storePurchase = new StringBuilder();
            _storePurchase.AppendLine($"{Environment.NewLine}You can afford 0 {UserData.Store.SelectedItem.Name.ToLowerInvariant()}.");
            _storePurchase.Append($"How many {UserData.Store.SelectedItem.Name.ToLowerInvariant()} to buy?");
            PurchaseItem = UserData.Store.SelectedItem;
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        public override void OnInputBufferReturned(string input)
        {
            if (!int.TryParse(input, out var userInput)) return;

            // TODO: Purchase the items

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