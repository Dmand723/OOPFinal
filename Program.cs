using System.Security.Cryptography;

namespace OOPFinal
{
    /// <summary>
    /// Handles global utilities and color settings for the game.
    /// </summary>
    class Globals
    {
        public static Dictionary<string, ConsoleColor> colorKey = new Dictionary<string, ConsoleColor>()
        {
            {"menu",ConsoleColor.DarkCyan},
            {"error",ConsoleColor.Red},
            {"stats",ConsoleColor.Magenta},
            {"continue",ConsoleColor.Yellow},
            {"battleMenu",ConsoleColor.DarkBlue },
            {"good",ConsoleColor.Green},
            {"bad",ConsoleColor.DarkRed },
            {"neutral", ConsoleColor.DarkYellow },
        };
        static ConsoleColor[] rainbowColors = new ConsoleColor[]
        {
            ConsoleColor.Red,       
            ConsoleColor.DarkYellow,
            ConsoleColor.Yellow,    
            ConsoleColor.Green,     
            ConsoleColor.Blue,      
            ConsoleColor.DarkBlue,  
            ConsoleColor.Magenta    
        };
        public static void WriteColoredLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text, Console.ForegroundColor);
            Console.ResetColor();
        }
        public static void WriteRainbow(string text)
        {
            int colorIndex = 0;
            foreach (char c in text)
            {
                Console.ForegroundColor = rainbowColors[colorIndex];
                Console.Write(c);
                colorIndex++;
                if(colorIndex >= rainbowColors.Length)
                {
                    colorIndex = 0;
                }
            }
            Console.WriteLine("");
        }

    }
    class Game
    {
        #region Fields
        bool isRunning = false;
        Enemy[] enemyList = new Enemy[5];
        Wepon[] wepons = new Wepon[3];
        Player player;
        Enemy curEnemy;
        #endregion

        #region Entry Point
        static void Main(string[] args)
        {
            Game game = new Game();
            game.setup();
            game.isRunning = true;
            game.mainLoop();
        }
        #endregion

        #region Setup Methods
        void setup()
        {
            Globals.WriteColoredLine("Welcome to the Game!!!", Globals.colorKey["good"]);
            createPlayer();
            createEnemeys();
            createWepons();
        }

        void createPlayer()//Creates the player with an entered name and makes sure it's not null or empty
        {
            Globals.WriteColoredLine("Please enter a name", Globals.colorKey["good"]);
            string? name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Globals.WriteColoredLine("Please enter a name with at least one charter:", Globals.colorKey["error"]);
                createPlayer();
            }
            else
            {
                player = new Player(name);
            }
        }

        void createEnemeys()//Creates 5 enemys of random types and adds them to the enemyList
        {
            for (int i = 0; i < 5; i++)
            {
                int enemyType = RandomNumberGenerator.GetInt32(3);
                switch (enemyType)
                {
                    case 0:
                        enemyList[i] = new Spider();
                        break;
                    case 1:
                        enemyList[i] = new Snake();
                        break;
                    case 2:
                        enemyList[i] = new Bee();
                        break;
                }
            }
        }

        void createWepons()//Creates 3 wepnons and adds them to the wepons array
        {
            wepons[0] = new Knife();
            wepons[1] = new Bat();
            wepons[2] = new Pistol();
        }
        #endregion

        #region Main Loop and Menu
        void mainLoop()
        {
            while (isRunning)
            {
                pickOption();
            }
        }

        void pickOption()//Asks the player to enter a number 1-4 for the main menu options and call the corresponding method
        {
            Globals.WriteColoredLine("Pick An Option:\n" +
                "1:Continue On\n" +
                "2:Veiw Player Stats\n" +
                "3:Heal\n" +
                "4:Exit The Game", Globals.colorKey["menu"]);
            if (int.TryParse(Console.ReadLine(), out int selectedOption))
            {
                if (selectedOption > 0 && selectedOption <= 4)
                {
                    switch (selectedOption)
                    {
                        case 1:
                            continueOn();
                            Game.pressAny();
                            break;
                        case 2:
                            Console.Clear();
                            Globals.WriteColoredLine($"Player Stats:\n{player.getStats()}", Globals.colorKey["stats"]);
                            Game.pressAny();
                            break;
                        case 3:
                            player.Heal();
                            Game.pressAny();
                            break;
                        case 4:
                            Globals.WriteColoredLine("Goodbye come back soon!!", Globals.colorKey["neutral"]);
                            isRunning = false;
                            break;
                    }
                }
                else
                {
                    Globals.WriteColoredLine("Please enter a valid option(1-4)", Globals.colorKey["error"]);
                    pickOption();
                }
            }
            else
            {
                Globals.WriteColoredLine("Please enter a valid option(1-4)", Globals.colorKey["error"]);
                pickOption();
            }
        }

        static void pressAny()//Tells the player to press any key to continue in bettween main menu actions
        {
            Globals.WriteColoredLine("Press Any Key To Continue...", Globals.colorKey["continue"]);
            Console.ReadKey();
            Console.Clear();
        }
        #endregion

        #region Game Logic
        void continueOn()//The first option to continue down the path with a random chance to find a monster,
                         //come accross a health potion, or continue down the path with noting happening
        {
            if (enemyList.Length > 0)
            {
                int randint = RandomNumberGenerator.GetInt32(6);
                if (randint >= 0 && randint <= 2)
                {
                    int rantMonstInt = RandomNumberGenerator.GetInt32(enemyList.Length);
                    curEnemy = enemyList[rantMonstInt];
                    Globals.WriteColoredLine($"You've come accross a mosnster!!\n" +
                        $"monster type:{curEnemy.getName()} \nMosnter Current Health:{curEnemy.getCurHp()}", Globals.colorKey["bad"]);
                    Globals.WriteColoredLine($"Players Current Helth:{player.getCurHp()}", Globals.colorKey["stats"]);
                    pickBattleOption();
                }
                else if (randint == 3)
                {
                    player.addHealthPotion();
                    Globals.WriteColoredLine($"You came accross a health potion!!!\n" +
                        $"New potion ammount {player.getHealthPotionAmmount()}", Globals.colorKey["good"]);
                }
                else
                {
                    Globals.WriteColoredLine("You come accross noting and move on", Globals.colorKey["neutral"]);
                }
            }
        }

        void pickBattleOption()//Another menu that asks the player to choose a number 1-4
                               //on what to do when encontering a monster and calls the corrisponding methods.
        {
            Globals.WriteColoredLine("What Will You Do??\n" +
                "1:Attack\n" +
                "2:Block\n" +
                "3:Heal\n" +
                "4:Run Away", Globals.colorKey["battleMenu"]);
            if (int.TryParse(Console.ReadLine(), out int selectedOption))
            {
                if (selectedOption > 0 && selectedOption <= 4)
                {
                    bool willAttack;
                    int willAttackInt = RandomNumberGenerator.GetInt32(13);
                    willAttack = willAttackInt >= 3;
                    switch (selectedOption)
                    {
                        case 1:
                            attack(willAttack);
                            break;
                        case 2:
                            block(willAttack);
                            break;
                        case 3:
                            player.Heal();
                            pickBattleOption();
                            break;
                        case 4:
                            Globals.WriteColoredLine($"You ran away from the mosnster you might see it again...\n" +
                                $"Monsters health left: {curEnemy.getCurHp()}\n" +
                                $"Player Health left: {player.getCurHp()}", Globals.colorKey["stats"]);
                            break;
                    }
                }
            }
            else
            {
                Globals.WriteColoredLine("Please Select a Valid Option (1-4)", Globals.colorKey["error"]);
                pickBattleOption();
            }
        }

        void attack(bool willAttack)//Chooses a random wepon and a random chance to crit attack
                                    //and calulates damage bassed on that
        {
            Console.Clear();
            int attackDmg;
            int randWeponInt = RandomNumberGenerator.GetInt32(3);
            Wepon randWepon = wepons[randWeponInt];
            bool isCrit = player.checkForCrit(randWepon.getCritChance());
            if (isCrit)
            {
                attackDmg = player.baseAttackPower + randWepon.getAttackBonus() + 2;
            }
            else
            {
                attackDmg = player.baseAttackPower + randWepon.getAttackBonus();
            }
            player.Attack(curEnemy, attackDmg);
            if (isCrit)
            {
                Globals.WriteColoredLine($"You got a crit!!\n" +
                    $"You attacked with {randWepon.getName()}\n" +
                    $"You delt {attackDmg} Damage\n" +
                    $"Enemys Remaining Health:{curEnemy.getCurHp()}HP", Globals.colorKey["good"]);
            }
            else
            {
                Globals.WriteColoredLine($"You didn't crit :(\n" +
                    $"You attacked with {randWepon.getName()}\n" +
                    $"You delt {attackDmg} Damage\n" +
                    $"Enemys Remaining Health:{curEnemy.getCurHp()}HP", Globals.colorKey["neutral"]);
            }

            if (curEnemy.getCurHp() <= 0)
            {
                Globals.WriteColoredLine($"You defeted the {curEnemy.getName()}!!", Globals.colorKey["good"]);
                RemoveEnemy(curEnemy);
                if (enemyList.Length <= 0)
                {
                    gameWin();
                }
            }
            else
            {
                if (willAttack)
                {
                    curEnemy.Attack(player, curEnemy.getAttackPower());
                    Globals.WriteColoredLine($"The Enemy attacked you!!\n" +
                        $"It delt {curEnemy.getAttackPower()} Damage\n" +
                        $"Player current health {player.getCurHp()} HP", Globals.colorKey["bad"]);
                    if (player.getCurHp() <= 0)
                    {
                        Globals.WriteColoredLine("You have died game over:(", Globals.colorKey["bad"]);
                        isRunning = false;
                    }
                    else pickBattleOption();
                }
                else
                {
                    pickBattleOption();
                }
            }
        }

        void block(bool willAttack)//Blocks enemy's attack if attacks 
        {
            if (willAttack)
            {
                Globals.WriteColoredLine($"Good job you blocked an attack of {curEnemy.getAttackPower()} HP!!!", Globals.colorKey["good"]);
            }
            else
            {
                Globals.WriteColoredLine("The enemy didn't attack", Globals.colorKey["neutral"]);
            }
            pickBattleOption();
        }

        void RemoveEnemy(Enemy enemyToRemove)//Called after an enemy dies to remove it from the enemys list 
        {
            int indexToRemove = Array.IndexOf(enemyList, enemyToRemove);
            if (indexToRemove >= 0)
            {
                enemyList[indexToRemove] = null;
                Enemy[] newEnemyList = enemyList.Where(e => e != null).ToArray();
                enemyList = newEnemyList;
            }
        }

        void gameWin()//Called afeter all enemys has been defeted telling the player they have won
                      //and ends the game loop
        {
            Globals.WriteRainbow("You defeted all enemys you win!!!!");
            isRunning = false;
        }
        #endregion
    }
}