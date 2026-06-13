using dotRobot.Common;
using Iot.Device.DCMotor;
using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Pwm;
using System.Text;

namespace dotRobot.Motor
{
    public class MotorController
    {
        private DCMotor leftMotor;
        private DCMotor rightMotor;
        private int speedLevel = SpeedLevels.Default;

        private const double maxMotorSpeed = 0.8;

        public MotorController()
        {
            Configuration.SetPinFunction(25, DeviceFunction.PWM1);
            Configuration.SetPinFunction(26, DeviceFunction.PWM2);

            leftMotor = DCMotor.Create(
                PwmChannel.CreateFromPin(25, 50, 0.0),
                18, 19);

            rightMotor = DCMotor.Create(
                PwmChannel.CreateFromPin(26, 50, 0.0),
                22, 21);

            leftMotor.Speed = 0;
            rightMotor.Speed = 0;
        }

        public void Forward()
        {
            leftMotor.Speed = CalculateSpeed();
            rightMotor.Speed = CalculateSpeed();
        }

        public void Backward()
        {
            leftMotor.Speed = -CalculateSpeed();
            rightMotor.Speed = -CalculateSpeed();
        }

        public void TurnLeft()
        {
            leftMotor.Speed = leftMotor.Speed / 2;
        }

        public void TurnRight()
        {
            rightMotor.Speed = rightMotor.Speed / 2;
        }

        public void StopForwardOrBackward()
        {
            leftMotor.Speed = 0;
            rightMotor.Speed = 0;
        }

        public void StopTurnLeft()
        {
            leftMotor.Speed = leftMotor.Speed * 2;
        }

        public void StopTurnRight()
        {
            rightMotor.Speed = rightMotor.Speed * 2;
        }

        public void SetSpeedLevel(int level)
        {
            speedLevel = level;
        }

        private double CalculateSpeed()
        {
            return ((double)speedLevel / SpeedLevels.Max) * maxMotorSpeed;
        }

        public void SetJoystickPosition(int x, int y)
        {
            //Reverse steering when going backwards
            if (y < 0)
                x = -x;

            var left = y + x;
            var right = y - x;

            var div = Math.Max(SpeedLevels.Max, Math.Max(Math.Abs(left), Math.Abs(right)));

            var leftPercent = (double)left / div;
            var rightPercent = (double)right / div;

            leftMotor.Speed = leftPercent * maxMotorSpeed;
            rightMotor.Speed = rightPercent * maxMotorSpeed;
        }
    }
}
