using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPFinal
{
    public class Wepon
    {
        string name = "";
        int attackBonus;
        int critChance;
        public Wepon(string name, int attackBonus,int critChance)
        {
            this.name = name;
            this.attackBonus = attackBonus;
            this.critChance = critChance;
        } 
        public string getName()
        {
            return name;    
        }
        public int getAttackBonus()
        {
            return attackBonus;
        }
        public int getCritChance()
        {
            return critChance;
        }

    }
    public class Knife : Wepon
    {
        public Knife() : base("Knife", 4,5)
        {

        }
    }
    public class Bat : Wepon
    {
        public Bat() : base("Bat", 3,7)
        {

        }
    }
    public class Pistol : Wepon
    {
        public Pistol() : base("Pistol", 10,2)
        {

        }
    }
}
