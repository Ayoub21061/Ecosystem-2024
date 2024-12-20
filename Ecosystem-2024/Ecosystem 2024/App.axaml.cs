using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Ecosystem_2024.ViewModels;
using Ecosystem_2024.Views;

namespace Ecosystem_2024;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindowViewModel game = new MainWindowViewModel();
            game.Start();
            desktop.MainWindow = new MainWindow
            {
                DataContext = game,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}