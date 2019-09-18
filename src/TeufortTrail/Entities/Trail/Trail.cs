using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TeufortTrail.Entities.Location;

namespace TeufortTrail.Entities.Trail
{
    public sealed class Trail
    {
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

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Trail" /> class.
        /// </summary>
        /// <param name="locations">List of locations in the order they will be visited on the trail.</param>
        /// <param name="minLength">Minimum length of any given trail segment.</param>
        /// <param name="maxLength">Maximum length of any given trail segment.</param>
        public Trail(IEnumerable<Location.Location> locations, int minLength, int maxLength)
        {
            // Check that the loaded trail is valid, if it is then load it's locations.
            if (locations == null) throw new ArgumentNullException(nameof(locations), "List of locations for the trail is null");
            _locations = new List<Location.Location>(locations);
            if (_locations.Count <= 1) throw new ArgumentException("List of locations count not greater than or equal to two!");

            // Set the trail length values.
            MinLength = minLength;
            MaxLength = maxLength;
            Length = GenerateDistances(_locations);

            // Mark the last location on the trail.
            FlagLastLocation();
        }

        /// <summary>
        /// Generate the distance between each location on the trail.
        /// </summary>
        /// <param name="locations">List of locations in the order they will be visited on the trail.</param>
        private int GenerateDistances(IEnumerable<Location.Location> locations)
        {
            var _totalDistance = 0;
            foreach (var location in locations)
            {
                // Loop through every location, calculate the distance between points, add to the total.
                location.TotalDistance = GameCore.Instance.Random.Next(MinLength, MaxLength);
                _totalDistance += location.TotalDistance;

                // If the location is a fork, generate the distances for its children.
                if (location is ForkInRoad)
                {
                    var forkInRoad = location as ForkInRoad;
                    GenerateDistances(forkInRoad.PathChoices);
                }
            }
            return _totalDistance;
        }

        /// <summary>
        /// Find and mark the last location on the trail.
        /// </summary>
        private void FlagLastLocation()
        {
            // Clear the flag from any location that may have one.
            foreach (var location in _locations)
                location.LastLocation = false;

            // Flag the last location on the trail being as such.
            _locations.Last().LastLocation = true;
        }

        /// <summary>
        /// Insert a location into the trail, recalculate the last location.
        /// </summary>
        /// <param name="index">Index into which the location will be inserted in the list.</param>
        /// <param name="location">Location that will be inserted into the list.</param>
        public void InsertLocation(int index, Location.Location location)
        {
            // Re-calculate the last location on the trail if a location has been added mid-game.
            _locations.Insert(index, location);
            FlagLastLocation();
        }
    }
}