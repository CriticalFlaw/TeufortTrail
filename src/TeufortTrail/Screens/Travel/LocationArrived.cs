using System;
using System.Text;
using TeufortTrail.Entities;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.Travel
{
    /// <summary>
    /// Displays a message letting the player know they have arrived at a new location.
    /// </summary>
    [ParentWindow(typeof(Travel))]
    public sealed class LocationArrived : InputForm<TravelInfo>
    {
        /// <summary>
        /// Sets the kind of prompt response the player can give. Could be Yes, No or Press Any.
        /// </summary>
        protected override DialogType DialogType => DialogType.YesNo;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationArrived" /> class.
        /// </summary>
        public LocationArrived(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        public override void OnFormPostCreate()
        {
            // Stop the vehicle because it has arrived at a location.
            base.OnFormPostCreate();
            GameCore.Instance.Vehicle.Status = VehicleStatus.Stopped;
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Display a message asking the player if they want to investigate the current location.
            var _locationArrived = new StringBuilder();
            _locationArrived.AppendLine($"{Environment.NewLine}You've arrived at {GameCore.Instance.Trail.CurrentLocation.Name}.");
            _locationArrived.Append("Would you like to look around? Y/N");
            return _locationArrived.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            if (response == DialogResponse.Yes)
                ClearForm();
            else
            {
                var travelMode = ParentWindow as Travel;
                if (travelMode == null) throw new InvalidCastException("Unable to cast parent game Windows into travel game Windows when it should be that!");
                travelMode.ContinueTrail();
            }
        }
    }
}