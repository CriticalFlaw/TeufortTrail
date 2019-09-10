namespace TeufortTrail.Entities.Item
{
    /// <summary>
    /// Defines the base game items that can be purchased, acquired and traded by the player.
    /// </summary>
    public sealed class Item : IEntity
    {
        /// <summary>
        /// Defines what kind of item this is.
        /// </summary>
        /// <example>Food, Hats, Ammunition</example>
        public ItemTypes Category { get; }

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
        /// Minimum quantity of the item that the player must have.
        /// </summary>
        public int MinQuantity { get; }

        /// <summary>
        /// Maximum quantity of the item that the player can have.
        /// </summary>
        public int MaxQuantity { get; }

        /// <summary>
        /// Current quantity of the item that the player has.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Starting quantity of the item that the player will have.
        /// </summary>
        private int StartingQuantity { get; }

        /// <summary>
        /// Total weight of the item, unit weight multiplied by inventory quantity.
        /// </summary>
        public int TotalWeight => Weight * Quantity;

        /// <summary>
        /// Total value of the item, unit value multiplied by inventory quantity.
        /// </summary>
        public float TotalValue => Value * Quantity;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Item" /> class.
        /// </summary>
        public Item(ItemTypes category, string name, float value, int weight = 1, int minQuantity = 1, int maxQuantity = 999, int startingQuantity = 0)
        {
            Category = category;
            Name = name;
            Value = value;
            Weight = weight;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            Quantity = startingQuantity;
            StartingQuantity = startingQuantity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item" /> class from the previous instance with updated quantities.
        /// </summary>
        public Item(Item oldItem, int newQuantity)
        {
            // Check that the new quantity is in range.
            if (newQuantity > oldItem.MaxQuantity)
                newQuantity = oldItem.MaxQuantity;
            if (newQuantity < oldItem.MinQuantity)
                newQuantity = oldItem.MinQuantity;

            Category = oldItem.Category;
            Name = oldItem.Name;
            Value = oldItem.Value;
            Weight = oldItem.Weight;
            MinQuantity = oldItem.MinQuantity;
            MaxQuantity = oldItem.MaxQuantity;
            Quantity = newQuantity;
            StartingQuantity = oldItem.StartingQuantity;
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        public void OnTick(bool systemTick, bool skipDay)
        {
        }

        /// <summary>
        /// Increment the item quantity by a given amount. Check for maximum and minimum values.
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
        /// Subtract the item quantity by a given amount. Check for maximum and minimum values.
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
        /// Reset the item quantity to the starting amount.
        /// </summary>
        public void ResetQuantity()
        {
            Quantity = StartingQuantity;
        }
    }
}