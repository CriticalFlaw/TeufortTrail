using TeufortTrail.Entities.Location;

namespace TeufortTrail.Events.Trail
{
    public static class TrailRegistry
    {
        public static Trail TeufortTrail
        {
            get
            {
                var debugTrail = new Location[]
                {
                    new Town("Start Of Test"),
                    new Town("End Of Test")
                };

                return new Trail(debugTrail, 50, 100);
            }
        }
    }
}