namespace TeufortTrail.Entities.Location
{
    public sealed class TollInRoad : Location
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

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Location.TollInRoad" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        public TollInRoad(string name) : base(name)
        {
        }
    }
}