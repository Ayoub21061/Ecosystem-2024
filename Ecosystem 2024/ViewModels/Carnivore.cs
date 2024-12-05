using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ecosystem_2024.ViewModels;

public partial class Animal : GameObject {

    [ObservableProperty]

    private Point velocity = new Point(1.0 , 0.0);    
    
    public Animal(Point location) : base(location) {
        
    }

    public void Move() {

        Location = Location + Velocity;
    }
}