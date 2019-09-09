﻿using System;
using System.Collections.Generic;
using System.Linq;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Person;
using WolfCurses.Utility;

namespace TeufortTrail.Entities.Vehicle
{
    public sealed class Vehicle : IEntity
    {
        #region VARIABLES

        /// <summary>
        /// List of passangers currently travelling in the vehicle.
        /// </summary>
        public List<Person.Person> Passengers;

        /// <summary>
        /// The ideal number of miles that will be travelled assuming nothing goes wrong.
        /// </summary>
        public int Mileage { get; private set; }

        /// <summary>
        /// Total number of miles the vehicle has traveled in this playthrough.
        /// </summary>
        public int Odometer { get; private set; }

        /// <summary>
        /// The vehicle's current status. (Stopped, Moving, etc.)
        /// </summary>
        public VehicleStatus Status { get; set; }

        /// <summary>
        /// The rate at which resources are consumed by the party on the trail.
        /// </summary>
        public RationLevel Ration { get; private set; }

        /// <summary>
        /// The total cash value amount that the party currently has.
        /// </summary>
        public float Balance
        {
            get => _inventory[Types.Money].TotalValue;
            private set
            {
                // Skip this step if the balance is already at the value we were going to set.
                if (value.Equals(_inventory[Types.Money].Quantity)) return;

                // Reset the balance value if it is somehow below or at zero.
                if (value <= 0)
                    _inventory[Types.Money].ResetQuantity();
                else
                    _inventory[Types.Money] = new Item.Item(_inventory[Types.Money], (int)value);
            }
        }

        /// <summary>
        /// Vehicle party's inventory of resources and money.
        /// </summary>
        public IDictionary<Types, Item.Item> Inventory => _inventory;

        private Dictionary<Types, Item.Item> _inventory;

        /// <summary>
        /// Default items every vehicle and store will have.
        /// </summary>
        internal static IDictionary<Types, Item.Item> DefaultInventory
        {
            get
            {
                // Create an inventory of items with default starting amounts.
                var defaultInventory = new Dictionary<Types, Item.Item>
                {
                    {Types.Food, Resources.Food},
                    {Types.Hats, Resources.Hats},
                    {Types.Ammo, Resources.Ammunition},
                    {Types.Money, Resources.Money}
                };

                // Zero out all of the quantities by removing their max quantity. Then return.
                foreach (var simItem in defaultInventory)
                    simItem.Value.SubtractQuantity(simItem.Value.MaxQuantity);
                return defaultInventory;
            }
        }

        /// <summary>
        /// Determine the ideal mileage value. If you run into problems, the mileage will be reduced.
        /// </summary>
        private int RandomMileage
        {
            get
            {
                var totalMiles = Mileage + (100 / Passengers.Where(x => x.HealthState != HealthStatus.Dead).Count()) / 2.5 + 10 * GameCore.Instance.Random.NextDouble();
                return (int)Math.Abs(totalMiles);
            }
        }

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Vehicle.Vehicle" /> class.
        /// </summary>
        public Vehicle()
        {
            // Set the default vehicle state and values.
            ResetVehicle();
            Mileage = 1;
            Status = VehicleStatus.Stopped;
        }

        /// <summary>
        /// Called when the simulation is ticked.
        /// </summary>
        /// <remarks>TODO: Trigger a random event. Insufficient food and clothing should cause illness.</remarks>
        public void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick vehicle at an inverval.
            if (systemTick) return;

            // Loop through the party and tick them moving.
            foreach (var person in Passengers)
                person.OnTick(false, skipDay);

            // Only tick vehicle if it is moving.
            if ((Status != VehicleStatus.Moving) || skipDay) return;

            // Determine how far the vehicle will travel to next point.
            Mileage = RandomMileage;

            // If things go too slowly on the trail, cut the mileage in half.
            if (GameCore.Instance.Random.NextBool() && (Mileage > 0)) Mileage /= 2;

            // Make sure the mileage is never below or at zero.
            if (Mileage <= 0) Mileage = 10;

            // Updated the total mileage travelled.
            Odometer += Mileage;
        }

        /// <summary>
        /// Reset the vehicle status and resources to the default.
        /// </summary>
        /// <param name="startingMoney">Amount of money the vehicle should have on reset.</param>
        internal void ResetVehicle(int startingMoney = 0)
        {
            _inventory = new Dictionary<Types, Item.Item>(DefaultInventory);
            Passengers = new List<Person.Person>();
            Odometer = 0;
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
        /// Adds an item to the vehicle inventory and subtracts its' cost multiplied by quantity from balance.
        /// </summary>
        public void PurchaseItem(Item.Item purchasedItem)
        {
            // Check that the player can afford the item
            if (Balance < purchasedItem.TotalValue) return;

            // Reduce the player's money by the total cost of the purchased item.
            Balance -= purchasedItem.TotalValue;

            // Increase the quantity of the item in the player's inventory.
            Inventory[purchasedItem.Category].AddQuantity(purchasedItem.Quantity);
        }

        /// <summary>
        /// Check if the vehicle is currently operational and is able to move.
        /// </summary>
        /// <remarks>TODO: Add a condition to check that would cause the vehicle to be disabled.</remarks>
        public void CheckStatus()
        {
            if (Status == VehicleStatus.Disabled) return;
            Status = VehicleStatus.Moving;
        }

        /// <summary>
        /// Sets the current party ration rate.
        /// </summary>
        /// <param name="ration">The rate at which the party is permitted to consume the food.</param>
        public void ChangeRations(RationLevel ration)
        {
            Ration = ration;
        }

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
        /// Return a schema item of random type and quantity.
        /// </summary>
        internal static Item.Item GetRandomItem()
        {
            // Loop through every item type in the schema.
            foreach (var item in DefaultInventory)
            {
                // Flip a coin to decide if this item will be processed.
                if (GameCore.Instance.Random.NextBool()) continue;

                // Only certain item types will be processed.
                switch (item.Value.Category)
                {
                    case Types.Food:
                    case Types.Hats:
                    case Types.Ammo:
                        // Generate a value representing the number of items that will be returned.
                        var quantity = item.Value.MaxQuantity / 4;

                        // Check that the quantity value is within range.
                        quantity = (quantity > item.Value.MaxQuantity) ? item.Value.MaxQuantity : quantity;
                        quantity = (quantity <= 0) ? 1 : quantity;

                        // Create and return the new item of random quantity.
                        return new Item.Item(item.Value, GameCore.Instance.Random.Next(1, quantity));

                    default:
                        continue;
                }
            }
            return null;
        }
    }

    #region ENUMERABLES

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
        Filling = 3,
        Meager = 2,
        [Description("Bare Bones")] BareBones = 1
    }

    #endregion ENUMERABLES
}