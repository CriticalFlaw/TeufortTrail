using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Menu
{
    /// <summary>
    /// Displays a victory message when the player finishes the game.
    /// </summary>
    [ParentWindow(typeof(GameOver))]
    public sealed class GameOver : InputForm<GameOverInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameOver" /> class.
        /// </summary>
        public GameOver(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        /// <remarks>TODO: Display the player's total score. Expand the failure message.</remarks>
        protected override string OnDialogPrompt()
        {
            return $"{Environment.NewLine}All the members of your party have died.{Environment.NewLine}GAME OVER.{Environment.NewLine}";
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            GameCore.Instance.Destroy();
        }
    }

    public sealed class GameOverInfo : WindowData
    {
        // Nothing to see here, move along...
    }
}