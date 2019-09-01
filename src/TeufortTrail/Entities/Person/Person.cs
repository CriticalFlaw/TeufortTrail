﻿using System;
using TeufortTrail.Entities.Item;

namespace TeufortTrail.Entities.Person
{
    internal class Person
    {
        #region VARIABLES

        /// <summary>
        /// Defines the the player class (See Classes enumerable)
        /// </summary>
        public Classes Class { get; }

        /// <summary>
        /// Flags the party member as the leader/player.
        /// </summary>
        public bool Leader { get; }

        /// <summary>
        /// Flags the party member as sick or infected.
        /// </summary>
        private bool Infected { get; set; }

        /// <summary>
        /// Flags the party member as hurt or injured.
        /// </summary>
        private bool Injured { get; set; }

        /// <summary>
        /// Defines the party member's status as a health value.
        /// </summary>
        private int Status { get; set; }

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Person.Person" /> class.
        /// </summary>
        /// <param name="_class">Party member class</param>
        /// <param name="leader">Flag to determine if this member is the leader/player</param>
        public Person(Classes _class, bool leader)
        {
            Class = _class;
            Leader = leader;
            Infected = false;
            Injured = false;
            Status = (int)HealthStatus.Good;
        }

        /// <summary>
        /// Called when the simulation is ticked.
        /// </summary>
        public void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick vehicle at an inverval.
            if (systemTick) return;

            // TODO: Add events that would change member's health and resource consumption

            // Only consume food if the whole day has passed.
            if (!skipDay) ConsumeFood();
        }

        /// <summary>
        /// Determines how much food party members in the vehicle will consume.
        /// </summary>
        private void ConsumeFood()
        {
            // Skip this step if the party member is already dead.
            if (Status == (int)HealthStatus.Dead) return;

            if (GameCore.Instance.Vehicle.Inventory[Types.Food].Quantity > 0)
            {
                // TODO: Reduce the food quantity based on the ration level.
                Heal();
            }
            else
                // Reduce party member's health if there is no food to eat.
                Damage(10, 50);
        }

        /// <summary>
        /// Increase party member's health until it reaches maximum value.
        /// </summary>
        private void Heal()
        {
            // Skip this step if the party member is already dead or healthy enough.
            if (Status == (int)HealthStatus.Dead || Status == (int)HealthStatus.Good) return;

            // Roll the dice to determine if the party member will even be healed.
            var game = GameCore.Instance;
            if (game.Random.NextBool()) return;

            // Heal the party member a greater amount if they are currently sick or hurt.
            Status += game.Random.Next(1, ((Infected || Injured) ? 20 : 10));
        }

        /// <summary>
        /// Check if the party member has an illness or injury.
        /// </summary>
        private void CheckIllness()
        {
            // Skip this step if the party member is already dead.
            if (Status == (int)HealthStatus.Dead) return;

            // Roll the dice to determine if the party member will even be healed.
            var game = GameCore.Instance;
            if (game.Random.NextBool()) return;

            // Reduce the party member's health depending on their health and debuff.
            switch (Status)
            {
                case (int)HealthStatus.Great:
                    if (Infected || Injured)
                        Damage(10, 50);
                    break;

                case (int)HealthStatus.Good:
                    if (Infected || Injured)
                        if (game.Random.NextBool())
                            Damage(10, 50);
                        else if (!Infected || !Injured)
                            Heal();
                    break;

                case (int)HealthStatus.Fair:
                    if (Infected || Injured)
                        Damage(5, 10);
                    break;

                case (int)HealthStatus.Poor:
                    Damage(1, 5);
                    break;

                case (int)HealthStatus.Dead:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Decrease person's health by a given amount.
        /// </summary>
        /// <param name="amount">Amount of health to decrease</param>
        private void Damage(int amount)
        {
            if (amount > 0) Status -= amount;
        }

        /// <summary>
        /// Decrease person's health by an amount between two values.
        /// </summary>
        /// <param name="minAmount">Minimum amount of damage that should be randomly generated.</param>
        /// <param name="maxAmount">Maximum amount of damage that should be randomly generated.</param>
        private void Damage(int minAmount, int maxAmount)
        {
            Status -= GameCore.Instance.Random.Next(minAmount, maxAmount);
        }

        /// <summary>
        /// Flags the person as being infected or sick.
        /// </summary>
        private void Infect()
        {
            Infected = true;
        }

        /// <summary>
        /// Flags the person as being hurt or injured.
        /// </summary>
        private void Injure()
        {
            Injured = true;
        }

        /// <summary>
        /// Flags the person as being dead or not alive.
        /// </summary>
        private void Kill()
        {
            Status = (int)HealthStatus.Dead;
        }
    }

    #region ENUMERABLES

    /// <summary>
    /// Defines all the playable classes.
    /// </summary>
    public enum Classes
    {
        // TODO: Add descriptons and unique stats to each class.
        Scout = 1,

        Soldier = 2,
        Pyro = 3,
        Demoman = 4,
        Heavy = 5,
        Medic = 6,
        Engineer = 7,
        Sniper = 8,
        Spy = 9
    }

    /// <summary>
    /// Defines the numeric health values each party member can have.
    /// </summary>
    public enum HealthStatus
    {
        Great = 500,
        Good = 400,
        Fair = 300,
        Poor = 200,
        Dead = 0
    }

    #endregion ENUMERABLES
}