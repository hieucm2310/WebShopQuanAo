using SWShop.DataAccess.Data;
using SWShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category {get;private set;}
        public IProductRepository Product {get;private set;}
        public IShoppingCartRepository ShoppingCart {get;private set;}
        public IApplicationUserRepository ApplicationUser {get;private set;}
        public IOrderHeaderRepository OrderHeader { get;private set;}
        public IOrderDetailRepository OrderDetail {get;private set;}
        public IProductImageRepository ProductImage { get;private set;}
        public IRateRepository Rate { get; private set; }
        public ILikeRepository Like { get; private set; }
        public ISizeRepository Size { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            ProductImage = new ProductImageRepository(_db);
            Rate = new RateRepository(_db);
            Like = new LikeRepository(_db);
            Size = new SizeRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
