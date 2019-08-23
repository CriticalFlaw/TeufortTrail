using System.Collections.Generic;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Person;

namespace TeufortTrail.Entities.Vehicle
{
    public sealed class Vehicle
    {
        public TravelPace Pace { get; private set; }
        public int Mileage { get; private set; }
        public VehicleStatus Status { get; set; }
        private List<Person.Person> Passengers;
        public Dictionary<Categories, Item.Item> Inventory;
        public RationLevel Ration { get; private set; }

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

                // Zero out all of the quantities by removing their max quantity.
                foreach (var simItem in defaultInventory)
                    simItem.Value.ReduceQuantity(simItem.Value.MaxQuantity);

                // Now we have default inventory of a store with all quantities zeroed out.
                return defaultInventory;
            }
        }

        public Vehicle()
        {
            ResetVehicle();
            Pace = TravelPace.Steady;
            Mileage = 1;
            Status = VehicleStatus.Stopped;
        }

        internal void ResetVehicle(int startingMoney = 0)
        {
            Inventory = new Dictionary<Categories, Item.Item>(DefaultInventory);
            Passengers = new List<Person.Person>();
            Ration = RationLevel.Filling;
            Status = VehicleStatus.Stopped;
        }

        internal void AddPerson(Person.Person person)
        {
            Passengers.Add(person);
        }
    }

    public enum TravelPace
    {
        Steady = 1,
        Strenuous = 2,
        Grueling = 3
    }

    public enum VehicleStatus
    {
        Stopped = 0,
        Moving = 1,
        Disabled = 2
    }
}