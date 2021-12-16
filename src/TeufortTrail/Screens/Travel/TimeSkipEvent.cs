using System;
using System.Text;
using TeufortTrail.Events.Director;
using WolfCurses.Core;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel
{
    /// <summary>
    /// Skip a set amount of time given an event.
    /// </summary>
    [ParentWindow(typeof(Event))]
    public sealed class EventSkipDay : Form<EventInfo>
    {
        private readonly StringBuilder _eventSkipDay;

        /// <summary>
        /// Determines if user input is allowed on this screen.
        /// </summary>
        public override bool InputFillsBuffer => false;

        /// <summary>
        /// Determines if user input is allowed on this screen.
        /// </summary>
        public override bool AllowInput => UserData.DaysToSkip <= 0;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSkipDay" /> class.
        /// </summary>
        public EventSkipDay(IWindow window) : base(window)
        {
            _eventSkipDay = new StringBuilder();
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Only change the vehicle status to stopped if it was moving.
            if (GameCore.Instance.Vehicle.Status == Entities.VehicleStatus.Moving)
                GameCore.Instance.Vehicle.Status = Entities.VehicleStatus.Stopped;
            UpdateDaysLeft();
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        public override void OnTick(bool systemTick, bool skipDay)
        {
            base.OnTick(systemTick, skipDay);

            // Only tick at an interval or if there are days to skip.
            if (systemTick || UserData.DaysToSkip <= 0) return;

            // Decrease number of days needed to skip, increment number of days skipped.
            UserData.DaysToSkip--;
            GameCore.Instance.TakeTurn();
            UpdateDaysLeft();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Skip if there are days to skip.
            if (UserData.DaysToSkip > 0) return;

            // Return to normal after the time skip.
            UserData.DaysToSkip = 0;
            ParentWindow.RemoveWindowNextTick();
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            return _eventSkipDay.ToString();
        }

        /// <summary>
        /// Display the number of days the player has lost as a result of a given event.
        /// </summary>
        private void UpdateDaysLeft()
        {
            // Print out the event information from user data.
            _eventSkipDay.Clear();
            if (!string.IsNullOrEmpty(UserData.EventText))
                _eventSkipDay.AppendLine($"{Environment.NewLine}{UserData.EventText}");

            // Determine how many days were skipped, then show the press enter when they can actually leave.
            _eventSkipDay.AppendLine((UserData.DaysToSkip > 1) ? $"Lose {UserData.DaysToSkip} days." : "Lose 1 day.");
            if (UserData.DaysToSkip <= 0) _eventSkipDay.AppendLine($"{Environment.NewLine}{InputManager.PRESSENTER}{Environment.NewLine}");
        }
    }
}