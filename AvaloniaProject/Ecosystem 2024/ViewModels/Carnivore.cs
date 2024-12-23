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
    private Point velocity = new Point(1.0 , 2.0); 

    [ObservableProperty]
    private Bitmap currentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Carnivore.png")));   

    [ObservableProperty]
    private int energy;

    [ObservableProperty]
    private int healthpoints;

    [ObservableProperty]
    private int organicWasteTime;

    private bool isDead;
    private bool isOrganicWaste;

    private bool poop = false;

    public double Vision {get; set;} = 100;
    public double Speed {get; set;} = 2.0;
    // On crée un cooldown qui permet de contrôler l'intervalle de temps entre chaque reproduction d'animal, permet d'éviter la création d'une infinité d'animal.
    private const int CoolDownTime = 5000;
    private const int CoolDownPoop = 3000;
   
    // On crée une variable qui va stocker le temps de la dernière reproduction entre chaque animal
    public DateTime LastReproductionTime {get; set;} = DateTime.MinValue;

     // On crée une variable qui va stocker le temps de la dernière défécation du carnivore.
    public DateTime LastPoopTime {get; set;} = DateTime.MinValue;
    // On crée un booléen qui permet la comparaison entre le temps actuel qui s'est écoulé depuis le lancement de l'application et le temps de la dernière reproduction.
    // Si celle-ci est plus grande que le cooldown imposé précedemment, alors la repoduction est possible et passe à True.
    public bool CanReproduce => (DateTime.Now - LastReproductionTime).TotalMilliseconds > CoolDownTime;
    public bool CanPoop => (DateTime.Now - LastPoopTime).TotalMilliseconds > CoolDownPoop;

    public Carnivore(Point location, int energy, int healthpoints, int organicWasteTime) : base(location) {
        this.Energy = energy;
        this.Healthpoints = healthpoints;
        this.OrganicWasteTime = organicWasteTime;
    }

    public bool CanReproduceIfNotDead() {        
        return CanReproduce && !isDead;
    }

    // Pour chaque reproduction réalisé, on impose que le temps de la dernière reproduvction vaut le temps pour lequel la reporduction a eu lieu.
    // Permet d'actualiser le booléen CanReproduce correctement.
    public void SetReproductionCooldown() {
        LastReproductionTime = DateTime.Now;
    }

    public void SetPoopCooldown() {
        LastPoopTime = DateTime.Now;
    }

    public GameObject? Poop() {
        // Vérifier si suffisamment de temps s'est écoulé depuis la dernière défécation
        if ((DateTime.Now - LastPoopTime).TotalMilliseconds >= CoolDownPoop && !poop) {
        
        LastPoopTime = DateTime.Now; // Mettre à jour le temps de la dernière défécation

        // Créer un déchet organique à proximité de la position actuelle
        var organicWaste = new OrganicWaste(new Point(Location.X, Location.Y+32));
        return organicWaste;
        }   

        return null; // Si le cooldown n'est pas écoulé, aucun déchet n'est créé
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
        poop = true; // Le carnivore ne peut plus déféquer
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
        if(!isOrganicWaste && Healthpoints <= 0) {
            OrganicWasteTime--;
            if(OrganicWasteTime == 0) {
                isOrganicWaste = true;
                CurrentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Déchet.png")));
            }
        }
    }

    public bool IsDechet() {
        return isOrganicWaste;
    }
}