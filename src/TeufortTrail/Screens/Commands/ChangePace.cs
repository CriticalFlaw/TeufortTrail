using System;
using System.Text;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.Location
{
    [ParentWindow(typeof(Travel))]
    public sealed class ChangePace : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _changePace;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Location.ChangePace" /> class.
        /// </summary>
        public ChangePace(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            _changePace = new StringBuilder();
            _changePace.AppendLine("Set the pace at which your party will travel.");
            _changePace.AppendLine($"Your options are:{Environment.NewLine}");
            _changePace.AppendLine("1. Steady pace.");
            _changePace.AppendLine("2. Strenuous pace.");
            _changePace.AppendLine("3. Grueling pace.");
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        /// <remarks>Add a help option, describing the purpose of pacing.</remarks>
        public override void OnInputBufferReturned(string input)
        {
            switch (input.ToUpperInvariant())
            {
                case "1":
                    GameCore.Instance.Vehicle.ChangePace(TravelPace.Steady);
                    ClearForm();
                    break;

                case "2":
                    GameCore.Instance.Vehicle.ChangePace(TravelPace.Strenuous);
                    ClearForm();
                    break;

                case "3":
                    GameCore.Instance.Vehicle.ChangePace(TravelPace.Grueling);
                    ClearForm();
                    break;

                default:
                    SetForm(typeof(ChangePace));
                    break;
            }
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            return _changePace.ToString();
        }
    }
}