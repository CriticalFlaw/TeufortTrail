namespace TeufortTrail.Entities.Location
{
    public sealed class Settlement : Location
    {
        /// <summary>
        /// Flags the location as having a store that the player can visit.
        /// </summary>
        public override bool ShoppingAllowed => true;

        /// <summary>
        /// Flags the location as having people that the player can talk to.
        /// </summary>
        public override bool TalkingAllowed => true;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Settlement" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        public Settlement(string name) : base(name)
        {
        }
    }
}