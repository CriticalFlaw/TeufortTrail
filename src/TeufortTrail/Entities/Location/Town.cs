namespace TeufortTrail.Entities.Location
{
    public sealed class Town : Location
    {
        #region VARIABLES

        /// <summary>
        /// Flags the location as having a store that the player can visit.
        /// </summary>
        public override bool ShoppingAllowed => true;

        /// <summary>
        /// Flags the location as having people that the player can talk to.
        /// </summary>
        public override bool TalkingAllowed => true;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Location.Town" /> class.
        /// </summary>
        /// <param name="name">Name of the town</param>
        public Town(string name) : base(name)
        {
        }
    }
}