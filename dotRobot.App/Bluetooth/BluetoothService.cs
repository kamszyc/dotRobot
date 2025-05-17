using dotRobot.Common;
using InTheHand.Bluetooth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotRobot.Bluetooth
{
    public class BluetoothService
    {
        private BluetoothDevice? btDevice;
        private GattService service;
        private GattCharacteristic characteristic;

        public bool IsConnected => btDevice?.Gatt.IsConnected ?? false;

        public async Task Connect()
        {
            var permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>();
            if (permissionResult != PermissionStatus.Granted)
            {
                Debug.WriteLine("Bluetooth scan permission denied.");
            }
            await ConnectInternal();
        }

        private async Task ConnectInternal()
        {
            var options = new RequestDeviceOptions();
            var filter = new BluetoothLEScanFilter
            {
                Name = Constants.BluetoothDeviceName
            };
            filter.Services.Add(Constants.ServiceGuid);
            options.Filters.Add(filter);

            var discoveredDevices = await InTheHand.Bluetooth.Bluetooth.ScanForDevicesAsync(options);
            btDevice = discoveredDevices.FirstOrDefault(x => x.Name == Constants.BluetoothDeviceName);
            if (btDevice == null)
            {
                throw new InvalidOperationException("No device found with the specified name.");
            }

            await btDevice.Gatt.ConnectAsync();
            if (!btDevice.Gatt.IsConnected)
            {
                throw new InvalidOperationException($"Failed to connect to {btDevice.Name}.");
            }
            Debug.WriteLine($"Connected to {btDevice.Name}.");

            service = await btDevice.Gatt.GetPrimaryServiceAsync(Constants.ServiceGuid);
            if (service == null)
            {
                throw new InvalidOperationException("Service not found.");
            }

            characteristic = await service.GetCharacteristicAsync(Constants.RobotControlCharactericticGuid);
            if (characteristic == null)
            {
                throw new InvalidOperationException("Characteristic not found.");
            }
        }

        public async Task SendCommand(string command)
        {
            if (characteristic == null)
            {
                Debug.WriteLine("Characteristic is null. Cannot send command.");
                return;
            }

            await characteristic.WriteValueWithoutResponseAsync(Encoding.UTF8.GetBytes(command));

            Debug.WriteLine($"Command sent: {command}");
        }
    }
}
