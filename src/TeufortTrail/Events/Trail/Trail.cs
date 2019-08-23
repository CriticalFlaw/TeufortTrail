using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeufortTrail.Entities.Location;

namespace TeufortTrail.Events.Trail
{
    public sealed class Trail
    {
        private readonly List<Location> _locations;
        public ReadOnlyCollection<Location> Locations => _locations.AsReadOnly();
        private int MinLength { get; }
        private int MaxLength { get; }
        public int Length { get; }

        public Trail(IEnumerable<Location> locations, int minLength, int maxLength)
        {
            if (locations == null) throw new ArgumentNullException(nameof(locations), "List of locations for the trail is null");

            _locations = new List<Location>(locations);
            MinLength = minLength;
            MaxLength = maxLength;
        }
    }
}