using System;
using System.Collections.Generic;
using System.Linq;
using TeufortTrail.Entities.Location;
using TeufortTrail.Entities.Trail;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Commands
{
    [ParentWindow(typeof(Travel))]
    public sealed class TalkToPeople : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.Location.TalkToPeople" /> class.
        /// </summary>
        public TalkToPeople(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Determine the type of advice to display.
            List<Advice> advice;
            if (GameCore.Instance.Trail.CurrentLocation is Landmark)
                advice = new List<Advice>(AdviceRegistry.Landmark);
            else if (GameCore.Instance.Trail.CurrentLocation is RiverCrossing)
                advice = new List<Advice>(AdviceRegistry.River);
            else if (GameCore.Instance.Trail.CurrentLocation is Settlement)
                advice = new List<Advice>(AdviceRegistry.Town);
            else
                advice = new List<Advice>(AdviceRegistry.Default);

            // Retrieve a single random piece of advice from the registry.
            var randomAdvice = advice.PickRandom(1).FirstOrDefault();

            // Render out the advice to the form.
            return randomAdvice == null
                ? $"{Environment.NewLine}AdviceRegistry.DEFAULTADVICE{Environment.NewLine}"
                : $"{Environment.NewLine}{randomAdvice.Name},{Environment.NewLine}{randomAdvice.Quote.WordWrap()}{Environment.NewLine}";
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