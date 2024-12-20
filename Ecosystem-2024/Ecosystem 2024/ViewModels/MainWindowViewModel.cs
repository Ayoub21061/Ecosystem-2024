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
    public Carnivore? carnivore;
    public Herbivore? herbivore;

    public Plante? plante;
    public int Width {get;} = 800;

    public int Height {get;} = 450;
    List<GameObject> ToRemove = new List<GameObject>();
    List<GameObject> ToAdd = new List<GameObject>(); 

    
    // Liste des objets à afficher
    public ObservableCollection<GameObject> GameObjects { get; } = new();
    public MainWindowViewModel() {
        var carnivores = new List<Carnivore>() {
            new Male(new Point(Width/2, Height/2)),
            new Male(new Point(Width / 2 + 50, Height / 2 + 50)),
            new Male(new Point(Width / 2 + 100, Height / 2 + 100)),
            new Femelle(new Point(Width/2 + 20, Height/2 + 20)),
            new Femelle(new Point(Width / 2 + 70, Height / 2 + 70)),
            new Femelle(new Point(Width / 2 + 120, Height / 2 + 120))

        };

        foreach(var carnivore in carnivores) {
            GameObjects.Add(carnivore);
        }
        
        var herbivores = new List<Herbivore>() {
            new Herbivore(new Point(Width / 2 + 10, Height / 2 + 10)),
            new Herbivore(new Point(Width / 2 + 60, Height / 2 + 30)),
            new Herbivore(new Point(Width / 2 + 80, Height / 2 + 80))
        };

        foreach(var herbivore in herbivores) {
            GameObjects.Add(herbivore);
        }

        var Plantes = new List<Plante>() {
            new Plante(new Point((Width + 1) *2 / 3, Height/3)),
            new Plante(new Point((Width + 50) *2 / 3, Height/3)),
            new Plante(new Point((Width + 150) *2 / 3, Height/3)),
            new Plante(new Point((Width + 250) *2 / 3, Height/3))
        };

        foreach(var plante in Plantes) {
            GameObjects.Add(plante);
        }
    }

    protected override void Tick() {
        foreach(var obj in GameObjects) {
            if (obj is Carnivore carnivore) {
                carnivore.ReduceEnergy();
                carnivore.OrganicWaste();

            // Permet de gérer la défecation des carnivores
            var poop = carnivore.Poop();
            if (poop != null) {
                ToAdd.Add(poop);
            }



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

                    if(carnivore.Location.X > Width-64) {
                    carnivore.Velocity = new Point(-carnivore.Velocity.X, carnivore.Velocity.Y);
                    }

                    if(carnivore.Location.X < 0) {
                    carnivore.Velocity = new Point(-carnivore.Velocity.X, carnivore.Velocity.Y);
                    }   

                    if(carnivore.Location.Y > Height) {
                        carnivore.Velocity = new Point(carnivore.Velocity.X, -carnivore.Velocity.Y);
                    }

                    if(carnivore.Location.Y < 32) {
                        carnivore.Velocity = new Point(carnivore.Velocity.X, -carnivore.Velocity.Y);
                    }
            }

            if (obj is Herbivore herbivore) {
                herbivore.Move();

                if(herbivore.Location.X > Width-64) {
                    herbivore.Velocity = new Point(-herbivore.Velocity.X, herbivore.Velocity.Y);
                }

                if(herbivore.Location.X < 0) {
                    herbivore.Velocity = new Point(-herbivore.Velocity.X, herbivore.Velocity.Y);
                }

                if(herbivore.Location.Y > Height-64) {
                    herbivore.Velocity = new Point(herbivore.Velocity.X, -herbivore.Velocity.Y);
                }

                if(herbivore.Location.Y < 0) {
                    herbivore.Velocity = new Point(herbivore.Velocity.X, -herbivore.Velocity.Y);
                }
            }

            if (obj is Plante plante){
                plante.ReduceEnergy();

                GameObject? closestOrganicWaste = null;
                double closestDistance = double.MaxValue;

                // Identifier les carnivores (Cranivore carn) quand ils deviennent des déchets organiques (carn.IsDechet) et les consommer si ils sont à proximité (plante.Saw_Waste(other)).
                foreach (var other in GameObjects) {
                    if (other is Carnivore carn && carn.IsDechet() && plante.Saw_Waste(other) || other is OrganicWaste waste && plante.Saw_Waste(other)) 
                    {
                        // On calcule la distance entre la plante et le déchet organique
                        var distance = Math.Sqrt(
                            Math.Pow(plante.Location.X - other.Location.X, 2) +
                            Math.Pow(plante.Location.Y - other.Location.Y, 2)
                        );
                        // Si la distance est plus petite que la distance la plus proche, alors on met à jour la distance la plus proche et le déchet organique le plus proche.
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestOrganicWaste = other;
                        }
                    }
                }

                // Si un déchet organique est détecté, on identifie la plante la plus proche qui consomme le déchet organique.
                if (closestOrganicWaste != null)
                {
                    Plante? closestPlant = null;
                    double minPlantDistance = double.MaxValue;

                    foreach (var otherPlant in GameObjects.OfType<Plante>()) {
                        // On établit la distance entre la plante et le déchet organique
                        var plantDistance = Math.Sqrt(
                            Math.Pow(otherPlant.Location.X - closestOrganicWaste.Location.X, 2) +
                            Math.Pow(otherPlant.Location.Y - closestOrganicWaste.Location.Y, 2)
                        );

                        if (plantDistance < minPlantDistance) {
                                minPlantDistance = plantDistance;
                                closestPlant = otherPlant;
                        }
                    }

                    // Seule la plante la plus proche consomme le déchet
                    if (closestPlant == plante && minPlantDistance <= plante.Rayon) {
                            ToRemove.Add(closestOrganicWaste);
                            plante.Energy += 30;
                    }
                }
            }   
        }

        foreach(GameObject obj in ToRemove) {
            GameObjects.Remove(obj);
        }

        // Implémentation de la reproduction entre carnviores
        foreach(var femelle in GameObjects.OfType<Femelle>()) {
            foreach(var male in GameObjects.OfType<Male>()) {

                // if (femelle != male) {

                    if (Math.Abs(femelle.Location.X - male.Location.X) < 5 && 
                        Math.Abs(femelle.Location.Y - male.Location.Y) < 5 && 
                        femelle.CanReproduce && male.CanReproduce)
                    {
                        Console.WriteLine("Reproduction !");
                        var BabyPosition = new Point((femelle.Location.X + male.Location.X) / 2, (male.Location.Y + femelle.Location.Y) / 2 );

                        var BabyCarnivore = new Carnivore(BabyPosition);
                        ToAdd.Add(BabyCarnivore);

                        // Met à jour le temps de la reproduction de chaque animal ayant eu recours à celle-ci.
                        femelle.SetReproductionCooldown();
                        male.SetReproductionCooldown();

                        // // Permet de ne pas créer une infinité de bébé
                        // break;
                    }
                // } 
            }
        }

        // On rajoute chaque objet de la liste ToAdd à la liste des objets affiché dans l'application qui est la liste GameObjects.
        foreach(GameObject obj in ToAdd) {
            GameObjects.Add(obj);
        }

        // On nettoie la liste après avoir ajouté des objets 
        ToAdd.Clear();
    }
}
