using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OOPFinal
{
    class Character
    {   
        internal int maxHealth;
        internal int curHp;
        internal string Name = "";
        internal int baseAttackPower;
        public Character(string name,int maxHp,int baseAttackPower)
        {
            this.Name = name;
            this.maxHealth = maxHp;
            this.curHp = maxHealth;
            this.baseAttackPower = baseAttackPower;
            
        }
        public void Attack(Character toAttack,int amountToAttack)
        {
            toAttack.takeDamage(amountToAttack);
        }
        void takeDamage(int ammount)
        {
            curHp -= ammount;
            if(curHp < 0) curHp = 0;
        }
        public int getAttackPower()
        {
            return baseAttackPower;
        }
        public string getName()
        {
            return Name;
        }
        public int getCurHp()
        {
            return curHp;
        }
    }
    class Enemy : Character
    {

        public Enemy(string name, int maxHp, int baseAttackPower) : base(name, maxHp, baseAttackPower)
        {

        }

    }
    class Player : Character
    {
        int heathPotionAmmount = 0;
        public Player(string name) : base(name,200,12)
        {
            
        }
        public void Heal()
        {
            if (heathPotionAmmount > 0)
            {
                if (curHp < maxHealth)
                {
                    curHp += 20;
                    if (curHp > maxHealth)
                    {
                        curHp = maxHealth;
                    }
                    heathPotionAmmount--;
                    Globals.WriteColoredLine($"You healed 20HP!!\n" +
                        $"New health {curHp}", Globals.colorKey["good"]);
                }
                else
                {
                    Globals.WriteColoredLine("You are at max health can not heal", Globals.colorKey["neutral"]);
                }
            }
            else
            {
                Globals.WriteColoredLine("Can not heal out of health potions", Globals.colorKey["error"]); 
            }
        }
        public bool checkForCrit(int weponCritChance)
        {
            int randint = RandomNumberGenerator.GetInt32(21);
            if(randint <= weponCritChance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string getStats()
        {
            return $"Player's Name: {Name}\n" +
                $"Curent Health:{curHp}\n" +
                $"Attack Power:{baseAttackPower}\n" +
                $"Health Potions:{heathPotionAmmount}";
        }
        public void addHealthPotion()
        {
            heathPotionAmmount++;
        }
        public int getHealthPotionAmmount()
        {
            return heathPotionAmmount;
        }
    }
    
    class Spider : Enemy
    {

        public Spider() : base("Spider", 150, 20)
        {

        }
    }
    class Snake : Enemy
    {
        public Snake() : base("Snake",100,13)
        {

        }
    }
    class Bee : Enemy
    {
        public Bee() : base("Bee",50,10)
        {

        }
    }

}
