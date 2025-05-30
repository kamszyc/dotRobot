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
        private readonly GamepadService gamepadService;

        private MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;

        public MainPage(MainPageViewModel mainPageViewModel, GamepadService gamepadService)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;

            ViewModel.RequestAlert += async (s, alert) =>
            {
                await DisplayAlert("Information", alert, "OK");
            };
            this.gamepadService = gamepadService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            gamepadService.Start(Dispatcher);
            gamepadService.ButtonStateChanged += GamepadService_ButtonStateChanged;
        }

        private void GamepadService_ButtonStateChanged(object? sender, GamepadButtonEventArgs e)
        {
            switch (e.Button)
            {
                case GamepadButtons.DPadUp:
                    if (e.IsPressed)
                    {
                        VisualStateManager.GoToState(ArrowUp, "Pressed");
                        ViewModel.ArrowUpPressedCommand.Execute(null);
                    }
                    else
                    {
                        VisualStateManager.GoToState(ArrowUp, "Normal");
                        ViewModel.ArrowUpReleasedCommand.Execute(null);
                    }
                    break;
                case GamepadButtons.DPadDown:
                    if (e.IsPressed)
                    {
                        VisualStateManager.GoToState(ArrowDown, "Pressed");
                        ViewModel.ArrowDownPressedCommand.Execute(null);
                    }
                    else
                    {
                        VisualStateManager.GoToState(ArrowDown, "Normal");
                        ViewModel.ArrowDownReleasedCommand.Execute(null);
                    }
                    break;
                case GamepadButtons.DPadLeft:
                    if (e.IsPressed)
                    {
                        VisualStateManager.GoToState(ArrowLeft, "Pressed");
                        ViewModel.ArrowLeftPressedCommand.Execute(null);
                    }
                    else
                    {
                        VisualStateManager.GoToState(ArrowLeft, "Normal");
                        ViewModel.ArrowLeftReleasedCommand.Execute(null);
                    }
                    break;
                case GamepadButtons.DPadRight:
                    if (e.IsPressed)
                    {
                        VisualStateManager.GoToState(ArrowRight, "Pressed");
                        ViewModel.ArrowRightPressedCommand.Execute(null);
                    }
                    else
                    {
                        VisualStateManager.GoToState(ArrowRight, "Normal");
                        ViewModel.ArrowRightReleasedCommand.Execute(null);
                    }
                    break;
                case GamepadButtons.LB:
                    if (e.IsPressed)
                    {
                        LeftTurnButton.IsChecked = !LeftTurnButton.IsChecked;
                        if (LeftTurnButton.IsChecked)
                        {
                            VisualStateManager.GoToState(LeftTurnButton, "Checked");
                        }
                        else
                        {
                            VisualStateManager.GoToState(LeftTurnButton, "Unchecked");
                        }
                    }
                    break;
                case GamepadButtons.RB:
                    if (e.IsPressed)
                    {
                        RightTurnButton.IsChecked = !RightTurnButton.IsChecked;
                        if (RightTurnButton.IsChecked)
                        {
                            VisualStateManager.GoToState(RightTurnButton, "Checked");
                        }
                        else
                        {
                            VisualStateManager.GoToState(RightTurnButton, "Unchecked");
                        }
                    }
                    break;
                case GamepadButtons.Y:
                    if (e.IsPressed)
                    {
                        LightsButton.IsChecked = !LightsButton.IsChecked;
                        if (LightsButton.IsChecked)
                        {
                            VisualStateManager.GoToState(LightsButton, "Checked");
                        }
                        else
                        {
                            VisualStateManager.GoToState(LightsButton, "Unchecked");
                        }
                    }
                    break;
                case GamepadButtons.X:
                    if (e.IsPressed)
                    {
                        VisualStateManager.GoToState(HornButton, "Pressed");
                        ViewModel.HornButtonPressedCommand.Execute(null);
                    }
                    else
                    {
                        VisualStateManager.GoToState(HornButton, "Normal");
                    }
                    break;
            }
        }
    }
}
