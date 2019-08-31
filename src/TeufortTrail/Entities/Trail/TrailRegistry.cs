using TeufortTrail.Entities.Location;

namespace TeufortTrail.Entities.Trail
{
    /// <summary>
    /// Defines the complete trails that the player can travel on.
    /// </summary>
    public static class TrailRegistry
    {
        /// <summary>
        /// The Teufort Trail, taking the player across the world of Team Fortress 2
        /// </summary>
        public static Trail TeufortTrail
        {
            get
            {
                var debugTrail = new Location.Location[]
                {
                    new Town("Start Of Test"),
                    new Town("End Of Test")
                };

                return new Trail(debugTrail, 50, 100);
            }
        }

        // TODO: Add more trails.
    }
}