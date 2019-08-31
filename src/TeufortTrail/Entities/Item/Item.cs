using WolfCurses.Utility;

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
        public Categories Category { get; }

        /// <summary>
        /// Display name of the item as it is known to the player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Monetary cost of the item.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Weight of a single item unit, in pounds.
        /// </summary>
        private int Weight { get; }

        /// <summary>
        /// Point value of the item awarded to the player's score.
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
        /// Current number of the item that the player has.
        /// </summary>
        private int StartingQuantity { get; }

        /// <summary>
        /// Single weight of the item, multiplied by player quantity.
        /// </summary>
        public int TotalWeight => Weight * Quantity;

        /// <summary>
        /// Single value of the item, multiplied by player quantity.
        /// </summary>
        public float TotalValue => Value * Quantity;

        /// <summary>
        /// Total number of points the player has accumulated.
        /// </summary>
        public int PointsTotal
        {
            get
            {
                if ((Quantity <= 0) || (Quantity < Points))
                    return 0;
                return Quantity * Points;
            }
        }

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
        public Item(Categories category, string name, float value, int weight = 1, int points = 1, int minQuantity = 1, int maxQuantity = 999, int startingQuantity = 0)
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
        /// Reduce the current quantity value by a given amount. Check for maximum and minimum values.
        /// </summary>
        /// <param name="amount">Amount the quantity should be reduced by.</param>
        public void ReduceQuantity(int amount)
        {
            var subtraction = Quantity - amount;
            Quantity = (subtraction <= 0) ? 0 : Quantity;
            Quantity = (subtraction > MaxQuantity) ? MaxQuantity : subtraction;
        }
    }

    #region ENUM

    /// <summary>
    /// Defines all possible item types.
    /// </summary>
    public enum Categories
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
        /// Used for hunting and fighting off the robot army during travel. Can be purchased in-store. Breaks after long use.
        /// </summary>
        [Description("Guns")] Weapons = 3,

        /// <summary>
        /// Used for hunting and fighting off the robot army during travel. Can be purchased in-store or traded with towns folks.
        /// </summary>
        Ammo = 4,

        /// <summary>
        /// Represents the camper van that the party members are travelling in. Holds their inventory, money, hats, hopes and dreams. May require replacement parts to be purchased.
        /// </summary>
        Vehicle = 5,

        /// <summary>
        /// Represents a given party member in the vehicle, this is used mostly to separate the player entities from vehicle and ensure the game never confuses them for being items.
        /// </summary>
        Person = 6,

        /// <summary>
        /// Money can be exchanged for goods (and service) at the store. Rarely if ever will it be counted by the cents.
        /// </summary>
        Money = 7,

        /// <summary>
        /// Represents the location along the trail that the player will visit. Could be a town, river crossing, etc.
        /// </summary>
        Location = 8
    }

    #endregion ENUM
}