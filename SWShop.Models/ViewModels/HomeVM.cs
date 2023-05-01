using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWShop.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Product> ProductList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<Product> HotTrend { get; set; }
        public IEnumerable<Product> BestSeller { get; set; }
        public IEnumerable<Product> SuperSale { get; set; }
        public List<string> SizeList{ get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
