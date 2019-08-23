using System;
using System.Text;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel.Location
{
    [ParentWindow(typeof(Travel))]
    public sealed class LocationArrived : InputForm<TravelInfo>
    {
        private StringBuilder _locationArrived;

        protected override DialogType DialogType => (GameCore.Instance.Trail.IsFirstLocation) ? DialogType.Prompt : DialogType.YesNo;

        public LocationArrived(IWindow window) : base(window)
        {
        }

        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;
        }

        protected override string OnDialogPrompt()
        {
            var game = GameCore.Instance;
            _locationArrived = new StringBuilder();
            if (game.Trail.IsFirstLocation)
                _locationArrived.AppendLine($"{Environment.NewLine}Starting your trail...{Environment.NewLine}");
            else
            {
                _locationArrived.AppendLine($"{Environment.NewLine}You've arrived at {game.Trail.CurrentLocation.Name}.");
                _locationArrived.Append("Would you like to look around? Y/N");
            }
            return _locationArrived.ToString();
        }

        protected override void OnDialogResponse(DialogResponse response)
        {
            if (GameCore.Instance.Trail.IsFirstLocation)
            {
                ClearForm();
                return;
            }

            // Subsequent locations will confirm what the player wants to do, they can stop or keep going on the trail at their own demise.
            switch (response)
            {
                case DialogResponse.Custom:
                case DialogResponse.No:
                    var travelMode = ParentWindow as Travel;
                    if (travelMode == null) throw new InvalidCastException("Unable to cast parent game Windows into travel game Windows when it should be that!");
                    travelMode.ContinueTrail();
                    break;

                case DialogResponse.Yes:
                    ClearForm();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(response), response, null);
            }
        }
    }
}