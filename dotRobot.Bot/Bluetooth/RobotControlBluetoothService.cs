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
        public const int CommandLength = 4;

        public delegate void CommandReceivedEventHandler(RobotControlBluetoothService sender, RobotControlCommandEventArgs robotControlCommandEventArgs);
        public CommandReceivedEventHandler CommandReceived;

        public void Advertise()
        {
            BluetoothLEServer server = BluetoothLEServer.Instance;
            server.DeviceName = Constants.BluetoothDeviceName;

            GattServiceProviderResult result = GattServiceProvider.Create(Constants.ServiceGuid);
            if (result.Error != BluetoothError.Success)
            {
                return;
            }

            GattServiceProvider serviceProvider = result.ServiceProvider;
            GattLocalService service = serviceProvider.Service;

            var characteristicResult = service.CreateCharacteristic(Constants.RobotControlCharactericticGuid,
                new GattLocalCharacteristicParameters()
                {
                    CharacteristicProperties = GattCharacteristicProperties.Write,
                    UserDescription = "Robot control",
                });

            if (characteristicResult.Error != BluetoothError.Success)
            {
                return;
            }

            var robotControlCharacteristic = characteristicResult.Characteristic;
            robotControlCharacteristic.WriteRequested += RobotControlCharacteristic_WriteRequested;

            serviceProvider.StartAdvertising();
        }

        private void RobotControlCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs writeRequestEventArgs)
        {
            GattWriteRequest request = writeRequestEventArgs.GetRequest();

            if (request.Value.Length != CommandLength)
            {
                request.RespondWithProtocolError((byte)BluetoothError.NotSupported);
                return;
            }

            DataReader rdr = DataReader.FromBuffer(request.Value);
            string command = rdr.ReadString(CommandLength);

            // Respond if Write requires response
            if (request.Option == GattWriteOption.WriteWithResponse)
            {
                request.Respond();
            }

            Debug.WriteLine($"Received command={command}");

            CommandReceived?.Invoke(this, new RobotControlCommandEventArgs(command));
        }
    }
}
