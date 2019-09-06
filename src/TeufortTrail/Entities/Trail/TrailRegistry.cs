using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.River;

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
                    new Settlement("Dustbowl"),
                    new RiverCrossing("Badwater Basin", RiverOptions.Ferry),
                    new Landmark("The Well"),
                    new Settlement("Coal Town"),
                    new Landmark("Ghost Town"),
                    new RiverCrossing("Hale River", RiverOptions.Float),
                    new Settlement("Manhattan"),
                    new Landmark("DeGroot Keep"),
                    new RiverCrossing("2Fort", RiverOptions.Float),
                    new Settlement("Gravel Pit"),
                    new Settlement("Teufort, New Mexico")
                };

                return new Trail(teufortTrail, 30, 150);
            }
        }
    }
}