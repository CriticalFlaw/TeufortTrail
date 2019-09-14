namespace TeufortTrail.Entities.Location
{
    /// <summary>
    /// Defines the trail location that the player will visit in their playthrough.
    /// </summary>
    public abstract class Location : IEntity
    {
        /// <summary>
        /// Display name of the location as it should be known to the player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Defines the status of the location in relation to player's trail progression.
        /// </summary>
        public LocationStatus Status { get; set; }

        /// <summary>
        /// Distance to the next location that the player must travel.
        /// </summary>
        public int TotalDistance { get; set; }

        /// <summary>
        /// Flags the location as having just been arrived at by the player.
        /// </summary>
        public bool ArrivalFlag { get; set; }

        /// <summary>
        /// Flags the location as being the last on the trail.
        /// </summary>
        public bool LastLocation { get; set; }

        /// <summary>
        /// Flags the location as having a store that the player can visit.
        /// </summary>
        public abstract bool ShoppingAllowed { get; }

        /// <summary>
        /// Flags the location as having people that the player can talk to.
        /// </summary>
        public abstract bool TalkingAllowed { get; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Location" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        protected Location(string name)
        {
            Name = name;
            Status = LocationStatus.Unreached;
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        public void OnTick(bool systemTick, bool skipDay = false)
        {
            // Only tick at an interval.
            if (systemTick) return;
        }
    }
}