using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Events.Director
{
    [ParentWindow(typeof(Event))]
    public sealed class EventExecutor : InputForm<EventInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Events.Director.EventExecutor" /> class.
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
            var _eventExecutor = new StringBuilder();
            _eventExecutor.AppendLine($"{Environment.NewLine}{eventText}");
            return _eventExecutor.ToString();
        }

        /// <summary>
        /// Process the player's response to the prompt message.
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