namespace TeufortTrail.Entities.Location
{
    /// <summary>
    /// Defines the trail location that the player will visit in their playthrough.
    /// </summary>
    public abstract class Location
    {
        #region VARIBLES

        /// <summary>
        /// Display name of the location as it should be known to the player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Defines the status of the location in relation to the player's trail progress.
        /// </summary>
        public LocationStatus Status { get; set; }

        /// <summary>
        /// Total distance to the next location that the player must travel.
        /// </summary>
        public int TotalDistance { get; set; }

        /// <summary>
        /// Flags the location as just being arrived at by the player.
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

        #endregion VARIBLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Location.Location" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        protected Location(string name)
        {
            Name = name;
            Status = LocationStatus.Unreached;
        }

        /// <summary>
        /// Called when the simulation is ticked.
        /// </summary>
        public void OnTick(bool systemTick)
        {
            // Only tick vehicle at an inverval.
            if (systemTick) return;
        }
    }

    #region ENUMERABLES

    /// <summary>
    /// Defines the status of the location in relation to the player's trail progress.
    /// </summary>
    public enum LocationStatus
    {
        Unreached = 0,
        Arrived = 1,
        Departed = 2
    }

    #endregion ENUMERABLES
}