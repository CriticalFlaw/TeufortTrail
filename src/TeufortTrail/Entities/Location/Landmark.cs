namespace TeufortTrail.Entities.Location
{
    public sealed class Landmark : Location
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
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Location.Landmark" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        public Landmark(string name) : base(name)
        {
        }
    }
}