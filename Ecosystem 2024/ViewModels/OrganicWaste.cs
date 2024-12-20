using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class OrganicWaste : GameObject
{
  
    [ObservableProperty]
    private Bitmap currentImagePoop = new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Déchet.png")));
    public OrganicWaste(Point location) : base(location)
    {
        Location = location;
    }

}
