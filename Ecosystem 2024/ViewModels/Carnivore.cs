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
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class Carnivore : GameObject {

    [ObservableProperty]
    private Point velocity = new Point(1.0 , 0.0); 

    [ObservableProperty]
    private Bitmap currentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Carnivore.png")));   

    [ObservableProperty]
    private int energy;

    [ObservableProperty]
    private int healthpoints;

    [ObservableProperty]
    private int organicWasteTime;

    private bool isDead;

    public double Vision {get; set;} = 100;
    public double Speed {get; set;} = 2.0;
    // On crée un cooldown qui permet de contrôler l'intervalle de temps entre chaque reproduction d'animal, permet d'éviter la création d'une infinité d'animal.
    private const int CoolDownTime = 5000;
    // On crée une variable qui va stocker le temps de la dernière reproduction entre chaque animal
    public DateTime LastReproductionTime {get; private set;} = DateTime.MinValue;
    // On crée un booléen qui permet la comparaison entre le temps actuel qui s'est écoulé depuis le lancement de l'application et le temps de la dernière reproduction.
    // Si celle-ci est plus grande que le cooldown imposé précedemment, alors la repoduction est possible et passe à True.
    public bool CanReproduce => (DateTime.Now - LastReproductionTime).TotalMilliseconds > CoolDownTime;

    // Pour chaque reproduction réalisé, on impose que le temps de la dernière reproduvction vaut le temps pour lequel la reporduction a eu lieu.
    // Permet d'actualiser le booléen CanReproduce correctement.
    public void SetReproductionCooldown() {
        LastReproductionTime = DateTime.Now;
    }

    
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
        CurrentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Viande.png")));
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
                    CurrentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Déchet.png")));
                }
        }
    }
}