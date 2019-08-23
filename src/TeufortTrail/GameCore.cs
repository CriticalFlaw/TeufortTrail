using System;
using System.Collections.Generic;
using TeufortTrail.Entities.Person;
using TeufortTrail.Entities.Vehicle;
using TeufortTrail.Screens.MainMenu;
using WolfCurses;

namespace TeufortTrail
{
    public class GameCore : SimulationApp
    {
        public static GameCore Instance { get; private set; }
        public Vehicle Vehicle { get; private set; }
        public int TotalTurns { get; private set; }

        public override IEnumerable<Type> AllowedWindows
        {
            get
            {
                var windowList = new List<Type>
                {
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
            return $"Turns: {TotalTurns:D4}\nLocation: UNKNOWN";
        }

        protected override void OnFirstTick()
        {
            TotalTurns = 0;
            Vehicle = new Vehicle();
            base.Restart();
            WindowManager.Add(typeof(MainMenu));
        }

        protected override void OnPreDestroy()
        {
            TotalTurns = 0;
            Vehicle = null;
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