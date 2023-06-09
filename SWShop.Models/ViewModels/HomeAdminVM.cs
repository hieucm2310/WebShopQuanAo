﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWShop.Models.ViewModels
{
    public class HomeAdminVM
    {
        public IEnumerable<Rate> RateList { get; set; }
        public int VNPayP { get; set; }
        public decimal VNPay { get; set; }
        public int CODP { get; set; }
        public decimal COD { get; set; }
        public decimal Total { get; set; }
    }
}
