using System.Collections.ObjectModel;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Travel;

namespace TeufortTrail.Events.Trail
{
    public sealed class TrailModule : WolfCurses.Module.Module
    {
        private Trail Trail { get; set; }
        public int LocationIndex { get; private set; }
        public ReadOnlyCollection<Location> Locations => Trail.Locations;
        public Location CurrentLocation => Locations[LocationIndex];
        public bool IsFirstLocation => LocationIndex <= 0;

        public Location NextLocation
        {
            get
            {
                var nextLocationIndex = LocationIndex + 1;
                return (nextLocationIndex >= Locations.Count) ? null : Locations[nextLocationIndex];
            }
        }

        public int NextLocationDistance { get; private set; }

        public TrailModule()
        {
            Trail = TrailRegistry.TeufortTrail;
            LocationIndex = 0;
            NextLocationDistance = 0;
        }

        public override void Destroy()
        {
            Trail = null;
            LocationIndex = 0;
            NextLocationDistance = 0;
        }

        public void ArriveAtLocation()
        {
            if (LocationIndex > Locations.Count) return;
            NextLocationDistance = CurrentLocation.TotalDistance;
            if (GameCore.Instance.TotalTurns > 0)
                LocationIndex++;
            CurrentLocation.Status = LocationStatus.Arrived;
            GameCore.Instance.WindowManager.Add(typeof(Travel));
        }
    }
}