using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExtended
{
    class Entity
    {
        // Declares the stats of an entity.
        private string _name;
        private float _health;
        private float _attackPower;
        private float _defensePower;

        // Gets the entity's name.
        public string Name
        {
            get { return _name; }
        }

        // Gets the entity's health.
        public float Health
        {
            get { return _health; }
        }

        // Gets the entity's attack power.
        public virtual float AttackPower
        {
            get { return _attackPower; }
        }

        // Gets the entity's health.
        public virtual float DefensePower
        {
            get { return _defensePower; }
        }

        /// <summary>
        /// Default constructor of entity.
        /// </summary>
        public Entity()
        {
            _name = "Default";
            _health = 0;
            _attackPower = 0;
            _defensePower = 0;
        }

        /// <summary>
        /// This constructor allows the user to input stats of an entity.
        /// </summary>
        /// <param name="name"> The entity's name. </param>
        /// <param name="health"> The entity's health. </param>
        /// <param name="attackPower"> The entity's attack. </param>
        /// <param name="defensePower"> The entity's defense. </param>
        public Entity(string name, float health, float attackPower, float defensePower)
        {
            _name = name;
            _health = health;
            _attackPower = attackPower;
            _defensePower = defensePower;
        }

        /// <summary>
        /// Allows the entity to take damage and decrease their health based on that damage.
        /// </summary>
        /// <param name="damageAmount"> The amount of damage being dealt. </param>
        /// <returns> The amount of damage being dealt. </returns>
        public float TakeDamage(float damageAmount)
        {
            float damageTaken = damageAmount - DefensePower;

            if (damageTaken < 0)
            {
                damageTaken = 0;
            }

            _health -= damageTaken;

            return damageTaken;
        }

        /// <summary>
        /// This entity will attack another entity, dealing damage to it.
        /// </summary>
        /// <param name="defender"> The target of the attack. </param>
        /// <returns> It will return the damage that is being taken. </returns>
        public float Attack(Entity defender)
        {
            return defender.TakeDamage(AttackPower);
        }

        public virtual void Save(StreamWriter writer)
        {
            writer.WriteLine(_name);
            writer.WriteLine(_health);
            writer.WriteLine(_attackPower);
            writer.WriteLine(_defensePower);
        }