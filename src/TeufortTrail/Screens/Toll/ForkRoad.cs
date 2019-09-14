using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Travel.Commands;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace TeufortTrail.Screens.Travel.Toll
{
    /// <summary>
    /// Displays the available routes when the player encounters a fork in the road.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class ForkRoad : Form<TravelInfo>
    {
        /// <summary>
        /// List of possible paths that the player can choose from.
        /// </summary>
        private Dictionary<int, Location> PathChoices;

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
            PathChoices = new Dictionary<int, Location>();
            var forkInRoad = GameCore.Instance.Trail.CurrentLocation as ForkInRoad;
            for (var index = 0; index < forkInRoad.PathChoices.Count; index++)
                PathChoices.Add(index + 1, forkInRoad.PathChoices[index]);
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is a valid integer, above zero.
            if (!int.TryParse(input, out var userInput) || userInput <= 0) return;

            // Dictionary of path choices must contain key with input number.
            if (PathChoices.ContainsKey(userInput))
            {
                // Generate a toll point if that's the selected location choice.
                if (PathChoices[userInput] is TollInRoad tollRoad)
                {
                    UserData.GenerateToll(tollRoad);
                    SetForm(typeof(TollRoad));
                }
                else
                {
                    GameCore.Instance.Trail.InsertLocation(PathChoices[userInput]);
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
            var _forkRoad = new StringBuilder();
            _forkRoad.AppendLine($"{Environment.NewLine}You encounter a fork in the road. You may:{Environment.NewLine}");
            foreach (var pathChoice in PathChoices)
            {
                _forkRoad.AppendLine($"{pathChoice.Key}. Travel to {pathChoice.Value.Name}");
                if (pathChoice.Key == PathChoices.Last().Key)
                    _forkRoad.Append($"{pathChoice.Key + 1}. Check the Map");
            }
            return _forkRoad.ToString();
        }
    }
}