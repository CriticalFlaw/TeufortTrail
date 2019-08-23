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
        private StringBuilder _storyHelp;

        public StorySetup(IWindow window) : base(window)
        {
            GameCore.Instance.SetStartInfo(UserData);
        }

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

        protected override void OnDialogResponse(DialogResponse response)
        {
            ParentWindow.RemoveWindowNextTick();
        }
    }
}