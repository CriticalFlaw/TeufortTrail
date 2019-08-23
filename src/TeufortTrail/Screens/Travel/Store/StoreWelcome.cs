﻿using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Store
{
    [ParentWindow(typeof(Travel))]
    public sealed class StoreWelcome : InputForm<TravelInfo>
    {
        private StringBuilder _storeWelcome;

        public StoreWelcome(IWindow window) : base(window)
        {
        }

        protected override string OnDialogPrompt()
        {
            _storeWelcome = new StringBuilder();
            _storeWelcome.Clear();
            _storeWelcome.Append($"{Environment.NewLine}Welcome to the Mann Co. Store! You fellas are heading to Teufort?");
            _storeWelcome.Append($"{Environment.NewLine}We can fix you right up with these fine products.");
            _storeWelcome.Append($"{Environment.NewLine}Your satisfaction is guaranteed or we'll beat it into you");
            _storeWelcome.Append($"{Environment.NewLine}So what do you need?");
            _storeWelcome.Append($"{Environment.NewLine} - Spare parts to keep your camper van going.");
            _storeWelcome.Append($"{Environment.NewLine} - Hats and clothing for both summer and winter.");
            _storeWelcome.Append($"{Environment.NewLine} - Plenty of food for the trip.");
            _storeWelcome.Append($"{Environment.NewLine} - Weapons and ammunition.");
            _storeWelcome.AppendLine(Environment.NewLine);
            return _storeWelcome.ToString();
        }

        protected override void OnDialogResponse(DialogResponse response)
        {
            SetForm(typeof(Store));
        }
    }
}