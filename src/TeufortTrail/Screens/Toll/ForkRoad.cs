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
    [ParentWindow(typeof(Travel))]
    public sealed class ForkRoad : Form<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _forkRoad;

        private Dictionary<int, Location> PathChoices;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Toll.ForkRoad" /> class.
        /// </summary>
        public ForkRoad(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when this screen has been created and now needs information to be displayed.
        /// </summary>
        public override void OnFormPostCreate()
        {
            // Create a dictionary that represents all the choices with index starting at one not zero.
            base.OnFormPostCreate();
            PathChoices = new Dictionary<int, Location>();
            var forkInRoad = GameCore.Instance.Trail.CurrentLocation as ForkInRoad;
            for (var index = 0; index < forkInRoad.PathChoices.Count; index++)
                PathChoices.Add(index + 1, forkInRoad.PathChoices[index]);
        }

        /// <summary>
        /// Called when the user has inputted something that needs to be processed.
        /// </summary>
        public override void OnInputBufferReturned(string input)
        {
            // Check that the user input is a valid intenger, above zero.
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
        /// Returns the text-only representation of the current game screen.
        /// </summary>
        public override string OnRenderForm()
        {
            // Clear the string builder and being building a new fork in the road based on current location skip choices.
            _forkRoad = new StringBuilder();
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