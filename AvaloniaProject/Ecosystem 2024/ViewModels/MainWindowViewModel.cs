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
    public int Width { get; } = 800;
    public int Height { get; } = 450;
    List<GameObject> ToRemove = new List<GameObject>();
    List<GameObject> ToAdd = new List<GameObject>();


    // Liste des objets à afficher
    public ObservableCollection<GameObject> GameObjects { get; } = new();
    public MainWindowViewModel()
    {
        var carnivores = new List<Carnivore>() {
            new MaleCarnivore(new Point(Width / 2, Height / 2)),
            new MaleCarnivore(new Point(Width / 2 + 50, Height / 2 + 50)),
            new MaleCarnivore(new Point(Width / 2 + 100, Height / 2 + 100)),
            new FemelleCarnivore(new Point(Width / 2 + 20, Height / 2 + 20)),
            new FemelleCarnivore(new Point(Width / 2 + 70, Height / 2 + 70)),
            new FemelleCarnivore(new Point(Width / 2 + 120, Height / 2 + 120))
        };

        foreach (var carnivore in carnivores)
        {
            GameObjects.Add(carnivore);
        }

        var herbivores = new List<Herbivore>() {
            new MaleHerbivore(new Point(Width / 2 + 10, Height / 2 + 10)),
            new MaleHerbivore(new Point(Width / 2 + 60, Height / 2 + 30)),
            new MaleHerbivore(new Point(Width / 2 + 80, Height / 2 + 80)),
            new MaleHerbivore(new Point(Width / 2 + 200, Height / 2 + 200)),
            new FemelleHerbivore(new Point(Width / 2 + 30, Height / 2 + 30)),
            new FemelleHerbivore(new Point(Width / 2 + 80, Height / 2 + 50)),
            new FemelleHerbivore(new Point(Width / 2 + 100, Height / 2 + 100)),
        };

        foreach (var herbivore in herbivores)
        {
            GameObjects.Add(herbivore);
        }

        var Plantes = new List<Plante>() {
            new Plante(new Point((Width + 1) * 2 / 3, Height / 3)),
            new Plante(new Point((Width + 50) * 2 / 3, Height / 3)),
            new Plante(new Point((Width + 150) * 2 / 3, Height / 3)),
            new Plante(new Point((Width + 250) * 2 / 3, Height / 3))
        };

        foreach (var plante in Plantes)
        {
            GameObjects.Add(plante);
        }
    }

    protected override void Tick()
    {
        foreach (var obj in GameObjects)
        {
            if (obj is Carnivore carnivore)
            {
                carnivore.ReduceEnergy();
                carnivore.OrganicWaste();

                var poop = carnivore.Poop();
                if (poop != null)
                {
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

                    // Cela permet de gérer les collisions de chaque carnivore selon la distance qu'ils ont avec leurs cibles herbivores
                    if (magnitude < 5)
                    {
                        ToRemove.Add(closestHerbivore);
                        Console.WriteLine("Je t'ai mangé");
                        carnivore.Energy += 20;
                    }
                }
                carnivore.Move();

                if (carnivore.Location.X > Width - 64)
                {
                    carnivore.Velocity = new Point(-carnivore.Velocity.X, carnivore.Velocity.Y);
                }

                if (carnivore.Location.X < 0)
                {
                    carnivore.Velocity = new Point(-carnivore.Velocity.X, carnivore.Velocity.Y);
                }

                if (carnivore.Location.Y > Height)
                {
                    carnivore.Velocity = new Point(carnivore.Velocity.X, -carnivore.Velocity.Y);
                }

                if (carnivore.Location.Y < 32)
                {
                    carnivore.Velocity = new Point(carnivore.Velocity.X, -carnivore.Velocity.Y);
                }
            }

            if (obj is Herbivore herbivore)
            {
                herbivore.ReduceEnergy();
                herbivore.OrganicWaste();

                var poop = herbivore.Poop();
                if (poop != null)
                {
                    ToAdd.Add(poop);
                }

                GameObject? closestPlant = null;
                double closestDistance = double.MaxValue;

                // Chercher une plante dans le champ de vision
                foreach (var other in GameObjects)
                {
                    if (other is Plante potentialPlant && herbivore.SawOpponent(potentialPlant))
                    {
                        var distance = Math.Sqrt(
                        Math.Pow(potentialPlant.Location.X - herbivore.Location.X, 2) +
                        Math.Pow(potentialPlant.Location.Y - herbivore.Location.Y, 2)
                    );

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestPlant = potentialPlant;
                        }
                    }
                }

                if (closestPlant != null)
                {
                    // Calcul de la direction vers la cible
                    var direction = new Point(
                        closestPlant.Location.X - herbivore.Location.X,
                        closestPlant.Location.Y - herbivore.Location.Y
                    );

                    // Calcul de la magnitude (distance)
                    var magnitude = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

                    if (magnitude > 0)
                    {
                        // Normaliser le vecteur et ajuster la vitesse
                        direction = new Point(direction.X / magnitude, direction.Y / magnitude);
                        herbivore.Velocity = new Point(direction.X * herbivore.Speed, direction.Y * herbivore.Speed);
                    }

                    // Gérer la collision avec la plante
                    if (magnitude < 5)
                    {
                        ToRemove.Add(closestPlant);
                        Console.WriteLine("Plante consommée !");
                        herbivore.Energy += 30; // Augmente l'énergie de l'herbivore
                    }
                }

                herbivore.Move();

                // Gestion des rebonds
                if (herbivore.Location.X > Width - 64)
                {
                    herbivore.Velocity = new Point(-herbivore.Velocity.X, herbivore.Velocity.Y);
                }
                if (herbivore.Location.X < 0)
                {
                    herbivore.Velocity = new Point(-herbivore.Velocity.X, herbivore.Velocity.Y);
                }
                if (herbivore.Location.Y > Height - 64)
                {
                    herbivore.Velocity = new Point(herbivore.Velocity.X, -herbivore.Velocity.Y);
                }
                if (herbivore.Location.Y < 0)
                {
                    herbivore.Velocity = new Point(herbivore.Velocity.X, -herbivore.Velocity.Y);
                }
            }
            if (obj is Plante plante)
            {
                plante.ReduceEnergy();
                plante.OrganicWaste();

                GameObject? closestOrganicWaste = null;
                double closestDistance = double.MaxValue;

                // Identifier les carnivores (Cranivore carn) quand ils deviennent des déchets organiques (carn.IsDechet) et les consommer si ils sont à proximité (plante.Saw_Waste(other)).
                foreach (var other in GameObjects)
                {
                    if (other is Carnivore carn && carn.IsDechet() && plante.Saw_Waste(other) || other is OrganicWaste waste && plante.Saw_Waste(other) || other is Herbivore herb && herb.IsDechet() && plante.Saw_Waste(other) || other is Plante plant && plant.IsDechet() && plante.Saw_Waste(other))
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

                    foreach (var otherPlant in GameObjects.OfType<Plante>())
                    {
                        // On établit la distance entre la plante et le déchet organique
                        var plantDistance = Math.Sqrt(
                            Math.Pow(otherPlant.Location.X - closestOrganicWaste.Location.X, 2) +
                            Math.Pow(otherPlant.Location.Y - closestOrganicWaste.Location.Y, 2)
                        );

                        if (plantDistance < minPlantDistance)
                        {
                            minPlantDistance = plantDistance;
                            closestPlant = otherPlant;
                        }
                    }

                    // Seule la plante la plus proche consomme le déchet
                    if (closestPlant == plante && minPlantDistance <= plante.Rayon)
                    {
                        ToRemove.Add(closestOrganicWaste);
                        plante.Energy += 30;
                    }

                    if(closestOrganicWaste is Plante) {
                        plante.Energy += 30;
                    }

                    // Créer une nouvelle plante uniquement si elle peut encore créer
                    if (plante.CanCreatePlant)
                    {
                        var newPlante = plante.CreatePlant();
                        if (newPlante != null)
                        {
                            ToAdd.Add(newPlante);  // Ajouter la nouvelle plante à la liste à ajouter
                            Console.WriteLine("Une nouvelle plante a été créée à la position : " + newPlante.Location);
                            newPlante.SetCreatePlantCooldown();  // Mettre la plante en cooldown après sa création
                        }
                    }
                }
            }
        }

        // Implémentation de la reproduction entre carnviores
        foreach (var femelleCarn in GameObjects.OfType<FemelleCarnivore>())
        {
            foreach (var maleCarn in GameObjects.OfType<MaleCarnivore>())
            {
                if (Math.Abs(femelleCarn.Location.X - maleCarn.Location.X) < 5 &&
                    Math.Abs(femelleCarn.Location.Y - maleCarn.Location.Y) < 5 &&
                    femelleCarn.CanReproduceIfNotDead() && maleCarn.CanReproduceIfNotDead())
                {
                    Console.WriteLine("Reproduction !");
                    var BabyPosition = new Point((femelleCarn.Location.X + maleCarn.Location.X) / 2, (maleCarn.Location.Y + femelleCarn.Location.Y) / 2);

                    var random = new Random();
                    var isMale = random.Next(2);

                    if (isMale == 0)
                    {
                        var BabyCarnivore = new MaleCarnivore(BabyPosition);
                        ToAdd.Add(BabyCarnivore);
                    }
                    else
                    {
                        var BabyCarnivore = new FemelleCarnivore(BabyPosition);
                        ToAdd.Add(BabyCarnivore);
                    }

                    // Met à jour le temps de la reproduction de chaque animal ayant eu recours à celle-ci.
                    femelleCarn.SetReproductionCooldown();
                    maleCarn.SetReproductionCooldown();
                }
            }
        }

        // Implémentation de la reproduction entre carnviores
        foreach (var femelleHerb in GameObjects.OfType<FemelleHerbivore>())
        {
            foreach (var maleHerb in GameObjects.OfType<MaleHerbivore>())
            {
                if (Math.Abs(femelleHerb.Location.X - maleHerb.Location.X) < 5 &&
                    Math.Abs(femelleHerb.Location.Y - maleHerb.Location.Y) < 5 &&
                    femelleHerb.CanReproduceIfNotDead() && maleHerb.CanReproduceIfNotDead())
                {
                    Console.WriteLine("Reproduction !");
                    var BabyPosition = new Point((femelleHerb.Location.X + maleHerb.Location.X) / 2, (maleHerb.Location.Y + femelleHerb.Location.Y) / 2);

                    var random = new Random();
                    var isMale = random.Next(2);

                    if (isMale == 0)
                    {
                        var BabyHerbivore = new MaleHerbivore(BabyPosition);
                        ToAdd.Add(BabyHerbivore);
                    }
                    else
                    {
                        var BabyHerbivore = new FemelleHerbivore(BabyPosition);
                        ToAdd.Add(BabyHerbivore);
                    }

                    // Met à jour le temps de la reproduction de chaque animal ayant eu recours à celle-ci.
                    femelleHerb.SetReproductionCooldown();
                    maleHerb.SetReproductionCooldown();
                }
            }
        }

        // Ici on doit implémenter le même système de reproduction pour les herbivores que pour les carnivores

        foreach (GameObject obj in ToRemove)
        {
            GameObjects.Remove(obj);
        }

        // On rajoute chaque objet de la liste ToAdd à la liste des objets affiché dans l'application qui est la liste GameObjects.
        foreach (GameObject obj in ToAdd)
        {
            GameObjects.Add(obj);
        }

        // On nettoie la liste après avoir ajouté des objets 
        ToAdd.Clear();
    }
}
