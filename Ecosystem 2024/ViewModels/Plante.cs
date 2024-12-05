using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class Plante : GameObject {
    [ObservableProperty]
    private int energy;

    [ObservableProperty]
    private int healthpoints;
   
    // Constructeur 
    public Plante (Point location) : base(location) {
        Energy = 100;
        Healthpoints = 100;
       
    }
}