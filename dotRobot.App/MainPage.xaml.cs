using CommunityToolkit.Mvvm.ComponentModel;
using dotRobot.Bluetooth;
using dotRobot.Common;
using dotRobot.Gamepad;
using Microsoft.Maui.Dispatching;
using Plugin.Maui.KeyListener;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace dotRobot
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
    }

}
