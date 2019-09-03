using System;
using System.Collections.Generic;
using System.Text;
using TeufortTrail.Entities.Person;
using TeufortTrail.Entities.Trail;
using TeufortTrail.Entities.Vehicle;
using TeufortTrail.Screens.MainMenu;
using TeufortTrail.Screens.Travel;
using WolfCurses;

namespace TeufortTrail
{
    /// <summary>
    /// The core game singleton. Processes various game entities, ticks, inputs, modules, etc.
    /// </summary>
    public class GameCore : SimulationApp
    {
        #region VARIABLES

        /// <summary>
        /// Singleton instance for the entire game simulation, does not block the calling thread though only listens for commands.
        /// </summary>
        public static GameCore Instance { get; private set; }

        /// <summary>
        ///  Defines the current vehicle in which player and their party members are travelling.
        /// </summary>
        public Vehicle Vehicle { get; private set; }

        /// <summary>
        ///  Defines the complete trail and all points of interaction the player will encounter.
        /// </summary>
        public TrailBase Trail { get; private set; }

        /// <summary>
        ///  Total number of turns that have been taken.
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
                    typeof(MainMenu)
                };

                return windowList;
            }
        }

        #endregion VARIABLES

        /// <summary>
        /// Create a new instance of the game core. Checks if an instance already exists.
        /// </summary>
        public static void Create()
        {
            if (Instance != null) throw new InvalidOperationException("A game instance already exists!");
            Instance = new GameCore();
        }

        /// <summary>
        /// Called by the text user interface to display a heading before the rest of the content.
        /// </summary>
        /// <returns></returns>
        public override string OnPreRender()
        {
            // Display the current location, vehicle status and turn count.
            var _gameCore = new StringBuilder();
            _gameCore.AppendLine($"Turn: {TotalTurns:D4}");
            _gameCore.AppendLine($"Location: {Trail?.CurrentLocation?.Status}");
            _gameCore.AppendLine($"Vehicle: {Vehicle?.Status}");
            _gameCore.AppendLine("------------------------------------------");
            return _gameCore.ToString();
        }

        /// <summary>
        /// Called on the first game tick. Meant to be used to setup and load base resources.
        /// </summary>
        protected override void OnFirstTick()
        {
            // Reset the turn counter.
            TotalTurns = 0;

            // Load the necessary trail and vehicle modules.
            Trail = new TrailBase();
            Vehicle = new Vehicle();

            // Reset the Window Manager, attach the main screens.
            base.Restart();
            WindowManager.Add(typeof(Travel));
            WindowManager.Add(typeof(MainMenu));
        }

        /// <summary>
        /// Cleans up remaining resources, right before the game closes.
        /// </summary>
        protected override void OnPreDestroy()
        {
            TotalTurns = 0;
            Trail.Destroy();
            Vehicle = null;
            Trail = null;
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
        /// Clear then load in the vehicle party and starting resources.
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
    }
}