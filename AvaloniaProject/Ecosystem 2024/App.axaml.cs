using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ecosystem_2024.ViewModels;
using Ecosystem_2024.Views;
using System;

namespace Ecosystem_2024
{
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
                var startWindow = new StartWindow();
                startWindow.GameStarted += OnGameStarted;
                desktop.MainWindow = startWindow;
                startWindow.Show();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void OnGameStarted(object? sender, EventArgs e)
        {
            var game = new MainWindowViewModel();
            game.Start();
            var mainWindow = new MainWindow()
            {
                DataContext = game,
            };

            // Définir MainWindow comme la fenêtre principale de l'application
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = mainWindow;
                mainWindow.Show();
            }

            // Retarder la fermeture de StartWindow
            if (sender is Window startWindow)
            {
                // Utiliser un DispatcherTimer pour attendre la fin de l'affichage de MainWindow
                var timer = new Avalonia.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(100) // Attendre 100ms
                };
                timer.Tick += (s, args) =>
                {
                    startWindow.Close(); 
                    timer.Stop(); 
                    Console.WriteLine("StartWindow fermée.");
                };

                timer.Start(); 
            }
        }
    }
}
