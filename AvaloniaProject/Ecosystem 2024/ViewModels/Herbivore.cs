using System;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class Herbivore : GameObject {

    public Herbivore(Point location, int energy, int healthpoints, int OrganicWasteTime) : base(location, 100, 100, 50, new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Herbivore.png")))) {
        this.Energy = energy;
        this.Healthpoints = healthpoints;
        this.OrganicWasteTime = OrganicWasteTime;
        this.CurrentImage = currentImage;
    }

    public void Move() {
        if (!isDead) {
            Location = Location + Velocity;
        }
    }
    
}