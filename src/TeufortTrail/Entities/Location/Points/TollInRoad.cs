﻿namespace TeufortTrail.Entities.Location
{
    public sealed class TollInRoad : Location
    {
        /// <summary>
        /// Flags the location as having a store that the player can visit.
        /// </summary>
        public override bool ShoppingAllowed => false;

        /// <summary>
        /// Flags the location as having people that the player can talk to.
        /// </summary>
        public override bool TalkingAllowed => false;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="TollInRoad" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        public TollInRoad(string name) : base(name)
        {
        }
    }
}