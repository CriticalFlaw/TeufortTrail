using System;
using System.Collections.Generic;
using System.Text;
using TeufortTrail.Entities.Item;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Menu;
using TeufortTrail.Screens.River;
using TeufortTrail.Screens.Travel.River;
using TeufortTrail.Screens.Travel.Store;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Control;

namespace TeufortTrail.Screens.Travel
{
    public sealed class Travel : Window<TravelCommands, TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Travel" /> class.
        /// </summary>
        public Travel(GameCore game) : base(game)
        {
        }

        protected override void OnFormChange()
        {
            base.OnFormChange();
            UpdateLocation();
        }

        /// <summary>
        /// Called when the screen has been activated.
        /// </summary>
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
        /// <remarks>TODO: Check if this is game over or all of the passengers are dead.</remarks>
        private void ArriveAtLocation()
        {
            var game = GameCore.Instance;

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
        /// <remarks>TODO: Add trading and hunting commands</remarks>
        private void UpdateLocation()
        {
            var _menuHeader = new StringBuilder();
            _menuHeader.Append(TravelInfo.TravelStatus);
            _menuHeader.Append("Here is what you can do:");
            MenuHeader = _menuHeader.ToString();

            // Initialize the available commands and their methods.
            ClearCommands();
            AddCommand(ContinueTrail, TravelCommands.ContinueOnTrail);
            AddCommand(CheckMap, TravelCommands.CheckMap);
            AddCommand(CheckSupplies, TravelCommands.CheckSupplies);
            AddCommand(StopToRest, TravelCommands.StopToRest);
            AddCommand(ChangePace, TravelCommands.ChangePace);
            AddCommand(ChangeRation, TravelCommands.ChangeRation);

            // Add additional commands depending on the current vehicle state.
            var location = GameCore.Instance.Trail.CurrentLocation;
            switch (location.Status)
            {
                case LocationStatus.Unreached:
                case LocationStatus.Departed:
                    break;

                case LocationStatus.Arrived:
                    if (location.TalkingAllowed) AddCommand(TalkToPeople, TravelCommands.TalkToPeople);
                    if (location.ShoppingAllowed) AddCommand(BuySupplies, TravelCommands.BuySupplies);
                    if (location.TalkingAllowed) AddCommand(TradeSupplies, TravelCommands.TradeSupplies);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Called when the player has chosen to continue on the trail.
        /// </summary>
        /// <remarks>TODO: Add other location types</remarks>
        internal void ContinueTrail()
        {
            if (GameCore.Instance.Trail.CurrentLocation.Status == LocationStatus.Departed)
            {
                SetForm(typeof(ContinueTrail));
                return;
            }

            // Throw to an appropriate screen depending on the location reached.
            if (GameCore.Instance.Trail.CurrentLocation is Landmark ||
                GameCore.Instance.Trail.CurrentLocation is Settlement)
                SetForm(typeof(ContinueTrail));
            else if (GameCore.Instance.Trail.CurrentLocation is RiverCrossing)
                SetForm(typeof(RiverCross));
        }

        /// <summary>
        /// Called when the player has chosen to check the map of the map.
        /// </summary>
        internal void CheckMap()
        {
            SetForm(typeof(Commands.CheckMap));
        }

        /// <summary>
        /// Called when the player has chosen to check their inventory and party status.
        /// </summary>
        internal void CheckSupplies()
        {
            SetForm(typeof(Commands.CheckSupplies));
        }

        /// <summary>
        /// Called when the player has chosen to rest at the current location.
        /// </summary>
        internal void StopToRest()
        {
            SetForm(typeof(Commands.StopToRest));
        }

        /// <summary>
        /// Called when the player has chosen to change the travel pace rate.
        /// </summary>
        internal void ChangePace()
        {
            SetForm(typeof(Commands.ChangePace));
        }

        /// <summary>
        /// Called when the player has chosen to change the party ration rate.
        /// </summary>
        internal void ChangeRation()
        {
            SetForm(typeof(Commands.ChangeRation));
        }

        /// <summary>
        /// Called when the player has chosen to talk to the townsfolk for advice.
        /// </summary>
        internal void TalkToPeople()
        {
            SetForm(typeof(Commands.TalkToPeople));
        }

        /// <summary>
        /// Called when the player has chosen to go to the store to buy supplies.
        /// </summary>
        internal void BuySupplies()
        {
            SetForm(typeof(Store.Store));
        }

        /// <summary>
        /// Called when the player wants to trade supplies with the towns folk.
        /// </summary>
        internal void TradeSupplies()
        {
            SetForm(typeof(Commands.TradeSupplies));
        }
    }

    /// <summary>
    /// Defines the options available to the player while they are at a location.
    /// </summary>
    public enum TravelCommands
    {
        [Description("Continue on the trail")] ContinueOnTrail = 1,
        [Description("Check the map")] CheckMap = 2,
        [Description("Check supplies")] CheckSupplies = 3,
        [Description("Stop to rest")] StopToRest = 4,
        [Description("Change Pace")] ChangePace = 5,
        [Description("Change Ration")] ChangeRation = 6,
        [Description("Talk to people")] TalkToPeople = 7,
        [Description("Buy supplies")] BuySupplies = 8,
        [Description("Trade supplies")] TradeSupplies = 9
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
        /// References the river object used for generating in-game river crossing locations and events.
        /// </summary>
        public RiverGenerator River { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.TravelInfo" /> class.
        /// </summary>
        public TravelInfo()
        {
            Store = new StoreGenerator();
        }

        public void GenerateRiver()
        {
            // Creates a new river. Skip if river has already been created.
            if (River != null) return;
            River = new RiverGenerator();
        }

        public void DestroyRiver()
        {
            // Destroy the river data. Skip if the river is already null.
            if (River == null) return;
            River = null;
        }

        /// <summary>
        /// Retrieves the current party status, resources and distance until next location.
        /// </summary>
        /// <remarks>TODO: Add time and weather</remarks>
        public static string TravelStatus
        {
            get
            {
                var game = GameCore.Instance;
                var foodCount = game.Vehicle.Inventory[Types.Food];
                var _trailStatus = new StringBuilder();
                _trailStatus.AppendLine($"Food:     {((foodCount != null) ? foodCount.TotalWeight : 0)} pounds");
                _trailStatus.AppendLine($"Odometer: {game.Vehicle.Odometer} miles ({game.Trail.NextLocationDistance} miles to next location)");
                _trailStatus.AppendLine($"Pace:     {game.Vehicle.Pace.ToDescriptionAttribute()}");
                _trailStatus.AppendLine($"Ration:   {game.Vehicle.Ration.ToDescriptionAttribute()}");
                _trailStatus.AppendLine("------------------------------------------");
                return _trailStatus.ToString();
            }
        }

        /// <summary>
        /// Retrieves the current party status.
        /// </summary>
        public static string PartyStatus
        {
            get
            {
                // Build up a list with tuple in it to hold our data about the party (health and debuffs).
                var partyList = new List<Tuple<string, string, string>>();

                // Loop through every passenger in the vehicle.
                foreach (var passenger in GameCore.Instance.Vehicle.Passengers)
                    partyList.Add(new Tuple<string, string, string>(passenger.Class.ToString(), passenger.HealthState.ToString(), passenger.Status));

                // Generate the formatted table of supplies we will show to user.
                return partyList.ToStringTable(new[] { "Person", "Health", "Status" }, u => u.Item1, u => u.Item2, u => u.Item3);
            }
        }

        /// <summary>
        /// Retrieves the current party supplies.
        /// </summary>
        public static string PartySupplies
        {
            get
            {
                // Build up a list with tuple in it to hold our data about supplies.
                var suppliesList = new List<Tuple<string, string>>();

                // Loop through every inventory item in the vehicle.
                foreach (var item in GameCore.Instance.Vehicle.Inventory)
                {
                    switch (item.Key)
                    {
                        case Types.Food:
                            suppliesList.Add(new Tuple<string, string>("Food", item.Value.TotalWeight.ToString("N0")));
                            break;

                        case Types.Hats:
                            suppliesList.Add(new Tuple<string, string>("Hats", item.Value.Quantity.ToString("N0")));
                            break;

                        case Types.Ammo:
                            suppliesList.Add(new Tuple<string, string>("Bullets", item.Value.Quantity.ToString("N0")));
                            break;

                        case Types.Money:
                            suppliesList.Add(new Tuple<string, string>("Money", item.Value.TotalValue.ToString("C")));
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // Generate the formatted table of supplies we will show to user.
                return suppliesList.ToStringTable(new[] { "Item Name", "Amount" }, u => u.Item1, u => u.Item2);
            }
        }
    }
}