using System;
using System.Threading;

namespace TeufortTrail
{
    /// <summary>
    /// The Teufort Trail
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Console application entry point.
        /// </summary>
        public static int Main()
        {
            // Setup the console.
            Console.Title = "Teufort Trail";
            Console.WriteLine("Loading...");
            Console.CursorVisible = false;
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Setup the game instance.
            GameCore.Create();

            // Loop through the game session.
            while (GameCore.Instance != null)
            {
                GameCore.Instance.OnTick(true);
                Thread.Sleep(1);
            }

            // Close the console.
            Console.Clear();
            Console.WriteLine("Goodbye!");
            Console.WriteLine("Press ANY KEY to close this window...");
            Console.ReadKey();
            return 0;
        }

        /// <summary>
        /// Called when the player presses the escape shortcut to get out of the game.
        /// </summary>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}