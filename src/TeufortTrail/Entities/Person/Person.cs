using System;
using System.Linq;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Vehicle;
using TeufortTrail.Events;

namespace TeufortTrail.Entities.Person
{
    public sealed class Person : IEntity
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
        public int Health { get; set; }

        /// <summary>
        /// Returns the party member's health value as a text indicator
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
        /// Returns the party member's status state as a text indicator
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

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Person.Person" /> class.
        /// </summary>
        /// <param name="_class">Party member class</param>
        /// <param name="leader">Flags the party member as the leader/player</param>
        public Person(Classes _class, bool leader)
        {
            Class = _class;
            Leader = leader;
            Infected = false;
            Injured = false;
            Health = (int)HealthStatus.Good;
        }

        /// <summary>
        /// Called when the simulation is ticked.
        /// </summary>
        /// <remarks>TODO: Add events that would change member's health and resource consumptions</remarks>
        public void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick vehicle at an inverval.
            if (systemTick) return;

            // Skip this step if the party member is already dead.
            if (Health == (int)HealthStatus.Dead) return;

            // Check for illness if the ration level is low.
            if ((GameCore.Instance.Vehicle.Ration == RationLevel.BareBones) ||
                (GameCore.Instance.Vehicle.Ration == RationLevel.Meager && GameCore.Instance.Random.NextBool()))
                CheckIllness();

            // Only consume food if the whole day has passed.
            if (!skipDay) ConsumeFood();
        }

        /// <summary>
        /// Determines how much food party members in the vehicle will consume.
        /// </summary>
        private void ConsumeFood()
        {
            // Skip this step if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            var vehicle = GameCore.Instance.Vehicle;
            if (vehicle.Inventory[Types.Food].Quantity > 0)
            {
                vehicle.Inventory[Types.Food].SubtractQuantity((int)vehicle.Ration * vehicle.Passengers.Where(x => x.HealthState != HealthStatus.Dead).Count());
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
            if (HealthState == HealthStatus.Dead || HealthState == HealthStatus.Good) return;

            // Roll the dice to determine if the party member will even be healed.
            if (GameCore.Instance.Random.NextBool()) return;

            // Heal the party member a greater amount if they are currently sick or hurt.
            Health += GameCore.Instance.Random.Next(1, ((Infected || Injured) ? 20 : 10));
        }

        /// <summary>
        /// Check if the party member has an illness or injury.
        /// </summary>
        private void CheckIllness()
        {
            // Skip this step if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            // Roll the dice to determine if the party member will even be healed.
            var game = GameCore.Instance;
            if (game.Random.NextBool()) return;

            // Reduce the party member's health if the ration level is low.
            if (game.Random.Next(100) <= 10 + 35 * ((int)game.Vehicle.Ration - 1))
                Damage(10, 50);

            // Reduce the party member's health depending on their health and debuff.
            switch (HealthState)
            {
                case HealthStatus.Great:
                    if ((Infected || Injured) && (game.Vehicle.Status != VehicleStatus.Stopped))
                        Damage(10, 50);
                    break;

                case HealthStatus.Good:
                    if ((Infected || Injured) && (game.Vehicle.Status != VehicleStatus.Stopped))
                        if (game.Random.NextBool())
                            Damage(10, 50);
                        else if (!Infected || !Injured)
                            Heal();
                    break;

                case HealthStatus.Fair:
                    if ((Infected || Injured) && (game.Vehicle.Status != VehicleStatus.Stopped))
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
        /// Decrease person's health by a given amount.
        /// </summary>
        /// <param name="amount">Amount of health to decrease</param>
        private void Damage(int amount)
        {
            if (amount > 0) Health -= amount;
        }

        /// <summary>
        /// Decrease person's health by an amount between two values.
        /// </summary>
        /// <param name="minAmount">Minimum amount of damage that should be randomly generated.</param>
        /// <param name="maxAmount">Maximum amount of damage that should be randomly generated.</param>
        /// <remarks>TODO: Show a prompt, notifying the player that a party member has died.</remarks>
        private void Damage(int minAmount, int maxAmount)
        {
            // Skip this step if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            // Subtract a random amount of health from the party member.
            Health -= GameCore.Instance.Random.Next(minAmount, maxAmount);

            if (Health <= (int)HealthStatus.Dead)
                GameCore.Instance.EventDirector.TriggerEvent(this, typeof(PassengerDeath));
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
            // Skip this step if the party member is already dead.
            if (HealthState == HealthStatus.Dead) return;

            Health = (int)HealthStatus.Dead;
        }
    }

    #region ENUMERABLES

    /// <summary>
    /// Defines all the playable classes.
    /// </summary>
    /// <remarks>TODO: Add descriptons and unique stats to each class.</remarks>
    public enum Classes
    {
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