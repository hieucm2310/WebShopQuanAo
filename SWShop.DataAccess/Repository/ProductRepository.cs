using Microsoft.EntityFrameworkCore;
using SWShop.DataAccess.Data;
using SWShop.DataAccess.Repository.IRepository;
using SWShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWShop.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _db.Products.ToListAsync();
        }
        public async Task<List<Product>> GetProductsAsync(string searchTerm, int pageNumber, int pageSize, int? category, decimal? minPrice = null, decimal? maxPrice = null, string? size = null, string? includeProperties = null)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm) || p.Brand.Contains(searchTerm));
            }

            if (category.HasValue)
            {
                query = query.Where(p => p.CategoryId == category);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value || p.SalePrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value || (p.SalePrice <= maxPrice.Value && p.SalePrice != 0));
            }

            if (!string.IsNullOrEmpty(size))
            {
                var listSize = size.Split(',');
                query = query.Where(p => p.Sizes.Any(s => listSize.Contains(s.Name)));
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<int> GetProductsCountAsync(string searchTerm, int pageNumber, int pageSize, int? category, decimal? minPrice = null, decimal? maxPrice = null, string? size = null, string? includeProperties = null)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm) || p.Brand.Contains(searchTerm));
            }

            if (category.HasValue)
            {
                query = query.Where(p => p.CategoryId == category);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value || p.SalePrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value || (p.SalePrice <= maxPrice.Value && p.SalePrice != 0));
            }

            if (!string.IsNullOrEmpty(size))
            {
                var listSize = size.Split(',');
                query = query.Where(p => p.Sizes.Any(s => listSize.Contains(s.Name) ));
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return await query.CountAsync();
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Brand = obj.Brand;
                objFromDb.Price = obj.Price;
                objFromDb.SalePrice = obj.SalePrice;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.QuantityRemain = obj.QuantityRemain;
                objFromDb.ProductImages = obj.ProductImages;
            }
        }
    }
}
