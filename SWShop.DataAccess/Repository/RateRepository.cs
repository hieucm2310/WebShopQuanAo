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
    public class RateRepository : Repository<Rate>, IRateRepository
    {
        private ApplicationDbContext _db;
        public RateRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Rate obj)
        {
            _db.Rates.Update(obj);
        }
    }
}
