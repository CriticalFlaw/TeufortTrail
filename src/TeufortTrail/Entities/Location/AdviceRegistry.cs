namespace TeufortTrail.Entities.Location
{
    public static class AdviceRegistry
    {
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

        public static Advice[] Town
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
    }

    public sealed class Advice
    {
        #region VARIABLES

        public string Name { get; }
        public string Quote { get; }

        #endregion VARIABLES

        public Advice(string name, string quote)
        {
            Name = name;
            Quote = quote;
        }
    }
}