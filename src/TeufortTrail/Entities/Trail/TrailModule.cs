using System.Collections.ObjectModel;
using TeufortTrail.Screens.Travel;

namespace TeufortTrail.Entities.Trail
{
    public sealed class TrailModule : WolfCurses.Module.Module
    {
        /// <summary>
        /// References to the trail that has been loaded for play.
        /// </summary>
        private Trail Trail { get; set; }

        /// <summary>
        /// References to all the locations on this trail, indexed in the order that they will be visited.
        /// </summary>
        public ReadOnlyCollection<Location.Location> Locations => Trail.Locations;

        /// <summary>
        /// Location the party are currently in, as an index value.
        /// </summary>
        public int LocationIndex { get; private set; }

        /// <summary>
        /// Location the party are currently in.
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
        /// Retrieve the next location on the trail. If there isn't one then the player has reached the end.
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

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="TrailModule" /> class.
        /// </summary>
        public TrailModule()
        {
            Trail = TrailRegistry.TeufortTrail;
            LocationIndex = 0;
            NextLocationDistance = 0;
        }

        /// <summary>
        /// Called when the game is closing and needs to clean up resources.
        /// </summary>
        public override void Destroy()
        {
            Trail = null;
            LocationIndex = 0;
            NextLocationDistance = 0;
        }

        /// <summary>
        /// Called when the simulation is ticked at a fixed or unpredictable interval.
        /// </summary>
        /// <param name="systemTick">TRUE if ticked unpredictably by an underlying system. FALSE if ticked by the game simulation at a fixed interval.</param>
        /// <param name="skipDay">TRUE if the game has forced a tick without advancing the game progression. FALSE otherwise.</param>
        public override void OnTick(bool systemTick, bool skipDay = false)
        {
            // Only tick at an interval.
            if (systemTick) return;

            // Tick the current location.
            CurrentLocation?.OnTick(false);

            // Tick the vehicle, update the total distance traveled.
            GameCore.Instance.Vehicle.OnTick(false, skipDay);

            // Prevent trail progress from ticking if the vehicle is not moving.
            if ((GameCore.Instance.Vehicle.Status != VehicleStatus.Moving) || skipDay) return;

            // Check if the player is still stopped at the current a location.
            if ((CurrentLocation?.Status == LocationStatus.Arrived) && (NextLocationDistance <= 0)) return;

            // Refresh the distance until the next location is reached.
            NextLocationDistance -= GameCore.Instance.Vehicle.Mileage;

            // Check for if the distance to the next location is zero, if so "arrive" at the location.
            if (NextLocationDistance >= 0) return;
            NextLocationDistance = 0;
            ArriveAtLocation();
        }

        /// <summary>
        /// Called when it is decided that the player has arrived at the next location.
        /// </summary>
        public void ArriveAtLocation()
        {
            // Check the player has reached the end of the trail.
            if (LocationIndex > Locations.Count) return;

            // Set the distance to the next location.
            NextLocationDistance = CurrentLocation.TotalDistance;

            // Correct the index incrementation if we are on the first turn.
            if (GameCore.Instance.TotalTurns > 0) LocationIndex++;

            // Set the status of the current location as currently being visited.
            CurrentLocation.Status = LocationStatus.Arrived;
            GameCore.Instance.WindowManager.Add(typeof(Travel));
        }

        /// <summary>
        /// Insert a location into the trail, recalculate the last location.
        /// </summary>
        /// <param name="location">Location that will be inserted into the list.</param>
        public void InsertLocation(Location.Location location)
        {
            Trail.InsertLocation(LocationIndex + 1, location);
        }
    }
}