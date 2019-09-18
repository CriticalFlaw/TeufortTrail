using TeufortTrail.Screens.Travel.Hunt;
using WolfCurses;

namespace TeufortTrail.Entities.Robot
{
    /// <summary>
    /// Defines a given animal the player can shoot and kill for it's meat. Depending on weather and current conditions the type of animal created may vary.
    /// </summary>
    public sealed class Robot : ITick
    {
        /// <summary>
        /// Defines the entity object related to this robot.
        /// </summary>
        public Item.Item Entity { get; }

        /// <summary>
        /// Determines the total number of seconds this prey has been on the list of possible targets. It'll be removed from the list if the threshold is reached.
        /// </summary>
        public int Lifetime { get; private set; }

        /// <summary>
        /// Maximum amount of time this robot will be on the list of possible targets in a given hunting session.
        /// </summary>
        public int LifetimeMax { get; private set; }

        /// <summary>
        /// Determines the total number of seconds this robot has been a valid target for the player to shoot.
        /// </summary>
        public int TargetTime { get; internal set; }

        /// <summary>
        /// Maximum amount of time that the given target can be identified as such, if this threshold is reached the target will retreat.
        /// </summary>
        public int TargetTimeMax { get; internal set; }

        /// <summary>
        /// Indicates whether the robot has become aware of the player and has decided to retreat.
        /// </summary>
        public bool ShouldRetreat { get; internal set; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Robot" /> class.
        /// </summary>
        public Robot()
        {
            Lifetime = 0;
            // Randomly generate the maximum amount of time this robot will be a possible target.
            LifetimeMax = GameCore.Instance.Random.Next(HuntGenerator.TotalHuntTime);

            TargetTime = 0;
            // Randomly generate the maximum amount of time this robot will be an active target.
            TargetTimeMax = GameCore.Instance.Random.Next(3, 10);

            // Randomly select a robot from the list to be the target.
            Entity = new Item.Item(HuntGenerator.RobotArmy[GameCore.Instance.Random.Next(HuntGenerator.RobotArmy.Count)], 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Robot" /> class from the previous instance.
        /// </summary>
        public Robot(Robot robot)
        {
            // Copy the previous robot instance values into the new instance.
            Lifetime = robot.Lifetime;
            LifetimeMax = robot.LifetimeMax;
            TargetTime = robot.TargetTime;
            TargetTimeMax = robot.TargetTimeMax;
            Entity = new Item.Item(robot.Entity, 1);
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

            // Increment the target timer until it reaches the threshold.
            if (Lifetime < LifetimeMax)
                Lifetime++;
            else if (Lifetime >= LifetimeMax)
                Lifetime = LifetimeMax;
        }

        internal void TickTarget()
        {
            // Skip if the target has retreated.
            if (ShouldRetreat) return;

            // Increment the active target timer until it reaches the threshold.
            if (TargetTime < TargetTimeMax)
                TargetTime++;
            else
            {
                // Mark the robot as retreated.
                TargetTime = TargetTimeMax;
                ShouldRetreat = true;
            }
        }
    }
}