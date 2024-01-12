using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter
        
        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
            // - un heal ne régénère pas plus que les HP Max
            // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
            // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type

    

        // ----- Test Types ----- //
        [Test]
        public void FireVSGrass()
        {
            Character salameche = new Character(150, 45, 15, 200, TYPE.FIRE);
            Character bulbizarre = new Character(90, 60, 10, 100, TYPE.GRASS);
            Skill fireBall = new FireBall();
            Skill magicalGrass = new MagicalGrass();

            salameche.ReceiveAttackWithFactorType(magicalGrass);
            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, salameche.BaseType), Is.EqualTo(0.8f)); // Resist
            Assert.That(salameche.IsAlive, Is.EqualTo(true));
            Assert.That(salameche.CurrentHealth, Is.EqualTo(109)); // health salameche - ((magicalGrass power * factor(water, fire) - defense salameche) => 150 - ((70 * 0.8) - 15) = 109

            bulbizarre.ReceiveAttackWithFactorType(fireBall);
            Assert.That(TypeResolver.GetFactor(fireBall.Type, bulbizarre.BaseType), Is.EqualTo(1.2f)); // Resist
            Assert.That(bulbizarre.IsAlive, Is.EqualTo(true));
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(40)); // health bulbizarre - ((fireball power * factor(fire, water) - defense bulbizarre) => 90 - ((50 * 1.2) - 10) = 40
        }

        [Test]
        public void GrassVSWater()
        {
            Character boustiflor = new Character(120, 50, 10, 45, TYPE.GRASS);
            Character tiplouf = new Character(150, 70, 15, 80, TYPE.WATER);
            Skill magicalGrass = new MagicalGrass();
            Skill waterBlouBlou = new WaterBlouBlou();

            boustiflor.ReceiveAttackWithFactorType(waterBlouBlou);
            Assert.That(TypeResolver.GetFactor(waterBlouBlou.Type, boustiflor.BaseType), Is.EqualTo(0.8f)); // Resist
            Assert.That(boustiflor.IsAlive, Is.EqualTo(true));
            Assert.That(boustiflor.CurrentHealth, Is.EqualTo(114)); // health boustiflor - ((waterbloublou power * factor(water, grass) - defense boustiflor) => 120 - ((20 * 0.8) - 10) = 114

            tiplouf.ReceiveAttackWithFactorType(magicalGrass);
            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, tiplouf.BaseType), Is.EqualTo(1.2f)); // Vulnerable
            Assert.That(tiplouf.IsAlive, Is.EqualTo(true));
            Assert.That(tiplouf.CurrentHealth, Is.EqualTo(81)); // health tiplouf - ((magicalGrass power * factor(grass, water) - defense tiplouf) => 150 - ((70 * 1.2) - 15) = 81
        }

        [Test]
        public void WaterVSFire()
        {
            Character ouisticram = new Character(100, 30, 8, 80, TYPE.FIRE);
            Character tortank = new Character(300, 80, 20, 60, TYPE.WATER);
            Skill fireBall = new FireBall();
            Skill waterBlouBlou = new WaterBlouBlou();

            ouisticram.ReceiveAttackWithFactorType(waterBlouBlou);
            Assert.That(TypeResolver.GetFactor(waterBlouBlou.Type, ouisticram.BaseType), Is.EqualTo(1.2f)); // Vulnerable
            Assert.That(ouisticram.IsAlive, Is.EqualTo(true));
            Assert.That(ouisticram.CurrentHealth, Is.EqualTo(84)); // health ouisticram - ((waterbloublou power * factor(water, fire) - defense ouisticram) => 100 - ((20 * 1.2) - 8) = 84

            tortank.ReceiveAttackWithFactorType(fireBall);
            Assert.That(TypeResolver.GetFactor(fireBall.Type, tortank.BaseType), Is.EqualTo(0.8f)); // Resist
            Assert.That(tortank.IsAlive, Is.EqualTo(true));
            Assert.That(tortank.CurrentHealth, Is.EqualTo(280)); // health tiplouf - ((magicalGrass power * factor(grass, water) - defense tiplouf) => 300 - ((50 * 0.8) - 20) = 280
        }

        [Test]
        public void NormalVSNormal()
        {
            Character grodoudou = new Character(160, 20, 5, 50, TYPE.NORMAL);

            Skill punch = new Punch();

            grodoudou.ReceiveAttackWithFactorType(punch);
            Assert.That(TypeResolver.GetFactor(punch.Type, grodoudou.BaseType), Is.EqualTo(1f)); // Normal & Same
            Assert.That(grodoudou.IsAlive, Is.EqualTo(true));
            Assert.That(grodoudou.CurrentHealth, Is.EqualTo(95)); // health grodoudou - ((punch power * factor(normal, normal) - defense grodoudou) => 60 - ((70 * 1) - 5) = 95
        }

        [Test]
        public void WaterVSWater()
        {
            Character lamantine = new Character(100, 30, 10, 50, TYPE.WATER);

            Skill waterbloublou = new WaterBlouBlou();

            lamantine.ReceiveAttackWithFactorType(waterbloublou);
            Assert.That(TypeResolver.GetFactor(waterbloublou.Type, lamantine.BaseType), Is.EqualTo(1f)); // Same
            Assert.That(lamantine.IsAlive, Is.EqualTo(true));
            Assert.That(lamantine.CurrentHealth, Is.EqualTo(90)); // health lamantine - ((waterBlouBlou power * factor(water, water) - defense lamantine) => 100 - ((20 * 1) - 10) = 90
        }

        
        [Test]
        public void NormalKillsFire()
        {
            Character pyroli = new Character(150, 40, 20, 45, TYPE.FIRE);
            Skill megaPunch = new MegaPunch();

            pyroli.ReceiveAttackWithFactorType(megaPunch);
            Assert.That(TypeResolver.GetFactor(megaPunch.Type, pyroli.BaseType), Is.EqualTo(1f)); // Normal
            Assert.That(pyroli.IsAlive, Is.EqualTo(false));
            Assert.That(pyroli.CurrentHealth, Is.EqualTo(0)); // health pyroli - ((megaPunch power * factor(normal, fire) - defense pyroli) => 150 - ((7000 * 1) - 20) = beaucoup moins que 0, rip
        }

        [Test]
        public void NullAttackOnGrass()
        {
            Character tournegrin = new Character(150, 40, 20, 45, TYPE.GRASS);

            Assert.Throws<ArgumentNullException>(() =>
            {
                tournegrin.ReceiveAttackWithFactorType(null);
            });
        }

        [Test]
        public void GrassVSDeadCharacter()
        {
            Character tournegrin = new Character(150, 40, 20, 45, TYPE.GRASS);
            Skill magicalGrass = new MagicalGrass();
            Skill megaPunch = new MegaPunch();

            tournegrin.ReceiveAttackWithFactorType(megaPunch);
            Assert.That(tournegrin.IsAlive, Is.EqualTo(false));
            Assert.That(tournegrin.CurrentHealth, Is.EqualTo(0)); // health tournegrin - ((megaPunch power * factor(normal, grass) - defense tournegrin) => 150 - ((7000 * 1) - 20) = beaucoup moins que 0, rip

            // Attaque sur le joueur deja mort, il devrait pas être en négatif
            tournegrin.ReceiveAttackWithFactorType(magicalGrass);
            Assert.That(tournegrin.IsAlive, Is.EqualTo(false));
            Assert.That(tournegrin.CurrentHealth, Is.EqualTo(0));
        }

        [Test]
        public void GrassKillsWater()
        {
            Character germignon = new Character(150, 20, 5, 80, TYPE.GRASS);
            Character ludicolo = new Character(40, 30, 5, 40, TYPE.WATER);

            Skill magicalGrass = new MagicalGrass();

            ludicolo.ReceiveAttackWithFactorType(magicalGrass);
            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, ludicolo.BaseType), Is.EqualTo(1.2f)); // Vulnerable
            Assert.That(ludicolo.IsAlive, Is.EqualTo(false));
            Assert.That(ludicolo.CurrentHealth, Is.EqualTo(0)); // health ludicolo - ((magicalGrass power * factor(grass, water) - defense ludicolo) => 40 - ((70 * 1.2) - 5) = -39 donc rip
        }

        [Test]
        public void CharacterDontTakeDamage()
        {
            Character dracaufeu = new Character(400, 50, 90, 120, TYPE.FIRE);
            Character chetiflor = new Character(40, 10, 5, 25, TYPE.GRASS);
            Skill magicalGrass = new MagicalGrass();

            dracaufeu.ReceiveAttackWithFactorType(magicalGrass);
            Assert.That(TypeResolver.GetFactor(magicalGrass.Type, dracaufeu.BaseType), Is.EqualTo(0.8f)); // Resist
            Assert.That(dracaufeu.IsAlive, Is.EqualTo(true));
            Assert.That(dracaufeu.CurrentHealth, Is.EqualTo(400)); // health dracaufeu - ((magicalGrass power * factor(grass, fire) - defense dracaufeu) => 400 - ((70 * 0.8) - 90) , 90 > (70 * 0.8) donc = 400 car on ne va pas faire 400 - -34, sa défense est trop élevée pour qu'il prenne des dégâts
        }

        [Test]
        public void TurnGrassVSFire()
        {
            Character ouisticram = new Character(100, 30, 8, 80, TYPE.FIRE);
            Character tortank = new Character(300, 80, 20, 60, TYPE.WATER);
            Skill fireBall = new FireBall();
            Skill waterBlouBlou = new WaterBlouBlou();

            Fight f = new Fight(ouisticram, tortank);
            f.ExecuteTurnWithFactorType(fireBall, waterBlouBlou);
            Assert.That(f.IsFightFinished, Is.EqualTo(false)); 
            Assert.That(ouisticram.IsAlive, Is.EqualTo(true));
            Assert.That(tortank.IsAlive, Is.EqualTo(true));
        }

        [Test]
        public void TurnGrassKillsWater()
        {
            Character germignon = new Character(150, 20, 5, 80, TYPE.GRASS);
            Character ludicolo = new Character(40, 30, 5, 40, TYPE.WATER);
            Skill magicalGrass = new MagicalGrass();
            Skill waterBlouBlou = new WaterBlouBlou();

            Fight f = new Fight(germignon, ludicolo);
            f.ExecuteTurnWithFactorType(magicalGrass, waterBlouBlou);
            Assert.That(f.IsFightFinished, Is.EqualTo(true));
            Assert.That(ludicolo.IsAlive, Is.EqualTo(false));
            Assert.That(germignon.IsAlive, Is.EqualTo(true));
        }
        // ----- Test Types ----- //
    }

}
