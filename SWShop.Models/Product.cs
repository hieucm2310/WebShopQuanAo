using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SWShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        [Display(Name = "Giá niêm yết")]
        [Range(1000, 1000000000)]
        public decimal ListPrice { get; set; }

        [Required]
        [Display(Name = "Giá hiện tại")]
        [Range(1, 1000000000)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Giá khuyến mãi")]
        [Range(0, 1000000000)]
        public decimal SalePrice { get; set; }

        [Required]
        public int QuantitySold { get; set; }
        [NotMapped]
        public int QuantityRemain { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public ICollection<Size> Sizes { get; set; }
        [ValidateNever]
        public ICollection<Like> Likes { get; set; }
        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public bool IsLike { get; set; } = false; 
        [NotMapped]
        public int Star { get; set; } = 0;
        [NotMapped]
        public int RateCount { get; set; }
        [NotMapped]
        [ValidateNever]
        public string SizeSelect { get; set; }
    }
}
