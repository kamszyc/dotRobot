using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace dotRobot.Style
{
    public class WindowStyler
    {
        public void Style(Window window)
        {
            var platformView = window.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
            if (platformView == null)
                return;

            platformView.SetIsMaximizable(false);
            platformView.SetIsResizable(false);
        }
    }
}
