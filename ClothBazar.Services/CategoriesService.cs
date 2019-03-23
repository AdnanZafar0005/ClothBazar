using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Services
{
    public class CategoriesService
    {

        #region Singleton
        public static CategoriesService Instance
        {
            get
            {
                if (instance == null)
                    instance = new CategoriesService();
                return instance;
            }

        }
        private static CategoriesService instance { get; set; }

        private CategoriesService()
        {
        }
        #endregion


        public Category GetCategory(int ID)
        {
            using (var context = new CBContext())
            {
                return context.Categories.Find(ID);
            }
        }

        

        public List<Category> GetAllCategories()
        {
            using (var context = new CBContext())
            {
                return context.Categories.ToList();
            }
        }

        public List<Category> GetFeaturedCategories()
        {
            using (var context = new CBContext())
            {
                return context.Categories.Where(x => x.isFeatured && x.ImageURl != null).ToList();
            }
        }


        public void SaveCategory(Category category)
        {
            using (var context = new CBContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var context = new CBContext())
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCategory(int ID)
        {
            using (var context = new CBContext())
            {
                var category = context.Categories.Where(x => x.ID == ID).Include(x => x.Products).FirstOrDefault();

                context.Products.RemoveRange(category.Products); //first delete products of this category
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }


        /////////////////////////////////////////////////////////////////////// Send sigle category id to Server and find

        /// /////////////////////////////////////////////////////////////////////Send Multiple category id to Server and show in list

        public int GetCategoriesCount(string search)
        {
            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Categories
                        .Where(category => category.Name != null && category.Name.ToLower().Contains(search.ToLower())).Count();



                }
                else
                {
                    return context.Categories.Count();


                }
            }
        }

        /// /////////////////////////////////////////////////////////////////////Send Multiple category id to Server and show in list

        public List<Category> GetCategories(string search, int pageNo)
        {
            int pageSize = 3;// int.Parse(ConfigurationsService.Instance.GetConfig("ListingPageSize").Value());
            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Categories
                        .Where(category => category.Name != null && category.Name.ToLower().Contains(search.ToLower()))
                        .OrderBy(x => x.ID)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Products)
                        .ToList();
                        
                       


                }
                else
                {
                    return context.Categories
                        .OrderBy(x => x.ID)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Products)
                        .ToList();

                }
            }
        }
      

        /////////////////////////////////////////////////////////////////////////////////////////////////// Update Category Single based on id
        
        



    }
}
