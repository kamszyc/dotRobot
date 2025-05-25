using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using CommunityToolkit.Mvvm.Messaging;
using dotRobot.Gamepad;

namespace dotRobot
{
    [Activity(Theme = "@style/Maui.MainTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public override bool DispatchKeyEvent(KeyEvent? e)
        {
            if (e == null)
            {
                return true;
            }

            WeakReferenceMessenger.Default.Send(new GamepadButtonMessage()
            {
                Button = MapButton(e.KeyCode),
                IsPressed = e.Action == KeyEventActions.Down
            });
            return true;
        }

        private GamepadButtons MapButton(Keycode keyCode)
        {
            return keyCode switch
            {
                Keycode.ButtonA => GamepadButtons.A,
                Keycode.ButtonB => GamepadButtons.B,
                Keycode.ButtonX => GamepadButtons.X,
                Keycode.ButtonY => GamepadButtons.Y,
                Keycode.DpadUp => GamepadButtons.DPadUp,
                Keycode.DpadDown => GamepadButtons.DPadDown,
                Keycode.DpadLeft => GamepadButtons.DPadLeft,
                Keycode.DpadRight => GamepadButtons.DPadRight,
                Keycode.ButtonL1 => GamepadButtons.LB,
                Keycode.ButtonR1 => GamepadButtons.RB,
                _ => GamepadButtons.None
            };
        }
    }
}
