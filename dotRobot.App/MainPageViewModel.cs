using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using dotRobot.Bluetooth;
using dotRobot.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dotRobot
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly BluetoothService bluetoothService;
        private bool isConnected;
        private bool isConnecting;
        private bool canConnect;
        private int currentSpeedLevel;

        public event EventHandler<string>? RequestAlert;

        public bool IsConnected
        {
            get => isConnected;
            set => SetProperty(ref isConnected, value);
        }

        public bool IsConnecting
        {
            get => isConnecting;
            set => SetProperty(ref isConnecting, value);
        }

        public bool CanConnect
        {
            get => canConnect;
            set => SetProperty(ref canConnect, value);
        }

        public int CurrentSpeedLevel
        {
            get => currentSpeedLevel;
            set => SetProperty(ref currentSpeedLevel, value);
        }

        public MainPageViewModel(BluetoothService bluetoothService)
        {
            IsConnected = false;
            IsConnecting = false;
            CanConnect = true;

            this.bluetoothService = bluetoothService;
            bluetoothService.Disconnected += BluetoothService_Disconnected;
        }

        [RelayCommand]
        private async Task ConnectToRobot()
        {
            try
            {
                if (!CanConnect)
                    return;

                IsConnecting = true;
                CanConnect = false;

                await bluetoothService.Connect();

                CurrentSpeedLevel = Constants.DefaultSpeedLevel;
                await SendSpeedCommand();

                IsConnecting = false;
                CanConnect = !bluetoothService.IsConnected;

                IsConnected = bluetoothService.IsConnected;
            }
            catch (Exception ex)
            {
                RequestAlert?.Invoke(this, ex.Message);
                IsConnecting = false;
                CanConnect = true;
            }
        }

        [RelayCommand]
        private async Task ArrowUpPressed() => await SendCommand(Commands.Forward);

        [RelayCommand]
        private async Task ArrowUpReleased() => await SendCommand(Commands.ForwardStop);

        [RelayCommand]
        private async Task ArrowDownPressed() => await SendCommand(Commands.Backward);

        [RelayCommand]
        private async Task ArrowDownReleased() => await SendCommand(Commands.BackwardStop);

        [RelayCommand]
        private async Task ArrowLeftPressed() => await SendCommand(Commands.TurnLeft);

        [RelayCommand]
        private async Task ArrowLeftReleased() => await SendCommand(Commands.TurnLeftStop);

        [RelayCommand]
        private async Task ArrowRightPressed() => await SendCommand(Commands.TurnRight);

        [RelayCommand]
        private async Task ArrowRightReleased() => await SendCommand(Commands.TurnRightStop);

        [RelayCommand]
        private async Task LightsButtonChecked() => await SendCommand(Commands.LightsOn);

        [RelayCommand]
        private async Task LightsButtonUnchecked() => await SendCommand(Commands.LightsOff);

        [RelayCommand]
        private async Task LeftTurnButtonChecked() => await SendCommand(Commands.LeftTurnOn);

        [RelayCommand]
        private async Task LeftTurnButtonUnchecked() => await SendCommand(Commands.LeftTurnOff);

        [RelayCommand]
        private async Task RightTurnButtonChecked() => await SendCommand(Commands.RightTurnOn);

        [RelayCommand]
        private async Task RightTurnButtonUnchecked() => await SendCommand(Commands.RightTurnOff);

        [RelayCommand]
        private async Task SpeedMinusPressed()
        {
            CurrentSpeedLevel = Math.Max(Constants.MinSpeedLevel, CurrentSpeedLevel - 1);
            await SendSpeedCommand();
        }

        [RelayCommand]
        private async Task SpeedPlusPressed()
        {
            CurrentSpeedLevel = Math.Min(Constants.MaxSpeedLevel, CurrentSpeedLevel + 1);
            await SendSpeedCommand();
        }

        private void BluetoothService_Disconnected(object? sender, EventArgs e)
        {
            IsConnected = false;
            CanConnect = true;
            CurrentSpeedLevel = Constants.DefaultSpeedLevel;
        }

        private async Task SendSpeedCommand()
        {
            string command = Commands.Speed + CurrentSpeedLevel;
            await SendCommand(command);
        }

        private async Task SendCommand(string command)
        {
            await bluetoothService.SendCommand(command);
        }
    }
}
