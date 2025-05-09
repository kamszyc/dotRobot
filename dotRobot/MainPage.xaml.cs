using Plugin.Maui.KeyListener;
using Microsoft.Maui.Dispatching;
using System.Diagnostics;

#if WINDOWS
using XInputium.XInput;
#endif

namespace dotRobot
{
    public partial class MainPage : ContentPage
    {
        private KeyboardBehavior keyboardBehavior = new();
        private IDispatcherTimer timer;
#if WINDOWS
        private readonly XGamepad gamepad;
#endif

        public MainPage()
        {
            InitializeComponent();
            timer = Dispatcher.CreateTimer();
            AddBehavior();
#if WINDOWS
            XGamepad gamepad = new();
            gamepad.ButtonPressed += (s, e) => Debug.WriteLine($"Button {e.Button} pressed");

            timer.Interval = TimeSpan.FromMilliseconds(16.666); // ~60Hz polling rate
            timer.Tick += (s, e) =>
            {
                gamepad.Update();
            };
            timer.Start();
#endif
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            timer.Start();
        }

        protected override void OnDisappearing()
        {
            timer.Stop();
            base.OnDisappearing();
        }

        private void AddBehavior()
        {
            keyboardBehavior.KeyDown += OnKeyDown;
            keyboardBehavior.KeyUp += OnKeyUp;
            this.Behaviors.Add(keyboardBehavior);
        }

        private async void OnKeyDown(object? sender, KeyPressedEventArgs e)
        {
            if (e.Keys == KeyboardKeys.LeftArrow)
            {
                await ArrowLeft.ScaleTo(0.8, 100);
                await ArrowLeft.ScaleTo(1.0, 100);
            }
        }

        private void OnKeyUp(object? sender, KeyPressedEventArgs e)
        {
        }
    }

}
