using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Events.Director
{
    [ParentWindow(typeof(Event))]
    public sealed class EventExecutor : InputForm<EventInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventExecutor" /> class.
        /// </summary>
        public EventExecutor(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Execute the event which should return us some text to display to user about what it did to running simulation.
            UserData.DirectorEvent.Execute(UserData);

            // Add event text to the user data object so it can be printed on another form.
            var eventText = UserData.DirectorEvent.Render(UserData);
            UserData.EventText = eventText;

            // Add event text to the user interface output.
            return $"{Environment.NewLine}{eventText}{Environment.NewLine}";
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            // Calls a method to perform an action before the event is closed.
            UserData.DirectorEvent.OnEventClose(UserData);

            // Only remove the entire event form if there are no more days to skip.
            ParentWindow.RemoveWindowNextTick();
        }
    }
}