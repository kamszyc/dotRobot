using System;
using System.Text;

namespace dotRobot.Bluetooth
{
    public class RobotControlCommandEventArgs : EventArgs
    {
        public RobotControlCommandEventArgs(string command)
        {
            Command = command;
        }

        public string Command { get; set; }
    }
}
