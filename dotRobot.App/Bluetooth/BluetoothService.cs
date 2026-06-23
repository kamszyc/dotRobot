using dotRobot.Common;
using InTheHand.Bluetooth;
using Polly;
using Polly.Retry;
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
        private GattCharacteristic? robotControlCharacteristic;
        private GattCharacteristic? batteryLevelCharacteristic;
        private string lastCommand = string.Empty;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public bool IsConnected => btDevice?.Gatt.IsConnected ?? false;

        public event EventHandler? Disconnected;

        public async Task Connect()
        {
            var permissionResult = await Permissions.RequestAsync<Permissions.Bluetooth>();
            if (permissionResult != PermissionStatus.Granted)
            {
                throw new InvalidOperationException("Bluetooth scan permission denied.");
            }

            btDevice = await ScanForDevice();
            if (btDevice == null)
            {
                throw new InvalidOperationException("No device found with the specified name.");
            }

            ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions()
                {
                    ShouldHandle = new PredicateBuilder().Handle<Exception>().HandleResult(_ => !btDevice.Gatt.IsConnected),
                    Delay = TimeSpan.FromSeconds(5),
                    MaxRetryAttempts = 2,
                })
                .AddTimeout(TimeSpan.FromSeconds(10))
                .Build();

            await pipeline.ExecuteAsync(async _ => await btDevice.Gatt.ConnectAsync());
            if (!btDevice.Gatt.IsConnected)
            {
                throw new InvalidOperationException($"Failed to connect to {btDevice.Name}.");
            }
            Debug.WriteLine($"Connected to {btDevice.Name}.");

            btDevice.GattServerDisconnected += OnGattServerDisconnected;

            var robotControlService = await btDevice.Gatt.GetPrimaryServiceAsync(Constants.RobotControlServiceGuid);
            if (robotControlService == null)
            {
                throw new InvalidOperationException("Service not found.");
            }

            robotControlCharacteristic = await robotControlService.GetCharacteristicAsync(Constants.RobotControlCharactericticGuid);
            if (robotControlCharacteristic == null)
            {
                throw new InvalidOperationException("Characteristic not found.");
            }

            var batteryService = await btDevice.Gatt.GetPrimaryServiceAsync(GattServiceUuids.Battery);
            if (batteryService == null)
            {
                throw new InvalidOperationException("Battery service not found.");
            }

            batteryLevelCharacteristic = await batteryService.GetCharacteristicAsync(GattCharacteristicUuids.BatteryLevel);
            if (batteryLevelCharacteristic == null)
            {
                throw new InvalidOperationException("Battery level characteristic not found.");
            }
        }

        public async Task SendCommand(string command)
        {
            try
            {
                await semaphoreSlim.WaitAsync();

                if (robotControlCharacteristic == null)
                {
                    Debug.WriteLine("Characteristic is null. Cannot send command.");
                    return;
                }

                if (command == lastCommand)
                {
                    return;
                }
                lastCommand = command;

                await robotControlCharacteristic.WriteValueWithResponseAsync([.. BitConverter.GetBytes((uint)command.Length), .. Encoding.UTF8.GetBytes(command)]);

                Debug.WriteLine($"Command sent: {command}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task<byte> ReadBatteryLevel()
        {
            if (batteryLevelCharacteristic == null)
            {
                throw new InvalidOperationException("Battery level characteristic is not initialized.");
            }

            var batteryLevelData = await batteryLevelCharacteristic.ReadValueAsync();
            return batteryLevelData?[0] ?? 0;
        }

        private async Task<BluetoothDevice?> ScanForDevice()
        {
            var options = new RequestDeviceOptions();
            var filter = new BluetoothLEScanFilter
            {
                Name = Constants.BluetoothDeviceName
            };
            filter.Services.Add(Constants.RobotControlServiceGuid);
            options.Filters.Add(filter);

            var discoveredDevices = await InTheHand.Bluetooth.Bluetooth.ScanForDevicesAsync(options);
            return discoveredDevices.FirstOrDefault(x => x.Name == Constants.BluetoothDeviceName);
        }

        private void OnGattServerDisconnected(object? sender, EventArgs e)
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
            btDevice = null;
            robotControlCharacteristic = null;
        }
    }
}
