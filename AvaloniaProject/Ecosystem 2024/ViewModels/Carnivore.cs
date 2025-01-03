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

    public Carnivore(Point location, int energy, int healthpoints, int organicWasteTime) : base(location, 100, 100, 50, new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Carnivore.png")))) {
        this.Energy = energy;
        this.Healthpoints = healthpoints;
        this.OrganicWasteTime = organicWasteTime;
        this.CurrentImage = currentImage;
    }

    public void Move() {
        if (!isDead) {
            Location = Location + Velocity;
        }
    }


}