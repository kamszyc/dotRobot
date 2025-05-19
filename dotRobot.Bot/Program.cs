using dotRobot.Bluetooth;
using dotRobot.Common;
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

        public static void Main()
        {
            robotControlBluetoothService = new RobotControlBluetoothService();
            robotControlBluetoothService.Advertise();
            robotControlBluetoothService.CommandReceived += RobotControlCommandReceived;

            motorController = new MotorController();

            Thread.Sleep(Timeout.Infinite);
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
            }
        }
    }
}
