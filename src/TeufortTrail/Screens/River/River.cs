using System;
using System.Text;
using TeufortTrail.Entities.Location;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.River
{
    [ParentWindow(typeof(Travel))]
    public sealed class River : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _river;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.River.RiverCross" /> class.
        /// </summary>
        public River(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            UserData.GenerateRiver();
            _river = new StringBuilder();
            _river.AppendLine($"{GameCore.Instance.Trail.CurrentLocation.Name}");
            _river.AppendLine("------------------------------------------");
            _river.AppendLine($"{Environment.NewLine}You must cross the river in order to continue.{Environment.NewLine}");
            _river.AppendLine($"1. {RiverCrossChoice.Float.ToDescriptionAttribute()}");
            _river.AppendLine($"2. {RiverCrossChoice.Ferry.ToDescriptionAttribute()}");
            _river.AppendLine($"3. {RiverCrossChoice.Help.ToDescriptionAttribute()}");
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        /// <param name="input">User input</param>
        public override void OnInputBufferReturned(string input)
        {
            switch (input.ToUpperInvariant())
            {
                case "1":
                    UserData.River.CrossingType = RiverCrossChoice.Float;
                    SetForm(typeof(Crossing)); // TEMP
                    break;

                case "2":
                    UserData.River.CrossingType = RiverCrossChoice.Ferry;
                    SetForm(typeof(Ferry)); // TEMP
                    break;

                case "3":
                    UserData.River.CrossingType = RiverCrossChoice.Help;
                    SetForm(typeof(Help)); // TEMP
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            return _river.ToString();
        }
    }

    public sealed class RiverGenerator
    {
        #region VARIABLES

        /// <summary>
        /// Determines how the vehicle would be crossing the river.
        /// </summary>
        public RiverCrossChoice CrossingType { get; internal set; }

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

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.River.RiverGenerator" />class.
        /// </summary>
        /// <remarks>TODO: Set the values in relation to the player's current monetary balance.</remarks>
        public RiverGenerator()
        {
            CrossingType = RiverCrossChoice.None;
            HelpCost = GameCore.Instance.Random.Next(3, 10);
            FerryCost = GameCore.Instance.Random.Next(5, 10);
            RiverWidth = GameCore.Instance.Random.Next(100, 1500);
        }
    }
}