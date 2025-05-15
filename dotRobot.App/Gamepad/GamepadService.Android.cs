using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotRobot.Gamepad
{
    public class GamepadService
    {
        public event EventHandler<GamepadButtonEventArgs> ButtonStateChanged;
        public event EventHandler<GamepadJoystickEventArgs> LeftJoystickMoved;

        public void Start(IDispatcher dispatcher)
        {
        }
    }
}
