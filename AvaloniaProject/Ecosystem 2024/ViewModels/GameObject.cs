using System;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class GameObject : ObservableObject {
    [ObservableProperty]
    private Point _location;

    [ObservableProperty]
    private Point velocity = new Point(1.0, 0.0);

    [ObservableProperty]
    private int energy;

    [ObservableProperty]
    private int healthpoints;

    [ObservableProperty]
    public Bitmap currentImage;

    [ObservableProperty]
    private int organicWasteTime;

    public Carnivore? carnivore;

    public Herbivore? herbivore;

    public bool isDead;
    private bool isOrganicWaste;

    private bool poop = false;

    public double Vision { get; set; } = 100;
    public double Speed { get; set; } = 2.0;

    private const int CoolDownTime = 5000;
    private const int CoolDownPoop = 3000;

    public DateTime LastReproductionTime { get; set; } = DateTime.MinValue;
    public DateTime LastPoopTime { get; set; } = DateTime.MinValue;

    public bool CanReproduce => (DateTime.Now - LastReproductionTime).TotalMilliseconds > CoolDownTime;
    public bool CanPoop => (DateTime.Now - LastPoopTime).TotalMilliseconds > CoolDownPoop;

    protected GameObject(Point location, int energy, int healthpoints, int organicWasteTime, Bitmap currentImage) {
        Location = location;
        this.Energy = energy;
        this.Healthpoints = healthpoints;
        this.OrganicWasteTime = organicWasteTime;
        this.CurrentImage = currentImage;
    }

    public bool CanReproduceIfNotDead() {
        return CanReproduce && !isDead;
    }

    public void SetReproductionCooldown() {
        LastReproductionTime = DateTime.Now;
    }

    public void SetPoopCooldown() {
        LastPoopTime = DateTime.Now;
    }

    public GameObject? Poop() {
        if ((DateTime.Now - LastPoopTime).TotalMilliseconds >= CoolDownPoop && !poop) {
            LastPoopTime = DateTime.Now;
            var organicWaste = new OrganicWaste(new Point(Location.X, Location.Y + 32));
            return organicWaste;
        }
        return null;
    }

    public bool SawOpponent(GameObject other) {
        var distance = Math.Sqrt(
            Math.Pow(Location.X - other.Location.X, 2) +
            Math.Pow(Location.Y - other.Location.Y, 2));
        return distance <= Vision;
    }


    public void DieAnimal() {
        isDead = true;
        poop = true;
        CurrentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Viande.png")));
    }

    public void DiePlant() {
        isDead = true;
        poop = true;
        CurrentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Déchet.png")));
    }

    public void ReduceEnergy() {
        if (!isDead) {
            Energy--;
            if (Energy <= 0) {
                Console.WriteLine("No Energy anymore");
                ReduceHealth();
                Energy = 0;
            }
        }
    }

    public void ReduceHealth() {
        if (!isDead) {
            Healthpoints--;
            if (Healthpoints <= 0) {
                if(this is Carnivore || this is Herbivore) {
                    DieAnimal();
                } 
                else {
                    DiePlant();
                }
            }
        }
    }

    public void OrganicWaste() {
        if (!isOrganicWaste && Healthpoints <= 0) {
            OrganicWasteTime--;
            if (OrganicWasteTime == 0) {
                isOrganicWaste = true;
                CurrentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Déchet.png")));
            }
        }
    }

    public bool IsDechet() {
        return isOrganicWaste;
    }

    public bool Saw_Waste(GameObject other) {
        var distance = Math.Sqrt(
            Math.Pow(Location.X - other.Location.X, 2) +
            Math.Pow(Location.Y - other.Location.Y, 2));
        return distance <= Vision;
    }
}
