using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Properties;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.MainMenu
{
    /// <summary>
    /// Displays a list of playable classes the player choose to play as.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class ClassSelect : Form<NewGameInfo>
    {
        private StringBuilder _classSelect;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassSelect" /> class.
        /// </summary>
        public ClassSelect(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            // Set the default player class and starting money.
            base.OnFormPostCreate();
            UserData.PlayerClass = Classes.Scout;
            UserData.StartingMoney = 10;
            GameCore.Instance.SetStartInfo(UserData);

            // Loop through all the class enumerables and grab their description for selection purposes.
            var classes = new List<Classes>(Enum.GetValues(typeof(Classes)).Cast<Classes>());
            _classSelect = new StringBuilder();
            _classSelect.AppendLine($"{Environment.NewLine}Choose your class:{Environment.NewLine}");
            for (var index = 0; index < classes.Count; index++)
                _classSelect.AppendLine($"  {(int)classes[index]}. {classes[index].ToDescriptionAttribute()}");
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        /// <remarks>TODO: Set player stats depending on the class chosen.</remarks>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is not empty.
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) return;

            // Cast user selection to the enumerable value.
            Enum.TryParse(input, out Classes userInput);

            // Based on the class selection, set the player class and starting money amount.
            UserData.PlayerClass = (Enum.IsDefined(typeof(Classes), userInput)) ? userInput : Classes.Scout;
            UserData.StartingMoney = (GameCore.UseSettings) ? GameCore.Settings.STARTING_MONEY : DefaultGame.Default.STARTING_MONEY;
            UserData.PartyClasses.Add(UserData.PlayerClass);

            // Add three unique, randomly selected classes to the player's party.
            var classes = Enum.GetValues(typeof(Classes)).Cast<Classes>().ToList();
            classes.Remove(UserData.PlayerClass);
            classes = classes.OrderBy(arg => Guid.NewGuid()).Take(3).ToList();
            foreach (var _class in classes)
                UserData.PartyClasses.Add(_class);

            // Go to the next screen.
            SetForm(typeof(StorySetup));
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            return _classSelect.ToString();
        }
    }
}