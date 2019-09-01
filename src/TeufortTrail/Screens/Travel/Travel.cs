using System;
using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Menu;
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

        protected override void OnFormChange()
        {
            base.OnFormChange();
            UpdateLocation();
        }

        public override void OnWindowPostCreate()
        {
            UpdateLocation();
            if (GameCore.Instance.Trail.IsFirstLocation && (GameCore.Instance.Trail.CurrentLocation?.Status == LocationStatus.Unreached))
                SetForm(typeof(StoreWelcome));
        }

        public override void OnWindowActivate()
        {
            ArriveAtLocation();
        }

        public override void OnWindowAdded()
        {
            if (!GameCore.Instance.Trail.CurrentLocation.ArrivalFlag) return;
            ArriveAtLocation();
        }

        /// <summary>
        /// Called when the party has arrived at a location.
        /// </summary>
        private void ArriveAtLocation()
        {
            var game = GameCore.Instance;
            // TODO: Check if this is game over or all of the passengers are dead.

            // Check if the party has arrived at the last location.
            if (game.Trail.CurrentLocation.LastLocation)
            {
                SetForm(typeof(GameOver));
                return;
            }

            if (game.Trail.CurrentLocation.Status == LocationStatus.Arrived)
            {
                game.Trail.CurrentLocation.ArrivalFlag = true;
                SetForm(typeof(LocationArrived));
                return;
            }

            UpdateLocation();
        }

        /// <summary>
        /// Called when the town information including party status and available commands need to be displayed.
        /// </summary>
        private void UpdateLocation()
        {
            var _menuHeader = new StringBuilder();
            _menuHeader.Append(TravelInfo.PartyStatus);
            _menuHeader.Append("Here is what you can do:");
            MenuHeader = _menuHeader.ToString();

            // Initialize the available commands and their methods.
            ClearCommands();
            AddCommand(ContinueTrail, TravelCommands.ContinueOnTrail);
            AddCommand(CheckSupplies, TravelCommands.CheckSupplies);
            AddCommand(StopToRest, TravelCommands.StopToRest);

            // Add additional commands depending on the current vehicle state.
            var location = GameCore.Instance.Trail.CurrentLocation;
            switch (location.Status)
            {
                case LocationStatus.Unreached:
                    break;

                case LocationStatus.Arrived:
                    // TOOD: Add trading
                    if (location.TalkingAllowed) AddCommand(TalkToPeople, TravelCommands.TalkToPeople);
                    if (location.ShoppingAllowed) AddCommand(BuySupplies, TravelCommands.BuySupplies);
                    break;

                case LocationStatus.Departed:
                    // TODO: Add trading and food gathering
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Called when the player has chosen to continue on the trail.
        /// </summary>
        internal void ContinueTrail()
        {
            if (GameCore.Instance.Trail.CurrentLocation.Status == LocationStatus.Departed)
            {
                SetForm(typeof(ContinueTrail));
                return;
            }

            // TODO: Add other location types
            if (GameCore.Instance.Trail.CurrentLocation is Town)
                SetForm(typeof(DepartLocation));
        }

        /// <summary>
        /// Called when the player has chosen to rest at the current location.
        /// </summary>
        internal void StopToRest()
        {
            SetForm(typeof(Store.Store));   // TEMP
        }

        /// <summary>
        /// Called when the player has chosen check their inventory and party status.
        /// </summary>
        internal void CheckSupplies()
        {
            SetForm(typeof(Location.CheckSupplies));
        }

        /// <summary>
        /// Called when the player has chosen to talk to the townsfolk for advice.
        /// </summary>
        internal void TalkToPeople()
        {
            SetForm(typeof(Store.Store));   // TEMP
        }

        /// <summary>
        /// Called when the player has chosen to go to the store to buy supplies.
        /// </summary>
        internal void BuySupplies()
        {
            SetForm(typeof(Store.Store));
        }
    }

    /// <summary>
    /// Defines the options available to the player while they are at a location.
    /// </summary>
    public enum TravelCommands
    {
        [Description("Continue on the trail")] ContinueOnTrail = 1,
        [Description("Check supplies")] CheckSupplies = 2,
        [Description("Stop to rest")] StopToRest = 3,
        [Description("Talk to people")] TalkToPeople = 4,
        [Description("Buy supplies")] BuySupplies = 5
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
                var foodCount = game.Vehicle.Inventory[Types.Food];
                var driveStatus = new StringBuilder();
                // TODO: Add time, weather and passenger health statuses
                driveStatus.AppendLine($"Available Food: {((foodCount != null) ? foodCount.TotalWeight : 0)} pounds");
                driveStatus.AppendLine($"Next landmark is in: {game.Trail.NextLocationDistance} miles");
                driveStatus.AppendLine($"You've traveled: {game.Vehicle.Odometer} miles");
                driveStatus.AppendLine("------------------------------------------");
                return driveStatus.ToString();
            }
        }
    }
}