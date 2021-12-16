using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Hunt;
using TeufortTrail.Screens.Hunt.Notifications;
using TeufortTrail.Screens.Menu;
using TeufortTrail.Screens.River;
using TeufortTrail.Screens.Store;
using TeufortTrail.Screens.Toll;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Control;

namespace TeufortTrail.Screens.Travel
{
    /// <summary>
    /// Displays a list of commands the player can perform while stopped.
    /// </summary>
    public sealed class Travel : Window<TravelCommands, TravelInfo>
    {
        private bool GameOver { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Travel" /> class.
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
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnWindowPostCreate()
        {
            UpdateLocation();
            if (GameCore.Instance.Trail.IsFirstLocation && (GameCore.Instance.Trail.CurrentLocation?.Status == LocationStatus.Unreached))
                SetForm(typeof(StoreWelcome));
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnWindowActivate()
        {
            ArriveAtLocation();
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
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
            if (GameOver) return;

            var game = GameCore.Instance;

            // Check if the party has arrived at the last location.
            if (game.Trail.CurrentLocation.LastLocation)
            {
                GameOver = true;
                SetForm(typeof(GameVictory));
                return;
            }
            
            if (!game.Vehicle.Passengers.Any(x => x.HealthState > (int)HealthStatus.Dead))
            {
                GameOver = true;
                SetForm(typeof(GameOver));
                return;
            }
            
            if (game.Trail.CurrentLocation.Status == LocationStatus.Arrived && !GameOver)
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
            var menuHeader = new StringBuilder();
            menuHeader.Append(TravelInfo.TravelStatus);
            menuHeader.Append("Here is what you can do:");
            MenuHeader = menuHeader.ToString();

            // Initialize the available commands and their methods.
            ClearCommands();
            AddCommand(ContinueTrail, TravelCommands.ContinueOnTrail);
            AddCommand(CheckMap, TravelCommands.CheckMap);
            AddCommand(CheckSupplies, TravelCommands.CheckSupplies);
            AddCommand(StopToRest, TravelCommands.StopToRest);
            AddCommand(ChangeRation, TravelCommands.ChangeRation);

            // Add additional commands depending on the current location state.
            var location = GameCore.Instance.Trail.CurrentLocation;
            switch (location.Status)
            {
                case LocationStatus.Unreached:
                    break;

                case LocationStatus.Arrived:
                    if (location.TalkingAllowed) AddCommand(TalkToPeople, TravelCommands.TalkToPeople);
                    if (location.ShoppingAllowed) AddCommand(BuySupplies, TravelCommands.BuySupplies);
                    if (location.TalkingAllowed) AddCommand(TradeSupplies, TravelCommands.TradeSupplies);
                    break;

                case LocationStatus.Departed:
                    AddCommand(TradeSupplies, TravelCommands.TradeSupplies);
                    AddCommand(HuntRobots, TravelCommands.HuntRobots);
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

            switch (GameCore.Instance.Trail.CurrentLocation)
            {
                // Throw to an appropriate screen depending on the location reached.
                case Landmark:
                case Settlement:
                case TollInRoad:
                    SetForm(typeof(ContinueTrail));
                    break;
                case RiverCrossing:
                    SetForm(typeof(River.River));
                    break;
                case ForkInRoad:
                    SetForm(typeof(ForkRoad));
                    break;
            }
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

        /// <summary>
        /// Called when the player wants to hunt robots for resources.
        /// </summary>
        internal void HuntRobots()
        {
            SetForm(GameCore.Instance.Vehicle.Inventory[ItemTypes.Ammo].Quantity > 0
                ? typeof(Hunting)
                : typeof(NoAmmo));
        }
    }

    /// <summary>
    /// Defines the options available to the player while they are at a location.
    /// </summary>
    public enum TravelCommands
    {
        [Description("Continue on the Trail")] ContinueOnTrail = 1,
        [Description("Check the Map")] CheckMap = 2,
        [Description("Check Supplies")] CheckSupplies = 3,
        [Description("Stop to Rest")] StopToRest = 4,
        [Description("Change Ration")] ChangeRation = 5,
        [Description("Talk to People")] TalkToPeople = 6,
        [Description("Buy Supplies")] BuySupplies = 7,
        [Description("Trade Supplies")] TradeSupplies = 8,
        [Description("Hunt Robots")] HuntRobots = 9
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
        /// References the toll object used for generating in-game toll road locations and events.
        /// </summary>
        public TollGenerator Toll { get; private set; }

        /// <summary>
        /// References the hunting object used for generating the hunting mini-game.
        /// </summary>
        public HuntGenerator Hunt { get; private set; }

        /// <summary>
        /// Number of days the party will rest for at the location.
        /// </summary>
        public int DaysToRest = 3;

        /// <summary>
        /// Retrieves the current party status, resources and distance until next location.
        /// </summary>
        /// <remarks>TODO: Display the current time and weather</remarks>
        public static string TravelStatus
        {
            get
            {
                var game = GameCore.Instance;
                var foodCount = game.Vehicle.Inventory[ItemTypes.Food];
                var trailStatus = new StringBuilder();
                trailStatus.AppendLine($"Food:     {foodCount?.TotalWeight ?? 0} pounds");
                trailStatus.AppendLine($"Odometer: {game.Vehicle.Odometer} miles ({game.Trail.NextLocationDistance} miles to next location)");
                trailStatus.AppendLine($"Ration:   {game.Vehicle.Ration}");
                trailStatus.AppendLine("------------------------------------------");
                return trailStatus.ToString();
            }
        }

        /// <summary>
        /// Retrieves the current party status.
        /// </summary>
        public static string PartyStatus
        {
            get
            {
                // Loop through every passenger in the vehicle. Build up a list with tuple in it to hold our data about the party (health and debuffs).
                var partyList = GameCore.Instance.Vehicle.Passengers.Select(passenger => new Tuple<string, string, string>(passenger.Class.ToString(), passenger.HealthState.ToString(), passenger.Status)).ToList();

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
                foreach (var (key, value) in GameCore.Instance.Vehicle.Inventory)
                {
                    switch (key)
                    {
                        case ItemTypes.Food:
                            suppliesList.Add(new Tuple<string, string>("Food", value.TotalWeight.ToString("N0")));
                            break;

                        case ItemTypes.Clothing:
                            suppliesList.Add(new Tuple<string, string>("Hats", value.Quantity.ToString("N0")));
                            break;

                        case ItemTypes.Ammo:
                            suppliesList.Add(new Tuple<string, string>("Bullets", value.Quantity.ToString("N0")));
                            break;

                        case ItemTypes.Money:
                            suppliesList.Add(new Tuple<string, string>("Money", value.TotalValue.ToString("C")));
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // Generate the formatted table of supplies we will show to user.
                return suppliesList.ToStringTable(new[] { "Item Name", "Amount" }, u => u.Item1, u => u.Item2);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TravelInfo" /> class.
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

        public void GenerateToll(TollInRoad tollRoad)
        {
            if (Toll != null) return;
            Toll = new TollGenerator(tollRoad);
        }

        public void DestroyToll()
        {
            if (Toll == null) return;
            Toll = null;
        }

        public void GenerateHunt()
        {
            if (Hunt != null) return;
            Hunt = new HuntGenerator();
        }

        public void DestroyHunt()
        {
            if (Hunt == null) return;
            Hunt = null;
        }
    }
}