using Microsoft.EntityFrameworkCore;
using SWShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWShop.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<List<Product>> GetProductsAsync();

        public Task<List<Product>> GetProductsAsync(string searchTerm, int pageNumber, int pageSize, int? category, decimal? minPrice = null, decimal? maxPrice = null, string? sizeId = null, string? includeProperties = null);
        public Task<int> GetProductsCountAsync(string searchTerm, int pageNumber, int pageSize, int? category, decimal? minPrice = null, decimal? maxPrice = null, string? sizeId = null, string? includeProperties = null);
        void Update(Product obj);
    }
}
