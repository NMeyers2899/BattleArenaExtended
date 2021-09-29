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
        protected float _health;
        private float _attackPower;
        private float _defensePower;
        private int _goldAmount;
        private string _description;

        // Gets the entity's name.
        public string Name
        {
            get { return _name; }
        }

        // Gets the entity's health.
        public virtual float Health
        {
            get { return _health; }
        }

        // Gets the entity's attack power.
        public virtual float AttackPower
        {
            get { return _attackPower; }
        }

        // Gets the entity's defense power.
        public virtual float DefensePower
        {
            get { return _defensePower; }
        }

        // Gets the amount of gold the entity drops.
        public int GoldAmount
        {
            get { return _goldAmount; }
        }

        public string Description
        {
            get { return _description; }
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
            _goldAmount = 0;
        }

        /// <summary>
        /// This constructor allows the user to input stats of an entity.
        /// </summary>
        /// <param name="name"> The entity's name. </param>
        /// <param name="health"> The entity's health. </param>
        /// <param name="attackPower"> The entity's attack. </param>
        /// <param name="defensePower"> The entity's defense. </param>
        public Entity(string name, float health, float attackPower, float defensePower, int goldAmount, 
            string description)
        {
            _name = name;
            _health = health;
            _attackPower = attackPower;
            _defensePower = defensePower;
            _goldAmount = goldAmount;
            _description = description;
        }

        /// <summary>
        /// Allows the entity to take damage and decrease their health based on that damage.
        /// </summary>
        /// <param name="damageAmount"> The amount of damage being dealt. </param>
        /// <returns> The amount of damage being dealt. </returns>
        public float TakeDamage(float damageAmount)
        {
            // Gets the amount of damage being dealt to the entity.
            float damageTaken = damageAmount - DefensePower;

            // If the damage being taken is less than zero...
            if (damageTaken < 0)
            {
                // ...it sets it equal to zero.
                damageTaken = 0;
            }

            _health -= damageTaken;

            Console.WriteLine(_name + " took " + damageTaken + " damage!");
            Console.ReadKey(true);
            Console.Clear();

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

        /// <summary>
        /// Saves the stats of the entity to a save file.
        /// </summary>
        /// <param name="writer"> The writer that saves the data. </param>
        public virtual void Save(StreamWriter writer)
        {
            writer.WriteLine(_name);
            writer.WriteLine(_health);
            writer.WriteLine(_attackPower);
            writer.WriteLine(_defensePower);
            writer.WriteLine(_goldAmount);
            writer.WriteLine(_description);
        }

        /// <summary>
        /// Loads the stats for the enemy that the player is battling.
        /// </summary>
        /// <param name="reader"> The reader that is currently reading from the save file. </param>
        /// <param name="itemList"> The list of items the the player will use to load their inventory. </param>
        /// <returns> If it could load correctly. </returns>
        public virtual bool Load(StreamReader reader, Item[] itemList)
        {
            _name = reader.ReadLine();

            if (!float.TryParse(reader.ReadLine(), out _health))
            {
                return false;
            }

            if (!float.TryParse(reader.ReadLine(), out _attackPower))
            {
                return false;
            }

            if (!float.TryParse(reader.ReadLine(), out _defensePower))
            {
                return false;
            }

            if(!int.TryParse(reader.ReadLine(), out _goldAmount))
            {
                return false;
            }

            _description = reader.ReadLine();

            return true;
        }
    }
}