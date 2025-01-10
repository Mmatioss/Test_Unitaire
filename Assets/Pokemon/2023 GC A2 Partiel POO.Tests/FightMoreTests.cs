using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using System;
namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer des features et les TU sur le reste du projet
        
        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
            // - un heal ne régénère pas plus que les HP Max
            // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
            // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type
        // - L'utilisation d'objets : Potion, SuperPotion, Vitess+, Attack+ etc.
        // - Gérer les PP (limite du nombre d'utilisation) d'une attaque,
            // si on selectionne une attack qui a 0 PP on inflige 0
        
        // Choisis ce que tu veux ajouter comme feature et fait en au max.
        // Les nouveaux TU doivent être dans ce fichier.
        // Modifiant des features il est possible que certaines valeurs
            // des TU précédentes ne matchent plus, tu as le droit de réadapter les valeurs
            // de ces anciens TU pour ta nouvelle situation.
        
        [Test]
        public void TestHeal()
        {
            Character pikachu = new Character(200, 20, 30, 20, TYPE.NORMAL);
            Character salameche = new Character(200, 20, 20, 10, TYPE.NORMAL);
            Fight f = new Fight(pikachu, salameche);
            Punch p = new Punch();
            Heal h = new Heal();

            f.ExecuteTurn(p, h); // salameche prend - 70 et regagne 50 = 180, pikachu reste à 200

            Assert.That(pikachu.CurrentHealth, Is.EqualTo(200));
            Assert.That(salameche.CurrentHealth, Is.EqualTo(180)); 
        }

        [Test]
        public void TestNoOverHeal()
        {
            Character pikachu = new Character(200, 20, 30, 20, TYPE.NORMAL);
            Character salameche = new Character(200, 20, 20, 10, TYPE.NORMAL);
            Fight f = new Fight(pikachu, salameche);
            Heal h = new Heal();

            f.ExecuteTurn(h, h); // 200 + 50 = 250, 200 + 50 = 250 mais max = 200

            Assert.That(pikachu.CurrentHealth, Is.EqualTo(200));
            Assert.That(salameche.CurrentHealth, Is.EqualTo(200)); 
        }

        [Test]
        public void TestBurn()
        {
            Character pikachu = new Character(200, 20, 30, 20, TYPE.NORMAL);
            Character salameche = new Character(200, 20, 20, 30, TYPE.NORMAL);
            Fight f = new Fight(salameche, pikachu);
            Punch p = new Punch();
            FireBall fi = new FireBall();

            f.ExecuteTurn(fi, p); // pikachu prend - 30 et brule - 10 = 160, salameche prend - 70 = 130

            Assert.That(salameche.CurrentHealth, Is.EqualTo(130)); 
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(160));
        }

        [Test]
        public void TestSleep()
        {
            Character pikachu = new Character(200, 20, 30, 20, TYPE.NORMAL);
            Character salameche = new Character(200, 20, 20, 30, TYPE.NORMAL);
            Fight f = new Fight(salameche, pikachu);
            Punch p = new Punch();
            MagicalGrass m = new MagicalGrass();

            f.ExecuteTurn(m, p); // salameche prend 0 car pikachu dort

            Assert.That(salameche.CurrentHealth, Is.EqualTo(200)); 
        }
    }
}
