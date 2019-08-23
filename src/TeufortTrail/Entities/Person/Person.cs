using System;
using TeufortTrail.Entities.Item;

namespace TeufortTrail.Entities.Person
{
    internal class Person
    {
        public Classes Class { get; }
        public bool Leader { get; }
        private bool Infected { get; set; }
        private bool Injured { get; set; }
        private int Status { get; set; }

        private bool IsDead;

        public Person(Classes _class, bool leader)
        {
            Class = _class;
            Leader = leader;
            Infected = false;
            Injured = false;
            Status = (int)HealthStatus.Good;
        }

        public void OnTick(bool systemTick, bool skipDay)
        {
        }

        private void ConsumeFood()
        {
            if (IsDead || Status == (int)HealthStatus.Dead) return;

            if (GameCore.Instance.Vehicle.Inventory[Categories.Food].Quantity > 0)
                Heal();
            else
                Damage(10, 50);
        }

        private void Heal()
        {
            if (IsDead || Status == (int)HealthStatus.Dead || Status == (int)HealthStatus.Good)
                return;

            var game = GameCore.Instance;
            if (game.Random.NextBool()) return;

            if (Infected || Injured)
                Status += game.Random.Next(1, 20);
            else
                Status += game.Random.Next(1, 10);
        }

        private void HealFull()
        {
            Status = (int)HealthStatus.Great;
            Infected = false;
            Injured = false;
        }

        private void CheckIllness()
        {
            var game = GameCore.Instance;
            if ((Status == (int)HealthStatus.Dead) || IsDead) return;
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
                    IsDead = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Damage(int amount)
        {
            if (amount > 0)
                Status -= amount;
        }

        private void Damage(int minAmount, int maxAmount)
        {
            Status -= GameCore.Instance.Random.Next(minAmount, maxAmount);
        }

        private void Infect()
        {
            Infected = true;
        }

        private void Injure()
        {
            Injured = true;
        }

        private void Kill()
        {
            Status = (int)HealthStatus.Dead;
        }
    }

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

    public enum HealthStatus
    {
        Great = 500,
        Good = 400,
        Fair = 300,
        Poor = 200,
        Dead = 0
    }

    public enum RationLevel
    {
        Filling = 1,
        Meager = 2,
        BareBones = 3
    }
}