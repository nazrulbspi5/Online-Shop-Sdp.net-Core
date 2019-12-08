using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OnlineShop.Models
{
    public class Products
    {
        
        public int Id { get; set; }
        [Required]
        //[Remote("IsProductNameExists", "Products", HttpMethod = "POST",ErrorMessage = "Product name already exists.")]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        [DisplayName("Product Color")]
        public string ProductColor { get; set; }
        [Required]
        [DisplayName("Available")]
        public bool IsAvailable { get; set; }
        [Required]
        [DisplayName("Product Type")]
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public ProductTypes ProductTypes { get; set; }
        [Required]
        [DisplayName("Special Tag")]
        public int SpeciaTagId { get; set; }
        [ForeignKey("SpeciaTagId")]
        public SpecialTag SpecialTag { get; set; }

    }
}
