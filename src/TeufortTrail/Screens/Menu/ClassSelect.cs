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

            // Set the default player class and starting money.
            UserData.PlayerClass = Classes.Scout;
            UserData.StartingMoney = 10;
            GameCore.Instance.SetStartInfo(UserData);

            // Loop through all the class enumerables and grab their description for selection purposes.
            _classSelect = new StringBuilder();
            _classSelect.AppendLine($"{Environment.NewLine}Choose your class:{Environment.NewLine}");
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

            // Cast user selection to the enumerable value.
            Enum.TryParse(input, out Classes playerChoice);

            // Set the player class based on the selection
            UserData.PlayerClass = playerChoice;

            // TODO: Set player stats depending on the class chosen.

            // Set the player's starting money, add them to the party.
            UserData.StartingMoney = 1000;
            UserData.PartyClasses.Add(playerChoice);

            // Add three randomly selected, unique people to the player's party.
            var classes = Enum.GetValues(typeof(Classes)).Cast<Classes>().ToList();
            classes.Remove(UserData.PlayerClass);
            classes = classes.OrderBy(arg => Guid.NewGuid()).Take(4).ToList();
            foreach (var _class in classes)
                UserData.PartyClasses.Add(_class);

            // Change to the next screen.
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