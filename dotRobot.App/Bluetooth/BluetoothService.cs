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
        private GattCharacteristic? characteristic;
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

            var service = await btDevice.Gatt.GetPrimaryServiceAsync(Constants.ServiceGuid);
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
            try
            {
                await semaphoreSlim.WaitAsync();

                if (characteristic == null)
                {
                    Debug.WriteLine("Characteristic is null. Cannot send command.");
                    return;
                }

                await characteristic.WriteValueWithResponseAsync(Encoding.UTF8.GetBytes(command));

                Debug.WriteLine($"Command sent: {command}");
            }
            finally
            {
                semaphoreSlim.Release();
            } 
        }

        private async Task<BluetoothDevice?> ScanForDevice()
        {
            var options = new RequestDeviceOptions();
            var filter = new BluetoothLEScanFilter
            {
                Name = Constants.BluetoothDeviceName
            };
            filter.Services.Add(Constants.ServiceGuid);
            options.Filters.Add(filter);

            var discoveredDevices = await InTheHand.Bluetooth.Bluetooth.ScanForDevicesAsync(options);
            return discoveredDevices.FirstOrDefault(x => x.Name == Constants.BluetoothDeviceName);
        }

        private void OnGattServerDisconnected(object? sender, EventArgs e)
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
            btDevice = null;
            characteristic = null;
        }
    }
}
