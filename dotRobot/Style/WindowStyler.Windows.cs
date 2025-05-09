using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotRobot.Style
{
    public class WindowStyler
    {
        public void Style(Window window)
        {
            var platformView = window.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
            if (platformView == null)
                return;

            var appWindowPresenter = platformView.AppWindow.Presenter as OverlappedPresenter;
            appWindowPresenter.IsResizable = false;
            appWindowPresenter.IsMaximizable = false;
        }
    }
}
