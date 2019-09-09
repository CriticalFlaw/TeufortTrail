using System;
using System.Text;
using TeufortTrail.Entities.Location;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.River
{
    [ParentWindow(typeof(Travel))]
    public sealed class CrossingResult : InputForm<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _crossingResult;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.River.CrossingResult" /> class.
        /// </summary>
        public CrossingResult(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Depending on crossing type we will say different things about the crossing.
            _crossingResult = new StringBuilder();
            switch (UserData.River.CrossingType)
            {
                case RiverCrossChoice.Float:
                    if (UserData.River.DisasterHappened)
                        _crossingResult.AppendLine($"{Environment.NewLine}Your party relieved to reach other side after trouble floating across.{Environment.NewLine}");
                    else
                        _crossingResult.AppendLine($"{Environment.NewLine}You had no trouble floating the wagon across.{Environment.NewLine}");
                    break;

                case RiverCrossChoice.Ferry:
                    if (UserData.River.DisasterHappened)
                        _crossingResult.AppendLine($"{Environment.NewLine}The ferry operator apologizes for the rough ride.{Environment.NewLine}");
                    else
                        _crossingResult.AppendLine($"{Environment.NewLine}The ferry got your party and wagon safely across.{Environment.NewLine}");
                    break;

                case RiverCrossChoice.Help:
                    if (UserData.River.DisasterHappened)
                        _crossingResult.AppendLine($"{Environment.NewLine}The Indian runs away as soon as you reach the shore.{Environment.NewLine}");
                    else
                        _crossingResult.AppendLine($"{Environment.NewLine}The Indian helped your wagon safely across.{Environment.NewLine}");
                    break;

                case RiverCrossChoice.None:
                    throw new InvalidOperationException($"Invalid river crossing result choice {UserData.River.CrossingType}.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return _crossingResult.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            // Clear the river data and continue on the trail.
            UserData.DestroyRiver();
            GameCore.Instance.TakeTurn(false);
            SetForm(typeof(ContinueTrail));
        }
    }
}