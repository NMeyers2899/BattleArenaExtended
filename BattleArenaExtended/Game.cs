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
        HEALTH,
        NONE
    }

    public enum Scene
    {
        START_MENU,
        NAME_CREATION,
        CHARACTER_SELECTION,
        BATTLE,
        SHOP_MENU,
        RESTART_MENU,
        BATTLE_PREP
    }

    public enum ItemName
    {
        BIG_STICK,
        BIG_WAND,
        BIG_SHIELD,
        FRESH_JS,
        WOMPUS_GUN,
        HEALTH_POTION,
        SKELLY_PIKE,
        IRON_CLUB,
        BIG_POTION,
        SMALL_SHIELD,
        THAEVE_BOW,
        PROTECTION_BAND,
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
        private Item[] _itemList;
        private Item[] _itemList1;
        private Item[] _itemList2;
        private Item[] _itemList3;
        private Item[] _itemList4;
        private Random randomNumber = new Random();
        private int _battlesFought = 0;
        private int _currentLevel = 1;

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
            InitializeEnemies();

            InitializeItems();
        }

        /// <summary>
        /// Initalizes the items for the game.
        /// </summary>
        public void InitializeItems()
        {
            // Initalizes the stats for the Big Wand.
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 14, BoostType = ItemType.ATTACK, 
            Cost = 14, ID = ItemName.BIG_WAND };

            // Initalizes the stats for the Big Shield.
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 22, BoostType = ItemType.DEFENSE,
            Cost = 28, ID = ItemName.BIG_SHIELD };

            // Initalizes the stats for the Big Stick.
            Item bigStick = new Item { Name = "Big Stick", StatBoost = 5, BoostType = ItemType.ATTACK,
            Cost = 2, ID = ItemName.BIG_STICK};

            // Initalizes the stats for the Fresh J's.
            Item freshJs = new Item { Name = "Fresh J's", StatBoost = 21, BoostType = ItemType.DEFENSE,
            Cost = 23, ID = ItemName.FRESH_JS};

            // Initalizes the stats for the Wompus Gun.
            Item wompusGun = new Item { Name = "Wompus Gun", StatBoost = 32, BoostType = ItemType.ATTACK,
            Cost = 44, ID = ItemName.WOMPUS_GUN };

            // Initalizes the stats for the Health Potion.
            Item healthPotion = new Item { Name = "Health Potion", StatBoost = 20, BoostType = ItemType.HEALTH,
            Cost = 15, ID = ItemName.HEALTH_POTION };

            // Initalizes the stats for the Skelly Pike.
            Item skellyPike = new Item { Name = "Skelly Pike", StatBoost = 34, BoostType = ItemType.ATTACK,
            Cost = 49, ID = ItemName.SKELLY_PIKE };

            // Initalizes the stats for the Iron Club.
            Item ironClub = new Item { Name = "Iron Club", StatBoost = 18, BoostType = ItemType.ATTACK,
            Cost = 17, ID = ItemName.IRON_CLUB };

            // Initalizes the stats for the Big Potion.
            Item bigPotion = new Item { Name = "Big Potion", StatBoost = 45, BoostType = ItemType.HEALTH,
            Cost = 35, ID = ItemName.BIG_POTION };

            // Initalizes the stats for the Small Shield.
            Item smallShield = new Item { Name = "Small Shield", StatBoost = 5, BoostType = ItemType.DEFENSE,
            Cost = 7, ID = ItemName.SMALL_SHIELD };

            // Initalizes the stats for the Thaeve Bow.
            Item thaeveBow = new Item { Name = "Thaeve Bow", StatBoost = 11, BoostType = ItemType.ATTACK,
            Cost = 12, ID = ItemName.THAEVE_BOW };

            // Initalizes the stats for the Protection Band.
            Item protectionBand = new Item { Name = "Protection Band", StatBoost = 9, BoostType = ItemType.DEFENSE,
            Cost = 10, ID = ItemName.PROTECTION_BAND };

            _offensiveInventory = new Item[] { bigStick };
            _defensiveInventory = new Item[] { smallShield };

            _itemList = new Item[] { bigStick, thaeveBow, bigWand, bigShield, ironClub, healthPotion, bigPotion,
            smallShield, skellyPike, ironClub, wompusGun, protectionBand};
            _itemList1 = new Item[] { bigWand, bigShield, healthPotion, skellyPike };
            _itemList2 = new Item[] { bigStick, bigPotion, ironClub, protectionBand, thaeveBow };
            _itemList3 = new Item[] { wompusGun, protectionBand, bigPotion, freshJs, skellyPike };
            _itemList4 = new Item[] { bigStick, thaeveBow, bigWand, bigShield, ironClub, healthPotion };
        }

        /// <summary>
        /// Initalizes the enemies on start and restart of the game.
        /// </summary>
        private void InitializeEnemies()
        {
            // Initalizes the Stats for Little Dude.
            Entity littleDude = new Entity("A Little Dude", 22, 18, 12, 10);

            // Initalizes the Stats for Thaeve.
            Entity thaeve = new Entity("Thaeve", 20, 12, 14, 12);

            // Initalizes the Stats for Wimpus.
            Entity wimpus = new Entity("Wimpus", 21, 19, 15, 14);

            // Initalizes the Stats for Durdle, the Great Turtle.
            Entity durdleTheTurtle = new Entity("Durdle, the Great Turtle", 24, 26, 30, 33);

            // Initalizes the Stats for Wompus.
            Entity wompus = new Entity("Wompus", 33, 22, 15, 25);

            // Initalizes the Stats for Moneybag.
            Entity moneybag = new Entity("Moneybag", 15, 34, 22, 43);

            // Initalizes the Stats for Big Dude.
            Entity bigDude = new Entity("A Big Dude", 31, 25, 18, 20);

            // Initalizes the Stats for Faceless Horror.
            Entity facelessHorror = new Entity("Faceless Horror", 26, 29, 18, 22);

            // Initalizes the Stats for Skelly
            Entity skelly = new Entity("Skelly", 30, 24, 22, 24);

            // Initalizes the Stats for Remnant of the World Eater.
            Entity remnant = new Entity("Remnant of the World Eater", 64, 32, 23, 53);

            // Initalizes the Stats for Spudette.
            Entity spudette = new Entity("Spudette", 42, 34, 32, 31);

            // Initalizes the Stats for Wompus With a Gun.
            Entity wompusWithGun = new Entity("Wompus With a Gun", 44, 32, 22, 34);

            // Initalizes the Stats for A Big Ol' Dude.
            Entity bigOlDude = new Entity("Big Ol' Dude", 48, 30, 27, 32);

            // Initalizes the Stats for The Final Boss.
            Entity theFinalBoss = new Entity("Krazarackaradareda the World Eater", 100, 34, 26, 0);

            // Initalizes the the list of enemies for the first level.
            Entity[] levelOne = new Entity[] { littleDude, thaeve, wimpus, durdleTheTurtle };

            // Initalizes the list of enemies for the second level.
            Entity[] levelTwo = new Entity[] { wompus, moneybag, bigDude, facelessHorror, skelly, remnant };

            // Initalizes the list of enemies for the final level.
            Entity[] levelThree = new Entity[] { wompusWithGun, bigOlDude, spudette, theFinalBoss };

            int percentage = randomNumber.Next(101);

            int randomEnemy = 0;

            // Initalizes the list of enemies that will be fought per level.
            if (_currentLevel == 1)
            {
                _enemies = levelOne;

                if(percentage >= 0 && percentage <= 25)
                {
                    randomEnemy = 0;
                }
                else if (percentage >= 26 && percentage <= 45)
                {
                    randomEnemy = 1;
                }
                else if (percentage >= 46 && percentage <= 73)
                {
                    randomEnemy = 2;
                }
                else
                {
                    randomEnemy = 3;
                }
            }
            else if(_currentLevel == 2)
            {
                _enemies = levelTwo;

                if (percentage >= 0 && percentage <= 25)
                {
                    randomEnemy = 0;
                }
                else if (percentage >= 26 && percentage <= 32)
                {
                    randomEnemy = 1;
                }
                else if (percentage >= 33 && percentage <= 48)
                {
                    randomEnemy = 2;
                }
                else if (percentage >= 49 && percentage <= 62)
                {
                    randomEnemy = 3;
                }
                else if (percentage >= 63 && percentage <= 74)
                {
                    randomEnemy = 4;
                }
                else
                {
                    randomEnemy = 5;
                }

            }
            else if(_currentLevel == 3)
            {
                _enemies = levelThree;

                if (percentage >= 0 && percentage <= 25)
                {
                    randomEnemy = 0;
                }
                else if (percentage >= 26 && percentage <= 45)
                {
                    randomEnemy = 1;
                }
                else if (percentage >= 46 && percentage <= 73)
                {
                    randomEnemy = 2;
                }
                else
                {
                    randomEnemy = 3;
                }
            }

            _currentEnemyIndex = randomEnemy;

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
            Console.WriteLine("You defeated " + _battlesFought + " enemies");

            Console.WriteLine("Farewell " + _player.Name + "!");
        }

        private void Save()
        {
            // Opens the writer for the save file.
            StreamWriter writer = new StreamWriter("SaveData.txt");

            // Writes down the current scene.
            writer.WriteLine(_currentScene);

            // Saves the current enemy index.
            writer.WriteLine(_currentEnemyIndex);

            // Saves the current stats for the enemy.
            _currentEnemy.Save(writer);

            // Saves the current stats for the player.
            _player.Save(writer);

            // Closes the file.
            writer.Close();
        }

        private bool Load()
        {
            // Checks to see if the save file exists. If it does not...
            if (!File.Exists("SaveData.txt"))
            {
                // ...it returns false.
                return false;
            }

            // Opens the reader for the save file.
            StreamReader reader = new StreamReader("SaveData.txt");

            // Tries to read the current scene and if it can't...
            if(!Scene.TryParse(reader.ReadLine(), out _currentScene))
            {
                // ...it returns false.
                return false;
            }

            // Sets the current enemy index from the file. If it can't...
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
            {
                // ...it returns false.
                return false;
            }

            // Creates a new instance of the current enemy.
            _currentEnemy = new Entity();

            // Tries to load the current enemy, if it can't...
            if (!_currentEnemy.Load(reader, _itemList1))
            {
                // ...it returns false.
                return false;
            }

            // Updates the enemy array to the current enemy's stats.
            _enemies[_currentEnemyIndex] = _currentEnemy;

            // Creates a new instance of the current enemy.
            _player = new Player();

            // Checks to see if the player can load their previous stats, if not...
            if (!_player.Load(reader, _itemList))
            {
                // ...it returns false;
                return false;
            }

            // Closes the file.
            reader.Close();

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
                // ...preparing to fight an enemy.
                case Scene.BATTLE_PREP:
                    BattlePrep();
                    break;
                // ...fighting enemies.
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
            int choice = GetInput("Welcome to your Odd Adventure!", "Start New Game", "Load Game");

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
                    _currentScene = Scene.CHARACTER_SELECTION;
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
                    _player = new Player(_playerName, 75, 22, 14, _offensiveInventory, "Offensive");
                    break;
                // ...or rely on defense more.
                case 1:
                    _player = new Player(_playerName, 75, 17, 21, _defensiveInventory, "Defensive");
                    break;
            }

            _currentScene = Scene.BATTLE_PREP;
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
            int choice = GetInput("Select an item to equip or use.", _player.GetItemNames());

            // Equip the item of the given index.
            _player.TryEquipItem(choice);

            // If the item that was equipped is an attack or defense type...
            if(_player.CurrentItem.BoostType == ItemType.ATTACK || 
                _player.CurrentItem.BoostType == ItemType.DEFENSE)
            {
                // ...it lets the player know which item they equipped.
                Console.WriteLine("You equipped " + _player.CurrentItem.Name + "!");
                Console.ReadKey(true);
                Console.Clear();
            }
        }

        /// <summary>
        /// If the enemy is a boss, we check to see if the player wishes to continue.
        /// </summary>
        /// <returns> If the boss is fought or not, if it is even encountered. </returns>
        public void BattlePrep()
        {
            int choice = 0;

            if (_currentEnemy.Name == "Durdle, the Great Turtle" || 
                _currentEnemy.Name == "Remnant of the World Eater" ||
                _currentEnemy.Name == "Krazarackaradareda the World Eater")
            {
                choice = GetInput("You feel a great presence just ahead. Do you-", "Continue Ahead", "Turn Back");

                switch (choice)
                {
                    case 0:
                        Console.WriteLine("You continue on ahead.");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case 1:
                        InitializeEnemies();
                        Console.WriteLine("You turn back, to return another day.");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                }
            }

            _currentScene = Scene.BATTLE;
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
                "Attack!", "Equip/Use Item.", "Remove Current Item.", "Save.");
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
                _battlesFought++;
                Console.ReadKey(true);
                Console.Clear();

                // Adds the gold the enemy drops to the player's gold count.
                _player.GetGold(_currentEnemy);

                // If the player has not killed all the enemies, if they have not...
                if (!(_currentEnemy.Name == "Durdle, the Great Turtle") && 
                    !(_currentEnemy.Name == "Remnant of the World Eater") &&
                    !(_currentEnemy.Name == "Krazarackaradareda the World Eater"))
                {
                    // ...ask the player if they wish to enter the shop.
                    EnterShop();
                }
                // If the player has killed Durdle, the system will...
                else if(_currentEnemy.Name == "Durdle, the Great Turtle")
                {
                    // ...ask the player if they wish to enter the shop, then increment the current level.
                    EnterShop();
                    _currentLevel++;
                }
                // If the player has killed the Remnant, the system will...
                else if (_currentEnemy.Name == "Remnant of the World Eater")
                {
                    // ...ask the player if they wish to enter the shop, then increment the current level.
                    EnterShop();
                    _currentLevel++;
                }
                // Check to see if the player has defeated every enemy.
                else
                {
                    _currentScene = Scene.RESTART_MENU;
                    Console.WriteLine("You are victorious!");
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                }

                InitializeEnemies();
                Console.ReadKey(true);
                Console.Clear();
            }

            // If the _player is dead, it asks the _player if they wish to restart the game.
            if (_player.Health <= 0)
            {
                Console.WriteLine("You have been slain.");
                DisplayRestartMenu();
            }
        }

        /// <summary>
        /// Gives the player the option to enter the shop or continue to the next battle.
        /// </summary>
        public void EnterShop()
        {
            if (_currentEnemyIndex >= _enemies.Length)
            {
                return;
            }

            int choice = GetInput("You come across a shop in your travels. Do you enter?", "Yes.", "No.");

            switch (choice)
            {
                case 0:
                    Console.WriteLine("You enter the shop.");
                    int randomShop = randomNumber.Next(0, 4);

                    switch (randomShop)
                    {
                        case 0:
                            _shop = new Shop(_itemList1);
                            break;
                        case 1:
                            _shop = new Shop(_itemList2);
                            break;
                        case 2:
                            _shop = new Shop(_itemList3);
                            break;
                        case 3:
                            _shop = new Shop(_itemList4);
                            break;
                    }
                    _currentScene = Scene.SHOP_MENU;
                    break;
                case 1:
                    Console.WriteLine("You move on to your next battle.");
                    _currentScene = Scene.BATTLE_PREP;
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
                _currentScene = Scene.BATTLE_PREP;
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
