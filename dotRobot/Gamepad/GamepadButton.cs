using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotRobot.Gamepad
{
    //
    // Summary:
    //     Exposes constants that represent a button of an XInput controller. These constants
    //     can be used as bitwise flags to represent several buttons.
    [Flags]
    public enum GamepadButtons
    {
        //
        // Summary:
        //     No button. This is used to represent no buttons.
        None = 0,
        //
        // Summary:
        //     D-Pad Up. This is one of the directional buttons.
        DPadUp = 1,
        //
        // Summary:
        //     D-Pad Down. This is one of the directional buttons.
        DPadDown = 2,
        //
        // Summary:
        //     D-Pad Left. This is one of the directional buttons.
        DPadLeft = 4,
        //
        // Summary:
        //     D-Pad Right. This is one of the directional buttons.
        DPadRight = 8,
        //
        // Summary:
        //     The Start button.
        Start = 0x10,
        //
        // Summary:
        //     The Back button.
        Back = 0x20,
        //
        // Summary:
        //     The LS (Left Stick) button.
        LS = 0x40,
        //
        // Summary:
        //     The RS (Right Stick) button.
        RS = 0x80,
        //
        // Summary:
        //     The LB (Left Shoulder) button.
        LB = 0x100,
        //
        // Summary:
        //     The RB (Right Shoulder).
        RB = 0x200,
        //
        // Summary:
        //     The A button.
        A = 0x1000,
        //
        // Summary:
        //     The B button.
        B = 0x2000,
        //
        // Summary:
        //     The X button.
        X = 0x4000,
        //
        // Summary:
        //     The Y button.
        Y = 0x8000
    }
}
