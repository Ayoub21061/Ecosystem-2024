using System;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class Plante : GameObject {

    [ObservableProperty]
    private int energy;

    [ObservableProperty]
    private int healthpoints;

    [ObservableProperty]
    private Bitmap currentImagePlant = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Plante.png"))); 

    [ObservableProperty]
    private bool isDead = false; 
    
    // Constructeur 
    public Plante (Point location) : base(location) {
        Energy = 100;
        Healthpoints = 100;
    }

    public void OrganicWaste() {
        if(!IsDead) {
            Energy--;
            if(Energy <= 0) {
                Energy = 0;
                ReduceHealth();
            }
        } 
    }

    public void ReduceHealth() {
        if(!IsDead) {
            Healthpoints--;
            if(Healthpoints <= 0) {
                IsDead = true;
                CurrentImagePlant = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Déchet.png")));
            }
        }  
    }
}