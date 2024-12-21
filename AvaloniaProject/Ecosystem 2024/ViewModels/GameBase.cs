using System;
using Avalonia.Threading;

namespace Ecosystem_2024.ViewModels;

// Impose d'implémenter la méthode Tick() et s'occupe de l'appeler 60 fois par seconde.
// Cela sert de "boucle" principale pour notre application graphique.
public abstract class GameBase: ViewModelBase
{
    public const int TicksPerSecond = 10;
    private readonly DispatcherTimer _timer = new() { Interval = new TimeSpan(0, 0, 0, 0, 1000 / TicksPerSecond) };

    protected GameBase()
    {
        _timer.Tick += delegate { DoTick(); };
    }

    public long CurrentTick { get; private set; }


    private void DoTick()
    {
        Tick();
        CurrentTick++;
    }

    protected abstract void Tick();

    // Appelé dans App.axaml.cs
    public void Start()
    {
        _timer.IsEnabled = true;
    }

    public void Stop()
    {
        _timer.IsEnabled = false;
    }
}