using System;
using System.Linq;
using TeufortTrail.Events.Person;

namespace TeufortTrail.Entities.Person
{
    public sealed class Person : IEntity
    {
        /// <summary>
        /// Defines the party member class. Used for setting game modifiers.
        /// </summary>
        /// <example>Scout, Soldier, Pyro, Demoman, Heavy, Medic, Engineer, Sniper, Spy</example>
        public Classes Class { get; }

        /// <summary>
        /// Flags the party member as the leader/player.
        /// </summary>
        public bool Leader { get; }

        /// <summary>
        /// Flags the party member as sick.
        /// </summary>
        private bool Infected { get; set; }

        /// <summary>
        /// Flags the party member as hurt.
        /// </summary>
        private bool Injured { get; set; }

        /// <summary>
        /// Defines the party member's health value.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Returns the party member's health value in text form.
        /// </summary>
        public HealthStatus HealthState
        {
            get
            {
                return Health switch
                {
                    >= (int) HealthStatus.Great and > (int) HealthStatus.Good => HealthStatus.Great,
                    <= (int) HealthStatus.Good and > (int) HealthStatus.Fair => HealthStatus.Good,
                    <= (int) HealthStatus.Fair and > (int) HealthStatus.Poor => HealthStatus.Fair,
                    <= (int) HealthStatus.Poor and > (int) HealthStatus.Dead => HealthStatus.Poor,
                    <= (int) HealthStatus.Dead => HealthStatus.Dead,
                    _ => HealthStatus.Good
                };
            }
        }

        /// <summary>
        /// Returns the party member's status in text form.
        /// </summary>
        public string Status
        {
            get
            {
                if (HealthState == (int)HealthStatus.Dead)
                    return "Dead";
                if (Infected)
                    return "Infected";
                if (Injured)
                    return "Injured";
                if (Infected && Injured)
                    return "Infected and Injured";
                return "OK";
            }
        }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Person" /> class.
        /// </summary>
        public Person(Classes player, bool leader)
        {
            Class = player;
            Leader = leader;
            Infected = false;
            Injured = false;
            Health = (int)HealthStatus.Good;
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

            // Skip if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            var game = GameCore.Instance;
            var vehicle = game.Vehicle;

            // Small rations wear the party down.
            if (game.Random.Next(100) < GameBalance.HungerChance(vehicle.Ration))
                Damage(10, 30);

            // Healthy party members can fall ill, more so without enough hats or food.
            var enoughHats = vehicle.Inventory[ItemTypes.Clothing].Quantity >= GameBalance.HatsPerPerson * vehicle.Passengers.Count(x => x.HealthState != HealthStatus.Dead);
            if (!Infected && !Injured && game.Random.Next(1000) < GameBalance.SicknessChance(vehicle.Ration, enoughHats))
                game.EventDirector.TriggerEvent(this, typeof(Events.Person.Infected));
            else if ((Infected || Injured) && vehicle.Status == VehicleStatus.Moving)
                CheckIllness();

            // Only consume food if the whole day has passed.
            if (!skipDay) ConsumeFood();
        }

        /// <summary>
        /// Determine how much food the party will consume, then subtract it from the inventory.
        /// </summary>
        public void ConsumeFood()
        {
            // Skip if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            var vehicle = GameCore.Instance.Vehicle;
            if (vehicle.Inventory[ItemTypes.Food].Quantity > 0)
            {
                vehicle.Inventory[ItemTypes.Food].SubtractQuantity(GameBalance.FoodPerPerson(vehicle.Ration));
                Heal();
            }
            else
                // Reduce party member's health if there is no food to eat.
                Damage(20, 45);
        }

        /// <summary>
        /// Increase party member's health by a random amount, depending on how well they are eating.
        /// </summary>
        private void Heal()
        {
            // Skip if the party member is dead, sick or already healthy.
            if (HealthState == HealthStatus.Dead || Infected || Injured || Health >= (int)HealthStatus.Good) return;

            // Roll the dice to determine if the party member will be healed.
            if (GameCore.Instance.Random.NextBool()) return;

            var (min, max) = GameBalance.FedHealing(GameCore.Instance.Vehicle.Ration);
            if (max > 0) AddHealth(GameCore.Instance.Random.Next(min, max + 1));
        }

