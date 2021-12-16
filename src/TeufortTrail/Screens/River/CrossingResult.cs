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
    /// Displays the outcome of crossing the river.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
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
            var crossingResult = new StringBuilder();
            switch (UserData.River.CrossingType)
            {
                case RiverOptions.Float:
                    crossingResult.AppendLine(UserData.River.DisasterHappened
                        ? $"{Environment.NewLine}Your party is relieved to have reached the other side after some trouble floating across.{Environment.NewLine}"
                        : $"{Environment.NewLine}You had no trouble floating the camper van across the river.{Environment.NewLine}");
                    break;

                case RiverOptions.Ferry:
                    crossingResult.AppendLine(UserData.River.DisasterHappened
                        ? $"{Environment.NewLine}The ferry operator apologizes for the rough ride.{Environment.NewLine}"
                        : $"{Environment.NewLine}The ferry got your party and camper van safely across the river.{Environment.NewLine}");
                    break;

                case RiverOptions.Help:
                    crossingResult.AppendLine(UserData.River.DisasterHappened
                        ? $"{Environment.NewLine}The civilians bid you adieu as soon as you reach the shore.{Environment.NewLine}"
                        : $"{Environment.NewLine}The civilians helped your party and camper van safely cross the river.{Environment.NewLine}");
                    break;

                case RiverOptions.None:
                    throw new InvalidOperationException($"Invalid river crossing result choice {UserData.River.CrossingType}.");

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return crossingResult.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            // Clear the river data and continue on the trail.
            UserData.DestroyRiver();
            GameCore.Instance.TakeTurn();
            SetForm(typeof(ContinueTrail));
        }
    }
}