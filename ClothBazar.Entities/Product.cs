using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
   public class Product : BaseEntity
    {
        
        public virtual Category Category { get; set; } // To make a relationship with Category class 1 product
                                                       //will belong to each category
      public int CategoryID { get; set; }
        public string ImageURl { get; set; }

        [Range(1,1000000)]
        public decimal Price  { get; set; }

    }
}
