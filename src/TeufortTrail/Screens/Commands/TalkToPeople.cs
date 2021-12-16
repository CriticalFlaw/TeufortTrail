using System;
using System.Collections.Generic;
using System.Linq;
using TeufortTrail.Entities.Location;
using TeufortTrail.Screens.Travel;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Commands
{
    /// <summary>
    /// Displays a random piece of advice given to the player by civilians.
    /// </summary>
    [ParentWindow(typeof(Travel.Travel))]
    public sealed class TalkToPeople : InputForm<TravelInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TalkToPeople" /> class.
        /// </summary>
        public TalkToPeople(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            var advice = GameCore.Instance.Trail.CurrentLocation switch
            {
                // Determine the type of advice to display.
                Landmark => new List<Advice>(AdviceRegistry.Landmark),
                RiverCrossing => new List<Advice>(AdviceRegistry.River),
                Settlement => new List<Advice>(AdviceRegistry.Settlement),
                _ => new List<Advice>(AdviceRegistry.Default)
            };

            // Retrieve a random piece of advice from the registry.
            var randomAdvice = advice.PickRandom(1).FirstOrDefault();

            // Render out the advice to the form.
            return (randomAdvice == null)
                ? $"{Environment.NewLine}AdviceRegistry.DEFAULTADVICE{Environment.NewLine}"
                : $"{Environment.NewLine}{randomAdvice.Name}:{Environment.NewLine}{Environment.NewLine}{randomAdvice.Quote.WordWrap(64)}{Environment.NewLine}";
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