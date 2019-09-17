using System;
using System.Text;
using TeufortTrail.Entities;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.River
{
    /// <summary>
    /// Displays information on crossing the river on a ferry.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class Ferry : InputForm<TravelInfo>
    {
        /// <summary>
        /// Flags the player as being unable to pay for the ferry.
        /// </summary>
        private bool CannotAfford => (UserData.River.FerryCost >= GameCore.Instance.Vehicle.Inventory[ItemTypes.Money].TotalValue) ? true : false;

        /// <summary>
        /// Sets the kind of prompt response the player can give. Could be Yes, No or Press Any.
        /// </summary>
        protected override DialogType DialogType => (CannotAfford) ? DialogType.Prompt : DialogType.YesNo;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Ferry" /> class.
        /// </summary>
        public Ferry(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            var _ferry = new StringBuilder();
            _ferry.AppendLine($"{Environment.NewLine}To use a ferry means to put your camper van on");
            _ferry.AppendLine($"on top of a flat boat and float it across. The");
            _ferry.AppendLine($"owner of the ferry will bring it across for {UserData.River.FerryCost:C2}.{Environment.NewLine}");
            _ferry.AppendLine(CannotAfford
                ? $"You do not have enough monies to take the ferry.{Environment.NewLine}"
                : "Do you accept this offer? Y/N");
            return _ferry.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            if (response == DialogResponse.Yes)
            {
                // Using the ferry will cause a time skip.
                for (var x = 0; x < 3; x++)
                    GameCore.Instance.TakeTurn(false);
                GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;
                SetForm(typeof(Crossing));
            }
            else if (response == DialogResponse.No || response == DialogResponse.Custom)
            {
                UserData.River.CrossingType = RiverOptions.None;
                SetForm(typeof(River));
            }
        }
    }
}