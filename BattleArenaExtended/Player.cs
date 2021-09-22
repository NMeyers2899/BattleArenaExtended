using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExtended
{
    class Player : Entity
    {
        private Item[] _items;
        private Item _currentItem;
        private int _currentItemIndex;
        private string _job;

        public Item CurrentItem
        {
            get { return _currentItem; }
        }

        public override float AttackPower
        {
            get
            {
                if (_currentItem.BoostType == ItemType.ATTACK)
                {
                    return base.AttackPower + CurrentItem.StatBoost;
                }

                return base.AttackPower;
            }
        }

        public override float DefensePower
        {
            get
            {
                if (_currentItem.BoostType == ItemType.DEFENSE)
                {
                    return base.DefensePower + CurrentItem.StatBoost;
                }

                return base.DefensePower;
            }
        }

        public string Job
        {
            get
            {
                return _job;
            }
            set
            {
                _job = value;
            }
        }

        public Player()
        {
            _items = new Item[0];
            _currentItem.Name = "Nothing";
            _currentItemIndex = -1;
        }

        public Player(Item[] items) : base()
        {
            _currentItem.Name = "Nothing";
            _items = items;
            _currentItemIndex = -1;
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[] items, string job) :
            base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.Name = "Nothing";
            _job = job;
            _currentItemIndex = -1;
        }

        /// <summary>
        /// Attempts to equip an item of an index given to us by the user.
        /// </summary>
        /// <param name="index"> The index which refers to an item. </param>
        /// <returns> If the user can equip the item or not. </returns>
        public bool TryEquipItem(int index)
        {
            // Checks to see if the index is out of bounds of our _items array. If it is...
            if (index >= _items.Length || index < 0)
            {
                // ...it returns false.
                _currentItemIndex = -1;
                return false;
            }

            // Updates currentItemIndex to be equal to the given index.
            _currentItemIndex = index;

            // Sets the current item to the item at the index.
            _currentItem = _items[_currentItemIndex];

            return true;
        }

        /// <summary>
        /// Sets the current item to nothing.
        /// </summary>
        /// <returns> Whether or not the player already had an item equipped. </returns>
        public bool TryUnequipItem()
        {
            // Checks to see if anything is equipped. If it is...
            if (_currentItem.Name == "Nothing")
            {
                // ...it returns false.
                return false;
            }

            _currentItemIndex = -1;

            // Sets the item to nothing.
            _currentItem = new Item();
            _currentItem.Name = "Nothing";

            return true;
        }

        /// <summary>
        /// Gets the list of item names our player currently has.
        /// </summary>
        /// <returns> The list of names of items that our player has. </returns>
        public string[] GetItemNames()
        {
            string[] itemNames = new string[_items.Length];

            for (int i = 0; i < _items.Length; i++)
            {
                itemNames[i] = _items[i].Name;
            }

            return itemNames;
        }
