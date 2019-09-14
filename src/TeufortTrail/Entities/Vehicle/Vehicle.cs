using System;
using System.Collections.Generic;
using System.Linq;
using TeufortTrail.Entities.Item;

namespace TeufortTrail.Entities.Vehicle
{
    public sealed class Vehicle : IEntity
    {
        /// <summary>
        /// List of passengers currently traveling in the vehicle.
        /// </summary>
        public List<Person.Person> Passengers;

        /// <summary>
        /// Number of miles that will be traveled between locations.
        /// </summary>
        public int Mileage { get; private set; }

        /// <summary>
        /// Total number of miles the vehicle has traveled on this playthrough.
        /// </summary>
        public int Odometer { get; private set; }

        /// <summary>
        /// Vehicle's current status.
        /// </summary>
        /// <example>Stopped, Moving, Disabled</example>
        public VehicleStatus Status { get; set; }

        /// <summary>
        /// Rate at which food is consumed by the party on the trail.
        /// </summary>
        public RationLevel Ration { get; private set; }

        /// <summary>
        /// Vehicle party's inventory of consumable resources and money.
        /// </summary>
        /// <remarks>TODO: Refactor</remarks>
        public IDictionary<ItemTypes, Item.Item> Inventory => _inventory;

        private Dictionary<ItemTypes, Item.Item> _inventory;

        /// <summary>
        /// Total amount of money that the party currently has.
        /// </summary>
        public float Balance
        {
            get => _inventory[ItemTypes.Money].TotalValue;
            private set
            {
                // Skip if the balance is already at the desired amount.
                if (value.Equals(_inventory[ItemTypes.Money].Quantity)) return;

                // Reset the balance value if it somehow gets below zero.
                if (value <= 0)
                    _inventory[ItemTypes.Money].ResetQuantity();
                else
                    _inventory[ItemTypes.Money] = new Item.Item(_inventory[ItemTypes.Money], (int)value);
            }
        }

        /// <summary>
        /// Default inventory of resources every vehicle and store will have.
        /// </summary>
        internal static IDictionary<ItemTypes, Item.Item> DefaultInventory
        {
            get
            {
                // Create an inventory of items with default starting amounts.
                var defaultInventory = new Dictionary<ItemTypes, Item.Item>
                {
                    {ItemTypes.Food, Resources.Food},
                    {ItemTypes.Clothing, Resources.Clothing},
                    {ItemTypes.Ammo, Resources.Ammo},
                    {ItemTypes.Money, Resources.Money}
                };

                // Zero out all of the quantities by removing their max quantity.
                foreach (var simItem in defaultInventory)
                    simItem.Value.SubtractQuantity(simItem.Value.MaxQuantity);
                return defaultInventory;
            }
        }

        /// <summary>
        /// Determine the mile amount traveled by the party on the trail.
        /// </summary>
        private int RandomMileage
        {
            get
            {
                return (int)Math.Abs(Mileage + (100 / Passengers.Where(x => x.HealthState != HealthStatus.Dead).Count()) / 2.5 + 10 * GameCore.Instance.Random.NextDouble());
            }
        }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle" /> class.
        /// </summary>
        public Vehicle()
        {
            ResetVehicle();
            Mileage = 1;
            Status = VehicleStatus.Stopped;
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        /// <remarks>TODO: Trigger a random event. Insufficient food and clothing should cause illness.</remarks>
        public void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick at an interval.
            if (systemTick) return;

            // Loop through the party and tick them moving.
            foreach (var person in Passengers)
                person.OnTick(false, skipDay);

            // Only tick vehicle if it is moving.
            if ((Status != VehicleStatus.Moving) || skipDay) return;

            // Determine how far the vehicle travels on this tick.
            Mileage = RandomMileage;

            // Reduce the mileage in half if things are too slow on the trail.
            if (GameCore.Instance.Random.NextBool() && (Mileage > 0)) Mileage /= 2;

            // Make sure the mileage is never below or at zero.
            if (Mileage <= 0) Mileage = 10;

            // Update the total mileage traveled.
            Odometer += Mileage;
        }

        /// <summary>
        /// Reset the vehicle status and resources to the default.
        /// </summary>
        internal void ResetVehicle(int startingMoney = 0)
        {
            _inventory = new Dictionary<ItemTypes, Item.Item>(DefaultInventory);
            Passengers = new List<Person.Person>();
            Odometer = 0;
            Status = VehicleStatus.Stopped;
            Ration = RationLevel.Filling;
            Balance = startingMoney;
        }

        /// <summary>
        /// Add a person to the party and the vehicle's list of passengers.
        /// </summary>
        internal void AddPerson(Person.Person person)
        {
            Passengers.Add(person);
        }

        /// <summary>
        /// Add an item to the inventory and subtracts its value from the party's total money.
        /// </summary>
        public void PurchaseItem(Item.Item purchasedItem)
        {
            // Check that the player can afford the item.
            if (Balance < purchasedItem.TotalValue) return;

            // Subtract the cost of the purchased item from the party's total money.
            Balance -= purchasedItem.TotalValue;

            // Increase the quantity of the item in the player's inventory.
            Inventory[purchasedItem.Category].AddQuantity(purchasedItem.Quantity);
        }

        /// <summary>
        /// Check that the vehicle is operational and is able to move.
        /// </summary>
        /// <remarks>TODO: Add a condition to check that would cause the vehicle to be disabled.</remarks>
        public void CheckStatus()
        {
            if (Status == VehicleStatus.Disabled) return;
            Status = VehicleStatus.Moving;
        }

        /// <summary>
        /// Sets the current party food consumption rate.
        /// </summary>
        /// <param name="ration">The rate at which the party is permitted to consume the food.</param>
        public void ChangeRations(RationLevel ration)
        {
            Ration = ration;
        }

        /// <summary>
        /// Check that the party has the given item in their inventory.
        /// </summary>
        internal bool HasInventoryItem(Item.Item wantedItem)
        {
            foreach (var item in Inventory)
                if ((item.Value.Name == wantedItem.Name) &&
                    (item.Value.Category == wantedItem.Category) &&
                    (item.Value.Quantity >= wantedItem.MinQuantity))
                    return true;
            return false;
        }

        /// <summary>
        /// Retrieve a schema item of random type and quantity.
        /// </summary>
        internal static Item.Item GetRandomItem()
        {
            // Loop through every item type in the schema.
            foreach (var item in DefaultInventory)
            {
                // Flip a coin to decide if this item will be processed.
                if (GameCore.Instance.Random.NextBool()) continue;

                // Only certain item types will be processed.
                if (item.Value.Category == ItemTypes.Food ||
                    item.Value.Category == ItemTypes.Clothing ||
                    item.Value.Category == ItemTypes.Ammo)
                {
                    // Generate a value representing the number of items that will be returned.
                    var quantity = item.Value.MaxQuantity / 4;

                    // Check that the quantity value is within range.
                    quantity = (quantity > item.Value.MaxQuantity) ? item.Value.MaxQuantity : quantity;
                    quantity = (quantity <= 0) ? 1 : quantity;

                    // Create and return the new item of random quantity.
                    return new Item.Item(item.Value, GameCore.Instance.Random.Next(1, quantity));
                }
                else
                    continue;
            }
            return null;
        }
    }
}