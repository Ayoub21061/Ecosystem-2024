using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using Avalonia.Markup.Xaml.MarkupExtensions;


namespace Ecosystem_2024.ViewModels;

public partial class MainWindowViewModel : GameBase
{
    
       // Liste des objets à afficher
    public ObservableCollection<GameObject> GameObjects { get; } = new();
    public MainWindowViewModel() {}

    protected override void Tick() {}

}
