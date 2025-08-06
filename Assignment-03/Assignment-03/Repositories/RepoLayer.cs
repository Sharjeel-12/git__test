using System;
using Assignment_03.Factories;
using Assignment_03.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_03.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
    public interface IProductRepository : IRepository<Product>
    {
        Product GetByCategory(string category);
        List<Product> GetLowStockProducts(int threshold);
    }
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetOrdersByCustomer(string customerId);
        List<Order> GetOrdersByDateRange(DateTime start, DateTime end);
    }
}
