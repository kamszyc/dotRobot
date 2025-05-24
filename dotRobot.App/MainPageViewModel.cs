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
        private bool isConnected;
        private bool isConnecting;
        private bool canConnect;
        private BluetoothService bluetoothService = new BluetoothService();

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
                //await DisplayAlert("Alert", ex.Message, "OK");
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
            await bluetoothService.SendCommand(Commands.Forward);
        }

        [RelayCommand]
        private async Task ArrowUpReleased()
        {
            await bluetoothService.SendCommand(Commands.Stop);
        }

        [RelayCommand]
        private async Task ArrowDownPressed()
        {
            await bluetoothService.SendCommand(Commands.Backward);
        }

        [RelayCommand]
        private async Task ArrowDownReleased()
        {
            await bluetoothService.SendCommand(Commands.Stop);
        }

        [RelayCommand]
        private async Task ArrowLeftPressed()
        {
            await bluetoothService.SendCommand(Commands.TurnLeft);
        }

        [RelayCommand]
        private async Task ArrowLeftReleased()
        {
            await bluetoothService.SendCommand(Commands.Stop);
        }

        [RelayCommand]
        private async Task ArrowRightPressed()
        {
            await bluetoothService.SendCommand(Commands.TurnRight);
        }

        [RelayCommand]
        private async Task ArrowRightReleased()
        {
            await bluetoothService.SendCommand(Commands.Stop);
        }

        [RelayCommand]
        private async Task LightsButtonChecked()
        {
            await bluetoothService.SendCommand(Commands.LightsOn);
        }

        [RelayCommand]
        private async Task LightsButtonUnchecked()
        {
            await bluetoothService.SendCommand(Commands.LightsOff);
        }

        [RelayCommand]
        private async Task LeftTurnButtonChecked()
        {
            await bluetoothService.SendCommand(Commands.LeftTurnOn);
        }

        [RelayCommand]
        private async Task LeftTurnButtonUnchecked()
        {
            await bluetoothService.SendCommand(Commands.LeftTurnOff);
        }

        [RelayCommand]
        private async Task RightTurnButtonChecked()
        {
            await bluetoothService.SendCommand(Commands.RightTurnOn);
        }

        [RelayCommand]
        private async Task RightTurnButtonUnchecked()
        {
            await bluetoothService.SendCommand(Commands.RightTurnOff);
        }
    }
}
