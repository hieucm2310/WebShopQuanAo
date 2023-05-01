using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SWShop.Models
{
    public class Rate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Chọn số sao bạn muốn đánh giá cho sản phẩm.")]
        public int Star { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }=String.Empty;
        public string Comment { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now; 
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public string TimeRate { get; set; }
    }
}
