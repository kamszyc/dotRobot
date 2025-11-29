using System;
using System.Text;

namespace dotRobot.Common
{
    public static class Constants
    {
        public static readonly string BluetoothDeviceName = "dotRobot";
        public static readonly Guid ServiceGuid = new Guid("2F52F5C6-F72E-499E-B340-104F839DB50A");
        public static readonly Guid RobotControlCharactericticGuid = new Guid("A7C4EDB9-A598-4E3A-A032-03A0813C0D75");

        public const int MinSpeedLevel = 1;
        public const int DefaultSpeedLevel = 5;
        public const int MaxSpeedLevel = 9;
    }
}
