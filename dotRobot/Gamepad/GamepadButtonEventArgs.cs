using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotRobot.Gamepad
{
    public class GamepadButtonEventArgs : EventArgs
    {
        public GamepadButtons Button { get; set; }
        public bool IsPressed { get; set; }
    }
}
