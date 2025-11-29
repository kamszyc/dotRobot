using Iot.Device.DCMotor;
using nanoFramework.Hardware.Esp32;
using System;
using System.Device.Pwm;
using System.Text;

namespace dotRobot.Motor
{
    public class MotorController
    {
        public DCMotor leftMotor;
        public DCMotor rightMotor;

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
            leftMotor.Speed = 0.8;
            rightMotor.Speed = 0.8;
        }

        public void Backward()
        {
            leftMotor.Speed = -0.8;
            rightMotor.Speed = -0.8;
        }

        public void TurnLeft()
        {
            leftMotor.Speed = -0.5;
            rightMotor.Speed = 0.5;
        }

        public void TurnRight()
        {
            leftMotor.Speed = 0.5;
            rightMotor.Speed = -0.5;
        }

        public void Stop()
        {
            leftMotor.Speed = 0;
            rightMotor.Speed = 0;
        }
    }
}
