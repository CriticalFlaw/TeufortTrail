using System.Collections.Generic;
using TeufortTrail.Entities.Item;

namespace TeufortTrail.Entities.Vehicle
{
    public sealed class Vehicle
    {
        #region VARIABLES

        /// <summary>
        /// List of passangers currently travelling in the vehicle.
        /// </summary>
        private List<Person.Person> Passengers;

        /// <summary>
        /// The number of miles travelled so far along the trail.
        /// </summary>
        public int Mileage { get; private set; }

        /// <summary>
        /// Defines the pace at which the vehicle is travelling.
        /// </summary>
        public TravelPace Pace { get; private set; }

        /// <summary>
        /// Defines the vehicle's current status.
        /// </summary>
        public VehicleStatus Status { get; set; }

        /// <summary>
        /// Defines the rate at which resources will be consumed on the trail.
        /// </summary>
        public RationLevel Ration { get; private set; }

        /// <summary>
        /// The total cash value amount that the party currently has.
        /// </summary>
        public float Balance
        {
            get => _inventory[Categories.Money].TotalValue;
            private set
            {
                if (value.Equals(_inventory[Categories.Money].Quantity))
                    return;
                if (value <= 0)
                    _inventory[Categories.Money].ResetQuantity();
                else
                    _inventory[Categories.Money] = new Item.Item(_inventory[Categories.Money], (int)value);
            }
        }

        /// <summary>
        /// Vehicle party's inventory of resources and money.
        /// </summary>
        public IDictionary<Categories, Item.Item> Inventory => _inventory;

        private Dictionary<Categories, Item.Item> _inventory;

        /// <summary>
        /// Default items every vehicle and store will have.
        /// </summary>
        internal static IDictionary<Categories, Item.Item> DefaultInventory
        {
            get
            {
                var defaultInventory = new Dictionary<Categories, Item.Item>
                {
                    {Categories.Food, Resources.Food},
                    {Categories.Hats, Resources.Hats},
                    {Categories.Weapons, Resources.Weapons},
                    {Categories.Ammo, Resources.Ammunition},
                    {Categories.Money, Resources.Money}
                };

                foreach (var simItem in defaultInventory)
                    simItem.Value.ReduceQuantity(simItem.Value.MaxQuantity);
                return defaultInventory;
            }
        }

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Vehicle.Vehicle" /> class.
        /// </summary>
        public Vehicle()
        {
            ResetVehicle();
            Mileage = 1;
            Pace = TravelPace.Steady;
            Status = VehicleStatus.Stopped;
        }

        /// <summary>
        /// Reset the vehicle status to the default.
        /// </summary>
        /// <param name="startingMoney">Amount of money the vehicle should have on reset.</param>
        internal void ResetVehicle(int startingMoney = 0)
        {
            _inventory = new Dictionary<Categories, Item.Item>(DefaultInventory);
            Passengers = new List<Person.Person>();
            Status = VehicleStatus.Stopped;
            Ration = RationLevel.Filling;
            Balance = startingMoney;
        }

        /// <summary>
        /// Adds a new party member to the vehicle's list of passengers.
        /// </summary>
        internal void AddPerson(Person.Person person)
        {
            Passengers.Add(person);
        }

        /// <summary>
        /// Adds a new item to the inventory of the vehicle and subtracts it's cost multiplied by quantity from balance.
        /// </summary>
        public void PurchaseItem(Item.Item purchasedItem)
        {
            if (Balance < purchasedItem.TotalValue) return;
            Balance -= purchasedItem.TotalValue;
            Inventory[purchasedItem.Category].AddQuantity(purchasedItem.Quantity);
        }
    }

    #region ENUM

    /// <summary>
    /// Defines the pace at which the vehicle is travelling.
    /// </summary>
    public enum TravelPace
    {
        Steady = 1,
        Strenuous = 2,
        Grueling = 3
    }

    /// <summary>
    /// Defines the vehicle's current status.
    /// </summary>
    public enum VehicleStatus
    {
        Stopped = 0,
        Moving = 1,
        Disabled = 2
    }

    /// <summary>
    /// Defines the rate at which resources will be consumed on the trail.
    /// </summary>
    public enum RationLevel
    {
        Filling = 1,
        Meager = 2,
        BareBones = 3
    }

    #endregion ENUM
}