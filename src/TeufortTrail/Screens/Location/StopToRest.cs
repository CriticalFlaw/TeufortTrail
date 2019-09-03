using System;
using System.Collections.Generic;
using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Location
{
    [ParentWindow(typeof(Travel))]
    public sealed class StopToRest : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _stopToRest;
        public override bool InputFillsBuffer => false;

        #endregion VARIABLES

        public StopToRest(IWindow window) : base(window)
        {

        }

        public override void OnTick(bool systemTick, bool skipDay)
        {
            base.OnTick(systemTick, skipDay);

            // Skip system ticks.
            if (systemTick) return;

            // Simulate the days to rest in time and event system, this will trigger random event game Windows if required.
            GameCore.Instance.TakeTurn(false);
        }

        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Only change the vehicle status to stopped if it is moving, it could just be stuck.
            if (GameCore.Instance.Vehicle.Status == VehicleStatus.Moving)
                GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;
        }

        public override string OnRenderForm()
        {
            _stopToRest = new StringBuilder();
            _stopToRest.AppendLine($"{Environment.NewLine}Your party has rested for one day.");
            return _stopToRest.ToString();
        }

        public override void OnInputBufferReturned(string input)
        {
            ClearForm();
        }
    }
}