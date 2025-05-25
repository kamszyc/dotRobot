namespace dotRobot
{
    public partial class App : Application
    {
        private readonly MainWindow mainWindow;

        public App(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return mainWindow;
        }
    }
}