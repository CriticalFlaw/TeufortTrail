using System;
using System.Text;
using TeufortTrail.Entities;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Commands
{
    /// <summary>
    /// Displays the map and current trail progress.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class CheckMap : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckMap" /> class.
        /// </summary>
        public CheckMap(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Generate a progress bar of the trail progress, then format it into a table.
            var _checkMap = new StringBuilder();
            _checkMap.AppendLine($"{Environment.NewLine}Trail progress:{Environment.NewLine}");
            _checkMap.AppendLine(TextProgress.DrawProgressBar(GameCore.Instance.Trail.LocationIndex + 1, GameCore.Instance.Trail.Locations.Count, 32) + Environment.NewLine);
            _checkMap.AppendLine(GameCore.Instance.Trail.Locations.ToStringTable(new[] { "Visited", "Location" }, u => u.Status >= LocationStatus.Arrived, u => u.Name));
            return _checkMap.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            ClearForm();
        }
    }
}