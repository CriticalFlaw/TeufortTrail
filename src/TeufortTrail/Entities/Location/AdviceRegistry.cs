namespace TeufortTrail.Entities.Location
{
    /// <summary>
    /// Defines the advice the player can receive from civilians depending on the location type.
    /// </summary>
    public static class AdviceRegistry
    {
        public static Advice[] Landmark
        {
            get
            {
                var advice = new[]
                {
                    new Advice("Slack jawed yokel", "There are very few sources of replenishing your health on the trail, so be sure to stock up on food before you depart.")
                };
                return advice;
            }
        }

        public static Advice[] River
        {
            get
            {
                var advice = new[]
                {
                    new Advice("Slack jawed yokel", "There are very few sources of replenishing your health on the trail, so be sure to stock up on food before you depart.")
                };
                return advice;
            }
        }

        public static Advice[] Settlement
        {
            get
            {
                var advice = new[]
                {
                    new Advice("Slack jawed yokel", "There are very few sources of replenishing your health on the trail, so be sure to stock up on food before you depart.")
                };
                return advice;
            }
        }

        public static Advice[] Default
        {
            get
            {
                var advice = new[]
                {
                    new Advice("A town resident", "You don't respawn on the Teufort Trail, so don't die!")
                };
                return advice;
            }
        }
    }

    public sealed class Advice
    {
        /// <summary>
        /// Display name of the civilian as it should be known to the player.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Civilian's advice to the player.
        /// </summary>
        public string Quote { get; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Advice" /> class.
        /// </summary>
        public Advice(string name, string quote)
        {
            Name = name;
            Quote = quote;
        }
    }
}