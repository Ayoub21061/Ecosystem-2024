using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System.ComponentModel;


namespace Ecosystem_2024.ViewModels;

public partial class MainWindowViewModel : GameBase
{
    public Carnivore carnivore;
    public Herbivore herbivore;
    public int Width {get;} = 800;

    public int Height {get;} = 450;
    List<GameObject> ToRemove = new List<GameObject>();

    
    // Liste des objets à afficher
    public ObservableCollection<GameObject> GameObjects { get; } = new();
    public MainWindowViewModel() {
        carnivore = new Carnivore(new Point(Width/2, Height/2));
        GameObjects.Add(carnivore);

        herbivore = new Herbivore(new Point(Width/2+32, Height/2+32));
        GameObjects.Add(herbivore);
    }

    protected override void Tick() {
        foreach(var obj in GameObjects) {
            if (obj is Carnivore carnivore) {
                carnivore.ReduceEnergy();
                carnivore.OrganicWaste();

            GameObject? closestHerbivore = null;
            double closestDistance = double.MaxValue;

            // Chercher un herbivore dans le champ de vision
            foreach (var other in GameObjects)
            {
                if (other is Herbivore potentialHerbivore && carnivore.SawOpponent(potentialHerbivore))
                {
                    var distance = Math.Sqrt(
                        Math.Pow(potentialHerbivore.Location.X - carnivore.Location.X, 2) +
                        Math.Pow(potentialHerbivore.Location.Y - carnivore.Location.Y, 2)
                    );

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestHerbivore = potentialHerbivore;
                    }
                }            
            }

            if (closestHerbivore != null)
            {
                // Calcul de la direction vers la cible
                var direction = new Point(
                    closestHerbivore.Location.X - carnivore.Location.X,
                    closestHerbivore.Location.Y - carnivore.Location.Y
                );

                // Calcul de la magnitude (distance)
                var magnitude = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

                if (magnitude > 0)
                {
                    // On crée un vecteur normalisé qui permettra d'établir le vecteur le plus petit possible pour parvenir le plus rapidement possible à la position de la proie. Egalement le vecteur le plus facile à initialiser pour le déplacement en 2D des être vivants. On met également à jour la vitesse du prédateur qui dépend de la position de la proie.
                    direction = new Point(direction.X / magnitude, direction.Y / magnitude);
                    carnivore.Velocity = new Point(direction.X * carnivore.Speed, direction.Y * carnivore.Speed);
                }

                // Cela permet de gérer les collisions de chaque carnivore selon la distance qu'ils ont avec leur cible herbivore
                if(magnitude < 5)
                {
                    ToRemove.Add(closestHerbivore);
                    Console.WriteLine("Je t'ai mangé");
                    carnivore.Energy += 20;
                }
            }  
                carnivore.Move();   
            }
            
            if (obj is Herbivore herbivore) {
                herbivore.Move();
            }
        }
            foreach(GameObject obj in ToRemove) {
                GameObjects.Remove(obj);
            }

        

        
    }
}
