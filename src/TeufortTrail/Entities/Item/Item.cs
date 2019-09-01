namespace TeufortTrail.Entities.Item
{
    /// <summary>
    /// Defines the base item that be purchased or acquired by the player.
    /// </summary>
    public sealed class Item
    {
        #region VARIABLES

        /// <summary>
        /// Defines what kind of item this is (Food, Ammo, Clothing etc.).
        /// </summary>
        public Types Category { get; }

        /// <summary>
        /// Display name of the item as it should be known to the player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Monetary value of the item.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Weight of the item in pounds.
        /// </summary>
        private int Weight { get; }

        /// <summary>
        /// Point value of the item awarded to the player.
        /// </summary>
        public int Points { get; }

        /// <summary>
        /// Minimum number of the item that the player must have.
        /// </summary>
        public int MinQuantity { get; }

        /// <summary>
        /// Maximum number of the item that the player can have.
        /// </summary>
        public int MaxQuantity { get; }

        /// <summary>
        /// Current number of the item that the player has.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Starting number of the item that the player will have.
        /// </summary>
        private int StartingQuantity { get; }

        /// <summary>
        /// Total weight of the item, weight multiplied by inventory quantity.
        /// </summary>
        public int TotalWeight => Weight * Quantity;

        /// <summary>
        /// Total value of the item, single value multiplied by inventory quantity.
        /// </summary>
        public float TotalValue => Value * Quantity;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Item.Item" /> class.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="weight"></param>
        /// <param name="points"></param>
        /// <param name="minQuantity"></param>
        /// <param name="maxQuantity"></param>
        /// <param name="startingQuantity"></param>
        public Item(Types category, string name, float value, int weight = 1, int points = 1, int minQuantity = 1, int maxQuantity = 999, int startingQuantity = 0)
        {
            Category = category;
            Name = name;
            Value = value;
            Weight = weight;
            Points = points;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            Quantity = startingQuantity;
            StartingQuantity = startingQuantity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Item.Item" /> class from previous instance and with updated quantity.
        /// </summary>
        /// <param name="oldItem"></param>
        /// <param name="newQuantity"></param>
        public Item(Item oldItem, int newQuantity)
        {
            // Check that the new quantity is within range.
            if (newQuantity > oldItem.MaxQuantity)
                newQuantity = oldItem.MaxQuantity;
            if (newQuantity < oldItem.MinQuantity)
                newQuantity = oldItem.MinQuantity;

            Category = oldItem.Category;
            Name = oldItem.Name;
            Value = oldItem.Value;
            Weight = oldItem.Weight;
            Points = (oldItem.Points <= 0) ? 1 : oldItem.Points;
            MinQuantity = oldItem.MinQuantity;
            MaxQuantity = oldItem.MaxQuantity;
            Quantity = newQuantity;
            StartingQuantity = oldItem.StartingQuantity;
        }

        /// <summary>
        /// Increase the current quantity value by a given amount. Check for maximum and minimum values.
        /// </summary>
        /// <param name="amount">Amount the quantity should be increased by.</param>
        public void AddQuantity(int amount)
        {
            var addition = Quantity + amount;
            // Check that the added amount is within range.
            Quantity = (addition < 0) ? 0 : Quantity;
            Quantity = (addition > MaxQuantity) ? MaxQuantity : addition;
        }

        /// <summary>
        /// Subtract the current quantity value by a given amount. Check for maximum and minimum values.
        /// </summary>
        /// <param name="amount">Amount the quantity should be reduced by.</param>
        public void SubtractQuantity(int amount)
        {
            var subtraction = Quantity - amount;
            // Check that the subtracted amount is within range.
            Quantity = (subtraction <= 0) ? 0 : Quantity;
            if (Quantity == 0) return;
            Quantity = (subtraction > MaxQuantity) ? MaxQuantity : subtraction;
            if (Quantity == MaxQuantity) return;
        }

        /// <summary>
        /// Reset the item's quantity to the starting amount.
        /// </summary>
        public void ResetQuantity()
        {
            Quantity = StartingQuantity;
        }
    }

    #region ENUMERABLES

    /// <summary>
    /// Defines all possible item types.
    /// </summary>
    public enum Types
    {
        /// <summary>
        /// Represents the food gathered from hunting and purchasing in store. Consumed by the party during travel.
        /// </summary>
        Food = 1,

        /// <summary>
        /// Worn by the party members to keep them warm and avoid the risk of getting sick when it is cold outside.
        /// </summary>
        Hats = 2,

        /// <summary>
        /// Used for hunting and fighting off the robot army during travel. Can be purchased in-store or traded with towns folks.
        /// </summary>
        Ammo = 3,

        /// <summary>
        /// Represents the camper van that the party members are travelling in. Holds their inventory, money, hats, hopes and dreams. May require replacement parts to be purchased.
        /// </summary>
        Vehicle = 4,

        /// <summary>
        /// Represents a given party member in the vehicle, this is used mostly to separate the player entities from vehicle and ensure the game never confuses them for being items.
        /// </summary>
        Person = 5,

        /// <summary>
        /// Money can be exchanged for goods (and service) at the store. Rarely if ever will it be counted by the cents.
        /// </summary>
        Money = 6,

        /// <summary>
        /// Represents the location along the trail that the player will visit. Could be a town, river crossing, etc.
        /// </summary>
        Location = 7
    }

    #endregion ENUMERABLES
}