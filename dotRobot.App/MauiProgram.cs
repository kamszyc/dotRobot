using CommunityToolkit.Maui;
using dotRobot.Bluetooth;
using dotRobot.Gamepad;
using Microsoft.Extensions.Logging;
using Plugin.Maui.KeyListener;

namespace dotRobot
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
			    .UseMauiCommunityToolkit()
                .UseKeyListener()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainWindow>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddTransient<GamepadService>();
            builder.Services.AddTransient<BluetoothService>();

            return builder.Build();
        }
    }
}
