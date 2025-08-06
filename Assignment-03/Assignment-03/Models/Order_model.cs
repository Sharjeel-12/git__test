using Assignment_03.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment_03.Factories;
namespace Assignment_03.Models
{
    public class Order
    {
        public  string Order_ID;
        public  string CustomerID;
        public DateTime DateTimeStamp;
        public Product AddedProduct;
        public Order(string orderID, string customerID,DateTime date, Product product)
        {
            Order_ID = orderID;
            CustomerID = customerID;
            DateTimeStamp = DateTime.Now;
            AddedProduct = product; 
        }

    }
}
