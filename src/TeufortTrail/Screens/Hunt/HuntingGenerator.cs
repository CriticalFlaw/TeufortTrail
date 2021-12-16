using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Robot;
using WolfCurses;

namespace TeufortTrail.Screens.Hunt
{
    public sealed class HuntGenerator : ITick
    {
        /// <summary>
        /// Defines the target robot that's been destroyed by the player.
        /// </summary>
        private Robot _robotTarget;

        /// <summary>
        /// References all of the robots created for this hunting session.
        /// </summary>
        private readonly List<Robot> _robotList;

        /// <summary>
        /// References all of the robots that have escaped the player.
        /// </summary>
        private readonly List<Robot> _robotsEscaped;

        /// <summary>
        /// References all of the robots that have destroyed by the player.
        /// </summary>
        private readonly List<Robot> _robotsDestroyed;

        /// <summary>
        /// Get the last known robot that has fled the hunting grounds.
        /// </summary>
        public Robot LastEscaped => _robotsEscaped.LastOrDefault();

        /// <summary>
        /// Get the last known robot destroyed by the player.
        /// </summary>
        public Robot LastDestroyed => _robotsDestroyed.LastOrDefault();

        /// <summary>
        /// The current hunting word that the player has to type out.
        /// </summary>
        public HuntingWord HuntWord { get; internal set; }

        /// <summary>
        /// References all of the words the player can type to shoot robots.
        /// </summary>
        public List<HuntingWord> HuntWordList { get; internal set; }

        /// <summary>
        /// Indicates that there's a robot currently in sight for the player to shoot.
        /// </summary>
        public bool RobotAvailable => HuntWord != HuntingWord.None;

        /// <summary>
        /// Total amount of time remaining in the current hunting session, measured in ticks.
        /// </summary>
        public int HuntTime;

        /// <summary>
        /// Total amount of time that every hunting session is given, measured in ticks.
        /// </summary>
        public const int TotalHuntTime = 30;

        /// <summary>
        /// Calls the event triggered by a robot target escaping.
        /// </summary>
        public event TargetFlee TargetFledEvent;

        /// <summary>
        /// Sends data to the hooked objects that want to know when a robot has escaped.
        /// </summary>
        public delegate void TargetFlee(Robot target);

        /// <summary>
        /// Determine the total amount of money collected from destroyed robots.
        /// </summary>
        public double MoneyCollected
        {
            get
            {
                if (_robotsDestroyed.Count <= 0) return 0;
                return _robotsDestroyed.Aggregate(0.00, (current, preyItem) => current + preyItem.Entity.TotalValue);
            }
        }

        /// <summary>
        /// References the list of all robots in the game, used to help determine which ones to spawn in a given session.
        /// </summary>
        internal static IList<Item> RobotArmy
        {
            get
            {
                var robotArmy = new List<Item>
                {
                    Entities.Robot.RobotArmy.Scout,
                    Entities.Robot.RobotArmy.Soldier,
                    Entities.Robot.RobotArmy.Pyro,
                    Entities.Robot.RobotArmy.Demoman,
                    Entities.Robot.RobotArmy.Heavy,
                    Entities.Robot.RobotArmy.Medic,
                    Entities.Robot.RobotArmy.Engineer,
                    Entities.Robot.RobotArmy.Sniper,
                    Entities.Robot.RobotArmy.Spy
                };

                foreach (var robot in robotArmy)
                    robot.SubtractQuantity(robot.MaxQuantity);
                return robotArmy;
            }
        }

