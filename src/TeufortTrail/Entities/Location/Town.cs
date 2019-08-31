namespace TeufortTrail.Entities.Location
{
    public sealed class Town : Location
    {
        /// <summary>
        /// Determines if this location has a store the player can visit.
        /// </summary>
        public override bool ShoppingAllowed => true;

        /// <summary>
        /// Determines if this location has people the player can talk to.
        /// </summary>
        public override bool TalkingAllowed => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Entities.Location.Town" /> class.
        /// </summary>
        /// <param name="name">Name of the town</param>
        public Town(string name) : base(name)
        {
        }
    }
}