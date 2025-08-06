using Assignment_03.Factories;
using Assignment_03.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_03.Models
{
    public class ProductManager : IProductRepository
    {
        public static List<Product> orders = new List<Product>();

        public async Task<Order> GetByIdAsync(string id)
        {
            var filtered = orders.Where(order => order.Order_ID == id).ToList();
            if (filtered.Count == 0)
            {
                Console.WriteLine("No such product found");
                return null;
            }
            return await Task.FromResult(filtered[0]);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            if (orders.Count == 0)
            {
                Console.WriteLine("No order has been placed yet");
            }
            return await Task.FromResult(orders.AsEnumerable());
        }

        public async Task AddAsync(Order order)
        {
            orders.Add(order);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Order order)
        {
            var existingOrder = orders.FirstOrDefault(o => o.Order_ID == order.Order_ID);
            if (existingOrder != null)
            {
                Console.WriteLine("Enter the new Order details : -");
                Console.Write("New Order ID: ");
                existingOrder.Order_ID = Console.ReadLine();

                Console.Write("New Customer ID: ");
                existingOrder.CustomerID = Console.ReadLine();

                existingOrder.DateTimeStamp = DateTime.Now;
            }
            else
            {
                Console.WriteLine("Order not found!");
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(string id)
        {
            var filtered = orders.Where(order => order.Order_ID == id).ToList();

            if (filtered.Count == 0)
            {
                Console.WriteLine("No such record found!");
            }
            else
            {
                orders.Remove(filtered[0]);
            }
            await Task.CompletedTask;
        }

        public List<Order> GetOrdersByCustomer(string customerId)
        {
            var filtered = orders.Where(order => order.CustomerID == customerId).ToList();
            if (filtered.Count == 0)
            {
                Console.WriteLine("No such record found!");
            }
            return filtered;
        }

        public List<Order> GetOrdersByDateRange(DateTime start, DateTime end)
        {
            var filtered = orders.Where(order => order.DateTimeStamp >= start && order.DateTimeStamp <= end).ToList();
            if (filtered.Count == 0)
            {
                Console.WriteLine("No such record found!");
            }
            return filtered;
        }
    }
}
