using Newtonsoft.Json;

namespace TeufortTrail
{
    public class GameSettings
    {
        [JsonProperty("STARTING_MONEY")]
        public int STARTING_MONEY { get; set; }
    }
}