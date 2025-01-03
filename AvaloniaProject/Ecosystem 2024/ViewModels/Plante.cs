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
    private bool AlreadyCreated = false;
    public double Rayon {get; set;} = 100;

    private const int CooldownPlant = 5000;
    public DateTime LastTimeCreatedPlant { get; set; } = DateTime.MinValue;

    public bool CanCreatePlant => (DateTime.Now - LastTimeCreatedPlant).TotalMilliseconds > CooldownPlant;

    
    // Constructeur 
    public Plante (Point location) : base(location, 200, 200, 50, new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Plante.png")))) {
        this.Energy = 200;
        this.Healthpoints = 200;
        this.CurrentImage = currentImage;
    }

       public void SetCreatePlantCooldown() {
        LastTimeCreatedPlant = DateTime.Now;
    }

    // Méthode pour créer une plante à un endroit aléatoire dans un rayon donné
    public Plante? CreatePlant() {

        if(AlreadyCreated) {
            return null;
        }

        var random = new Random();
        double angle = random.NextDouble() * 2 * Math.PI;  // Angle aléatoire
        double distance = random.NextDouble() * Rayon;  // Distance aléatoire dans le rayon de la plante
        double newX = Location.X + distance * Math.Cos(angle);
        double newY = Location.Y + distance * Math.Sin(angle);

        AlreadyCreated = true;

        // Crée une nouvelle plante à la position calculée
        return new Plante(new Point(newX, newY));
    }
}


