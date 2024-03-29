﻿using System;
using System.Collections.Generic;
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
        private int RandomMileage => (int)Math.Abs(Mileage + 15 / 2.5 + 10 * GameCore.Instance.Random.NextDouble());

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

            // Check for a random event that could occur.
            GameCore.Instance.EventDirector.TriggerEventByType(this, EventCategory.Vehicle);

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
        /// Retrieve a schema item of random type and quantity to be added to the player inventory.
        /// </summary>
        internal static Item.Item CreateRandomItem()
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
            }
            return null;
        }

        /// <summary>
        /// Loop through the party and roll the dice to see if they should be killed.
        /// </summary>
        public IEnumerable<Person.Person> TryKill()
        {
            var peopleKilled = new List<Person.Person>();
            foreach (var person in GameCore.Instance.Vehicle.Passengers)
            {
                // Roll the dice, proceed if true otherwise continue. Skip if the party member is already dead.
                if (!GameCore.Instance.Random.NextBool() || (person.HealthState == HealthStatus.Dead)) continue;

                // Kill the party member and add them to list.
                person.Kill();
                peopleKilled.Add(person);
            }
            return peopleKilled;
        }

        /// <summary>
        /// Retrieve a schema item list of random type and quantity to be added to the player inventory.
        /// </summary>
        public IDictionary<ItemTypes, int> CreateRandomItems()
        {
            // Create and empty item list and a copy of the current inventory.
            var createdItems = new Dictionary<ItemTypes, int>();
            var inventoryCopy = new Dictionary<ItemTypes, Item.Item>(Inventory);

            // Loop through the inventory and decide which items to give to the player.
            foreach (var item in inventoryCopy)
            {
                // Skip this step if the item quantity is at a maximum.
                if (item.Value.Quantity >= item.Value.MaxQuantity) continue;

                // Skip this step if the dice roll returns a false.
                if (GameCore.Instance.Random.NextBool()) continue;

                // Generate a random amount to represent the quantity of items created.
                var createdAmount = GameCore.Instance.Random.Next(1, item.Value.MaxQuantity / 4);

                // Check that the item quantity after the addition is still within range.
                var simulatedAmountAdd = item.Value.Quantity + createdAmount;
                if (simulatedAmountAdd >= item.Value.MaxQuantity) simulatedAmountAdd = item.Value.MaxQuantity;

                // Add the amount of items created to the player inventory.
                Inventory[item.Key] = new Item.Item(item.Value, simulatedAmountAdd);
                createdItems.Add(item.Key, createdAmount);
            }

            // Clear out the copied list and return the created one.
            inventoryCopy.Clear();
            return createdItems;
        }

        /// <summary>
        /// Retrieve a schema item list of random type and quantity to be removed from the player inventory.
        /// </summary>
        public IDictionary<ItemTypes, int> DestroyRandomItems()
        {
            // Create and empty item list and a copy of the current inventory.
            var destroyedItems = new Dictionary<ItemTypes, int>();
            var inventoryCopy = new Dictionary<ItemTypes, Item.Item>(Inventory);

            // Loop through the inventory and decide which items to take away from the player.
            foreach (var item in inventoryCopy)
            {
                // Skip this step if the item quantity is at a minimum.
                if (item.Value.Quantity < 1) continue;

                // Skip this step if the dice roll returns a false.
                if (GameCore.Instance.Random.NextBool()) continue;

                // Generate a random amount to represent the quantity of items created.
                var destroyAmount = GameCore.Instance.Random.Next(1, item.Value.Quantity);

                // Remove the generated amount of items from the player inventory.
                Inventory[item.Key].SubtractQuantity(destroyAmount);
                destroyedItems.Add(item.Key, destroyAmount);
            }

            // Clear out the copied list and return the created one.
            inventoryCopy.Clear();
            return destroyedItems;
        }
    }
}