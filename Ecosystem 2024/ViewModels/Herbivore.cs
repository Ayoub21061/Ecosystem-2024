using System.Buffers.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class Herbivore : GameObject {
    [ObservableProperty]
    private Point velocity = new Point(1.0 , 0.0);

    [ObservableProperty]
    private int energy;

    [ObservableProperty]
    private int healthpoints;

    public Herbivore(Point location) : base(location) {
        Energy = 150;
        Healthpoints = 100;
    }

   public void Move()
    {
        Location = Location + Velocity; // Appelle la logique générale de déplacement
    }
}