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
        public event EventHandler<string>? RequestAlert;

        private bool isConnected;
        private bool isConnecting;
        private bool canConnect;
        private readonly BluetoothService bluetoothService = new();

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

        public MainPageViewModel()
        {
            IsConnected = false;
            IsConnecting = false;
            CanConnect = true;
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

                IsConnecting = false;
                CanConnect = !bluetoothService.IsConnected;

                IsConnected = bluetoothService.IsConnected;
            }
            catch (InvalidOperationException ex)
            {
                RequestAlert?.Invoke(this, ex.Message);
                IsConnecting = false;
                CanConnect = true;
            }
        }

        private void BluetoothService_Disconnected(object? sender, EventArgs e)
        {
            IsConnected = false;
            CanConnect = true;
        }

        [RelayCommand]
        private async Task ArrowUpPressed()
        {
            await SendCommand(Commands.Forward);
        }

        [RelayCommand]
        private async Task ArrowUpReleased()
        {
            await SendCommand(Commands.Stop);
        }

        [RelayCommand]
        private async Task ArrowDownPressed()
        {
            await SendCommand(Commands.Backward);
        }

        [RelayCommand]
        private async Task ArrowDownReleased()
        {
            await SendCommand(Commands.Stop);
        }

        [RelayCommand]
        private async Task ArrowLeftPressed()
        {
            await SendCommand(Commands.TurnLeft);
        }

        [RelayCommand]
        private async Task ArrowLeftReleased()
        {
            await SendCommand(Commands.Stop);
        }

        [RelayCommand]
        private async Task ArrowRightPressed()
        {
            await SendCommand(Commands.TurnRight);
        }

        [RelayCommand]
        private async Task ArrowRightReleased()
        {
            await SendCommand(Commands.Stop);
        }

        [RelayCommand]
        private async Task LightsButtonChecked()
        {
            await SendCommand(Commands.LightsOn);
        }

        [RelayCommand]
        private async Task LightsButtonUnchecked()
        {
            await SendCommand(Commands.LightsOff);
        }

        [RelayCommand]
        private async Task LeftTurnButtonChecked()
        {
            await SendCommand(Commands.LeftTurnOn);
        }

        [RelayCommand]
        private async Task LeftTurnButtonUnchecked()
        {
            await SendCommand(Commands.LeftTurnOff);
        }

        [RelayCommand]
        private async Task RightTurnButtonChecked()
        {
            await SendCommand(Commands.RightTurnOn);
        }

        [RelayCommand]
        private async Task RightTurnButtonUnchecked()
        {
            await SendCommand(Commands.RightTurnOff);
        }

        private Task SendCommand(string command)
        {
            return bluetoothService.SendCommand(Commands.Forward);
        }
    }
}
