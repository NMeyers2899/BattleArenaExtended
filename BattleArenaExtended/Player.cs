﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExtended
{
    class Player : Entity
    {
        private Item[] _inventory;
        private Item _currentItem;
        private int _currentItemIndex;
        private string _job;
        private int _gold;

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

        public int Gold
        {
            get { return _gold; }
        }

        public Player()
        {
            _inventory = new Item[0];
            _currentItem.Name = "Nothing";
            _currentItemIndex = -1;
        }

        public Player(Item[] items) : base()
        {
            _currentItem.Name = "Nothing";
            _inventory = items;
            _currentItemIndex = -1;
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[] items, string job) :
            base(name, health, attackPower, defensePower)
        {
            _inventory = items;
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
            if (index >= _inventory.Length || index < 0)
            {
                // ...it returns false.
                _currentItemIndex = -1;
                return false;
            }

            // Updates currentItemIndex to be equal to the given index.
            _currentItemIndex = index;

            // Sets the current item to the item at the index.
            _currentItem = _inventory[_currentItemIndex];

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
        /// The player will attempt to buy an item. If they can, their gold will decrement by the item's cost
        /// and the item is added to their inventory.
        /// </summary>
        /// <param name="item"> The item that the player wishes to buy. </param>
        public void Buy(Item item)
        {
            // Creates a copy of the players inventory with a new space to add the item to.
            Item[] newPlayerInventory = new Item[_inventory.Length + 1];

            // Tells the player they purchased the item.
            Console.WriteLine("You purchased " + item.Name + "!");

            // Adds each item from the player's inventory to the new array.
            for (int i = 0; i < _inventory.Length; i++)
            {
                newPlayerInventory[i] = _inventory[i];
            }

            // Subtracts the item's cost from the player's gold.
            _gold -= item.Cost;

            // Appends the new item to the end of the player's inventory.
            newPlayerInventory[_inventory.Length] = item;

            // Sets the old inventory equal to the new one so it adds the new item.
            _inventory = newPlayerInventory;

            Console.ReadKey(true);
            Console.Clear();

        }

        // Gets the names of the items in the player's inventory.
        public string[] GetItemNames()
        {
            string[] itemList = new string[_inventory.Length];

            for (int i = 0; i < _inventory.Length; i++)
            {
                itemList[i] = _inventory[i].Name;
            }

            return itemList;
        }
    }
}