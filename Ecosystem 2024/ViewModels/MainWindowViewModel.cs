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
using System.ComponentModel;


namespace Ecosystem_2024.ViewModels;

public partial class MainWindowViewModel : GameBase
{
    public Carnivore carnivore;
    public Herbivore herbivore;
    public int Width {get;} = 800;

    public int Height {get;} = 450;
    
       // Liste des objets à afficher
    public ObservableCollection<GameObject> GameObjects { get; } = new();
    public MainWindowViewModel() {
        carnivore = new Carnivore(new Point(Width/2, Height/2));
        GameObjects.Add(carnivore);

        herbivore = new Herbivore(new Point(Width/2+32, Height/2+32));
        GameObjects.Add(herbivore);
    }

    protected override void Tick() {
        carnivore.ReduceEnergy();
        carnivore.OrganicWaste();
    }

}
