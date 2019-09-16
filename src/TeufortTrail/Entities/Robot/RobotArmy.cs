namespace TeufortTrail.Entities.Robot
{
    /// <summary>
    /// Defines the various robots that can be hunted by the player.
    /// </summary>
    public static class RobotArmy
    {
        public static Item.Item Scout => new Item.Item(
            category: ItemTypes.Money,
            name: "Scout",
            value: GameCore.Instance.Random.Next(50, 125));

        public static Item.Item Soldier => new Item.Item(
            category: ItemTypes.Money,
            name: "Soldier",
            value: GameCore.Instance.Random.Next(50, 200));

        public static Item.Item Pyro => new Item.Item(
            category: ItemTypes.Money,
            name: "Pyro",
            value: GameCore.Instance.Random.Next(50, 175));

        public static Item.Item Demoman => new Item.Item(
            category: ItemTypes.Money,
            name: "Demoman",
            value: GameCore.Instance.Random.Next(50, 175));

        public static Item.Item Heavy => new Item.Item(
            category: ItemTypes.Money,
            name: "Heavy",
            value: GameCore.Instance.Random.Next(50, 300));

        public static Item.Item Medic => new Item.Item(
            category: ItemTypes.Money,
            name: "Medic",
            value: GameCore.Instance.Random.Next(50, 150));

        public static Item.Item Engineer => new Item.Item(
            category: ItemTypes.Money,
            name: "Engineer",
            value: GameCore.Instance.Random.Next(50, 125));

        public static Item.Item Sniper => new Item.Item(
            category: ItemTypes.Money,
            name: "Sniper",
            value: GameCore.Instance.Random.Next(50, 125));

        public static Item.Item Spy => new Item.Item(
            category: ItemTypes.Money,
            name: "Spy",
            value: GameCore.Instance.Random.Next(50, 125));
    }
}