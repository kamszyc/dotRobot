using dotRobot.Common;
using Iot.Device.DCMotor;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Pwm;
using System.Diagnostics;
using System.Threading;

namespace dotRobot
{
    public class Program
    {
        static DCMotor leftMotor;
        static DCMotor rightMotor;
        public const int CommandLength = 4;

        public static void Main()
        {
            Configuration.SetPinFunction(25, DeviceFunction.PWM1);
            Configuration.SetPinFunction(26, DeviceFunction.PWM2);

            leftMotor = DCMotor.Create(
                PwmChannel.CreateFromPin(25, 50, 0.0),
                18, 19);

            rightMotor = DCMotor.Create(
                PwmChannel.CreateFromPin(26, 50, 0.0),
                21, 22);

            leftMotor.Speed = 0;
            rightMotor.Speed = 0;

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

            Thread.Sleep(Timeout.Infinite);
        }

        private static void RobotControlCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs writeRequestEventArgs)
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


            if (command == Commands.TurnRight)
            {
                TurnRight();
                Thread.Sleep(100);
                Stop();
            }
            else if (command == Commands.TurnLeft)
            {
                TurnLeft();
                Thread.Sleep(100);
                Stop();
            }
            else if (command == Commands.Forward)
            {
                Forward();
                Thread.Sleep(100);
                Stop();
            }
            else if (command == Commands.Backward)
            {
                Backward();
                Thread.Sleep(100);
                Stop();
            }
        }

        static void Forward()
        {
            leftMotor.Speed = 0.2;
            rightMotor.Speed = 0.2;
        }

        static void Backward()
        {
            leftMotor.Speed = -0.2;
            rightMotor.Speed = -0.2;
        }

        static void TurnLeft()
        {
            leftMotor.Speed = -0.2;
            rightMotor.Speed = 0.2;
        }

        static void TurnRight()
        {
            leftMotor.Speed = 0.2;
            rightMotor.Speed = -0.2;
        }

        static void Stop()
        {
            leftMotor.Speed = 0;
            rightMotor.Speed = 0;
        }
    }
}
