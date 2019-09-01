namespace TeufortTrail.Entities.Item
{
    /// <summary>
    /// Defines items that can be used or consumed by party memebers.
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Represents the food gathered from hunting and purchasing in store. Consumed by the party during travel.
        /// </summary>
        public static Item Food => new Item(
            category: Types.Food,
            name: "Food",
            value: 0.20f,
            weight: 1,
            points: 1,
            minQuantity: 1,
            maxQuantity: 2000,
            startingQuantity: 0);

        /// <summary>
        /// Worn by the party members to keep them warm and avoid the risk of getting sick when it is cold outside.
        /// </summary>
        public static Item Hats => new Item(
            category: Types.Hats,
            name: "Hats",
            value: 10,
            weight: 1,
            points: 2,
            minQuantity: 1,
            maxQuantity: 50,
            startingQuantity: 0);

        /// <summary>
        /// Used for hunting and fighting off the robot army during travel. Can be purchased in-store or traded with towns folks.
        /// </summary>
        public static Item Ammunition => new Item(
            category: Types.Ammo,
            name: "Ammunition",
            value: 2,
            weight: 0,
            points: 1,
            minQuantity: 20,
            maxQuantity: 99,
            startingQuantity: 0);

        /// <summary>
        /// Represents the vehicle parts used by the camper van carrying the party. These will need to be periodically replaced to continue travelling.
        /// </summary>
        public static Item Parts => new Item(
            category: Types.Vehicle,
            name: "Parts",
            value: 50,
            weight: 500,
            points: 50,
            minQuantity: 1,
            maxQuantity: 2000,
            startingQuantity: 0);

        /// <summary>
        /// Represents a given party member in the vehicle, this is used mostly to separate the player entities from vehicle and ensure the game never confuses them for being items.
        /// </summary>
        public static Item Person => new Item(
            category: Types.Person,
            name: "Person",
            value: 0,
            weight: 2000,
            points: 800,
            minQuantity: 1,
            maxQuantity: 2000,
            startingQuantity: 0);

        /// <summary>
        /// Money can be exchanged for goods (and service) at the store. Rarely if ever will it be counted by the cents.
        /// </summary>
        public static Item Money => new Item(
            category: Types.Money,
            name: "Money",
            value: 1,
            weight: 0,
            points: 1,
            minQuantity: 1,
            maxQuantity: int.MaxValue,
            startingQuantity: 0);
    }
}