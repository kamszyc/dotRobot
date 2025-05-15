using dotRobot.Style;
using Microsoft.Maui.Platform;

namespace dotRobot;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		Page = new MainPage();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        var styler = new WindowStyler();
        styler.Style(this);
    }
}