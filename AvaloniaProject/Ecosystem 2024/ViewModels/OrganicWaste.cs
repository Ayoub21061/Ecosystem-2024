using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class OrganicWaste : GameObject
{
    public OrganicWaste(Point location) : base(location, 100, 100, 50, new Bitmap(AssetLoader.Open(new Uri("avares://Ecosystem 2024/Assets/Déchet.png"))))
    {
        Location = location;
    }

}
