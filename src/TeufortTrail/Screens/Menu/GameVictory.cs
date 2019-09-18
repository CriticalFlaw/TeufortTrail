using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Menu
{
    /// <summary>
    /// Displays a victory message when the player wins the game.
    /// </summary>
    [ParentWindow(typeof(GameOver))]
    public sealed class GameVictory : InputForm<GameOverInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameVictory" /> class.
        /// </summary>
        public GameVictory(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        /// <remarks>TODO: Display the player's total score. Expand the victory message.</remarks>
        protected override string OnDialogPrompt()
        {
            return $"{Environment.NewLine}Congratulations! You have made it to Teufort!{Environment.NewLine}";
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            GameCore.Instance.Destroy();
        }
    }
}