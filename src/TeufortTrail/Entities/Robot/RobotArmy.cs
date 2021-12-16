namespace TeufortTrail.Entities.Robot
{
    /// <summary>
    /// Defines the various robots that can be hunted by the player.
    /// </summary>
    public static class RobotArmy
    {
        public static Item.Item Scout => new(
            category: ItemTypes.Money,
            name: "Scout",
            value: GameCore.Instance.Random.Next(50, 125));

        public static Item.Item Soldier => new(
            category: ItemTypes.Money,
            name: "Soldier",
            value: GameCore.Instance.Random.Next(50, 200));

        public static Item.Item Pyro => new(
            category: ItemTypes.Money,
            name: "Pyro",
            value: GameCore.Instance.Random.Next(50, 175));

        public static Item.Item Demoman => new(
            category: ItemTypes.Money,
            name: "Demoman",
            value: GameCore.Instance.Random.Next(50, 175));

        public static Item.Item Heavy => new(
            category: ItemTypes.Money,
            name: "Heavy",
            value: GameCore.Instance.Random.Next(50, 300));

        public static Item.Item Medic => new(
            category: ItemTypes.Money,
            name: "Medic",
            value: GameCore.Instance.Random.Next(50, 150));

        public static Item.Item Engineer => new(
            category: ItemTypes.Money,
            name: "Engineer",
            value: GameCore.Instance.Random.Next(50, 125));

        public static Item.Item Sniper => new(
            category: ItemTypes.Money,
            name: "Sniper",
            value: GameCore.Instance.Random.Next(50, 125));

        public static Item.Item Spy => new(
            category: ItemTypes.Money,
            name: "Spy",
            value: GameCore.Instance.Random.Next(50, 125));
    }
}