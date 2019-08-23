using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.MainMenu
{
    [ParentWindow(typeof(MainMenu))]
    public sealed class ConfirmParty : InputForm<NewGameInfo>
    {
        private StringBuilder _confirmParty;
        protected override DialogType DialogType => DialogType.Prompt;

        public ConfirmParty(IWindow window) : base(window)
        {
        }

        protected override string OnDialogPrompt()
        {
            GameCore.Instance.SetStartInfo(UserData);
            _confirmParty = new StringBuilder();
            _confirmParty.AppendLine($"{Environment.NewLine}Your team ensemble:{Environment.NewLine}");
            var crewNumber = 1;

            for (var index = 0; index < UserData.PartyClasses.Count - 1; index++)
            {
                var name = UserData.PartyClasses[index];
                var isPlayer = (UserData.PartyClasses.IndexOf(name) == 0) && (crewNumber == 1);
                _confirmParty.AppendLine($"  {crewNumber} - {name} " + (isPlayer ? "(You)" : ""));
                crewNumber++;
            }
            _confirmParty.AppendLine(Environment.NewLine);
            return _confirmParty.ToString();
        }

        protected override void OnDialogResponse(DialogResponse response)
        {
            SetForm(typeof(StorySetup));
        }
    }
}