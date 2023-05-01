﻿using SWShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWShop.DataAccess.Repository.IRepository
{
    public interface IRateRepository : IRepository<Rate>
    {
        void Update(Rate obj);
    }
}
