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
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        private ApplicationDbContext _db;
        public SizeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Size obj)
        {
            _db.Sizes.Update(obj);
        }
    }
}
