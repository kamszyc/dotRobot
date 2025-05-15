using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotRobot.Gamepad
{
    public class GamepadJoystickEventArgs : EventArgs
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
