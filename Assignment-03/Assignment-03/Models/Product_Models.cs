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

    }

    public class AirConditioner:IProductFeatures
    {
        public string getDescription() => "I am an air conditioner";
        public string getType() => "Electronics";
        public int getPrice() => 1000;
        public int getQuantity() => 20;
    }

    public class Computer : IProductFeatures
    {
        public string getDescription() => "I am a desktop computer";
        public string getType() => "Electronics";
        public int getPrice() => 2000;
        public int getQuantity() => 10;
    }
    public class Fridge : IProductFeatures
    {
        public string getDescription() => "I am a room fridge";
        public string getType() => "Electronics";
        public int getPrice() => 2400;
        public int getQuantity() => 10;
    }
    public class DressShirt : IProductFeatures
    {
        public string getDescription() => "I am a dress shirt";
        public string getType() => "Clothing";
        public int getPrice() => 1400;
        public int getQuantity() => 5;
    }

    public class Coat : IProductFeatures
    {
        public string getDescription() => "I am a cozy coat";
        public string getType() => "Clothing";
        public int getPrice() => 1900;
        public int getQuantity() => 2;
    }
    public class EnglishBook : IProductFeatures
    {
        public string getDescription() => "I am an english book";
        public string getType() => "Books";
        public int getPrice() => 900;
        public int getQuantity() => 22;
    }
    public class MathBook : IProductFeatures
    {
        public string getDescription() => "I am a mathematics book";
        public string getType() => "Books";
        public int getPrice() => 900;
        public int getQuantity() => 12;
    }

    public class MangoGarden : IProductFeatures {

        public string getDescription() => "I am a mango garden";
        public string getType() => "Home Garden";
        public int getPrice() => 900000;
        public int getQuantity() => 2;


    }
    public class AppleGarden : IProductFeatures
    {

        public string getDescription() => "I am an apple garden";
        public string getType() => "Apple Garden";
        public int getPrice() => 1000000;
        public int getQuantity() => 1;


    }




    //*****************************************************************************

}
