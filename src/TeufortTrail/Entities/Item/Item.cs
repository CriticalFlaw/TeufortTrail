namespace TeufortTrail.Entities.Item
{
    public sealed class Item
    {
        public Categories Category { get; }
        public string Name { get; }
        public float Value { get; }
        private int Weight { get; }
        public int Points { get; }
        public int MinQuantity { get; }
        public int MaxQuantity { get; }
        public int Quantity { get; private set; }
        private int StartingQuantity { get; }
        public int TotalWeight => Weight * Quantity;
        public float TotalValue => Value * Quantity;

        public int PointsTotal
        {
            get
            {
                if ((Quantity <= 0) || (Quantity < Points))
                    return 0;
                return Quantity * Points;
            }
        }

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

        public void ReduceQuantity(int amount)
        {
            var subtraction = Quantity - amount;
            Quantity = (subtraction <= 0) ? 0 : Quantity;
            Quantity = (subtraction > MaxQuantity) ? MaxQuantity : subtraction;
        }
    }

    public enum Categories
    {
        Food = 1,
        Hats = 2,
        Weapons = 3,
        Ammo = 4,
        Vehicle = 5,
        Person = 6,
        Money = 7,
        Location = 8
    }
}