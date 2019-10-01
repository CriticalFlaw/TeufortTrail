using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TeufortTrail.Entities.Person;
using TeufortTrail.Entities.Trail;
using TeufortTrail.Entities.Vehicle;
using TeufortTrail.Events.Director;
using TeufortTrail.Screens.MainMenu;
using TeufortTrail.Screens.Menu;
using TeufortTrail.Screens.Travel;
using WolfCurses;

namespace TeufortTrail
{
    /// <summary>
    /// Core game singleton, responsible for processing game entities, ticks, events, etc.
    /// </summary>
    public class GameCore : SimulationApp
    {
        /// <summary>
        /// Singleton instance for the game simulation, listens for commands.
        /// </summary>
        public static GameCore Instance { get; private set; }

        /// <summary>
        /// Defines the current vehicle in which party is traveling.
        /// </summary>
        public Vehicle Vehicle { get; private set; }

        /// <summary>
        /// Defines the event director used for handling game events.
        /// </summary>
        public EventDirector EventDirector { get; private set; }

        /// <summary>
        /// Defines the trail and all points of interaction the player will encounter.
        /// </summary>
        public TrailModule Trail { get; private set; }

        /// <summary>
        /// Total number of turns that have been taken. Used as a count of days passed.
        /// </summary>
        public int TotalTurns { get; private set; }

        /// <summary>
        /// Determines the screen types that will be loaded and used by the window manager.
        /// </summary>
        public override IEnumerable<Type> AllowedWindows
        {
            get
            {
                var windowList = new List<Type>
                {
                    typeof(Travel),
                    typeof(MainMenu),
                    typeof(Event),
                    typeof(GameOver)
                };

                return windowList;
            }
        }

        /// <summary>
        /// Game settings loaded from the JSON file.
        /// </summary>
        public static GameSettings Settings { get; set; } = new GameSettings();

        /// <summary>
        /// Flags the game as using the loaded settings instead of the defaults.
        /// </summary>
        public static bool UseSettings { get; private set; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Create a new instance of the game singleton.
        /// </summary>
        public static void Create()
        {
            // Checks if an instance already exists.
            if (Instance != null) throw new InvalidOperationException("A game instance already exists!");
            Instance = new GameCore();
            LoadGameSettings();
        }

        /// <summary>
        /// Called by the text user interface to display a heading before the rest of the content.
        /// </summary>
        public override string OnPreRender()
        {
            // Display the current location, vehicle status and turn count.
            var _gameCore = new StringBuilder();
            _gameCore.AppendLine($"Day: {TotalTurns:D4}");
            _gameCore.AppendLine($"Location: {Trail?.CurrentLocation?.Status}");
            _gameCore.AppendLine($"Vehicle:  {Vehicle?.Status}");
            _gameCore.AppendLine("------------------------------------------");
            return _gameCore.ToString();
        }

        /// <summary>
        /// Called on the first game tick. Used to setup and load base resources.
        /// </summary>
        protected override void OnFirstTick()
        {
            // Reset the turn counter.
            TotalTurns = 0;

            // Load the required modules.
            Trail = new TrailModule();
            EventDirector = new EventDirector();
            Vehicle = new Vehicle();

            // Reset the Window Manager, attach the main screens.
            base.Restart();
            WindowManager.Add(typeof(Travel));
            WindowManager.Add(typeof(MainMenu));
        }

        /// <summary>
        /// Clean up remaining resources, right before the game closes.
        /// </summary>
        protected override void OnPreDestroy()
        {
            TotalTurns = 0;
            Trail.Destroy();
            EventDirector.Destroy();
            Vehicle = new Vehicle();
            Vehicle = null;
            Trail = null;
            EventDirector = null;
            Instance = null;
        }

        /// <summary>
        /// Increment the turn counter.
        /// </summary>
        public void TakeTurn(bool skipDay = false)
        {
            if (!skipDay) TotalTurns++;
        }

        /// <summary>
        /// Load in the vehicle party and starting resources.
        /// </summary>
        /// <param name="startInfo"></param>
        internal void SetStartInfo(NewGameInfo startInfo)
        {
            var crewNumber = 1;
            Vehicle.ResetVehicle(startInfo.StartingMoney);
            foreach (var _class in startInfo.PartyClasses)
            {
                Vehicle.AddPerson(new Person(_class, ((startInfo.PartyClasses.IndexOf(_class) == 0) && (crewNumber == 1)) ? true : false));
                crewNumber++;
            }
        }

        public static void LoadGameSettings()
        {
            // Check that the game settings file exists.
            if (!File.Exists("settings.json")) return;

            // Load the settings by reading the JSON file.
            var json = new StreamReader(File.OpenRead("Settings.json"), new UTF8Encoding(false)).ReadToEnd();
            Settings = JsonConvert.DeserializeObject<GameSettings>(json);

            // Check that the loading settings are valid
            UseSettings = (Settings.STARTING_MONEY == 0) ? false : true;
        }
    }
}