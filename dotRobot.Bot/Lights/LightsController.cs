using System;
using System.Device.Gpio;
using System.Text;

namespace dotRobot.Lights
{
    public class LightsController
    {
        private GpioPin whiteLed;
        private GpioPin leftTurnYellowLed;
        private GpioPin rightTurnYellowLed;
        private bool leftTurnOn = false;
        private bool rightTurnOn = false;

        public LightsController()
        {
            GpioController gpio = new GpioController();
            whiteLed = gpio.OpenPin(13, PinMode.Output);
            whiteLed.Write(PinValue.Low);
            leftTurnYellowLed = gpio.OpenPin(14, PinMode.Output);
            leftTurnYellowLed.Write(PinValue.Low);
            rightTurnYellowLed = gpio.OpenPin(32, PinMode.Output);
            rightTurnYellowLed.Write(PinValue.Low);
        }

        public void TurnOnWhiteLed()
        {
            whiteLed.Write(PinValue.High);
        }

        public void TurnOffWhiteLed()
        {
            whiteLed.Write(PinValue.Low);
        }

        public void ToggleLeftTurn()
        {
            leftTurnYellowLed.Write(leftTurnOn ? PinValue.Low : PinValue.High);
            leftTurnOn = !leftTurnOn;
        }

        public void TurnOffLeftTurn()
        {
            leftTurnYellowLed.Write(PinValue.Low);
        }

        public void ToggleRightTurn()
        {
            rightTurnYellowLed.Write(rightTurnOn ? PinValue.Low : PinValue.High);
            rightTurnOn = !rightTurnOn;
        }

        public void TurnOffRightTurn()
        {
            rightTurnYellowLed.Write(PinValue.Low);
        }
    }
}
