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
        private bool leftTurn = false;
        private bool rightTurn = false;
        private bool turnLightsCurrentlyOn = false;

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

        public void Tick()
        {
            if (leftTurn && turnLightsCurrentlyOn)
            {
                TurnOnLeftTurn();
            }
            else
            {
                TurnOffLeftTurn();
            }

            if (rightTurn && turnLightsCurrentlyOn)
            {
                TurnOnRightTurn();
            }
            else
            {
                TurnOffRightTurn();
            }

            turnLightsCurrentlyOn = !turnLightsCurrentlyOn;
        }

        public void EnableLeftTurn()
        {
            leftTurn = true;
        }

        public void DisableLeftTurn()
        {
            leftTurn = false;
        }

        public void EnableRightTurn()
        {
            rightTurn = true;
        }

        public void DisableRightTurn()
        {
            rightTurn = false;
        }

        public void TurnOnWhiteLed()
        {
            whiteLed.Write(PinValue.High);
        }

        public void TurnOffWhiteLed()
        {
            whiteLed.Write(PinValue.Low);
        }

        private void TurnOnLeftTurn()
        {
            leftTurnYellowLed.Write(PinValue.High);
        }

        private void TurnOffLeftTurn()
        {
            leftTurnYellowLed.Write(PinValue.Low);
        }

        private void TurnOnRightTurn()
        {
            rightTurnYellowLed.Write(PinValue.High);
        }

        private void TurnOffRightTurn()
        {
            rightTurnYellowLed.Write(PinValue.Low);
        }
    }
}
