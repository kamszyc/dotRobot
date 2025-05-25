using dotRobot.Style;
using Microsoft.Maui.Platform;

namespace dotRobot;

public partial class MainWindow : Window
{
    private readonly WindowStyler windowStyler;

    public MainWindow(MainPage mainPage, WindowStyler windowStyler)
	{
		InitializeComponent();
		Page = mainPage;
        this.windowStyler = windowStyler;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        windowStyler.Style(this);
    }
}