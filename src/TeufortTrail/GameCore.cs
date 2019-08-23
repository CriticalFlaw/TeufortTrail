using System;
using System.Collections.Generic;
using TeufortTrail.Entities.Person;
using TeufortTrail.Entities.Vehicle;
using TeufortTrail.Events.Trail;
using TeufortTrail.Screens.MainMenu;
using TeufortTrail.Screens.Travel;
using WolfCurses;

namespace TeufortTrail
{
    public class GameCore : SimulationApp
    {
        public static GameCore Instance { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public TrailModule Trail { get; private set; }
        public int TotalTurns { get; private set; }

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

        public static void Create()
        {
            if (Instance != null) throw new InvalidOperationException("A game instance already exists!");
            Instance = new GameCore();
        }

        public override string OnPreRender()
        {
            return $"Turns: {TotalTurns:D4}\nLocation: {Trail?.CurrentLocation?.Status}\nVehicle: {Vehicle?.Status}";
        }

        protected override void OnFirstTick()
        {
            TotalTurns = 0;

            Vehicle = new Vehicle();
            Trail = new TrailModule();

            // Reset the Window Manager
            base.Restart();

            WindowManager.Add(typeof(Travel));
            WindowManager.Add(typeof(MainMenu));
        }

        protected override void OnPreDestroy()
        {
            TotalTurns = 0;

            Trail.Destroy();

            Vehicle = null;
            Trail = null;
            Instance = null;
        }

        public void TakeTurn()
        {
            TotalTurns++;
        }

        internal void SetStartInfo(NewGameInfo startInfo)
        {
            var crewNumber = 1;
            Vehicle.ResetVehicle(startInfo.StartingMoney);
            foreach (var _class in startInfo.PartyClasses)
            {
                // First name in list is always the leader.
                var personLeader = (startInfo.PartyClasses.IndexOf(_class) == 0) && (crewNumber == 1);
                Vehicle.AddPerson(new Person(startInfo.PlayerClass, personLeader));
                crewNumber++;
            }
        }
    }
}