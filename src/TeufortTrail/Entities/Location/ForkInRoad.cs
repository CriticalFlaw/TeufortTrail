using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TeufortTrail.Entities.Location
{
    public sealed class ForkInRoad : Location
    {
        #region VARIABLES

        /// <summary>
        /// Flags the location as having a store that the player can visit.
        /// </summary>
        public override bool ShoppingAllowed => false;

        /// <summary>
        /// Flags the location as having people that the player can talk to.
        /// </summary>
        public override bool TalkingAllowed => false;

        /// <summary>
        /// List of possible paths that the player can choose from.
        /// </summary>
        public ReadOnlyCollection<Location> PathChoices => _pathChoices.AsReadOnly();

        public readonly List<Location> _pathChoices;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Location.ForkInRoad" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        public ForkInRoad(string name, IEnumerable<Location> paths) : base(name)
        {
            if (paths != null) _pathChoices = new List<Location>(paths);
        }
    }
}