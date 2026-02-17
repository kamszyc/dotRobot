namespace dotRobot;

public partial class MainWindow : Window
{
    public MainWindow(MainPage mainPage)
	{
		InitializeComponent();
		Page = mainPage;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
    }
}