        /// <summary>
        /// Displays the active hunting session status.
        /// </summary>
        public string HuntInfo
        {
            get
            {
                var huntInfo = new StringBuilder();
                huntInfo.AppendLine((GameCore.Instance.Trail.CurrentLocation.Status != LocationStatus.Departed)
                    ? $"Hunting outside {GameCore.Instance.Trail.CurrentLocation.Name}"
                    : $"Hunting near {GameCore.Instance.Trail.NextLocation.Name}");
                huntInfo.AppendLine("------------------------------------------");

                // Represent seconds remaining as daylight left percentage.
                huntInfo.AppendLine($"Daylight Remaining: {(HuntTime / (decimal)TotalHuntTime) * 100:N0}%");
                //_huntInfo.AppendLine($"Robots Remaining:   {RobotsList.Count:N0} ");
                huntInfo.AppendLine($"Current Target:     {(_robotTarget != null ? "ROBOT " + _robotTarget.Entity.Name.ToUpperInvariant() : "")}{Environment.NewLine}");

                // Prompt the player with information about what to do.
                huntInfo.Append((HuntWord != HuntingWord.None)
                    ? $"Type in the word '{HuntWord.ToString().ToUpperInvariant()}' to take a shot!"
                    : "Waiting for a robot to appear...");
                return huntInfo.ToString();
            }
        }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="HuntGenerator" /> class.
        /// </summary>
        public HuntGenerator()
        {
            _robotList = GenerateRobots();
            _robotsEscaped = new List<Robot>();
            _robotsDestroyed = new List<Robot>();
            HuntTime = TotalHuntTime;
            HuntWordList = Enum.GetValues(typeof(HuntingWord)).Cast<HuntingWord>().ToList();
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        public void OnTick(bool systemTick, bool skipDay)
        {
            // Skip if this is a system or a day tick.
            if (systemTick || skipDay) return;

            // Check that there's time remaining in the hunt.
            if (HuntTime <= 0) return;

            // Remove one second from the remaining total hunt time.
            HuntTime--;

            // Tick the active robot target's awareness.
            TickAwareness();

            // Loop through every spawned robot. Remove inactive ones, otherwise tick.
            var robotList = new List<Robot>(_robotList);
            foreach (var robot in robotList)
                if (robot.Lifetime >= robot.LifetimeMax)
                    _robotList.Remove(robot);
                else
                    robot.OnTick(false, false);

            // Randomly select the next robot target and key word for the player.
            PickNextRobot();
        }

        /// <summary>
        /// Tick the current robot target. Check for the key word and if the robot has retreated.
        /// </summary>
        private void TickAwareness()
        {
            // Check if there's an active target. Roll the die to continue.
            if (_robotTarget == null || GameCore.Instance.Random.NextBool()) return;

            // Check that there's an active hunting word, if not pick one.
            if ((_robotTarget != null) && (HuntWord != HuntingWord.None))
                _robotTarget.TickTarget();

            // Trigger an event if the robot has retreated.
            if (!_robotTarget.ShouldRetreat) return;
            _robotsEscaped.Add(new Robot(_robotTarget));
            TargetFledEvent?.Invoke(_robotTarget);
            ClearTarget();
        }

        /// <summary>
        /// Randomly select the next "shooting" key word and a robot target if one wasn't already set. Clear the word if there's no target.
        /// </summary>
        private void PickNextRobot()
        {
            // Skip this step if there's no active target, robot list or the dice roll returns a negative.
            if (_robotTarget != null || _robotList.Count <= 0 || GameCore.Instance.Random.NextBool()) return;

            // Randomly select a key word, check that it isn't empty or same as the currently active word.
            var currentWord = (HuntingWord)GameCore.Instance.Random.Next(HuntWordList.Count);
            if (currentWord == HuntingWord.None || currentWord == HuntWord) return;
            HuntWord = currentWord;

            // Randomly select robot from the list. Check that its activity timer hasn't expired.
            var randomRobot = _robotList[GameCore.Instance.Random.Next(_robotList.Count)];
            if (randomRobot.Lifetime > randomRobot.LifetimeMax) return;
            _robotTarget = new Robot(randomRobot);

            // Remove the selected robot from the list so it does not get picked again.
            _robotList.Remove(randomRobot);
        }

        /// <summary>
        /// Generate random number of robots for the hunting session.
        /// </summary>
        private List<Robot> GenerateRobots()
        {
            var robotList = new List<Robot>();
            for (var index = 0; index < GameCore.Instance.Random.Next(1, 15); index++)
                robotList.Add(new Robot());
            return robotList.OrderByDescending(x => x.LifetimeMax).Distinct().ToList();
        }

        /// <summary>
        /// Clears the current robot target, key word and input buffer.
        /// </summary>
        private void ClearTarget()
        {
            _robotTarget = null;
            HuntWord = HuntingWord.None;
            GameCore.Instance.InputManager.ClearBuffer();
        }

        /// <summary>
        /// Determine if the player has successfully destroyed the robot. Depending on how long it takes them to type the shooting word correctly, and a roll of the dice will determine if they hit their mark or not.
        /// </summary>
        internal bool DestroyRobot()
        {
            // Skip this step if there's no active target.
            if (_robotTarget == null) return false;

            // Subtract ammo from the supply after the player takes a shot.
            var ammoSupply = GameCore.Instance.Vehicle.Inventory[ItemTypes.Ammo];
            ammoSupply.SubtractQuantity((int)ammoSupply.TotalValue - 10 - GameCore.Instance.Random.Next() * 4);

            // Check if the player missed their target based on the calculation or if the player fired in less than half the maximum target time.
            if (100 * GameCore.Instance.Random.Next() < ((int)GameCore.Instance.Vehicle.Passengers.First(x => x.Leader).Class - 13) * _robotTarget.TargetTime ||
                _robotTarget.TargetTime > _robotTarget.TargetTimeMax / 2)
            {
                // Clear the target and mark the robot as escaped if the shot missed.
                _robotsEscaped.Add(_robotTarget);
                ClearTarget();
                return false;
            }

            // Add the robot to the list of destroyed robots.
            _robotsDestroyed.Add(_robotTarget);
            ClearTarget();
            return true;
        }
    }
}