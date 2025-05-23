using dotRobot.Bluetooth;
using dotRobot.Common;
using dotRobot.Lights;
using dotRobot.Motor;
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
        private static RobotControlBluetoothService robotControlBluetoothService;
        private static MotorController motorController;
        private static LightsController lightsController;

        public static void Main()
        {
            robotControlBluetoothService = new RobotControlBluetoothService();
            robotControlBluetoothService.Advertise();
            robotControlBluetoothService.CommandReceived += RobotControlCommandReceived;

            motorController = new MotorController();
            lightsController = new LightsController();

            while (true)
            {
                lightsController.Tick();
                Thread.Sleep(333);
            }
        }

        private static void RobotControlCommandReceived(RobotControlBluetoothService sender, RobotControlCommandEventArgs eventArgs)
        {
            switch (eventArgs.Command)
            {
                case Commands.TurnRight:
                    motorController.TurnRight();
                    break;
                case Commands.TurnLeft:
                    motorController.TurnLeft();
                    break;
                case Commands.Forward:
                    motorController.Forward();
                    break;
                case Commands.Backward:
                    motorController.Backward();
                    break;
                case Commands.Stop:
                    motorController.Stop();
                    break;
                case Commands.LightsOn:
                    lightsController.TurnOnWhiteLed();
                    break;
                case Commands.LightsOff:
                    lightsController.TurnOffWhiteLed();
                    break;
                case Commands.LeftTurnOn:
                    lightsController.EnableLeftTurn();
                    break;
                case Commands.LeftTurnOff:
                    lightsController.DisableLeftTurn();
                    break;
                case Commands.RightTurnOn:
                    lightsController.EnableRightTurn();
                    break;
                case Commands.RightTurnOff:
                    lightsController.DisableRightTurn();
                    break;
            }
        }
    }
}
