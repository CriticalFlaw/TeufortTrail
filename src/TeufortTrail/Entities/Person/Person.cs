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

            // Check for illness if the ration rate is too low.
            if ((GameCore.Instance.Vehicle.Ration == RationLevel.Bare) ||
                (GameCore.Instance.Vehicle.Ration == RationLevel.Meager && GameCore.Instance.Random.NextBool()))
                CheckIllness();

            // Check for illness if the amount of hats is too low.
            var playerClothing = GameCore.Instance.Vehicle.Inventory[ItemTypes.Clothing];
            if (playerClothing.TotalValue < GameCore.Instance.Random.Next(0, Convert.ToInt32(Math.Round(0.20 * playerClothing.MaxQuantity))))
                CheckIllness();
            else
            {
                if (GameCore.Instance.Random.NextBool() && GameCore.Instance.Random.NextBool())
                    CheckIllness();
            }

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
                // Subtract the amount of food consumed from the party inventory. Ration Rate x Passengers (Alive)
                vehicle.Inventory[ItemTypes.Food].SubtractQuantity((int)vehicle.Ration * vehicle.Passengers.Count(x => x.HealthState != HealthStatus.Dead));
                Heal();
            }
            else
                // Reduce party member's health if there is no food to eat.
                Damage(10, 50);
        }

        /// <summary>
        /// Increase party member's health by a random amount.
        /// </summary>
        private void Heal()
        {
            // Skip if the party member is already dead or healthy enough.
            if (HealthState == HealthStatus.Dead || HealthState == HealthStatus.Good) return;

            // Roll the dice to determine if the party member will be healed.
            if (GameCore.Instance.Random.NextBool()) return;

            // Roll the dice, the person can get better or worse if they are injured.
            if (Infected || Injured)
                GameCore.Instance.EventDirector.TriggerEvent(this, GameCore.Instance.Random.NextBool() ? typeof(WellAgain) : typeof(TurnForWorse));
            else
                Health += GameCore.Instance.Random.Next(1, 15);
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
        /// Check the party member for illness or injury.
        /// </summary>
        private void CheckIllness()
        {
            // Skip if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            // Roll the dice to determine if the party member will be checked for illness.
            if (GameCore.Instance.Random.NextBool()) return;

            // Reduce the party member's health if the ration level is too low.
            if (GameCore.Instance.Random.Next(100) <= 40 * ((int)GameCore.Instance.Vehicle.Ration - 1))
                Damage(10, 50);

            // Reduce the party member's health if they are sick or injured while the party is traveling.
            switch (HealthState)
            {
                case HealthStatus.Great:
                    if ((Infected || Injured) && (GameCore.Instance.Vehicle.Status != VehicleStatus.Stopped))
                        Damage(10, 50);
                    break;

                case HealthStatus.Good:
                    if ((Infected || Injured) && (GameCore.Instance.Vehicle.Status != VehicleStatus.Stopped))
                        if (GameCore.Instance.Random.NextBool())
                            Damage(10, 50);
                        else if ((!Infected || !Injured) && (GameCore.Instance.Vehicle.Status != VehicleStatus.Stopped))
                            Heal();
                    break;

                case HealthStatus.Fair:
                    if ((Infected || Injured) && (GameCore.Instance.Vehicle.Status != VehicleStatus.Stopped))
                        Damage(5, 10);
                    break;

                case HealthStatus.Poor:
                    Damage(1, 5);
                    break;

                case (int)HealthStatus.Dead:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
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
        private void Damage(int minAmount, int maxAmount)
        {
            // Skip if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            // Subtract a random amount of health from the party member.
            Health -= GameCore.Instance.Random.Next(minAmount, maxAmount);

            // Chance of injury or illness if the person doesn't already have it.
            if (!Infected || !Injured)
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