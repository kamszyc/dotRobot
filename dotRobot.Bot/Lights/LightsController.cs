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

        public void Tick()
        {
            if (leftTurn)
            {
                ToggleLeftTurn();
            }
            else
            {
                TurnOffLeftTurn();
            }

            if (rightTurn)
            {
                ToggleRightTurn();
            }
            else
            {
                TurnOffRightTurn();
            }
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

        private void ToggleLeftTurn()
        {
            leftTurnYellowLed.Write(leftTurnOn ? PinValue.Low : PinValue.High);
            leftTurnOn = !leftTurnOn;
        }

        private void ToggleRightTurn()
        {
            rightTurnYellowLed.Write(rightTurnOn ? PinValue.Low : PinValue.High);
            rightTurnOn = !rightTurnOn;
        }

        private void TurnOffLeftTurn()
        {
            leftTurnYellowLed.Write(PinValue.Low);
        }

        private void TurnOffRightTurn()
        {
            rightTurnYellowLed.Write(PinValue.Low);
        }
    }
}
