using System;
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
                    return base.AttackPower + _currentItem.StatBoost;
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
                    return base.DefensePower + _currentItem.StatBoost;
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

        public Player(string name, float health, float attackPower, float defensePower, Item[] items, string job,
            int goldAmount = 0) : base(name, health, attackPower, defensePower, goldAmount)
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
            int previousIndex = _currentItemIndex;

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

            // If the item boosts health...
            if (_currentItem.BoostType == ItemType.HEALTH)
            {
                // ...add the stat boost of the item to the player's health...
                _health += _inventory[_currentItemIndex].StatBoost;
                Console.WriteLine("You recovered " + _currentItem.StatBoost + " health!");
                Console.ReadKey(true);
                Console.Clear();

                Item[] newInventory = new Item[_inventory.Length - 1];

                int j = 0;

                bool itemRemoved = false;

                // ...and remove the item from the inventory.
                for(int i = 0; i < _inventory.Length; i++)
                {
                    if(_inventory[i].ID != _currentItem.ID || itemRemoved)
                    {
                        newInventory[j] = _inventory[i];
                        j++;
                    }
                    else
                    {
                        itemRemoved = true;
                    }
                }

                // Sets the old inventory equal to the new one.
                _inventory = newInventory;

                // Checks to see if there was no item previously equipped.
                if (previousIndex == -1)
                {
                    return true;
                }
                // Checks to see if the previous index was the last item in the old array. If it is...
                else if (previousIndex >= _inventory.Length)
                {
                    // ...subtract one from the previous index.
                    previousIndex--;
                }

                // Sets the current item 
                _currentItem = _inventory[previousIndex];

                return true;
            }

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

        /// <summary>
        /// The player adds the amount of gold an enemy drops to their gold amount.
        /// </summary>
        /// <param name="enemy"> The enemy whose gold the player is taking. </param>
        public void GetGold(Entity enemy)
        {
            _gold += enemy.GoldAmount;
        }

        /// <summary>
        /// Saves what is important for the player to the file.
        /// </summary>
        /// <param name="writer"> What writes to the file. </param>
        public override void Save(StreamWriter writer)
        {
            writer.WriteLine(_job);
            base.Save(writer);
            writer.WriteLine(_inventory.Length);
            writer.WriteLine(_gold);
            writer.WriteLine(_currentItemIndex);

            for (int i = 0; i < _inventory.Length; i++)
            {
                writer.WriteLine(_inventory[i].ID);
            }
        }

        /// <summary>
        /// Attempts to load the player's previous information.
        /// </summary>
        /// <param name="reader"> What reads the save file. </param>
        /// <param name="itemList"> The list of items the the player will use to load their inventory. </param>
        /// <returns> If the player's information could be properly loaded. </returns>
        public override bool Load(StreamReader reader, Item[] itemList)
        {
            // Sets the job equal to the player's previous job.
            _job = reader.ReadLine();

            // Checks to see if the base load of the entity fails. If it does...
            if (!base.Load(reader, itemList))
            {
                // ...it returns false.
                return false;
            }

            // Checks to see if the reader can read the player's inventory size, if it can't...
            if (!int.TryParse(reader.ReadLine(), out int length))
            {
                // ...it returns false;
                return false;
            }

            // This sets the inventory size to the read length.
            _inventory = new Item[length];

            // Checks to see if it can load the player's gold. If it can't...
            if (!int.TryParse(reader.ReadLine(), out _gold))
            {
                // ...it returns false.
                return false;
            }

            // Checks to see if it can get the current item index. It it can't...
            if (!int.TryParse(reader.ReadLine(), out _currentItemIndex))
            {
                // ...it returns false.
                return false;
            }

            int i = 0;

            while (!reader.EndOfStream)
            {
                // Checks to see if the next line is an item's ID. If it isn't...
                if (!ItemName.TryParse(reader.ReadLine(), out ItemName itemID))
                {
                    // ...it returns false.
                    return false;
                }

                for (int j = 0; j < itemList.Length; j++)
                {
                    // Checks to see if the itemID found is the same as an item in the list...
                    if (itemID == itemList[j].ID)
                    {
                        // ...and if it is, it sets the iventory at the current positon equal to the item
                        // in the item list.
                        _inventory[i] = itemList[j];
                    }
                }

                i++;
            }

            // If everything was found correctly, it attempts to equip the item.
            TryEquipItem(_currentItemIndex);

            return true;
        }
    }
}
