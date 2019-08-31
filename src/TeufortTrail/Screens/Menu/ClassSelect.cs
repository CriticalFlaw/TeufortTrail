using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities.Person;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.MainMenu
{
    /// <summary>
    /// Allow the player to select the class they want to play.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class ClassSelect : Form<NewGameInfo>
    {
        #region VARIABLES

        private StringBuilder _classSelect;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.MainMenu.ClassSelect" /> class.
        /// </summary>
        /// <param name="window"></param>
        public ClassSelect(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            UserData.PlayerClass = Classes.Scout;
            UserData.StartingMoney = 10;
            GameCore.Instance.SetStartInfo(UserData);

            _classSelect = new StringBuilder();
            _classSelect.AppendLine($"{Environment.NewLine}Choose your class:{Environment.NewLine}");

            // Loop through all the class enumerables and grab their description for selection purposes.
            var classes = new List<Classes>(Enum.GetValues(typeof(Classes)).Cast<Classes>());
            for (var index = 0; index < classes.Count; index++)
            {
                // Get the current profession choice enumeration value we casted into list.
                var classChoice = classes[index];
                _classSelect.AppendLine($"  {(int)classChoice}. {classChoice.ToDescriptionAttribute()}");
            }
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        public override void OnInputBufferReturned(string input)
        {
            // Skip this step if the input is null or empty.
            if (string.IsNullOrWhiteSpace(input)) return;

            // Cast user selection to the enum value.
            Enum.TryParse(input, out Classes playerChoice);

            // Set starting player stats depending on the class chosen.
            switch (playerChoice)
            {
                default:
                case Classes.Scout:
                    UserData.PlayerClass = Classes.Scout;
                    break;

                case Classes.Soldier:
                    UserData.PlayerClass = Classes.Soldier;
                    break;

                case Classes.Pyro:
                    UserData.PlayerClass = Classes.Pyro;
                    break;

                case Classes.Demoman:
                    UserData.PlayerClass = Classes.Demoman;
                    break;

                case Classes.Heavy:
                    UserData.PlayerClass = Classes.Heavy;
                    break;

                case Classes.Medic:
                    UserData.PlayerClass = Classes.Medic;
                    break;

                case Classes.Engineer:
                    UserData.PlayerClass = Classes.Engineer;
                    break;

                case Classes.Sniper:
                    UserData.PlayerClass = Classes.Sniper;
                    break;

                case Classes.Spy:
                    UserData.PlayerClass = Classes.Spy;
                    break;
            }

            UserData.StartingMoney = 10;
            UserData.PartyClasses.Add(playerChoice);
            var classes = Enum.GetValues(typeof(Classes)).Cast<Classes>().ToList();
            classes.Remove(UserData.PlayerClass);
            classes = classes.OrderBy(arg => Guid.NewGuid()).Take(4).ToList();
            foreach (var _class in classes)
                UserData.PartyClasses.Add(_class);
            SetForm(typeof(StorySetup));
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            // Return a text representation of the current game window.
            return _classSelect.ToString();
        }
    }
}