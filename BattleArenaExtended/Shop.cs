using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArenaExtended
{
    class Shop
    {
        private Item[] _inventory;

        public Shop(Item[] shopInventory)
        {
            _inventory = shopInventory;
        }

        /// <summary>
        /// Looks to see if a player has enough gold to buy an item.
        /// </summary>
        /// <param name="player"> The player attempting to buy the item. </param>
        /// <param name="position"> The position of the item they wish to buy. </param>
        /// <returns> Whether or not the player has enough gold for the item. </returns>
        public bool Sell(Player player, int position)
        {
            if (player.Gold < _inventory[position].Cost)
            {
                return false;
            }
            else
            {
                player.Buy(_inventory[position]);
                return true;
            }
        }

        /// <summary>
        /// Gets the list of item names from the shop.
        /// </summary>
        /// <returns> Returns the list of item names that are in the shop. </returns>
        public string[] GetItemNames()
        {
            string[] itemList = new string[_inventory.Length];

            for (int i = 0; i < _inventory.Length; i++)
            {
                itemList[i] = _inventory[i].Name + " - " + _inventory[i].Cost + "GP";
            }

            return itemList;
        }
    }
}
