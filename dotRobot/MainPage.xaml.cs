using Plugin.Maui.KeyListener;
using Microsoft.Maui.Dispatching;
using System.Diagnostics;
using dotRobot.Gamepad;

namespace dotRobot
{
    public partial class MainPage : ContentPage
    {
        private KeyboardBehavior keyboardBehavior = new KeyboardBehavior();
        private GamepadService gamepadService = new GamepadService();

        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AddBehavior();
            gamepadService.Start(this.Dispatcher);
            gamepadService.ButtonStateChanged += GamepadService_ButtonStateChanged;
            gamepadService.LeftJoystickMoved += GamepadService_LeftJoystickMoved;
        }

        private void GamepadService_LeftJoystickMoved(object? sender, GamepadJoystickEventArgs e)
        {
        }

        private void GamepadService_ButtonStateChanged(object? sender, GamepadButtonEventArgs e)
        {
        }

        protected override void OnDisappearing()
        {
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
