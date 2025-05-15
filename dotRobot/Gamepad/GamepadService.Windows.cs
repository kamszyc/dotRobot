using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Dispatching;
using XInputium.XInput;

namespace dotRobot.Gamepad
{
    public class GamepadService
    {
        public event EventHandler<GamepadButtonEventArgs> ButtonStateChanged;
        public event EventHandler<GamepadJoystickEventArgs> LeftJoystickMoved;

        public void Start(IDispatcher dispatcher)
        {
            var gamepad = new XGamepad();
            var timer = dispatcher.CreateTimer();
            gamepad.ButtonStateChanged += (s, e) =>
            {
                ButtonStateChanged?.Invoke(this, new GamepadButtonEventArgs
                {
                    Button = (GamepadButtons)e.Button.Button,
                    IsPressed = e.Button.IsPressed,
                });
            };
            gamepad.LeftJoystickMove += (s, e) =>
            {
                LeftJoystickMoved?.Invoke(this, new GamepadJoystickEventArgs
                {
                    X = gamepad.LeftJoystick.X,
                    Y = gamepad.LeftJoystick.Y,
                });
            };

            timer.Interval = TimeSpan.FromMilliseconds(16.666); // ~60Hz polling rate
            timer.Tick += (s, e) =>
            {
                gamepad.Update();
            };
            timer.Start();
        }
    }
}
