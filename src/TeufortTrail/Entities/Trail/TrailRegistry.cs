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
                var teufortTrail = new Location.Location[]
                {
                    new Town("Dustbowl"),
                    new Town("Teufort, New Mexico")
                };

                return new Trail(teufortTrail, 50, 100);
            }
        }

        // TODO: Add more trails.
    }
}