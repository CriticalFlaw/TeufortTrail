using System;
using System.Text;
using TeufortTrail.Entities.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel
{
    [ParentWindow(typeof(Travel))]
    public sealed class LocationArrived : InputForm<TravelInfo>
    {
        #region VARIABLES

        private StringBuilder _locationArrived;

        /// <summary>
        /// Defines the kind of dialog prompt this screen will be (Yes/No or Press Any Key)
        /// </summary>
        protected override DialogType DialogType => (GameCore.Instance.Trail.IsFirstLocation) ? DialogType.Prompt : DialogType.YesNo;

        #endregion VARIABLES

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TeufortTrail.Screens.Travel.LocationArrived" /> class.
        /// </summary>
        public LocationArrived(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the screen has been created.
        /// </summary>
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();
            GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;
        }

        /// <summary>
        /// Called when the screen needs a prompt to be displayed to the player.
        /// </summary>
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

        /// <summary>
        /// Process the player's response to the prompt message.
        /// </summary>
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