using System;
using System.Diagnostics;
using NUnit.Framework;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition d'un personnage
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Stat de base, HP
        /// </summary>
        int _baseHealth;
        /// <summary>
        /// Stat de base, ATK
        /// </summary>
        int _baseAttack;
        /// <summary>
        /// Stat de base, DEF
        /// </summary>
        int _baseDefense;
        /// <summary>
        /// Stat de base, SPE
        /// </summary>
        int _baseSpeed;
        /// <summary>
        /// Type de base
        /// </summary>
        TYPE _baseType;

        public Character(int baseHealth, int baseAttack, int baseDefense, int baseSpeed, TYPE baseType)
        {
            _baseHealth = baseHealth;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
            _baseSpeed = baseSpeed;
            _baseType = baseType;
            CurrentHealth = baseHealth;
        }
        /// <summary>
        /// HP actuel du personnage
        /// </summary>
        public int CurrentHealth { get; private set; }
        public TYPE BaseType { get => _baseType;}
        /// <summary>
        /// HPMax, prendre en compte base et equipement potentiel
        /// </summary>
        public int MaxHealth
        {
            get
            {
                return _baseHealth + (CurrentEquipment == null ? 0 : CurrentEquipment.BonusHealth);
            }
        }
        /// <summary>
        /// ATK, prendre en compte base et equipement potentiel
        /// </summary>
        public int Attack
        {
            get
            {
                return _baseAttack + (CurrentEquipment == null ? 0 : CurrentEquipment.BonusAttack);
            }
        }
        /// <summary>
        /// DEF, prendre en compte base et equipement potentiel
        /// </summary>
        public int Defense
        {
            get
            {
                return _baseDefense + (CurrentEquipment == null ? 0 : CurrentEquipment.BonusDefense);
            }
        }
        /// <summary>
        /// SPE, prendre en compte base et equipement potentiel
        /// </summary>
        public int Speed
        {
            get
            {
                return _baseSpeed + (CurrentEquipment == null ? 0 : CurrentEquipment.BonusSpeed);
            }
        }
        /// <summary>
        /// Equipement unique du personnage
        /// </summary>
        public Equipment CurrentEquipment { get; private set; }
        /// <summary>
        /// null si pas de status
        /// </summary>
        public StatusEffect CurrentStatus { get; private set; }

        public bool IsAlive => CurrentHealth > 0;


        /// <summary>
        /// Application d'un skill contre le personnage
        /// On pourrait potentiellement avoir besoin de connaitre le personnage attaquant,
        /// Vous pouvez adapter au besoin
        /// </summary>
        /// <param name="s">skill attaquant</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// Cette méthode prend en compte l'attake du pokémon adverse et adapter au script Fight.cs
        public void ReceiveAttack(Skill s, Character attacker)
        {
            if(attacker.CurrentStatus!=null && !attacker.CurrentStatus.CanAttack)
            {
                attacker.CurrentStatus?.EndTurn();
                return;
            }
            if(s.Status!=StatusPotential.HEAL)
            {
                CurrentStatus = StatusEffect.GetNewStatusEffect(s.Status);
                CurrentHealth -= (int)(attacker.Attack+s.Power * TypeResolver.GetFactor(s.Type,BaseType)) - Defense;
            }
            else
            {
                attacker.CurrentHealth += s.Power;
                if(attacker.CurrentHealth>attacker.MaxHealth)
                {
                    attacker.CurrentHealth=attacker.MaxHealth;
                }
            }
            if(attacker.CurrentStatus!=null)
            {
                if(attacker.CurrentStatus.DamageOnAttack>0)
                {
                attacker.CurrentHealth -= (int)(attacker.Attack * attacker.CurrentStatus.DamageOnAttack);
                }
                attacker.CurrentStatus?.EndTurn();
                if(attacker.CurrentStatus.RemainingTurn==0)
                {
                    attacker.CurrentStatus=null;
                }
            }
            if(CurrentHealth<=0)
            {
                CurrentHealth=0;
            }

        }
        ///  Cette méthode est adapter au test Unitaire
        public void ReceiveAttack(Skill s)
        {
            CurrentHealth -= (int)(s.Power * TypeResolver.GetFactor(s.Type,BaseType))- Defense;
            CurrentStatus = StatusEffect.GetNewStatusEffect(s.Status);
            if(CurrentHealth<=0)
            {
                CurrentHealth=0;
            }
        }
        /// <summary>
        /// Equipe un objet au personnage
        /// </summary>
        /// <param name="newEquipment">equipement a appliquer</param>
        /// <exception cref="ArgumentNullException">Si equipement est null</exception>
        public void Equip(Equipment newEquipment)
        {
            if (newEquipment == null)
            {
                throw new ArgumentNullException();
            }
            else if (CurrentEquipment != null)
            {
                throw new InvalidOperationException("Character already equipped");
            }
            CurrentEquipment = newEquipment;
        }
        /// <summary>
        /// Desequipe l'objet en cours au personnage
        /// </summary>
        public void Unequip()
        {
            CurrentEquipment = null;
        }

    }
}
