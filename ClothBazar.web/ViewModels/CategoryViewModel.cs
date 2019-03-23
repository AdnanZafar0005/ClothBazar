using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClothBazar.web.ViewModels
{
    public class NewCategoryViewModel
    {

        [Required]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }
        [ MaxLength(500)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        public bool isFeatured { get; set; }
        public int CategoryID { get; set; }
    }

    public class CategorySearchViewModel
    {
        public List<Category> Categories { get; set; }
        public string SearchTerm { get; set; }
        public int PageNo { get; set; }
        public Pager Pager { get; set; }


    }
    public class EditCategoryViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string ImageURL { get; set; }

        public bool isFeatured { get; set; }
    }

}