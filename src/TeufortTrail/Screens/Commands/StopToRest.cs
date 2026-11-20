using System;
using System.Linq;
using TeufortTrail.Entities;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Commands
{
    /// <summary>
    /// Displays the number of days the party has rested at the current location.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class StopToRest : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StopToRest" /> class.
        /// </summary>
        public StopToRest(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            var foodConsumedTotal = 0;
            // Rest for a few days: everyone eats, recovers health, and may shake off illness or injury.
            for (var i = 0; i < UserData.DaysToRest; i++)
            {
                GameCore.Instance.TakeTurn();
                foreach (var person in GameCore.Instance.Vehicle.Passengers.Where(x => x.HealthState != HealthStatus.Dead))
                    foodConsumedTotal += person.Rest();
            }

            var rest = new System.Text.StringBuilder();
            rest.AppendLine($"{Environment.NewLine}Your party has rested for {UserData.DaysToRest} days.");
            rest.AppendLine($"In that time they consumed {foodConsumedTotal} pounds of food.{Environment.NewLine}");
            rest.AppendLine(TravelInfo.PartyStatus);
            return rest.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponseEnum response)
        {
            ClearForm();
        }
    }
}