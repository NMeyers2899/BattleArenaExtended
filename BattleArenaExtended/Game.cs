using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArenaExtended
{
    public enum ItemType
    {
        DEFENSE,
        ATTACK,
        NONE
    }

    public enum Scene
    {
        START_MENU,
        NAME_CREATION,
        CHARACTER_SELECTION,
        BATTLE,
        SHOP_MENU,
        RESTART_MENU
    }

    public enum ItemName
    {
        BIG_STICK,
        BIG_WAND,
        BIG_SHIELD,
        FRESH_JS,
        WOMPUS_GUN,
        COUNT
    }

    public struct Item
    {
        public string Name;
        public float StatBoost;
        public ItemType BoostType;
        public int Cost;
        public ItemName ID;
    }

    public class Game
    {
        private bool _gameOver = false;
        private Scene _currentScene = 0;
        private int _currentEnemyIndex = 0;
        private Player _player;
        private Shop _shop;
        private string _playerName;
        private Entity[] _enemies;
        private Entity _currentEnemy;
        private Item[] _offensiveInventory;
        private Item[] _defensiveInventory;

        /// <summary>
        /// Function that starts the main game loop.
        /// </summary>
        public void Run()
        {
            Start();

            while (!_gameOver)
            {
                Update();
            }

            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default.
        /// </summary>
        private void Start()
        {
            _gameOver = false;

            _currentScene = 0;

            InitializeEnemies();

            InitializeItems();
        }

        /// <summary>
        /// Initalizes the items for the different classes.
        /// </summary>
        public void InitializeItems()
        {
            // Defensive Items
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 20, BoostType = ItemType.ATTACK, 
            Cost = 25, ID = ItemName.BIG_WAND };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 25, BoostType = ItemType.DEFENSE,
            Cost = 22, ID = ItemName.BIG_SHIELD };

            // Offensive Items
            Item bigStick = new Item { Name = "Big Stick", StatBoost = 20, BoostType = ItemType.ATTACK,
            Cost = 2, ID = ItemName.BIG_STICK};
            Item freshJs = new Item { Name = "Fresh J's", StatBoost = 10, BoostType = ItemType.DEFENSE,
            Cost = 52, ID = ItemName.FRESH_JS};

            // Other Items
            Item wompusGun = new Item { Name = "Wompus' Gun", StatBoost = 30, BoostType = ItemType.ATTACK,
            Cost = 35, ID = ItemName.WOMPUS_GUN };

            _defensiveInventory = new Item[] { bigWand, bigShield };
            _offensiveInventory = new Item[] { bigStick, freshJs };

            Item[] itemList = new Item[] { bigWand, bigShield, bigStick, freshJs, wompusGun };

            _shop = new Shop(itemList);
        }

        /// <summary>
        /// Initalizes the enemies on start and restart of the game.
        /// </summary>
        private void InitializeEnemies()
        {
            _currentEnemyIndex = 0;

            // Initalizes the Stats for Little Dude.
            Entity littleDude = new Entity("A Little Dude", 30, 25, 15, 15);

            // Initalizes the Stats for Big Dude.
            Entity bigDude = new Entity("A Big Dude", 35, 30, 20, 20);

            // Initalizes the Stats for Wompus With a Gun.
            Entity wompusWithGun = new Entity("Wompus With a Gun", 40, 35, 20, 30);

            // Initalizes the Stats for The Final Boss.
            Entity theFinalBoss = new Entity("Krazarackaradareda the World Eater", 50, 35, 20, 50);

            // Initalizes the list of _enemies that will be fought in this order.
            _enemies = new Entity[] { littleDude, bigDude, theFinalBoss };

            _currentEnemy = _enemies[_currentEnemyIndex];

        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        private void Update()
        {
            DisplayCurrentScene();
        }

        /// <summary>
        /// This function is called before the applications closes.
        /// </summary>
        private void End()
        {
            Console.WriteLine("Farewell " + _player.Name + "!");
        }

        private void Save()
        {

        }

        private bool Load()
        {
            return true;
        }

        /// <summary>
        /// Gets an input from the player based on some given decision.
        /// </summary>
        /// <param name="description"> The context for the input </param>
        /// <param name="options"> The options given to the player. </param>
        /// <returns> The users input of a given choice. </returns>
        private int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputRecieved = -1;

            while (inputRecieved == -1)
            {
                // Print out all options.
                Console.WriteLine(description);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);
                }
                Console.Write("> ");

                input = Console.ReadLine();

                // If a player typed an int...
                if (int.TryParse(input, out inputRecieved))
                {
                    // ...decrement the input and check if it's within bounds of the array.
                    inputRecieved--;
                    if (inputRecieved < 0 || inputRecieved >= options.Length)
                    {
                        // Sets inputRecieved to the default value.
                        inputRecieved = -1;
                        //Display error message.
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }
                // If the user didn't type an int.
                else
                {
                    // Sets inputRecieved to the default value.
                    inputRecieved = -1;
                    //Display error message.
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey(true);
                }

                Console.Clear();
            }

            return inputRecieved;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index.
        /// </summary>
        private void DisplayCurrentScene()
        {
            // Finds the current scene for...
            switch (_currentScene)
            {
                // ...the start menu.
                case Scene.START_MENU:
                    DisplayStartMenu();
                    break;
                case Scene.NAME_CREATION:
                    GetPlayerName();
                    break;
                // ...character selection.
                case Scene.CHARACTER_SELECTION:
                    CharacterSelection();
                    break;
                // ...fighting _enemies.
                case Scene.BATTLE:
                    Battle();
                    break;
                // ...shoping for items.
                case Scene.SHOP_MENU:
                    DisplayShopMenu();
                    break;
                // ...asking the player to restart the game.
                case Scene.RESTART_MENU:
                    DisplayRestartMenu();
                    break;
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game.
        /// </summary>
        private void DisplayRestartMenu()
        {
            int choice = GetInput("Would you like to restart the game?", "Yes!", "No.");
            // Finds out whether the _player wishes to...
            switch (choice)
            {
                // ...restart the game.
                case 0:
                    _currentScene = 0;
                    InitializeEnemies();
                    break;
                // ...end the game.
                case 1:
                    _gameOver = true;
                    break;
            }
        }

        /// <summary>
        /// Displays the menu for the start of the game, allowing the player to start through the battles
        /// or load a previous save.
        /// </summary>
        public void DisplayStartMenu()
        {
            int choice = GetInput("Welcome to the Battle Arena!", "Start New Game", "Load Game");

            switch (choice)
            {
                case 0:
                    _currentScene = Scene.NAME_CREATION;
                    break;
                case 1:
                    if (Load())
                    {
                        Console.WriteLine("Load Succssesful");
                        Console.ReadKey(true);
                        Console.Clear();
                        _currentScene = Scene.BATTLE;
                    }
                    else
                    {
                        Console.WriteLine("Load Failed");
                        Console.ReadKey(true);
                        Console.Clear();
                        _currentScene = Scene.START_MENU;
                    }
                    break;
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the _player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.Write("What is your name, adventurer? \n> ");
            _playerName = Console.ReadLine();
            Console.Clear();

            int choice = GetInput("Would you like to keep your name?", "Yes.", "No.");

            switch (choice)
            {
                case 0:
                    _currentScene++;
                    break;
                case 1:
                    break;
            }
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int choice = 0;

            // Checks to see if the _player kept their fighting style from another playthough.
            choice = GetInput("Which style of fighting do you align with?",
               "Brute Force!", "Defensive Tactics.");

            // Finds out whether the _player wants to...
            switch (choice)
            {
                // ...be a more physical fighter.
                case 0:
                    _player = new Player(_playerName, 100, 35, 10, _offensiveInventory, "Offensive");
                    break;
                // ...or rely on defense more.
                case 1:
                    _player = new Player(_playerName, 75, 20, 15, _defensiveInventory, "Defensive");
                    break;
            }

            _currentScene++;
        }

        /// <summary>
        /// Prints a character's stats to the console.
        /// </summary>
        /// <param name="character"> The character that will have its stats shown </param>
        void DisplayStats(Entity character)
        {
            Console.WriteLine(character.Name + "'s stats:");
            Console.WriteLine("Health: " + character.Health);
            Console.WriteLine("Attack: " + character.AttackPower);
            Console.WriteLine("Defense: " + character.DefensePower);
        }

        /// <summary>
        /// Allows the player to equip or unequip an item.
        /// </summary>
        public void DisplayEquipItemMenu()
        {
            // Get the item index.
            int choice = GetInput("Select an item to equip.", _player.GetItemNames());

            // Equip the item of the given index.
            _player.TryEquipItem(choice);

            if (!_player.TryEquipItem(choice))
            {
                Console.WriteLine("You couldn't find that item in the bag.");
                Console.ReadKey(true);
                Console.Clear();
            }

            Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle()
        {
            float damageDealt = 0;

            // Gives updates on the _player and current enemy's stats.
            DisplayStats(_player);
            Console.WriteLine("");
            DisplayStats(_currentEnemy);
            Console.WriteLine("");

            int choice = GetInput(_currentEnemy.Name + " stands before you! What will you do?",
                "Attack!", "Equip Item.", "Remove Current Item.", "Save.");
            // Finds out if the _player wishes to...
            switch (choice)
            {
                // ...attack, dealing damage to the enemy. In turn taking damage from the enemy.
                case 0:
                    damageDealt = _player.Attack(_currentEnemy);
                    break;
                // ... dodge the enemy's attack, but deal no damage in return.
                case 1:
                    DisplayEquipItemMenu();
                    return;
                case 2:
                    if (!_player.TryUnequipItem())
                    {
                        Console.WriteLine("You don't have anything equipped.");
                    }
                    else
                    {
                        Console.WriteLine("You placed the item in your bag.");
                    }

                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                case 3:
                    Save();
                    Console.WriteLine("Saved Game");
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
            }

            damageDealt = _currentEnemy.Attack(_player);

            CheckBattleResults();
        }

        /// <summary>
        /// Checks to see if either the _player or the enemy has won the current battle.
        /// Updates the game based on who won the battle.
        /// </summary>
        void CheckBattleResults()
        {
            // If the _player is still alive and the enemy is dead, it moves on to the next fight.
            if (_currentEnemy.Health <= 0)
            {
                Console.WriteLine("You defeated " + _currentEnemy.Name + "!");
                // Adds the gold the enemy drops to the player's gold count.
                _player.GetGold(_currentEnemy);
                _currentEnemyIndex++;

                if (_currentEnemyIndex >= _enemies.Length)
                {
                    _currentScene = Scene.RESTART_MENU;
                    Console.WriteLine("You are victorious!");
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                }

                _currentEnemy = _enemies[_currentEnemyIndex];
                Console.ReadKey(true);
                Console.Clear();
            }

            // If the _player is dead, it asks the _player if they wish to restart the game.
            if (_player.Health <= 0)
            {
                Console.WriteLine("You have been slain.");
                DisplayRestartMenu();
            }

            // Allows the player the option to shop.
            EnterShop();
        }

        /// <summary>
        /// Gives the player the option to enter the shop or continue to the next battle.
        /// </summary>
        public void EnterShop()
        {
            int choice = GetInput("You come across a shop in your travels. Do you enter?", "Yes.", "No.");

            switch (choice)
            {
                case 0:
                    Console.WriteLine("You enter the shop.");
                    _currentScene = Scene.SHOP_MENU;
                    break;
                case 1:
                    Console.WriteLine("You move on to your next battle.");
                    break;
            }
        }

        /// <summary>
        /// Gets the item names from the shop and adds a save and quit option.
        /// </summary>
        /// <returns> The string for the menu options. </returns>
        private string[] GetShopMenuOptions()
        {
            // Grabs the item names and their costs from the shop.
            string[] shopItems = _shop.GetItemNames();

            // Creates a new array that will append the save and quit feature to the items.
            string[] shopOptions = new string[shopItems.Length + 1];

            // Sets all of the items as menu options.
            for (int i = 0; i < shopItems.Length; i++)
            {
                shopOptions[i] = shopItems[i];
            }

            // Appends the new options to the item list from the shop.
            shopOptions[shopItems.Length] = "Leave Shop";

            // Returns the new list.
            return shopOptions;
        }

        /// <summary>
        /// Displays the shop menu to the player, allowing them to buy the items and save or quit.
        /// </summary>
        private void DisplayShopMenu()
        {
            string[] playerInventory = _player.GetItemNames();

            // Displays the player's gold and items to the screen.
            Console.WriteLine("Your Gold: " + _player.Gold);
            Console.WriteLine("Your Inventory: ");
            for (int i = 0; i < playerInventory.Length; i++)
            {
                Console.WriteLine(playerInventory[i]);
            }

            Console.WriteLine();

            // Gets the menu options from the get function.
            string[] menuOptions = GetShopMenuOptions();

            // Asks the player which item they would like to buy, and if they would like to save or quit.
            int choice = GetInput("What will you be buying?", menuOptions);

            // If the player picks...
            if (choice == menuOptions.Length - 1)
            {
                Console.WriteLine("Do come again, friend!");
                Console.ReadKey(true);
                Console.Clear();
                _currentScene = Scene.BATTLE;
            }
            else if (!_shop.Sell(_player, choice))
            {
                Console.WriteLine("Very sorry. I do not go lower in my prices.");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }
}
