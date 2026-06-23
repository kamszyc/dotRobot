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
        private static BluetoothService bluetoothService;
        private static MotorController motorController;
        private static LightsController lightsController;

        public static void Main()
        {
            bluetoothService = new BluetoothService();
            bluetoothService.Advertise();
            bluetoothService.CommandReceived += RobotControlCommandReceived;

            motorController = new MotorController();
            lightsController = new LightsController();

            while (true)
            {
                lightsController.Tick();
                Thread.Sleep(333);
            }
        }

        private static void RobotControlCommandReceived(BluetoothService sender, RobotControlCommandEventArgs eventArgs)
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
                case Commands.ForwardStop:
                    motorController.StopForwardOrBackward();
                    break;
                case Commands.BackwardStop:
                    motorController.StopForwardOrBackward();
                    break;
                case Commands.JoystickReset:
                    motorController.StopForwardOrBackward();
                    break;
                case Commands.TurnLeftStop:
                    motorController.StopTurnLeft();
                    break;
                case Commands.TurnRightStop:
                    motorController.StopTurnRight();
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

            if (eventArgs.Command.StartsWith(Commands.Speed))
            {
                string speedLevelString = eventArgs.Command.Substring(Commands.Speed.Length);
                if (int.TryParse(speedLevelString, out int speedValue))
                {
                    motorController.SetSpeedLevel(speedValue);
                }
            }

            if (eventArgs.Command.StartsWith(Commands.JoystickMove))
            {
                string xString = eventArgs.Command.Substring(Commands.JoystickMove.Length, 2);
                string yString = eventArgs.Command.Substring(Commands.JoystickMove.Length + 2, 2);

                if (int.TryParse(xString, out int x) && int.TryParse(yString, out int y))
                {
                    motorController.SetJoystickPosition(x, y);
                }
            }
        }
    }
}
