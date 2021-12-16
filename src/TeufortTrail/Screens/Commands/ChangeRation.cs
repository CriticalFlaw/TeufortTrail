using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Screens.Travel;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Commands
{
    /// <summary>
    /// Displays a prompt, asking the player to set the party's food consumption rate.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class ChangeRation : Form<TravelInfo>
    {
        private StringBuilder _changeRation;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRation" /> class.
        /// </summary>
        public ChangeRation(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            _changeRation = new StringBuilder();
            _changeRation.AppendLine();
            _changeRation.AppendLine("Set the amount of food your party will consume each day.");
            _changeRation.AppendLine($"Your options are:{Environment.NewLine}");
            _changeRation.AppendLine($"  1. {RationLevel.Filling.ToDescriptionAttribute()}");
            _changeRation.AppendLine($"  2. {RationLevel.Meager.ToDescriptionAttribute()}");
            _changeRation.Append($"  3. {RationLevel.Bare.ToDescriptionAttribute()}");
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            switch (input.ToUpperInvariant())
            {
                case "1":
                    GameCore.Instance.Vehicle.ChangeRations(RationLevel.Filling);
                    ClearForm();
                    break;

                case "2":
                    GameCore.Instance.Vehicle.ChangeRations(RationLevel.Meager);
                    ClearForm();
                    break;

                case "3":
                    GameCore.Instance.Vehicle.ChangeRations(RationLevel.Bare);
                    ClearForm();
                    break;

                default:
                    SetForm(typeof(ChangeRation));
                    break;
            }
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            return _changeRation.ToString();
        }
    }
}