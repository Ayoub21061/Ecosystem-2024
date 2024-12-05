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

    [ObservableProperty]
    private int organicWasteTime;

    private bool isDead;

    public double Vision {get; set;} = 100;
    public double Speed {get; set;} = 2.0;
    
    public Carnivore(Point location) : base(location) {
        Energy = 100;
        Healthpoints = 100;
        OrganicWasteTime = 50;
    }

    // Code qui permet de calculer la distance entre un carnivore et un autre être vivant
    public bool SawOpponent(GameObject other) {
        // Formule de la distance 
        var distance = Math.Sqrt(
            Math.Pow(Location.X - other.Location.X, 2) +
            Math.Pow(Location.Y - other.Location.Y, 2));
        // Si la distance est plus petite que le rayon de vision de l'animal, alors l'animal target l'être vivant.
        return distance <= Vision;                                   
    }

    public void Move() {
        if(!isDead) {
            Location = Location + Velocity;
        }
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

    public void OrganicWaste() {
        if(Healthpoints == 0) {
            OrganicWasteTime--;
                if(OrganicWasteTime == 0) {
                    CurrentImage = new Bitmap("Assets/Déchet.png");
                }
        }
    }
}