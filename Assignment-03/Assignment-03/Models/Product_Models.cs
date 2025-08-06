using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_03.Models
{
    public interface IProductFeatures
    {
        string getDescription();
        string getType();
        int getPrice();
        int getQuantity();
        string getID();

    }

    public class AirConditioner:IProductFeatures
    {   public string ID ;
        public static int ID_index=0;
        public AirConditioner()
        {
         ID_index++;
            ID = $"Air_Conditioner_{ID_index}";
        }
        public string getDescription() => "I am an air conditioner";
        public string getType() => "Electronics";
        public int getPrice() => 1000;
        public int getQuantity() => 20;
        public string getID() => ID;
    }

    public class Computer : IProductFeatures
    {
        public string ID;
        public static int ID_index = 0;
        public Computer()
        {
            ID_index++;
            ID = $"Computer_{ID_index}";
        }
        public string getDescription() => "I am a desktop computer";
        public string getType() => "Electronics";
        public int getPrice() => 2000;
        public int getQuantity() => 10;
        public string getID() => ID;

    }
    public class Fridge : IProductFeatures
    {
        public string ID;
        public static int ID_index = 0;
        public Fridge()
        {
            ID_index++;
            ID = $"Fridge_{ID_index}";
        }
        public string getDescription() => "I am a room fridge";
        public string getType() => "Electronics";
        public int getPrice() => 2400;
        public int getQuantity() => 10;
        public string getID() => ID;

    }
    public class DressShirt : IProductFeatures
    {
        public string ID;
        public static int ID_index = 0;
        public DressShirt()
        {
            ID_index++;
            ID = $"Dress_Shirt_{ID_index}";
        }
        public string getDescription() => "I am a dress shirt";
        public string getType() => "Clothing";
        public int getPrice() => 1400;
        public int getQuantity() => 5;
        public string getID() => ID;

    }

    public class Coat : IProductFeatures
    {
        public string ID;
        public static int ID_index = 0;
        public Coat()
        {
            ID_index++;
            ID = $"Coat_{ID_index}";
        }
        public string getDescription() => "I am a cozy coat";
        public string getType() => "Clothing";
        public int getPrice() => 1900;
        public int getQuantity() => 2;
        public string getID() => ID;

    }
    public class EnglishBook : IProductFeatures
    {
        public string ID;
        public static int ID_index = 0;
        public EnglishBook()
        {
            ID_index++;
            ID = $"English_Book_{ID_index}";
        }
        public string getDescription() => "I am an english book";
        public string getType() => "Books";
        public int getPrice() => 900;
        public int getQuantity() => 22;
        public string getID() => ID;

    }
    public class MathBook : IProductFeatures
    {
        public string ID;
        public static int ID_index = 0;
        public MathBook()
        {
            ID_index++;
            ID = $"Math_Book_{ID_index}";
        }
        public string getDescription() => "I am a mathematics book";
        public string getType() => "Books";
        public int getPrice() => 900;
        public int getQuantity() => 12;
        public string getID() => ID;

    }

    public class MangoGarden : IProductFeatures {
        public string ID;
        public static int ID_index = 0;
        public MangoGarden()
        {
            ID_index++;
            ID = $"Mango_Garden_{ID_index}";
        }
        public string getDescription() => "I am a mango garden";
        public string getType() => "Home Garden";
        public int getPrice() => 900000;
        public int getQuantity() => 2;
        public string getID() => ID;



    }
    public class AppleGarden : IProductFeatures
    {
        public string ID;
        public static int ID_index = 0;
        public AppleGarden()
        {
            ID_index++;
            ID = $"Apple_Garden_{ID_index}";
        }
        public string getDescription() => "I am an apple garden";
        public string getType() => "Apple Garden";
        public int getPrice() => 1000000;
        public int getQuantity() => 1;
        public string getID() => ID;



    }




    //*****************************************************************************

}
