using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.MainMenu
{
    [ParentWindow(typeof(MainMenu))]
    public sealed class StorySetup : InputForm<NewGameInfo>
    {
        #region VARIABLES

        private StringBuilder _storyHelp;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.MainMenu.StorySetup" /> class.
        /// </summary>
        public StorySetup(IWindow window) : base(window)
        {
            GameCore.Instance.SetStartInfo(UserData);
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            _storyHelp = new StringBuilder();
            _storyHelp.AppendLine($"{Environment.NewLine}The year is 1972. You must travel with your team");
            _storyHelp.AppendLine("from Dustbowl, Badlands to Teufort, New Mexico.");
            _storyHelp.AppendLine($"{Environment.NewLine}Before leaving Dustbowl you need to buy equipment and");
            _storyHelp.AppendLine($"supplies from the Mann Co. Store. You have {UserData.StartingMoney:C2} in cash,");
            _storyHelp.AppendLine($"but you don't have to spend it all now.{Environment.NewLine}");
            return _storyHelp.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ParentWindow.RemoveWindowNextTick();
        }
    }
}