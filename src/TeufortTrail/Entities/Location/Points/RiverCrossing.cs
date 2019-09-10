namespace TeufortTrail.Entities.Location
{
    public sealed class RiverCrossing : Location
    {
        /// <summary>
        /// Flags the location as having a store that the player can visit.
        /// </summary>
        public override bool ShoppingAllowed => false;

        /// <summary>
        /// Flags the location as having people that the player can talk to.
        /// </summary>
        public override bool TalkingAllowed => false;

        /// <summary>
        /// Defines the type of river crossing this location will be.
        /// </summary>
        public RiverOptions CrossingOption { get; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="RiverCrossing" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        public RiverCrossing(string name, RiverOptions crossingOption = RiverOptions.Float) : base(name)
        {
            // Set the type of river crossing this location will be.
            CrossingOption = crossingOption;
        }
    }
}