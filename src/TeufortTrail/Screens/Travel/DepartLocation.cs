using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel
{
    [ParentWindow(typeof(Travel))]
    public sealed class DepartLocation : InputForm<TravelInfo>
    {
        private StringBuilder _departLocation;

        public DepartLocation(IWindow window) : base(window)
        {
        }

        protected override string OnDialogPrompt()
        {
            _departLocation = new StringBuilder();
            var nextPoint = GameCore.Instance.Trail.NextLocation;
            _departLocation.AppendLine($"{Environment.NewLine}From {GameCore.Instance.Trail.CurrentLocation.Name} it is {GameCore.Instance.Trail.NextLocationDistance} miles to {nextPoint.Name}{Environment.NewLine}");
            return _departLocation.ToString();
        }

        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(ContinueTrail));
        }
    }
}