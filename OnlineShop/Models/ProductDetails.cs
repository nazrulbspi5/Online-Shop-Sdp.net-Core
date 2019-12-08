using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class ProductDetails
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        [ForeignKey("ProductMaster")]
        public int ProductMasterID { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
    }
}
