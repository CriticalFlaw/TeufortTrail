using System;
using System.Text;
using TeufortTrail.Entities.Location;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Commands
{
    [ParentWindow(typeof(Travel))]
    public sealed class CheckMap : InputForm<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _checkMap;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Location.CheckMap" /> class.
        /// </summary>
        public CheckMap(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Create visual progress representation of the trail.
            _checkMap = new StringBuilder();
            _checkMap.AppendLine($"{Environment.NewLine}Trail progress:{Environment.NewLine}");

            // Show a progress bar of the trail progress so far.
            _checkMap.AppendLine(TextProgress.DrawProgressBar(GameCore.Instance.Trail.LocationIndex + 1, GameCore.Instance.Trail.Locations.Count, 32) + Environment.NewLine);

            // Generate the formatted table of locations on the trail.
            var trailTable = GameCore.Instance.Trail.Locations.ToStringTable(new[] { "Visited", "Location" }, u => u.Status >= LocationStatus.Arrived, u => u.Name);

            // Return the generated tables.
            _checkMap.AppendLine(trailTable);
            return _checkMap.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ClearForm();
        }
    }
}