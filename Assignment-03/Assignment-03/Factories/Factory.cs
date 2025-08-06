using Assignment_03.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_03.Factories
{
    public abstract class Product
    {
        
        public void getProductInfo()
        {
            var product = createProduct();
            Console.WriteLine("The information of the poruct is as follows: -");
            Console.WriteLine($"Product Description: {product.getDescription()}");
            Console.WriteLine($"Product Type: {product.getType()}");
            Console.WriteLine($"Product Price: {product.getPrice()}");
            Console.WriteLine($"Product Quantity: {product.getQuantity()}");
            Console.WriteLine($"Product ID: {product.getID()}");
        }
        public abstract IProductFeatures createProduct();
    }

    public class Clothing : Product
    {
        public IProductFeatures Instance;
        public Clothing(string name)
        {
            string Name=name.ToLower();
            switch (Name)
            {
                case "coat": Instance = new Coat(); break;
                case "dress shirt": Instance = new DressShirt(); break;
             

            }
        }
        public override IProductFeatures createProduct() => Instance;        
    }
    public class Electronics : Product
    {
        public IProductFeatures Instance;
        public Electronics(string name)
        {
            string Name = name.ToLower();
            switch (Name)
            {
                case "fridge": Instance = new Fridge(); break;
                case "air conditioner": Instance = new AirConditioner(); break;
                case "computer": Instance = new Computer(); break;


            }
        }
        public override IProductFeatures createProduct() => Instance;
    }


    public class HomeGarden : Product
    {
        public IProductFeatures Instance;
        public HomeGarden(string name)
        {
            string Name = name.ToLower();
            switch (Name)
            {
                case "mango garden": Instance = new MangoGarden(); break;
                case "apple garden": Instance = new AppleGarden(); break;

            }
        }
        public override IProductFeatures createProduct() => Instance;
    }

    public class Book : Product
    {
        public IProductFeatures Instance;
        public Book(string name)
        {
            string Name = name.ToLower();
            switch (Name)
            {
                case "math book": Instance = new MathBook(); break;
                case "english book": Instance = new EnglishBook(); break;

            }
        }
        public override IProductFeatures createProduct() => Instance;
    }



}





