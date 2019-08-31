using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Travel;

namespace TeufortTrail.Entities.Trail
{
    public sealed class Trail
    {
        private readonly List<Location.Location> _locations;
        public ReadOnlyCollection<Location.Location> Locations => _locations.AsReadOnly();
        private int MinLength { get; }
        private int MaxLength { get; }
        public int Length { get; }

        public Trail(IEnumerable<Location.Location> locations, int minLength, int maxLength)
        {
            if (locations == null) throw new ArgumentNullException(nameof(locations), "List of locations for the trail is null");

            _locations = new List<Location.Location>(locations);
            MinLength = minLength;
            MaxLength = maxLength;
        }
    }

    public sealed class TrailBase : WolfCurses.Module.Module
    {
        private Trail Trail { get; set; }
        public int LocationIndex { get; private set; }
        public ReadOnlyCollection<Location.Location> Locations => Trail.Locations;
        public Location.Location CurrentLocation => Locations[LocationIndex];
        public bool IsFirstLocation => LocationIndex <= 0;

        public Location.Location NextLocation
        {
            get
            {
                var nextLocationIndex = LocationIndex + 1;
                return (nextLocationIndex >= Locations.Count) ? null : Locations[nextLocationIndex];
            }
        }

        public int NextLocationDistance { get; private set; }

        public TrailBase()
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