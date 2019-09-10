using System;
using System.Text;
using TeufortTrail.Entities;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.River
{
    /// <summary>
    /// Displays the outcome of crossing the river.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class CrossingResult : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrossingResult" /> class.
        /// </summary>
        public CrossingResult(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Show a different message outcome from crossing the river.
            var _crossingResult = new StringBuilder();
            switch (UserData.River.CrossingType)
            {
                case RiverOptions.Float:
                    _crossingResult.AppendLine(UserData.River.DisasterHappened
                        ? $"{Environment.NewLine}Your party relieved to reach other side after trouble floating across.{Environment.NewLine}"
                        : $"{Environment.NewLine}You had no trouble floating the wagon across.{Environment.NewLine}");
                    break;

                case RiverOptions.Ferry:
                    _crossingResult.AppendLine(UserData.River.DisasterHappened
                        ? $"{Environment.NewLine}The ferry operator apologizes for the rough ride.{Environment.NewLine}"
                        : $"{Environment.NewLine}The ferry got your party and wagon safely across.{Environment.NewLine}");
                    break;

                case RiverOptions.Help:
                    _crossingResult.AppendLine(UserData.River.DisasterHappened
                        ? $"{Environment.NewLine}The Indian runs away as soon as you reach the shore.{Environment.NewLine}"
                        : $"{Environment.NewLine}The Indian helped your wagon safely across.{Environment.NewLine}");
                    break;

                case RiverOptions.None:
                    throw new InvalidOperationException($"Invalid river crossing result choice {UserData.River.CrossingType}.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return _crossingResult.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
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