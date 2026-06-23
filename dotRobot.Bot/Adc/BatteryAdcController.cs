using System;
using System.Device.Adc;
using System.Text;

namespace dotRobot.Adc
{
    public class BatteryAdcController
    {
        private AdcChannel adcChannel;

        public void Initialize()
        {
            AdcController adcController = new AdcController();
            adcChannel = adcController.OpenChannel(6); // pin 34, https://docs.nanoframework.net/content/esp32/esp32_pin_out.html#adc
        }

        public int ReadBatteryLevel()
        {
            double ratio = adcChannel.ReadRatio();
            int batteryLevel = ConvertToBatteryLevel(ratio);
            return batteryLevel;
        }

        private int ConvertToBatteryLevel(double ratio)
        {
            double voltage = ratio * 3.3; // Assuming a 3.3V reference voltage
            double batteryVoltage = voltage * (100.0 + 47.0) / 47.0;
            return (int)((batteryVoltage / 8.4) * 100); // 8.4V is the maximum battery voltage (2 x Li-ion 18650 battery)
        }
    }
}
