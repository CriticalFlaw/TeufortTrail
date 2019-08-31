using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeufortTrail.Entities.Location;
using TeufortTrail.Entities.Vehicle;
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
        /// References to all the locations on this trail, indexed in the order that they will be visited.
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
            // Check the loaded trail is valid, if it is then load it's locations.
            if (locations == null) throw new ArgumentNullException(nameof(locations), "List of locations for the trail is null");
            _locations = new List<Location.Location>(locations);
            if (_locations.Count <= 1) throw new ArgumentException("List of locations count not greater than or equal to two!");

            // Set the trail length values.
            MinLength = minLength;
            MaxLength = maxLength;
            Length = GenerateDistances(_locations);

            // TODO: Flag the last location on the trail.
        }

        /// <summary>
        /// Determine the total lenght of the entire trail.
        /// </summary>
        /// <param name="locations"></param>
        private int GenerateDistances(IEnumerable<Location.Location> locations)
        {
            // Loop through every location, and calculate the total trail length.
            var _totalTrailLength = 0;
            foreach (var location in locations)
            {
                location.TotalDistance = GameCore.Instance.Random.Next(MinLength, MaxLength);
                _totalTrailLength += location.TotalDistance;
            }

            // Return the total length of the entire trail.
            return _totalTrailLength;
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

        /// <summary>
        /// References to all the locations on this trail, indexed in the order that they will be visited.
        /// </summary>
        public ReadOnlyCollection<Location.Location> Locations => Trail.Locations;

        /// <summary>
        /// Determines the location the player and their party are currently in.
        /// </summary>
        public Location.Location CurrentLocation => Trail.Locations[LocationIndex];

        /// <summary>
        /// Distance in miles the player needs to travel before they arrive at the next location.
        /// </summary>
        public int NextLocationDistance { get; private set; }

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
                // Get the index of the next location.
                var nextLocationIndex = LocationIndex + 1;
                // Check that the next point exists on the trail.
                return (nextLocationIndex >= Locations.Count) ? null : Locations[nextLocationIndex];
            }
        }

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
        /// Called when the simulation is ticked.
        /// </summary>
        public override void OnTick(bool systemTick, bool skipDay = false)
        {
            // Only tick vehicle at an inverval.
            if (systemTick) return;

            // Get the current instance of the vehicle.
            var vehicle = GameCore.Instance.Vehicle;

            // Tick the current location, randomizing some of it's variables (TODO)
            CurrentLocation?.OnTick(false);

            // Tick the vehicle, updating it's current total distance travelled.
            vehicle.OnTick(false, skipDay);

            // Prevent trail progress from ticking if the vehicle is not moving.
            if ((vehicle.Status != VehicleStatus.Moving) || skipDay) return;

            // Check if the player is still investigating the location they are currently at.
            if ((CurrentLocation?.Status == LocationStatus.Arrived) && (NextLocationDistance <= 0)) return;

            // Refresh the distance until reaching the next location.
            NextLocationDistance -= vehicle.Mileage;

            // Check for if the distance to the next location is zero, if so "arrive" at that location.
            if (NextLocationDistance >= 0) return;
            NextLocationDistance = 0;
            ArriveAtLocation();
        }

        /// <summary>
        /// Called when it is decided that the player has arrived at the next location.
        /// </summary>
        public void ArriveAtLocation()
        {
            // Check if the end of the trail has been reached.
            if (LocationIndex > Locations.Count) return;

            // Set the distance to the next location.
            NextLocationDistance = CurrentLocation.TotalDistance;

            // Correct the index incrementation if we are on the first turn.
            if (GameCore.Instance.TotalTurns > 0) LocationIndex++;

            // Set the status of the current location as currently being visited.
            CurrentLocation.Status = LocationStatus.Arrived;
            GameCore.Instance.WindowManager.Add(typeof(Travel));
        }
    }
}