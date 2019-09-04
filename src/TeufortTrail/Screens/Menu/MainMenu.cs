﻿using System;
using System.Collections.Generic;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Person;
using WolfCurses.Utility;
using WolfCurses.Window;

namespace TeufortTrail.Screens.MainMenu
{
    public sealed class MainMenu : Window<MainMenuCommands, NewGameInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Menu.MainMenu" /> class.
        /// </summary>
        public MainMenu(GameCore game) : base(game)
        {
        }

        /// <summary>
        /// Called when the screen has been activated.
        /// </summary>
        /// <remarks>TODO: Add a welcome message and game information</remarks>
        public override void OnWindowPostCreate()
        {
            // Initialize the main menu title and available commands.
            MenuHeader = $"{Environment.NewLine}The Teurfort Trail";
            AddCommand(PlayTheGame, MainMenuCommands.PlayTheGame);
            AddCommand(CloseTheGame, MainMenuCommands.CloseTheGame);
        }

        /// <summary>
        /// Called when the player chooses the option to play the game.
        /// </summary>
        private void PlayTheGame()
        {
            SetForm(typeof(ClassSelect));
        }

        /// <summary>
        /// Called when the player chooses the optiont to exit the game.
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
    /// References the information required to start the game simulation onto a trail path with people, classes, vehicle, money and starting inventory.
    /// </summary>
    public sealed class NewGameInfo : WindowData
    {
        #region VARIABLES

        /// <summary>
        /// Defines the current player class
        /// </summary>
        public Classes PlayerClass { get; set; }

        /// <summary>
        /// References all the unique classes currently in the party.
        /// </summary>
        public List<Classes> PartyClasses { get; set; }

        /// <summary>
        /// References all the starting items that the player has purchased from the first store.
        /// </summary>
        public List<Item> StartingInventory { get; set; }

        /// <summary>
        /// References the starting amount of money that the player has to spend on store items.
        /// </summary>
        public int StartingMoney { get; set; }

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.MainMenu.NewGameInfo" /> class.
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