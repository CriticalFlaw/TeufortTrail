using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Menu
{
    [ParentWindow(typeof(GameOver))]
    public sealed class GameOver : InputForm<GameOverInfo>
    {
        #region VARIABLES

        private StringBuilder _gameOver;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Menu.GameOver" /> class.
        /// </summary>
        public GameOver(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            _gameOver = new StringBuilder();
            _gameOver.AppendLine($"{Environment.NewLine}Congratulations! You have made it to Teufort!");
            return _gameOver.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
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