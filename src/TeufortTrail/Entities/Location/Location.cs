namespace TeufortTrail.Entities.Location
{
    public abstract class Location
    {
        public string Name { get; }
        public LocationStatus Status { get; set; }
        public bool IsLastLocation { get; set; }
        public int TotalDistance { get; set; }

        protected Location(string name)
        {
            Name = name;
            Status = LocationStatus.Unreached;
        }
    }

    public enum LocationStatus
    {
        Unreached = 0,
        Arrived = 1,
        Departed = 2
    }
}