using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.River
{
    /// <summary>
    /// Displays information on crossing the river on a ferry.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class Ferry : InputForm<TravelInfo>
    {
        /// <summary>
        /// Flags the player as being unable to pay for the ferry.
        /// </summary>
        private bool CannotAfford => (UserData.River.FerryCost >= GameCore.Instance.Vehicle.Inventory[ItemTypes.Money].TotalValue);

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
            var ferry = new StringBuilder();
            ferry.AppendLine($"{Environment.NewLine}The ferry will to put your camper van on top of a flat boat and float it across.");
            ferry.AppendLine($"The owner of the ferry will bring it across for {UserData.River.FerryCost:C2}.{Environment.NewLine}");
            ferry.Append(CannotAfford
                ? "You do not have enough monies to take the ferry."
                : "Do you accept this offer? Y/N");
            return ferry.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            switch (response)
            {
                case DialogResponse.Yes:
                {
                    // Using the ferry will cause a time skip.
                    for (var x = 0; x < 3; x++)
                        GameCore.Instance.TakeTurn();
                    GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;
                    SetForm(typeof(Crossing));
                    break;
                }
                case DialogResponse.No:
                case DialogResponse.Custom:
                    UserData.River.CrossingType = RiverOptions.None;
                    SetForm(typeof(River));
                    break;
            }
        }
    }
}