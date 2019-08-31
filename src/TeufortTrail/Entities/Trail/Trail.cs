using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Travel;

namespace TeufortTrail.Entities.Trail
{
    public sealed class Trail
    {
        #region VARIABLES

        /// <summary>
        /// Minimum length of any given trail segment.
        /// </summary>
        private int MinLength { get; }

        /// <summary>
        /// Maximum length of any given trail segment.
        /// </summary>
        private int MaxLength { get; }

        /// <summary>
        /// Total length of the entire trail, distance between points is calculated randomly.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// References to all the locations in this trail, indexed in order of they will be visited.
        /// </summary>
        public ReadOnlyCollection<Location.Location> Locations => _locations.AsReadOnly();

        private readonly List<Location.Location> _locations;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Trail.Trail" /> class.
        /// </summary>
        /// <param name="locations">List of locations in the order they will be visited along the trail</param>
        /// <param name="minLength">Minimum length of any given trail segment.</param>
        /// <param name="maxLength">Maximum length of any given trail segment.</param>
        public Trail(IEnumerable<Location.Location> locations, int minLength, int maxLength)
        {
            if (locations == null) throw new ArgumentNullException(nameof(locations), "List of locations for the trail is null");
            MinLength = minLength;
            MaxLength = maxLength;
            _locations = new List<Location.Location>(locations);
        }
    }

    public sealed class TrailBase : WolfCurses.Module.Module
    {
        #region VARIABLES

        /// <summary>
        /// References to the trail that has been loaded for the player by the game.
        /// </summary>
        private Trail Trail { get; set; }

        /// <summary>
        /// Determines the location the player and their party are currently in as an index value.
        /// </summary>
        public int LocationIndex { get; private set; }

        public ReadOnlyCollection<Location.Location> Locations => Trail.Locations;

        /// <summary>
        /// Determines the location the player and their party are currently in as a location entity.
        /// </summary>
        public Location.Location CurrentLocation => Locations[LocationIndex];

        /// <summary>
        /// Determines if the current location is the first one of the game.
        /// </summary>
        public bool IsFirstLocation => LocationIndex <= 0;

        /// <summary>
        /// Retrieve the next location in the list. If there isn't one then the player has reached the end.
        /// </summary>
        public Location.Location NextLocation
        {
            get
            {
                var nextLocationIndex = LocationIndex + 1;
                return (nextLocationIndex >= Locations.Count) ? null : Locations[nextLocationIndex];
            }
        }

        /// <summary>
        /// Distance in miles the player needs to travel before they arrive at the next location.
        /// </summary>
        public int NextLocationDistance { get; private set; }

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Trail.TrailBase" /> class.
        /// </summary>
        public TrailBase()
        {
            Trail = TrailRegistry.TeufortTrail;
            LocationIndex = 0;
            NextLocationDistance = 0;
        }

        /// <summary>
        /// Called when the game is closing and needs to clean up data remanents.
        /// </summary>
        public override void Destroy()
        {
            Trail = null;
            LocationIndex = 0;
            NextLocationDistance = 0;
        }

        /// <summary>
        /// Called when it is decided that the player has arrived at the next location.
        /// </summary>
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