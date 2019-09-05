using System;
using System.Text;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.Location
{
    [ParentWindow(typeof(Travel))]
    public sealed class ChangeRation : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _changeRation;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Location.ChangeRation" /> class.
        /// </summary>
        public ChangeRation(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            _changeRation = new StringBuilder();
            _changeRation.AppendLine("Set the amount of food your party will eat each day.");
            _changeRation.AppendLine($"Your options are:{Environment.NewLine}");
            _changeRation.AppendLine("1. Filling - meals are large and generous.");
            _changeRation.AppendLine("2. Meager - meals are small, but adequate.");
            _changeRation.AppendLine("3. Bare Bones - meals are very small, everyone stays hungry.");
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        /// <remarks>Add a help option, describing the purpose of rationing.</remarks>
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
                    GameCore.Instance.Vehicle.ChangeRations(RationLevel.BareBones);
                    ClearForm();
                    break;

                default:
                    SetForm(typeof(ChangeRation));
                    break;
            }
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            return _changeRation.ToString();
        }
    }
}