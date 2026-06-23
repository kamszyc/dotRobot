using dotRobot.Common;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace dotRobot.Bluetooth
{
    public class RobotControlBluetoothService
    {
        public const int MinCommandLength = 4;

        public delegate void CommandReceivedEventHandler(RobotControlBluetoothService sender, RobotControlCommandEventArgs robotControlCommandEventArgs);
        public CommandReceivedEventHandler CommandReceived;

        public void Advertise()
        {
            BluetoothLEServer server = BluetoothLEServer.Instance;
            server.DeviceName = Constants.BluetoothDeviceName;

            bool success = CreateRobotControlCharacteristic(out GattServiceProvider robotControlServiceProvider, out GattLocalCharacteristic robotControlCharacteristic);
            if (!success)
            {
                return;
            }

            success = CreateBatteryLevelCharacteristic(out GattServiceProvider batteryServiceProvider, out GattLocalCharacteristic batteryLevelCharacteristic);
            if (!success)
            {
                return;
            }

            robotControlCharacteristic.WriteRequested += RobotControlCharacteristic_WriteRequested;
            batteryLevelCharacteristic.ReadRequested += BatteryLevelCharacteristic_ReadRequested;

            robotControlServiceProvider.StartAdvertising();
            batteryServiceProvider.StartAdvertising();
        }

        private static bool CreateRobotControlCharacteristic(out GattServiceProvider serviceProvider, out GattLocalCharacteristic robotControlCharacteristic)
        {
            serviceProvider = null;
            robotControlCharacteristic = null;

            GattServiceProviderResult result = GattServiceProvider.Create(Constants.RobotControlServiceGuid);
            if (result.Error != BluetoothError.Success)
            {
                return false;
            }

            serviceProvider = result.ServiceProvider;
            GattLocalService service = serviceProvider.Service;

            var characteristicResult = service.CreateCharacteristic(Constants.RobotControlCharactericticGuid,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Write,
                    UserDescription = "Robot control",
                });

            if (characteristicResult.Error != BluetoothError.Success)
            {
                return false;
            }

            robotControlCharacteristic = characteristicResult.Characteristic;
            return true;
        }

        private static bool CreateBatteryLevelCharacteristic(out GattServiceProvider serviceProvider, out GattLocalCharacteristic batteryLevelCharacteristic)
        {
            serviceProvider = null;
            batteryLevelCharacteristic = null;

            GattServiceProviderResult result = GattServiceProvider.Create(GattServiceUuids.Battery);
            if (result.Error != BluetoothError.Success)
            {
                return false;
            }

            serviceProvider = result.ServiceProvider;
            GattLocalService service = serviceProvider.Service;

            var characteristicResult = service.CreateCharacteristic(GattCharacteristicUuids.BatteryLevel,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.Notify,
                    UserDescription = "Battery level",
                });

            if (characteristicResult.Error != BluetoothError.Success)
            {
                return false;
            }

            batteryLevelCharacteristic = characteristicResult.Characteristic;
            return true;
        }

        private void RobotControlCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs writeRequestEventArgs)
        {
            GattWriteRequest request = writeRequestEventArgs.GetRequest();

            if (request.Value.Length < MinCommandLength)
            {
                request.RespondWithProtocolError((byte)BluetoothError.NotSupported);
                return;
            }
            DataReader rdr = DataReader.FromBuffer(request.Value);
            uint commandLength = rdr.ReadUInt32();
            string command = rdr.ReadString(commandLength);

            // Respond if Write requires response
            if (request.Option == GattWriteOption.WriteWithResponse)
            {
                request.Respond();
            }

            Debug.WriteLine($"Received command={command}");

            CommandReceived?.Invoke(this, new RobotControlCommandEventArgs(command));
        }

        private void BatteryLevelCharacteristic_ReadRequested(GattLocalCharacteristic sender, GattReadRequestedEventArgs readRequestEventArgs)
        {
            GattReadRequest request = readRequestEventArgs.GetRequest();
            request.RespondWithValue(new Buffer(new byte[] { 42 }));
        }
    }
}
