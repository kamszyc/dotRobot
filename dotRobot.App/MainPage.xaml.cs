using CommunityToolkit.Mvvm.ComponentModel;
using dotRobot.Bluetooth;
using dotRobot.Common;
using dotRobot.Gamepad;
using Microsoft.Maui.Dispatching;
using Plugin.Maui.KeyListener;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace dotRobot
{
    public partial class MainPage : ContentPage
    {
        private GamepadService gamepadService = new GamepadService();
        private BluetoothService bluetoothService = new BluetoothService();

        private MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            gamepadService.Start(this.Dispatcher);
            gamepadService.ButtonStateChanged += GamepadService_ButtonStateChanged;
            gamepadService.LeftJoystickMoved += GamepadService_LeftJoystickMoved;
            ConnectToRobot.Clicked += ConnectToRobot_Clicked;

            ArrowLeft.Pressed += ArrowPressed;
            ArrowRight.Pressed += ArrowPressed;
            ArrowUp.Pressed += ArrowPressed;
            ArrowDown.Pressed += ArrowPressed;

            ArrowLeft.Released += ArrowReleased;
            ArrowRight.Released += ArrowReleased;
            ArrowUp.Released += ArrowReleased;
            ArrowDown.Released += ArrowReleased;

            bluetoothService.Disconnected += BluetoothService_Disconnected;
        }

        private void BluetoothService_Disconnected(object? sender, EventArgs e)
        {
            ViewModel.IsConnected = false;
            ViewModel.CanConnect = true;
        }

        private async void ArrowPressed(object? sender, EventArgs e)
        {
            if (sender == ArrowRight)
            {
                await bluetoothService.SendCommand(Commands.TurnRight);
            }
            else if (sender == ArrowLeft)
            {
                await bluetoothService.SendCommand(Commands.TurnLeft);
            }
            else if (sender == ArrowUp)
            {
                await bluetoothService.SendCommand(Commands.Forward);
            }
            else if (sender == ArrowDown)
            {
                await bluetoothService.SendCommand(Commands.Backward);
            }
        }

        private async void ArrowReleased(object? sender, EventArgs e)
        {
            await bluetoothService.SendCommand(Commands.Stop);
        }

        private async void ConnectToRobot_Clicked(object? sender, EventArgs e)
        {
            try
            {
                if (!ViewModel.CanConnect)
                    return;

                ViewModel.IsConnecting = true;
                ViewModel.CanConnect = false;

                await bluetoothService.Connect();

                ViewModel.IsConnecting = false;
                ViewModel.CanConnect = !bluetoothService.IsConnected;

                ViewModel.IsConnected = bluetoothService.IsConnected;
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
                ViewModel.IsConnecting = false;
                ViewModel.CanConnect = true;
            }
        }

        private void GamepadService_LeftJoystickMoved(object? sender, GamepadJoystickEventArgs e)
        {
        }

        private void GamepadService_ButtonStateChanged(object? sender, GamepadButtonEventArgs e)
        {
        }
    }

}
