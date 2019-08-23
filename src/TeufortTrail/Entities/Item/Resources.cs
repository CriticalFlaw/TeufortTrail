namespace TeufortTrail.Entities.Item
{
    public static class Resources
    {
        public static Item Food => new Item(
            category: Categories.Food,
            name: "Food",
            value: 0.20f,
            weight: 1,
            points: 1,
            minQuantity: 1,
            maxQuantity: 2000,
            startingQuantity: 0);

        public static Item Hats => new Item(
            category: Categories.Hats,
            name: "Hats",
            value: 10,
            weight: 1,
            points: 2,
            minQuantity: 1,
            maxQuantity: 50,
            startingQuantity: 0);

        public static Item Weapons => new Item(
            category: Categories.Weapons,
            name: "Weapons",
            value: 3,
            weight: 1,
            points: 1,
            minQuantity: 1,
            maxQuantity: 10,
            startingQuantity: 4);

        public static Item Ammunition => new Item(
            category: Categories.Ammo,
            name: "Ammunition",
            value: 2,
            weight: 0,
            points: 1,
            minQuantity: 20,
            maxQuantity: 99,
            startingQuantity: 0);

        public static Item Parts => new Item(
            category: Categories.Vehicle,
            name: "Parts",
            value: 50,
            weight: 500,
            points: 50,
            minQuantity: 1,
            maxQuantity: 2000,
            startingQuantity: 0);

        public static Item Person => new Item(
            category: Categories.Person,
            name: "Person",
            value: 0,
            weight: 2000,
            points: 800,
            minQuantity: 1,
            maxQuantity: 2000,
            startingQuantity: 0);

        public static Item Money => new Item(
            category: Categories.Money,
            name: "Money",
            value: 1,
            weight: 0,
            points: 1,
            minQuantity: 1,
            maxQuantity: int.MaxValue,
            startingQuantity: 0);
    }
}