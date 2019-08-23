using System;
using System.Collections.Generic;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Person;
using WolfCurses;
using WolfCurses.Utility;
using WolfCurses.Window;

namespace TeufortTrail.Screens.MainMenu
{
    public sealed class MainMenu : Window<MainMenuCommands, NewGameInfo>
    {
        public MainMenu(SimulationApp app) : base(app)
        {
        }

        public override void OnWindowPostCreate()
        {
            MenuHeader = $"{Environment.NewLine}The Teurfort Trail";
            // TODO: Add a welcome message and game information
            AddCommand(PlayTheGame, MainMenuCommands.PlayTheGame);
            AddCommand(CloseTheGame, MainMenuCommands.CloseTheGame);
        }

        private void PlayTheGame()
        {
            SetForm(typeof(ClassSelect));
        }

        private static void CloseTheGame()
        {
            GameCore.Instance.Destroy();
        }
    }

    public enum MainMenuCommands
    {
        [Description("Play")] PlayTheGame = 1,
        [Description("Quit")] CloseTheGame = 2
    }

    public sealed class NewGameInfo : WindowData
    {
        public int PlayerIndex { get; set; }
        public Classes PlayerClass { get; set; }
        public List<Classes> PartyClasses { get; set; }
        public List<Item> StartingInventory { get; set; }
        public int StartingMoney { get; set; }

        public NewGameInfo()
        {
            PlayerIndex = 0;
            PlayerClass = Classes.Scout;
            PartyClasses = new List<Classes>();
            StartingInventory = new List<Item>();
            StartingMoney = 0;
        }
    }
}