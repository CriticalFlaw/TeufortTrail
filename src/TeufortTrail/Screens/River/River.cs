using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Travel;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.River
{
    /// <summary>
    /// Displays the notification to the player, letting them know of the river they have to cross.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class River : Form<TravelInfo>
    {
        /// <summary>
        /// Retrieve instance of the river that will be crossed.
        /// </summary>
        private RiverCrossing _river = GameCore.Instance.Trail.CurrentLocation as RiverCrossing;

        private StringBuilder _crossingOptions;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="River" /> class.
        /// </summary>
        public River(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            UserData.GenerateRiver();
            _crossingOptions = new StringBuilder();
            //_river.AppendLine($"{GameCore.Instance.Trail.CurrentLocation.Name}");
            //_river.AppendLine("------------------------------------------");
            _crossingOptions.AppendLine($"{Environment.NewLine}A {UserData.River.RiverWidth} feet wide river separates you from the trail.");
            _crossingOptions.AppendLine($"You must cross it in order to continue.{Environment.NewLine}");
            _crossingOptions.AppendLine($"  1. {RiverOptions.Float.ToDescriptionAttribute()}");
            if (_river.CrossingOption == RiverOptions.Ferry)
                _crossingOptions.Append($"  2. {RiverOptions.Ferry.ToDescriptionAttribute()}");
            else if (_river.CrossingOption == RiverOptions.Help)
                _crossingOptions.Append($"  2. {RiverOptions.Help.ToDescriptionAttribute()}");
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is not empty.
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) return;

            switch (input.ToUpperInvariant())
            {
                case "1":
                    UserData.River.CrossingType = RiverOptions.Float;
                    SetForm(typeof(Crossing));
                    break;

                case "2":
                    switch (_river.CrossingOption)
                    {
                        case RiverOptions.Ferry:
                            UserData.River.CrossingType = RiverOptions.Ferry;
                            SetForm(typeof(Ferry));
                            break;
                        case RiverOptions.Help:
                            UserData.River.CrossingType = RiverOptions.Help;
                            SetForm(typeof(Help));
                            break;
                        case RiverOptions.Float:
                            break;
                        case RiverOptions.None:
                        default:
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            return _crossingOptions.ToString();
        }
    }

    public sealed class RiverGenerator
    {
        /// <summary>
        /// Determines how the vehicle can cross the river.
        /// </summary>
        public RiverOptions CrossingType { get; internal set; }

        /// <summary>
        /// How much the ferry will charge the player to cross the river.
        /// </summary>
        public float FerryCost { get; set; }

        /// <summary>
        /// How much the locals will charge the player to cross the river.
        /// </summary>
        public int HelpCost { get; set; }

        /// <summary>
        /// How wide the river is, used for events where the player needs to cross it.
        /// </summary>
        public int RiverWidth { get; }

        /// <summary>
        /// Flags the current river crossing attempts as having encountered a disaster event.
        /// </summary>
        public bool DisasterHappened { get; set; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="RiverGenerator" />class.
        /// </summary>
        public RiverGenerator()
        {
            var game = GameCore.Instance;
            var maxHelpCost = (game.Vehicle.Inventory[ItemTypes.Clothing].Quantity >= game.Vehicle.Inventory[ItemTypes.Clothing].MaxQuantity / 2) ? 10 : 20;
            var maxFerryCost = (game.Vehicle.Inventory[ItemTypes.Money].Quantity > 500) ? 10 : 20;

            CrossingType = RiverOptions.None;
            HelpCost = game.Random.Next(5, maxHelpCost);
            FerryCost = game.Random.Next(5, maxFerryCost);
            RiverWidth = game.Random.Next(120, 1200);
        }
    }
}