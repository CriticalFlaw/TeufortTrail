using System;
using System.Linq;
using System.Text;
using TeufortTrail.Entities;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Menu
{
    /// <summary>
    /// Displays a victory message when the player wins the game.
    /// </summary>
    [ParentWindow(typeof(GameOver))]
    public sealed class GameVictory : InputForm<GameOverInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameVictory" /> class.
        /// </summary>
        public GameVictory(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            var game = GameCore.Instance;
            var survivors = game.Vehicle.Passengers.Where(x => x.HealthState != HealthStatus.Dead).ToList();
            var victory = new StringBuilder();
            victory.AppendLine($"{Environment.NewLine}Congratulations! You have made it to Teufort in {game.TotalTurns} days!{Environment.NewLine}");
            victory.AppendLine($"Survivors: {survivors.Count} of {game.Vehicle.Passengers.Count} ({string.Join(", ", survivors.Select(x => x.Class))})");
            victory.AppendLine(survivors.Count switch
            {
                4 => "A flawless run. The whole team made it, and Gray Mann is furious.",
                3 => "A strong run. The team will drink to absent friends.",
                2 => "A rough trip, but you got there. Half the team lives to fight another day.",
                _ => "You made it to Teufort... alone. Was it worth it?"
            });
            return victory.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponseEnum response)
        {
            GameCore.Instance.Destroy();
        }
    }
}