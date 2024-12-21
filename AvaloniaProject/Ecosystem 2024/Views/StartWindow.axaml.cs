using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Ecosystem_2024.Views
{
    public partial class StartWindow : Window
    {
        public event EventHandler? GameStarted;

        public StartWindow()
        {
            InitializeComponent();
            this.KeyDown += OnKeyDown;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                
                // Déclencher l'événement de démarrage du jeu
                GameStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}