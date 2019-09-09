using WolfCurses.Utility;

namespace TeufortTrail.Entities.Location
{
    public sealed class RiverCrossing : Location
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
        /// Defines the type of river crossing this location will be.
        /// </summary>
        public RiverOptions RiverCrossOption { get; }

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Location.River" /> class.
        /// </summary>
        /// <param name="name">Name of the location</param>
        public RiverCrossing(string name, RiverOptions riverOption = RiverOptions.Float) : base(name)
        {
            // Set the type of river crossing this location will be.
            RiverCrossOption = riverOption;
        }
    }

    /// <summary>
    /// Defines the different types of river crossings, which will have different scenario and options for the player.
    /// </summary>
    public enum RiverCrossChoice
    {
        None = 0,
        [Description("Ford across the river.")] Float = 1,
        [Description("Take a ferry across.")] Ferry = 2,
        [Description("Ask locals for help.")] Help = 3
    }

    /// <summary>
    /// Defines the different types of river crossings, which will have different scenario and options for the player.
    /// </summary>
    public enum RiverOptions
    {
        None = 0,
        Float = 1,
        Ferry = 2,
        Help = 3
    }
}