using System;
using TeufortTrail.Entities.Robot;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.Hunt
{
    /// <summary>
    /// Displays the robot hunting mini-game.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class Hunting : Form<TravelInfo>
    {
        /// <summary>
        /// Sets the visibility of the prompt for the player to provide input.
        /// </summary>
        public override bool InputFillsBuffer => UserData.Hunt.RobotAvailable;

        /// <summary>
        /// Sets the flag that allows user input to be accepted.
        /// </summary>
        public override bool AllowInput => UserData.Hunt.RobotAvailable;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Hunting" /> class.
        /// </summary>
        public Hunting(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        public override void OnTick(bool systemTick, bool skipDay)
        {
            base.OnTick(systemTick, skipDay);

            // Tick the mini-game if the timer hasn't run out.
            if (UserData.Hunt.HuntTime > 0)
                UserData.Hunt?.OnTick(systemTick, skipDay);
            else
            {
                // Disconnect the events and display the result screen.
                UserData.Hunt.TargetFledEvent -= Hunt_TargetFledEvent;
                SetForm(typeof(HuntingResult));
            }
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Generate a new instance of the hunting mini-game.
            UserData.GenerateHunt();

            // Listen for the event where the target robot has retreated.
            UserData.Hunt.TargetFledEvent += Hunt_TargetFledEvent;
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is not empty.
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) return;

            // Check that there is a robot to shoot.
            if (!UserData.Hunt.RobotAvailable) return;

            // Check that the player inputted the word correctly.
            if (!input.Equals(UserData.Hunt.HuntWord.ToString(), StringComparison.OrdinalIgnoreCase)) return;

            // Determine if the player has shot the robot or missed the shot.
            SetForm(UserData.Hunt.DestroyRobot() ? typeof(RobotHit) : typeof(RobotMissed));
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            return UserData.Hunt.HuntInfo;
        }

        /// <summary>
        /// Calls the screen letting the player know that the target has escaped.
        /// </summary>
        private void Hunt_TargetFledEvent(Robot target)
        {
            SetForm(typeof(RobotFled));
        }
    }
}