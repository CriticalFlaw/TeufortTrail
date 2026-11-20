namespace TeufortTrail.Entities
{
    /// <summary>
    /// Difficulty tuning values, kept in one place so the game can be rebalanced easily.
    /// </summary>
    public static class GameBalance
    {
        //------------------------------------------------------------------------------------------------- Food

        /// <summary>
        /// Pounds of food each living party member eats per day.
        /// </summary>
        public static int FoodPerPerson(RationLevel ration) => ration switch
        {
            RationLevel.Filling => 5,
            RationLevel.Meager => 3,
            _ => 2
        };

        /// <summary>
        /// Chance out of 100 per day that hunger hurts a party member.
        /// </summary>
        public static int HungerChance(RationLevel ration) => ration switch
        {
            RationLevel.Filling => 0,
            RationLevel.Meager => 8,
            _ => 30
        };

        /// <summary>
        /// Health a fed party member below Good health may recover each day.
        /// </summary>
        public static (int Min, int Max) FedHealing(RationLevel ration) => ration switch
        {
            RationLevel.Filling => (1, 6),
            RationLevel.Meager => (0, 3),
            _ => (0, 0)
        };

        //------------------------------------------------------------------------------------------------- Illness

        /// <summary>
        /// Hats each party member needs to stay warm. Fewer than this makes illness more likely.
        /// </summary>
        public const int HatsPerPerson = 2;

        /// <summary>
        /// Chance out of 1000 per day that a healthy party member falls ill.
        /// </summary>
        public static int SicknessChance(RationLevel ration, bool enoughHats)
        {
            var chance = 5;
            if (!enoughHats) chance += 30;
            if (ration == RationLevel.Meager) chance += 6;
            if (ration == RationLevel.Bare) chance += 20;
            return chance;
        }

        /// <summary>
        /// Chance out of 100 per travel day that a sick or injured party member takes damage, and how much.
        /// </summary>
        public const int SickDamageChance = 40;
        public const int SickDamageMin = 10;
        public const int SickDamageMax = 35;

        /// <summary>
        /// Chance out of 100 per day that a sick or injured party member recovers, while travelling and while resting.
        /// </summary>
        public const int RecoverChanceTravelling = 6;
        public const int RecoverChanceResting = 30;

        /// <summary>
        /// Chance out of 100 per travel day that a sick or injured party member takes a turn for the worse.
        /// </summary>
        public const int TurnForWorseChance = 5;

        /// <summary>
        /// Health lost when an illness or injury takes a turn for the worse.
        /// </summary>
        public const int TurnForWorseDamage = 130;

        /// <summary>
        /// Health recovered per day of rest, for party members below Good health.
        /// </summary>
        public const int RestHealMin = 15;
        public const int RestHealMax = 35;

        //------------------------------------------------------------------------------------------------- Trail events

        /// <summary>
        /// Chance out of 100 per travel day of each kind of random event. At most one happens per day.
        /// </summary>
        public const int WildEventChance = 7;
        public const int VehicleEventChance = 3;
        public const int AnimalEventChance = 3;

        /// <summary>
        /// Largest share of a supply that a bad event can destroy.
        /// </summary>
        public const double EventLossShare = 0.3;

        /// <summary>
        /// Ammo the party needs on hand to fight off a robot ambush without heavy injuries.
        /// </summary>
        public const int AmmoToRepelRobots = 10;

        //------------------------------------------------------------------------------------------------- Rivers

        /// <summary>
        /// Chance out of 100 that fording floods the van, on the narrowest and widest rivers.
        /// </summary>
        public const int FordFloodChanceNarrow = 10;
        public const int FordFloodChanceWide = 55;

        /// <summary>
        /// Ferry fare in dollars, and the help fee in hats.
        /// </summary>
        public const int FerryCostMin = 20;
        public const int FerryCostMax = 50;
        public const int HelpCostMin = 3;
        public const int HelpCostMax = 8;

        //------------------------------------------------------------------------------------------------- Hunting

        /// <summary>
        /// Robots that may appear in one hunting trip.
        /// </summary>
        public const int HuntRobotsMin = 2;
        public const int HuntRobotsMax = 8;

        /// <summary>
        /// Chance out of 100 that a shot misses even when it was fast enough. Snipers are better shots.
        /// </summary>
        public const int HuntMissChance = 20;
        public const int HuntMissChanceSniper = 5;

        /// <summary>
        /// Ammo used per shot.
        /// </summary>
        public const int HuntAmmoPerShotMin = 1;
        public const int HuntAmmoPerShotMax = 3;
    }
}
