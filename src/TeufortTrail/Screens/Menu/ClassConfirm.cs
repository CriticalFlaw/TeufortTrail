using System;
using System.Text;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace TeufortTrail.Screens.MainMenu
{
    /// <summary>
    /// Displays the class attributes and asks the player if they want to proceed.
    /// </summary>
    [ParentWindow(typeof(MainMenu))]
    public sealed class ClassConfirm : InputForm<NewGameInfo>
    {
        private StringBuilder _classConfirm;

        /// <summary>
        /// Sets the kind of prompt response the player can give.
        /// </summary>
        protected override DialogType DialogType => DialogType.YesNo;

        //-------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassConfirm" /> class.
        /// </summary>
        public ClassConfirm(IWindow window) : base(window)
        {
        }

        /// <summary>
        /// Called when the attached screen is activated and needs a text prompt to be returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            // Display the selected class attribute and ask if the player wants to continue.
            GameCore.Instance.SetStartInfo(UserData);
            _classConfirm = new StringBuilder();
            _classConfirm.AppendLine(Environment.NewLine + UserData.PlayerClass.ToDescriptionAttribute());
            _classConfirm.Append("Would you like to proceed? Y/N");
            return _classConfirm.ToString();
        }

        /// <summary>
        /// Called when player input has been detected and an appropriate response needs to be determined.
        /// </summary>
        protected override void OnDialogResponse(DialogResponse response)
        {
            if (response == DialogResponse.Yes)
                SetForm(typeof(StorySetup));
            else
                SetForm(typeof(ClassSelect));
        }
    }
}