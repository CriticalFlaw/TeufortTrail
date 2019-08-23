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
        public Travel(SimulationApp app) : base(app)
        {
        }

        public override void OnWindowPostCreate()
        {
            SetForm(typeof(StoreWelcome));
        }

        internal void ContinueTrail()
        {
            if (GameCore.Instance.Trail.CurrentLocation is Town)
                SetForm(typeof(DepartLocation));
        }
    }

    public enum TravelCommands
    {
        [Description("Continue on the trail")] ContinueOnTrail = 1,
        [Description("Stop to rest")] StopToRest = 2,
        [Description("Talk to people")] TalkToPeople = 3,
        [Description("Buy supplies")] BuySupplies = 4,
        [Description("Check supplies")] CheckSupplies = 5
    }

    public sealed class TravelInfo : WindowData
    {
        public StoreGenerator Store { get; }

        public TravelInfo()
        {
            Store = new StoreGenerator();
        }

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