        /// <summary>
        /// Rest for one day: eat, recover health, and maybe shake off an illness or injury.
        /// </summary>
        /// <returns>Pounds of food eaten.</returns>
        public int Rest()
        {
            if (HealthState == HealthStatus.Dead) return 0;
            var game = GameCore.Instance;
            var food = game.Vehicle.Inventory[ItemTypes.Food];
            var eaten = 0;
            if (food.Quantity > 0)
            {
                eaten = System.Math.Min(food.Quantity, GameBalance.FoodPerPerson(game.Vehicle.Ration));
                food.SubtractQuantity(eaten);
                AddHealth(game.Random.Next(GameBalance.RestHealMin, GameBalance.RestHealMax + 1));
            }
            else
                Damage(15, 40);

            if ((Infected || Injured) && game.Random.Next(100) < GameBalance.RecoverChanceResting)
            {
                Infected = false;
                Injured = false;
            }
            return eaten;
        }

        /// <summary>
        /// Increase health without going above Good.
        /// </summary>
        private void AddHealth(int amount)
        {
            Health = System.Math.Min((int)HealthStatus.Good, System.Math.Max(Health, Health + amount));
        }

        /// <summary>
        /// Fully restore a party member's health to normal.
        /// </summary>
        public void HealFully()
        {
            Infected = false;
            Injured = false;
            Health = (int)HealthStatus.Good;
        }

        /// <summary>
        /// Shake off any illness or injury, without restoring lost health.
        /// </summary>
        public void Recover()
        {
            Infected = false;
            Injured = false;
        }

        /// <summary>
        /// A sick or injured party member travelling on the trail may get better, get worse, or take damage.
        /// </summary>
        private void CheckIllness()
        {
            // Skip if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            var game = GameCore.Instance;
            var roll = game.Random.Next(100);
            if (roll < GameBalance.RecoverChanceTravelling)
                game.EventDirector.TriggerEvent(this, typeof(WellAgain));
            else if (roll < GameBalance.RecoverChanceTravelling + GameBalance.TurnForWorseChance)
                game.EventDirector.TriggerEvent(this, typeof(TurnForWorse));
            else if (game.Random.Next(100) < GameBalance.SickDamageChance)
                Damage(GameBalance.SickDamageMin, GameBalance.SickDamageMax, false);
        }

        /// <summary>
        /// Decrease party member's health by a given amount.
        /// </summary>
        /// <param name="amount">Amount of health to decrease</param>
        public void Damage(int amount)
        {
            if (amount > 0) Health -= amount;
        }

        /// <summary>
        /// Decrease party member's health by an amount between two values.
        /// </summary>
        private void Damage(int minAmount, int maxAmount, bool canSicken = true)
        {
            // Skip if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            // Subtract a random amount of health from the party member.
            Health -= GameCore.Instance.Random.Next(minAmount, maxAmount);

            // Chance of injury or illness if the person doesn't already have it.
            if (canSicken && !Infected && !Injured)
                GameCore.Instance.EventDirector.TriggerEventByType(this, EventCategory.Person);

            // Check if the person health has dropped to death levels.
            if (Health > (int)HealthStatus.Dead) return;

            // At this point, we assume that the person has died, and need to display a screen.
            Health = (int)HealthStatus.Dead;
            GameCore.Instance.EventDirector.TriggerEvent(this, (Leader) ? typeof(PlayerDeath) : typeof(PassengerDeath));
        }

        /// <summary>
        /// Flag the party member as sick.
        /// </summary>
        public void Infect()
        {
            Infected = true;
        }

        /// <summary>
        /// Flag the party member as hurt.
        /// </summary>
        public void Injure()
        {
            Injured = true;
        }

        /// <summary>
        /// Flag the party member as dead.
        /// </summary>
        public void Kill()
        {
            // Skip if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            Health = (int)HealthStatus.Dead;
        }
    }
}