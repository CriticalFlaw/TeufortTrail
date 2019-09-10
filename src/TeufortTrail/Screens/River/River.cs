using System;
using System.Text;
using TeufortTrail.Entities;
using TeufortTrail.Entities.Location;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.River
{
    /// <summary>
    /// Displays the notification to the player, letting them know of the river they have to cross.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class River : Form<TravelInfo>
    {
        private StringBuilder _river;

        /// <summary>
        /// Retrieve instance of the river that will be crossed.
        /// </summary>
        private RiverCrossing river = GameCore.Instance.Trail.CurrentLocation as RiverCrossing;

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
            _river = new StringBuilder();
            _river.AppendLine($"{GameCore.Instance.Trail.CurrentLocation.Name}");
            _river.AppendLine("------------------------------------------");
            _river.AppendLine($"{Environment.NewLine}A {UserData.River.RiverWidth} feet wide river separates from your trail.");
            _river.AppendLine($"You must cross it in order to continue.{Environment.NewLine}");
            _river.AppendLine($"1. {RiverOptions.Float.ToDescriptionAttribute()}");
            if (river.CrossingOption == RiverOptions.Ferry)
                _river.AppendLine($"2. {RiverOptions.Ferry.ToDescriptionAttribute()}");
            else if (river.CrossingOption == RiverOptions.Help)
                _river.AppendLine($"2. {RiverOptions.Help.ToDescriptionAttribute()}");
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
                    if (river.CrossingOption == RiverOptions.Ferry)
                    {
                        UserData.River.CrossingType = RiverOptions.Ferry;
                        SetForm(typeof(Ferry));
                    }
                    else if (river.CrossingOption == RiverOptions.Help)
                    {
                        UserData.River.CrossingType = RiverOptions.Help;
                        SetForm(typeof(Help));
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            return _river.ToString();
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
        /// Flags the current river crossing attempts as having encoutnered a disaster event.
        /// </summary>
        public bool DisasterHappened { get; set; }

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="RiverGenerator" />class.
        /// </summary>
        /// <remarks>TODO: Set the values in relation to the player's current monetary balance.</remarks>
        public RiverGenerator()
        {
            CrossingType = RiverOptions.None;
            HelpCost = GameCore.Instance.Random.Next(3, 10);
            FerryCost = GameCore.Instance.Random.Next(5, 10);
            RiverWidth = GameCore.Instance.Random.Next(120, 1200);
        }
    }
}