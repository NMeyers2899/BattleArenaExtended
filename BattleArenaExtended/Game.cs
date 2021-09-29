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
        JUDGEMENT_BLADE,
        AEGIS,
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
        private Item[] _itemList5;
        private Random _randomNumber = new Random();
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
        private void InitializeItems()
        {
            // Initalizes the stats for the Big Wand.
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 14, BoostType = ItemType.ATTACK, 
            Cost = 14, ID = ItemName.BIG_WAND };

            // Initalizes the stats for the Big Shield.
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 22, BoostType = ItemType.DEFENSE,
            Cost = 55, ID = ItemName.BIG_SHIELD };

            // Initalizes the stats for the Big Stick.
            Item bigStick = new Item { Name = "Big Stick", StatBoost = 5, BoostType = ItemType.ATTACK,
            Cost = 2, ID = ItemName.BIG_STICK};

            // Initalizes the stats for the Fresh J's.
            Item freshJs = new Item { Name = "Fresh J's", StatBoost = 18, BoostType = ItemType.DEFENSE,
            Cost = 34, ID = ItemName.FRESH_JS};

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
            Item ironClub = new Item { Name = "Iron Club", StatBoost = 16, BoostType = ItemType.ATTACK,
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

            // Initalizes the stats for the Judgement Blade.
            Item judgementBlade = new Item { Name = "Judgement Blade", StatBoost = 47, BoostType = ItemType.ATTACK,
            Cost = 67, ID = ItemName.JUDGEMENT_BLADE };

            // Initalizes the stats for the Aegis
            Item aegis = new Item { Name = "Aegis", StatBoost = 26, BoostType = ItemType.DEFENSE, Cost = 64,
            ID = ItemName.AEGIS };

            _offensiveInventory = new Item[] { thaeveBow, smallShield };
            _defensiveInventory = new Item[] { bigStick, protectionBand };

            // Initalizes the list of every item in the game.
            _itemList = new Item[] { bigStick, thaeveBow, bigWand, bigShield, ironClub, healthPotion, bigPotion,
            smallShield, skellyPike, ironClub, wompusGun, protectionBand, freshJs, judgementBlade, aegis};

            // Initalizes the lists of the items the shop will use when called upon.
            _itemList1 = new Item[] { bigWand, smallShield, healthPotion, skellyPike, aegis };
            _itemList2 = new Item[] { bigStick, bigPotion, ironClub, protectionBand, thaeveBow, judgementBlade };
            _itemList3 = new Item[] { wompusGun, protectionBand, bigPotion, freshJs, skellyPike };
            _itemList4 = new Item[] { bigStick, thaeveBow, bigWand, bigShield, ironClub, healthPotion };
            _itemList5 = new Item[] { bigShield, smallShield, protectionBand, bigPotion, wompusGun };
        }

        /// <summary>
        /// Initalizes the enemies on start and restart of the game.
        /// </summary>
        private void InitializeEnemies()
        {
            // Initalizes the Stats for Little Dude.
            Entity littleDude = new Entity("A Little Dude", 26, 23, 18, 12, "A territorial creature that weilds a" +
                " small spear.");

            // Initalizes the Stats for Thaeve.
            Entity thaeve = new Entity("Thaeve", 24, 24, 15, 12, "These mischevious fiends scour towns in the" +
                " dead of the night to find anything they can scavenge.");

            // Initalizes the Stats for Wimpus.
            Entity wimpus = new Entity("Wimpus", 26, 26, 20, 15, "A juvenile wompus. They have hard shells that" +
                " they later shed once they mature.");

            // Initalizes the Stats for Mad Man.
            Entity madMan = new Entity("Mad Man", 22, 28, 16, 13, "Corrupted by something horrific.");

            // Initalizes the Stats for Durdle, the Great Turtle.
            Entity durdleTheTurtle = new Entity("Durdle, the Great Turtle", 35, 29, 32, 33, "The first real battle" +
                " you'll have. Durdle has high defense, so best bring a good weapon along.");

            // Initalizes the Stats for Wompus.
            Entity wompus = new Entity("Wompus", 33, 34, 25, 25, "Because of their short legs, they use their " +
                "burly arms to carry them around.");

            // Initalizes the Stats for Moneybag.
            Entity moneybag = new Entity("Moneybag", 22, 39, 21, 43, "A living moneybag, defeat it to get the gold" +
                " it holds.");

            // Initalizes the Stats for Big Dude.
            Entity bigDude = new Entity("A Big Dude", 31, 32, 27, 20, "The big brother of the little dudes. Despite" +
                " their similarities, it is unknown if the two are related.");

            // Initalizes the Stats for Faceless Horror.
            Entity facelessHorror = new Entity("Faceless Horror", 38, 33, 25, 22, "It stares without eyes.");

            // Initalizes the Stats for Skelly
            Entity skelly = new Entity("Skelly", 36, 31, 26, 24, "A fallen soldier of a long forgotten kingdom.");

            // Initalizes the Stats for Remnant of the World Eater.
            Entity remnant = new Entity("Remnant of the World Eater", 69, 45, 34, 53, "This is what is left after" +
                " an attack from the World Eater. Best to put it out of its misery.");

            // Initalizes the Stats for Spudette.
            Entity spudette = new Entity("Spudette", 42, 43, 32, 31, "A chonk.");

            // Initalizes the Stats for Wompus With a Gun.
            Entity wompusWithGun = new Entity("Wompus With a Gun", 44, 45, 28, 34, "Why does it have a gun?! Who gave" +
                " it a gun?! Who thought it would be a good idea to give these things guns?!");

            // Initalizes the Stats for A Big Ol' Dude.
            Entity bigOlDude = new Entity("A Big Ol' Dude", 48, 39, 27, 32, "The biggest of dudes, and the most" +
                " dangerous.");

            // Initalizes the Stats for Thwompus.
            Entity thwompus = new Entity("Thwompus", 38, 40, 35, 30, "When a wimpus can't shed its shell, it" +
                " is considered a thwompus.");

            // Initalizes the Stats for The Final Boss.
            Entity theFinalBoss = new Entity("Krazarackaradareda the World Eater", 96, 54, 43, 0, "This is it, the" +
                " World Eater. Each time it devours a world, it adds a syllable to its name.");

            // If the level is one...
            if (_currentLevel == 1)
            {
                // ...initalize the list for level one.
                _enemies = new Entity[] { littleDude, thaeve, wimpus, madMan, durdleTheTurtle }; ;

            }
            // If the level is two...
            else if (_currentLevel == 2)
            {
                // ...initalize the list for level two.
                _enemies = new Entity[] { wompus, moneybag, bigDude, facelessHorror, skelly, remnant };

            }
            // If the level is three...
            else if (_currentLevel == 3)
            {
                // ...initalize the list for level three.
                _enemies = new Entity[] { wompusWithGun, bigOlDude, thwompus, spudette, theFinalBoss };
            }

            int randomEnemy = _randomNumber.Next(_enemies.Length);

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

            // Saves the current level the player is on.
            writer.WriteLine(_currentLevel);

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

            // Sets the current level from the file. If it can't...
            if (!int.TryParse(reader.ReadLine(), out _currentLevel))
            {
                // ...it returns false.
                return false;
            }

            // Gets the current group of enemies that it should look at.
            InitializeEnemies();

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

            // Creates a new instance of the player.
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
            int choice = GetInput("Would you like to restart the game?", "Yes!", "No.", "Load Game.");
            // Finds out whether the _player wishes to...
            switch (choice)
            {
                // ...restart the game.
                case 0:
                    _currentScene = 0;
                    _currentLevel = 1;
                    InitializeEnemies();
                    break;
                // ...end the game.
                case 1:
                    _gameOver = true;
                    break;
                // ...load a previous save.
                case 2:
                    // Check to see if the load is successful...
                    if (Load())
                    {
                        // ...if it is, it returns the user to the battle.
                        Console.WriteLine("Load Succssesful");
                        Console.ReadKey(true);
                        Console.Clear();
                        _currentScene = Scene.BATTLE;
                    }
                    // If the load is not successful...
                    else
                    {
                        // ...it lets the player know the load failed, and sends them back to the start menu.
                        Console.WriteLine("Load Failed");
                        Console.ReadKey(true);
                        Console.Clear();
                        _currentScene = Scene.START_MENU;
                    }
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
                // This sends the user to name their character.
                case 0:
                    _currentScene = Scene.NAME_CREATION;
                    break;
                // This allows the player to load a previous save.
                case 1:
                    // Check to see if the load is successful...
                    if (Load())
                    {
                        // ...if it is, it returns the user to the battle.
                        Console.WriteLine("Load Succssesful");
                        Console.ReadKey(true);
                        Console.Clear();
                        _currentScene = Scene.BATTLE;
                    }
                    // If the load is not successful...
                    else
                    {
                        // ...it lets the player know the load failed, and sends them back to the start menu.
                        Console.WriteLine("Load Failed");
                        Console.ReadKey(true);
                        Console.Clear();
                        _currentScene = Scene.START_MENU;
                    }
                    break;
            }
        }

        /// <summary>
        /// Displays text asking for the player's name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            // Asks the user for their name.
            Console.Write("What is your name, adventurer? \n> ");
            _playerName = Console.ReadLine();
            Console.Clear();

            // Asks the user if they would like to keep their name.
            int choice = GetInput("Would you like to keep your name?", "Yes.", "No.");

            // Looks at the users choice, depending on what it is...
            switch (choice)
            {
                // ...it will send them to choose their class.
                case 0:
                    _currentScene = Scene.CHARACTER_SELECTION;
                    break;
                // ...or it will replay the event, allowing them to choose a new name.
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
                    _player = new Player(_playerName, 50, 22, 15, _offensiveInventory, "Offensive");
                    break;
                // ...or rely on defense more.
                case 1:
                    _player = new Player(_playerName, 50, 20, 16, _defensiveInventory, "Defensive");
                    break;
            }

            _currentScene = Scene.BATTLE_PREP;
        }

        /// <summary>
        /// Prints a character's stats to the console.
        /// </summary>
        /// <param name="character"> The character that will have its stats shown. </param>
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
        public void BattlePrep()
        {
            int choice = 0;

            // Checks to see if the enemy is one of the three bosses. If it is...
            if (_currentEnemy.Name == "Durdle, the Great Turtle" || 
                _currentEnemy.Name == "Remnant of the World Eater" ||
                _currentEnemy.Name == "Krazarackaradareda the World Eater")
            {
                // ...it asks if they would like to fight them or go to a different fight.
                choice = GetInput("You feel a great presence just ahead. Do you-", "Continue Ahead", "Turn Back");

                // Checks the users choice, depending on what they choose it...
                switch (choice)
                {
                    // ...lets them fight the boss.
                    case 0:
                        Console.WriteLine("You continue on ahead.");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    // ...sets the current enemy to something new, and goes back through battle prep.
                    case 1:
                        InitializeEnemies();
                        Console.WriteLine("You turn back, to return another day.");
                        Console.ReadKey(true);
                        Console.Clear();
                        _currentScene = Scene.BATTLE_PREP;
                        return;
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

            // Gives updates on the player and current enemy's stats.
            DisplayStats(_player);
            Console.WriteLine("");
            DisplayStats(_currentEnemy);
            Console.WriteLine("");

            int choice = GetInput(_currentEnemy.Name + " stands before you! What will you do?",
                "Attack!", "Equip/Use Item.", "Remove Current Item.", "Inspect", "Save.");
            // Finds out if the player wishes to...
            switch (choice)
            {
                // ...attack, dealing damage to the enemy. In turn taking damage from the enemy.
                case 0:
                    damageDealt = _player.Attack(_currentEnemy);
                    break;
                // ...go through the items they have and use or equip one.
                case 1:
                    DisplayEquipItemMenu();
                    return;
                // ...attempt to unequip the items they currently have on them.
                case 2:
                    if (!_player.TryUnequipItem())
                    {
                        Console.WriteLine("You don't have anything equipped.");
                    }
                    else
                    {
                        Console.WriteLine("You place your equipment in your bag.");
                    }
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                // ...view a description of the enemy.
                case 3:
                    Console.WriteLine(_currentEnemy.Description);
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                // ...save the game.
                case 4:
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
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle.
        /// </summary>
        void CheckBattleResults()
        {
            // If the _player is still alive and the enemy is dead, it moves on to the next fight.
            if (_currentEnemy.Health <= 0 && _player.Health > 0)
            {
                Console.WriteLine("You defeated " + _currentEnemy.Name + "!");
                Console.ReadKey(true);
                Console.Clear();

                // Adds the gold the enemy drops to the player's gold count.
                _player.GetGold(_currentEnemy);

                // If the player has not killed all the bosses, if they have not...
                if (!(_currentEnemy.Name == "Durdle, the Great Turtle") &&
                    !(_currentEnemy.Name == "Remnant of the World Eater") &&
                    !(_currentEnemy.Name == "Krazarackaradareda the World Eater") &&
                    !(_currentEnemy.Name == "[REDACTED]"))
                {
                    // ...ask the player if they wish to enter the shop.
                    EnterShop();
                }
                // If the player has killed Durdle or Remnant, the system will...
                else if(_currentEnemy.Name == "Durdle, the Great Turtle" || 
                    _currentEnemy.Name == "Remnant of the World Eater")
                {
                    // ...ask the player if they wish to enter the shop, then increment the current level.
                    EnterShop();
                    _currentLevel++;
                }
                // If the player has killed the final boss, it will...
                else if (_currentEnemy.Name == "Krazarackaradareda the World Eater")
                {
                    // ...commence the sequence for the true final battle.
                    Console.WriteLine("But you didn't think that was it, did you?");
                    Console.ReadKey(true);
                    Console.Clear();

                    Console.WriteLine("Thought you'd beat the monster, save the day?");
                    Console.ReadKey(true);
                    Console.Clear();

                    Console.WriteLine(_player.Name + ", you thought wrong.");
                    Console.ReadKey(true);
                    Console.Clear();

                    Console.WriteLine("The infection spreads.");
                    Console.ReadKey(true);
                    Console.Clear();

                    // Initalizes the Stats for [REDACTED].
                    _currentEnemy = new Entity("[REDACTED]", 123, 64, 40, 0, "There is nothing you can know.");
                    return;
                }
                // Check to see if the player has defeated [REDACTED].
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
            // Gets the player's choice for whether or not they wish to enter the shop.
            int choice = GetInput("You come across a shop in your travels. Do you enter?", "Yes.", "No.");

            // If they choose...
            switch (choice)
            {
                // ...to enter, they get a random shop that has a list of random items.
                case 0:
                    Console.WriteLine("You enter the shop.");
                    int randomShop = _randomNumber.Next(0, 5);

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
                        case 4:
                            _shop = new Shop(_itemList5);
                            break;
                    }
                    _currentScene = Scene.SHOP_MENU;
                    break;
                // ...they do not shop, and instead recieve this message.
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
                // ...to exit the shop.
                Console.WriteLine("Do come again, friend!");
                Console.ReadKey(true);
                Console.Clear();
                _currentScene = Scene.BATTLE_PREP;
            }
            // If the player picks...
            else if (!_shop.Sell(_player, choice))
            {
                // ...an item, it checks to see if the shop can sell. If it cannot, they recieve this message.
                Console.WriteLine("Very sorry. I do not go lower in my prices.");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }
}
