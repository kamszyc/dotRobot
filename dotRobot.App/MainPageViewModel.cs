using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotRobot
{
    public class MainPageViewModel : ObservableObject
    {
        private bool isConnected;
        private bool isConnecting;
        private bool canConnect;

        public MainPageViewModel()
        {
            IsConnected = false;
            IsConnecting = false;
            CanConnect = true;
        }

        public bool IsConnected
        {
            get => isConnected;
            set => SetProperty(ref isConnected, value);
        }

        public bool IsConnecting
        {
            get => isConnecting;
            set => SetProperty(ref isConnecting, value);
        }

        public bool CanConnect
        {
            get => canConnect;
            set => SetProperty(ref canConnect, value);
        }
    }
}
