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
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        private ApplicationDbContext _db;
        public LikeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Like obj)
        {
            _db.Likes.Update(obj);
        }
    }
}
