﻿using System;
using System.Linq;
using TeufortTrail.Events;

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
                if (Health >= (int)HealthStatus.Great && Health >= (int)HealthStatus.Good)
                    return HealthStatus.Great;
                if (Health <= (int)HealthStatus.Good && Health >= (int)HealthStatus.Fair)
                    return HealthStatus.Good;
                if (Health <= (int)HealthStatus.Fair && Health >= (int)HealthStatus.Poor)
                    return HealthStatus.Fair;
                if (Health <= (int)HealthStatus.Poor && Health >= (int)HealthStatus.Dead)
                    return HealthStatus.Poor;
                if (Health <= (int)HealthStatus.Dead)
                    return HealthStatus.Dead;
                return HealthStatus.Good;
            }
        }

        /// <summary>
        /// Returns the party member's status in text form.
        /// </summary>
        public string Status
        {
            get
            {
                if (Infected && Injured)
                    return "Infected and Injured";
                if (Infected && Injured)
                    return "Injured";
                if (Infected && Injured)
                    return "Infected";
                if (Health == (int)HealthStatus.Dead)
                    return "Dead";
                return "OK";
            }
        }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Person" /> class.
        /// </summary>
        public Person(Classes class_, bool leader)
        {
            Class = class_;
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
        /// <remarks>TODO: Add events that would change member's health and resource consumptions</remarks>
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

            // Only consume food if the whole day has passed.
            if (!skipDay) ConsumeFood();
        }

        /// <summary>
        /// Determine how much food the party will consume, then subtract it from the inventory.
        /// </summary>
        private void ConsumeFood()
        {
            // Skip if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            var vehicle = GameCore.Instance.Vehicle;
            if (vehicle.Inventory[ItemTypes.Food].Quantity > 0)
            {
                // Subtract the amount of food consumed from the party inventory. Ration Rate x Passengers (Alive)
                vehicle.Inventory[ItemTypes.Food].SubtractQuantity((int)vehicle.Ration * vehicle.Passengers.Where(x => x.HealthState != HealthStatus.Dead).Count());
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

            // Heal the party member a greater amount if they are currently sick or injured.
            Health += GameCore.Instance.Random.Next(1, ((Infected || Injured) ? 20 : 10));
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
            if (GameCore.Instance.Random.Next(100) <= 45 * ((int)GameCore.Instance.Vehicle.Ration - 1))
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

            // Display a notification letting the player know that a party member has died.
            if (Health <= (int)HealthStatus.Dead)
                GameCore.Instance.EventDirector.TriggerEvent(this, typeof(PassengerDeath));
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