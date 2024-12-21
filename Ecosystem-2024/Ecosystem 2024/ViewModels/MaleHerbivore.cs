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

public partial class MaleHerbivore : Herbivore {
    public MaleHerbivore(Point location) : base(location, 100, 100) {
    
    }
    
}