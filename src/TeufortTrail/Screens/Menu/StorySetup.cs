using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.MainMenu
{
    /// <summary>
    /// Displays the game story and objective when starting a new game.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class StorySetup : InputForm<NewGameInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorySetup" /> class.
        /// </summary>
        public StorySetup(IWindow window) : base(window)
        {
            GameCore.Instance.SetStartInfo(UserData);
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            GameCore.Instance.SetStartInfo(UserData);

            // Generate a string listing the player's party members.
            var _partyList = new StringBuilder();
            for (var index = 1; index < UserData.PartyClasses.Count; index++)
            {
                // Slightly modify the text for the last person on the list.
                if (index == UserData.PartyClasses.Count - 1)
                    _partyList.Append($" and {UserData.PartyClasses[index]}.");
                else
                    _partyList.Append($" {UserData.PartyClasses[index]},");
            }

            // Output the game story and objective.
            var _storyHelp = new StringBuilder();
            _storyHelp.AppendLine($"{Environment.NewLine}The year is 1972. Gray Mann's robot army have captured majority");
            _storyHelp.AppendLine("of the Southwest. You and your team must travel from Dustbowl");
            _storyHelp.AppendLine("to Teufort, New Mexico to escape the mechanical menace.");
            _storyHelp.AppendLine($"{Environment.NewLine}Your team will consist of:{_partyList}");
            _storyHelp.AppendLine($"{Environment.NewLine}Before leaving Dustbowl you need to buy supplies and");
            _storyHelp.AppendLine($"from the Mann Co. Store. You have {UserData.StartingMoney:C2} in cash,");
            _storyHelp.AppendLine($"but you don't have to spend it all now.{Environment.NewLine}");
            return _storyHelp.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ParentWindow.RemoveWindowNextTick();
        }
    }
}