using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Travel.Store;
using WolfCurses;
using WolfCurses.Utility;
using WolfCurses.Window;

namespace TeufortTrail.Screens.Travel
{
    public sealed class Travel : Window<TravelCommands, TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Travel" /> class.
        /// </summary>
        public Travel(SimulationApp app) : base(app)
        {
        }

        public override void OnWindowPostCreate()
        {
            SetForm(typeof(StoreWelcome));
        }

        /// <summary>
        /// Called when the player has chosen to continue on the trail.
        /// </summary>
        internal void ContinueTrail()
        {
            if (GameCore.Instance.Trail.CurrentLocation is Town)
                SetForm(typeof(DepartLocation));
        }
    }

    /// <summary>
    /// Defines the options available to the player while they are at a location.
    /// </summary>
    public enum TravelCommands
    {
        [Description("Continue on the trail")] ContinueOnTrail = 1,
        [Description("Stop to rest")] StopToRest = 2,
        [Description("Talk to people")] TalkToPeople = 3,
        [Description("Buy supplies")] BuySupplies = 4,
        [Description("Check supplies")] CheckSupplies = 5
    }

    /// <summary>
    /// Retrieves the current party status information such as how many resources are left and how long until the next stop.
    /// </summary>
    public sealed class TravelInfo : WindowData
    {
        /// <summary>
        /// References the store object used for keeping tracking of on-going purchase transactions.
        /// </summary>
        public StoreGenerator Store { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.TravelInfo" /> class.
        /// </summary>
        public TravelInfo()
        {
            Store = new StoreGenerator();
        }

        /// <summary>
        /// Retrieves the current party status, resources and distance until next location.
        /// </summary>
        public static string PartyStatus
        {
            get
            {
                var game = GameCore.Instance;
                var foodCount = game.Vehicle.Inventory[Categories.Food];
                var driveStatus = new StringBuilder();
                driveStatus.AppendLine("--------------------------------");
                // TODO: Add time, weather and passenger health statuses
                driveStatus.AppendLine($"Food: {((foodCount != null) ? foodCount.TotalWeight : 0)} pounds");
                driveStatus.AppendLine($"Next landmark: {game.Trail.NextLocationDistance} miles");
                driveStatus.AppendLine($"Miles traveled: {game.Vehicle.Mileage} miles");
                driveStatus.AppendLine("--------------------------------");
                return driveStatus.ToString();
            }
        }
    }
}