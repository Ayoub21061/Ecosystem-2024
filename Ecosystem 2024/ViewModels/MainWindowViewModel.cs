﻿using Avalonia;
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
    public Carnivore carnivore;
    public int Width {get;} = 800;

    public int Height {get;} = 450;
    
       // Liste des objets à afficher
    public ObservableCollection<GameObject> GameObjects { get; } = new();
    public MainWindowViewModel() {
        carnivore = new Carnivore(new Point(Width/2, Height/2));
        GameObjects.Add(carnivore);
    }

    protected override void Tick() {}

}
