using ClothBazar.Database;
using ClothBazar.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ClothBazar.Services
{
    public class ProductsService
    {
        #region Singleton
        public static ProductsService Instance
        {
            get
            {
                if (instance == null)
                    instance = new ProductsService();
                return instance;
            }

        }
        private static ProductsService instance { get; set; }

   

        private ProductsService()
        {
        }


        #endregion

        public List<Product> SearchProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy, int pageNo,int pageSize)
        {

            using (var context = new CBContext())
            {
                var products = context.Products.ToList();
                if(categoryID.HasValue)
                {
                    products = products.Where(x => x.CategoryID == categoryID.Value).ToList();

                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();

                }
                if(minimumPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }

                if(sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;

                        default:
                            products = products.OrderByDescending(x => x.ID).ToList();

                            break;
                    }
                }

                return products.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            }
        }

        public int SearchProductsCount(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy)
        {

            using (var context = new CBContext())
            {
                var products = context.Products.ToList();
                if (categoryID.HasValue)
                {
                    products = products.Where(x => x.CategoryID == categoryID.Value).ToList();

                }

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower())).ToList();

                }
                if (minimumPrice.HasValue)
                {
                    products = products.Where(x => x.Price >= minimumPrice.Value).ToList();
                }

                if (maximumPrice.HasValue)
                {
                    products = products.Where(x => x.Price <= maximumPrice.Value).ToList();
                }

                if (sortBy.HasValue)
                {
                    switch (sortBy.Value)
                    {
                        case 2:
                            products = products.OrderByDescending(x => x.ID).ToList();
                            break;
                        case 3:
                            products = products.OrderBy(x => x.Price).ToList();
                            break;
                        case 4:
                            products = products.OrderByDescending(x => x.Price).ToList();
                            break;

                        default:
                            products = products.OrderByDescending(x => x.ID).ToList();

                            break;
                    }
                }

                return products.Count;

            }
        }


        ///////////////////////////////////////////////////////////////////////// Send sigle Product id to Server and find
        public Product GetProduct(int ID)
        {
            using (var context = new CBContext())
            {
                return context.Products.Where(x => x.ID == ID).Include(x => x.Category).FirstOrDefault();

                // return context.Products.Find(ID);
                //return context.Products.Where(x => x.ID == ID).Include(x => x.Category).FirstOrDefault();
            }
        }
        ///////////////////////////////////////////////////////////////////////// Send sigle Product id List to Server and find
        
        public List<Product> GetProducts(List<int> IDs)
        {
            using (var context = new CBContext())
            {
                return context.Products.Where(product => IDs.Contains(product.ID)).ToList();
            }
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////// Pagination
        /// </summary>
        /// <returns></returns>
        /// 
        public List<Product> GetProducts(string search, int pageNo, int pageSize)
        {
            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products
                        .Where(product => product.Name != null && product.Name.ToLower().Contains(search.ToLower()))
                        .OrderBy(x => x.ID)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Category)
                        .ToList();
 }
                else
                {
                    return context.Products
                        .OrderBy(x => x.ID)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Include(x => x.Category)
                        .ToList();

                }
            }
        }




        public int GetMaximumPrice()
        {
            using (var context = new CBContext())
            {
                return (int)(context.Products.Max(x => x.Price));

            }
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////Send Multiple Product id to Server and show in list
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts(int pageNo)
        {
            //var context = new CBContext();
            //return context.Products.ToList();
            int pageSize = 5;// int.Parse(ConfigurationsService.Instance.GetConfig("ListingPageSize").Value());

            using (var context = new CBContext())
            {
                return context.Products.OrderBy(x=>x.ID).Skip((pageNo-1)*pageSize).Take(pageSize).Include(x=>x.Category).ToList();
                //return context.Products.Include(x => x.Category).ToList();

            }
        }

        public List<Product> GetProducts(int pageNo,int pageSize)
        {

            using (var context = new CBContext())
            {
                return context.Products.OrderByDescending(x => x.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).Include(x => x.Category).ToList();

            }
        }

        public int GetProductsCount(string search)
        {
            using (var context = new CBContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    return context.Products
                        .Where(product => product.Name != null && product.Name.ToLower().Contains(search.ToLower()))
                      .Count();
                }
                else
                {
                    return context.Products
                      .Where(product => product.Name != null && product.Name.ToLower().Contains(search.ToLower()))
                      .Count();
                }
            }
        }




        public List<Product> GetProductsByCategory (int categoryID, int pageSize)
        {

            using (var context = new CBContext())
            {
                return context.Products.Where(x=>x.Category.ID==categoryID).OrderByDescending(x => x.ID).Take(pageSize).Include(x => x.Category).ToList();

            }
        }


        public List<Product> GetLatestProducts(int numberOfProducts)
        {
            using (var context = new CBContext())
            {
                return context.Products.OrderByDescending(x => x.ID).Take(numberOfProducts).Include(x => x.Category).ToList();

            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////////////////////  Save Products 

        public void SaveProduct(Product product)
        {
            using (var context = new CBContext())
            {
                context.Entry(product.Category).State = System.Data.Entity.EntityState.Unchanged;

                context.Products.Add(product);
                context.SaveChanges();
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////// Update Products Single based on id
        public void UpdateProducts(Product product)
        {
            using (var context = new CBContext())
            {
                
                context.Entry(product).State = System.Data.Entity.EntityState.Modified;
                
               context.SaveChanges();

               
                
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////// Delete Products Single based on id
        public void DeleteProduct(int ID)
        {
            using (var context = new CBContext())
            {
                // context.Entry(category).State = System.Data.Entity.EntityState.Deleted;
                var product = context.Products.Find(ID);
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }




    }
}
