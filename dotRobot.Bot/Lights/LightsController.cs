using System;
using System.Device.Gpio;
using System.Text;

namespace dotRobot.Lights
{
    public class LightsController
    {
        private GpioPin whiteLed;

        public LightsController()
        {
            GpioController gpio = new GpioController();
            whiteLed = gpio.OpenPin(13, PinMode.Output);
            whiteLed.Write(PinValue.Low);
        }

        public void TurnOnWhiteLed()
        {
            whiteLed.Write(PinValue.High);
        }

        public void TurnOffWhiteLed()
        {
            whiteLed.Write(PinValue.Low);
        }
    }
}
