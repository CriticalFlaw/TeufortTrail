namespace TeufortTrail.Entities.Item
{
    /// <summary>
    /// Defines the resource items that can be acquired and consumed by the party.
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Food purchased in store or acquired through trade. Consumed by the party during travel.
        /// </summary>
        public static Item Food => new Item(
            category: ItemTypes.Food,
            name: "Food",
            value: 0.20f,
            weight: 1,
            minQuantity: 1,
            maxQuantity: 2000,
            startingQuantity: 0);

        /// <summary>
        /// Clothing worn by the party to keep them warm and avoid the risk of contracting an illness.
        /// </summary>
        public static Item Clothing => new Item(
            category: ItemTypes.Clothing,
            name: "Hats",
            value: 10,
            weight: 1,
            minQuantity: 1,
            maxQuantity: 50,
            startingQuantity: 0);

        /// <summary>
        /// Ammunition used fighting off the robot army during travel. Can be purchased in-store or traded with towns folks.
        /// </summary>
        public static Item Ammo => new Item(
            category: ItemTypes.Ammo,
            name: "Ammunition",
            value: 2,
            weight: 0,
            minQuantity: 20,
            maxQuantity: 99,
            startingQuantity: 0);

        /// <summary>
        /// Money that can be exchanged for goods and service during the playthrough.
        /// </summary>
        public static Item Money => new Item(
            category: ItemTypes.Money,
            name: "Money",
            value: 1,
            weight: 0,
            minQuantity: 1,
            maxQuantity: int.MaxValue,
            startingQuantity: 0);
    }
}