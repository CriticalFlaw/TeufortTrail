using System;
using System.Collections.Generic;
using TeufortTrail.Entities;
using TeufortTrail.Entities.Item;
using WolfCurses.Utility;
using WolfCurses.Window;

namespace TeufortTrail.Screens.MainMenu
{
    /// <summary>
    /// Displays the main menu when the game is launched.
    /// </summary>
    public sealed class MainMenu : Window<MainMenuCommands, NewGameInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu" /> class.
        /// </summary>
        public MainMenu(GameCore game) : base(game)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        /// <remarks>TODO: Add a welcome message and game information</remarks>
        public override void OnWindowPostCreate()
        {
            // Initialize the main menu title and available commands.
            MenuHeader = $"{Environment.NewLine}The Teufort Trail";
            AddCommand(PlayTheGame, MainMenuCommands.PlayTheGame);
            AddCommand(CloseTheGame, MainMenuCommands.CloseTheGame);
        }

        /// <summary>
        /// Called when the player selects the option to play the game.
        /// </summary>
        private void PlayTheGame()
        {
            SetForm(typeof(ClassSelect));
        }

        /// <summary>
        /// Called when the player selects the option to exit the game.
        /// </summary>
        private static void CloseTheGame()
        {
            GameCore.Instance.Destroy();
        }
    }

    /// <summary>
    /// Defines the main menu options available to the player.
    /// </summary>
    public enum MainMenuCommands
    {
        [Description("Begin the Trail")] PlayTheGame = 1,
        [Description("Quit")] CloseTheGame = 2
    }

    /// <summary>
    /// References the information required to start the game simulation with party, inventory and money resources.
    /// </summary>
    public sealed class NewGameInfo : WindowData
    {
        /// <summary>
        /// Defines the player's current class.
        /// </summary>
        public Classes PlayerClass { get; set; }

        /// <summary>
        /// References all the classes currently in the party.
        /// </summary>
        public List<Classes> PartyClasses { get; set; }

        /// <summary>
        /// Starting items that the player has purchased in the first store.
        /// </summary>
        public List<Item> StartingInventory { get; set; }

        /// <summary>
        /// Starting amount of money that the player will start the game with.
        /// </summary>
        public int StartingMoney { get; set; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="NewGameInfo" /> class.
        /// </summary>
        public NewGameInfo()
        {
            PlayerClass = Classes.Scout;
            PartyClasses = new List<Classes>();
            StartingInventory = new List<Item>();
            StartingMoney = 0;
        }
    }
}