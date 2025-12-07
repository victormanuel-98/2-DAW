using MauiAppDAW.Views;

namespace MauiAppDAW;

public partial class App : Application
{
    private readonly IServiceProvider? _serviceProvider;

    public static IServiceProvider? ServiceProvider { get; private set; }

    public App()
    {
        InitializeComponent();
    }

    public App(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        ServiceProvider = serviceProvider;
    }

    // Sobrescribimos CreateWindow en lugar de usar MainPage.set
    protected override Window CreateWindow(IActivationState? activationState)
    {
        Page startPage;

        if (_serviceProvider != null)
        {
            startPage = _serviceProvider.GetService<SimpleLoginPage>() ?? new SimpleLoginPage();
        }
        else
        {
            startPage = new SimpleLoginPage();
        }

        var nav = new NavigationPage(startPage);
        var window = new Window(nav);
        return window;
    }
}
