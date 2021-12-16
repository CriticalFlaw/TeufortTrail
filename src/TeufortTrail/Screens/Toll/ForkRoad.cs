using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Commands;
using TeufortTrail.Screens.Travel;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Toll
{
    /// <summary>
    /// Displays the available routes when the player encounters a fork in the road.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class ForkRoad : Form<TravelInfo>
    {
        /// <summary>
        /// List of possible paths that the player can choose from.
        /// </summary>
        private Dictionary<int, Location> _pathChoices;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ForkRoad" /> class.
        /// </summary>
        public ForkRoad(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            // Create a dictionary of possible path choices.
            base.OnFormPostCreate();
            _pathChoices = new Dictionary<int, Location>();
            if (GameCore.Instance.Trail.CurrentLocation is not ForkInRoad forkInRoad) return;
            for (var index = 0; index < forkInRoad.PathChoices.Count; index++)
                _pathChoices.Add(index + 1, forkInRoad.PathChoices[index]);
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is a valid integer, above zero.
            if (!int.TryParse(input, out var userInput) || userInput <= 0) return;

            // Dictionary of path choices must contain key with input number.
            if (_pathChoices.ContainsKey(userInput))
            {
                // Generate a toll point if that's the selected location choice.
                if (_pathChoices[userInput] is TollInRoad tollRoad)
                {
                    UserData.GenerateToll(tollRoad);
                    SetForm(typeof(TollRoad));
                }
                else
                {
                    GameCore.Instance.Trail.InsertLocation(_pathChoices[userInput]);
                    SetForm(typeof(ContinueTrail));
                }
            }
            else
                SetForm(typeof(CheckMap));
        }

        /// <summary>
        /// Called when the text representation of the current game screen needs to be returned.
        /// </summary>
        public override string OnRenderForm()
        {
            var forkRoad = new StringBuilder();
            forkRoad.AppendLine($"{Environment.NewLine}You encounter a fork in the road. You can:{Environment.NewLine}");
            foreach (var (key, value) in _pathChoices)
            {
                forkRoad.AppendLine($"  {key}. Travel to {value.Name}");
                if (key == _pathChoices.Last().Key)
                    forkRoad.Append($"  {key + 1}. Check the Map");
            }
            return forkRoad.ToString();
        }
    }
}