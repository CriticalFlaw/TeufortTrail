using System;
using System.Collections.Generic;
using WolfCurses;

namespace TeufortTrail
{
    public class GameCore : SimulationApp
    {
        public static GameCore Instance { get; private set; }
        public int TotalTurns { get; private set; }

        public override IEnumerable<Type> AllowedWindows
        {
            get
            {
                var windowList = new List<Type>
                {
                    // TODO: Add screen types
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

            // TODO: Initialize screens

            // Reset the Window Manager
            base.Restart();
        }

        protected override void OnPreDestroy()
        {
            TotalTurns = 0;
            Instance = null;
        }

        public void TakeTurn()
        {
            TotalTurns++;
        }
    }
}