namespace TeufortTrail.Entities.Location
{
    /// <summary>
    /// Defines the trail location that the player will visit in their playthrough.
    /// </summary>
    public abstract class Location
    {
        #region VARIBLES

        /// <summary>
        /// Name of the current location as it should be known to players.
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
    }

    #region ENUM

    /// <summary>
    /// Defines the status of the location in relation to the player's trail progress.
    /// </summary>
    public enum LocationStatus
    {
        Unreached = 0,
        Arrived = 1,
        Departed = 2
    }

    #endregion ENUM
}