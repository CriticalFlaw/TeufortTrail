using System;
using System.Threading;

namespace TeufortTrail
{
    /// <summary>
    /// The Teufort Trail
    /// </summary>
    internal static class Program
    {
        public static int Main()
        {
            // Setup the console
            Console.Title = "Teufort Trail";
            Console.WriteLine("Loading...");
            Console.CursorVisible = false;
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Setup the game instance
            GameCore.Create();
            GameCore.Instance.SceneGraph.ScreenBufferDirtyEvent += ScreenBufferDirtyEvent;

            while (GameCore.Instance != null)
            {
                GameCore.Instance.OnTick(true);
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            GameCore.Instance.InputManager.SendInputBufferAsCommand();
                            break;

                        case ConsoleKey.Backspace:
                            GameCore.Instance.InputManager.RemoveLastCharOfInputBuffer();
                            break;

                        default:
                            GameCore.Instance.InputManager.AddCharToInputBuffer(key.KeyChar);
                            break;
                    }
                }
                Thread.Sleep(1);
            }

            // Clean the console
            Console.Clear();
            Console.WriteLine("Goodbye!");
            Console.WriteLine("Press ANY KEY to close this window...");
            Console.ReadKey();
            return 0;
        }

        /// <summary>
        /// Outputs all the queued game text to the screen.
        /// </summary>
        /// <param name="input"></param>
        private static void ScreenBufferDirtyEvent(string input)
        {
            var content = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (var index = 0; index < Console.WindowHeight - 1; index++)
            {
                Console.CursorLeft = 0;
                Console.SetCursorPosition(0, index);
                var output = new string(' ', Console.WindowWidth);
                if (content.Length > index)
                    output = content[index].PadRight(Console.WindowWidth);
                Console.Write(output);
            }
        }

        /// <summary>
        /// Called when the player presses the escape shortcut to get out of the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}