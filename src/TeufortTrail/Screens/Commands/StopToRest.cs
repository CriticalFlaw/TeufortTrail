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
            // Increment the turn counter depending on the number of days the party has rested.
            for (var i = 0; i < UserData.DaysToRest; i++)
            {
                GameCore.Instance.TakeTurn();
                foreach (var person in GameCore.Instance.Vehicle.Passengers.Where(x => x.HealthState != HealthStatus.Dead))
                {
                    // Subtract the amount of food consumed from the party member.
                    var vehicle = GameCore.Instance.Vehicle;
                    if (vehicle.Inventory[ItemTypes.Food].Quantity > 0)
                    {
                        var foodConsumed = GameCore.Instance.Random.Next(1, 3) * vehicle.Passengers.Count(x => x.HealthState != HealthStatus.Dead);
                        vehicle.Inventory[ItemTypes.Food].SubtractQuantity(foodConsumed);
                        foodConsumedTotal += foodConsumed;
                    }

                    // Increase the party member's health.
                    if (person.HealthState != HealthStatus.Good)
                        person.Health += GameCore.Instance.Random.Next(1, 10);
                }
            }
            return $"{Environment.NewLine}Your party has rested for {UserData.DaysToRest} days.{Environment.NewLine}In that time they consumed {foodConsumedTotal} pounds of food.{Environment.NewLine}{Environment.NewLine}";
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ClearForm();
        }
    }
}