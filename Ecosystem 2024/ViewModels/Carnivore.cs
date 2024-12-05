using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class Carnivore : GameObject {

    [ObservableProperty]
    private Point velocity = new Point(1.0 , 0.0); 

    [ObservableProperty]
    private Bitmap currentImage = new Bitmap("Assets/Carnivore.png");   

    [ObservableProperty]
    private int energy;

    [ObservableProperty]
    private int healthpoints;


    private bool isDead;
    
    public Carnivore(Point location) : base(location) {
        Energy = 100;
        Healthpoints = 100;
    }

    public void Move() {
        Location = Location + Velocity;
    }

    public void Die() {
        isDead = true;
        CurrentImage = new Bitmap("Assets/Viande.png");
    }

    public void ReduceEnergy() {
        if(!isDead) {
            Energy--;
            if(Energy <= 0) {
                Console.WriteLine("No Energy anymore");
                ReduceHealth();
                Energy = 0;
            }      
        }
     }

    public void ReduceHealth() {
        if(!isDead) {
            Healthpoints--;
            if(Healthpoints <= 0) {
                Die();

            }
        }
    }
}