using System;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class Herbivore : GameObject {
    [ObservableProperty]
    private Point velocity = new Point(1.0 , 0.0);

    [ObservableProperty]
    private int energy;

    [ObservableProperty]
    private int healthpoints;

    [ObservableProperty]
    public Bitmap currentImage = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Herbivore.png")));

    public Herbivore(Point location, int energy, int healthpoints) : base(location) {
        this.Energy = energy;
        this.Healthpoints = healthpoints;
    }

   public void Move()
    {
        Location = Location + Velocity; // Appelle la logique générale de déplacement
    }
}