using System;
using TeufortTrail.Entities.Item;

namespace TeufortTrail.Entities.Person
{
    internal class Person
    {
        #region VARIABLES

        /// <summary>
        /// Determines the player class (Scout, Soldier, Heavy, etc.)
        /// </summary>
        public Classes Class { get; }

        /// <summary>
        /// Flag to determine if this party member is the leader/player.
        /// </summary>
        public bool Leader { get; }

        /// <summary>
        /// Flag to determine if this party member is sick or infected.
        /// </summary>
        private bool Infected { get; set; }

        /// <summary>
        /// Flag to determine if this party member is hurt or injuried.
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
        /// Called every in-game tick. Used to roll a die in deciding how much the person's health will change.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably or FALSE if ticked at a fixed interval.</param>
        /// <param name="skipDay">Determines if the game has force ticked without advancing time. Used by special events that want to simulate passage of time without actually any actual time moving by.</param>
        public void OnTick(bool systemTick, bool skipDay)
        {
            // TODO: Add events that would change member's health and resource consumption
        }

        /// <summary>
        /// Determines how much food party members in the vehicle will eat today.
        /// </summary>
        private void ConsumeFood()
        {
            if (Status == (int)HealthStatus.Dead) return;

            if (GameCore.Instance.Vehicle.Inventory[Categories.Food].Quantity > 0)
                Heal();
            else
                Damage(10, 50);
        }

        /// <summary>
        /// Increase person's health until it reaches maximum value.
        /// </summary>
        private void Heal()
        {
            if (Status == (int)HealthStatus.Dead || Status == (int)HealthStatus.Good)
                return;

            var game = GameCore.Instance;
            if (game.Random.NextBool()) return;

            if (Infected || Injured)
                Status += game.Random.Next(1, 20);
            else
                Status += game.Random.Next(1, 10);
        }

        /// <summary>
        /// Increase person's health until it reaches maximum value.
        /// </summary>
        private void HealFull()
        {
            Status = (int)HealthStatus.Great;
            Infected = false;
            Injured = false;
        }

        /// <summary>
        /// Check if the party leader or member has or has been killed by an illness.
        /// </summary>
        private void CheckIllness()
        {
            if (Status == (int)HealthStatus.Dead) return;
            var game = GameCore.Instance;
            if (game.Random.NextBool()) return;

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
            if (amount > 0)
                Status -= amount;
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

    #region ENUM

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

    #endregion ENUM
}