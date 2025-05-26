using Iot.Device.Yx5300;
using nanoFramework.Hardware.Esp32;
using System;
using System.Text;

namespace dotRobot.Sound
{
    public class SoundController
    {
        private Yx5300 mp3Player;

        public SoundController()
        {
            Configuration.SetPinFunction(Gpio.IO17, DeviceFunction.COM2_TX);
            Configuration.SetPinFunction(Gpio.IO16, DeviceFunction.COM2_RX);

            mp3Player = new Yx5300("COM2");
            mp3Player.Volume(Yx5300.MaxVolume / 2);
        }

        public void PlayHornSound()
        {
            mp3Player.PlayTrack(1);
        }
    }
}
