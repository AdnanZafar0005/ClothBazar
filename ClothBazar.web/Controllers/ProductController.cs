using ClothBazar.Entities;
using ClothBazar.Services;
using ClothBazar.web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ClothBazar.web.Controllers
{
    public class ProductController : Controller
    {
        //ProductsService productsService = new ProductsService();
       // CategoriesService categoriesService = new CategoriesService();




        // GET: Product
        ///////////////////////////////////////////////////////////////////////////////////////////// Index But Nothing
        public ActionResult Index()
        {
            return View();
        }
        #region ProductTable
        ///////////////////////////////////////////////////////////////////////////////////////////// List of products showing and searching


        public ActionResult ProductTable(string search,int? pageNo)
        {
            var pageSize = ConfigurationsService.Instance.PageSize();
            ProductSearchViewModel model = new ProductSearchViewModel();
            model.SearchTerm = search;

            pageNo = pageNo.HasValue ? pageNo.Value > 0? pageNo.Value : 1 : 1;



            var totalRecords = ProductsService.Instance.GetProductsCount(search);

            model.Products = ProductsService.Instance.GetProducts(search, pageNo.Value,pageSize);
            model.Pager = new Pager(totalRecords, pageNo, pageSize);
            
            return PartialView(model);
        }
        #endregion

        #region Creation
        /////////////////////////////////////////////////////////////////////////////////Category Create

        [HttpGet]
        public ActionResult Create()
        {
            //CategoriesService categoriesService;// = new CategoriesService();
            NewProductViewModel model = new NewProductViewModel();

            model.AvailableCategories = CategoriesService.Instance.GetAllCategories();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create(NewCategoryViewModel model)
        {
            
                var newProduct = new Product();
                newProduct.Name = model.Name;
                newProduct.Description = model.Description;
                newProduct.Price = model.Price;
                newProduct.Category = CategoriesService.Instance.GetCategory(model.CategoryID);
                newProduct.ImageURl = model.ImageURL;

                ProductsService.Instance.SaveProduct(newProduct);

                return RedirectToAction("ProductTable");
          
        }

        #endregion

        #region Updation
        /////////////////////////////////////////////////////////////////////////////////Proudct Edit on id based
        [HttpGet]
        public ActionResult Edit(int ID)
        {
            EditProductViewModel model = new EditProductViewModel();
            var product = ProductsService.Instance.GetProduct(ID);
            model.ID = product.ID;
            model.Name = product.Name;
            model.Description = product.Description;
            model.Price = product.Price;

            //model.CategoryID = product.Category != null ? product.Category.ID : 0;
            // model.CategoryID = product.Category != null ? product.Category.ID : 0;
           // model.CategoryID = product.Category != null ? product.CategoryID : 0;

            model.ImageURL = product.ImageURl;
            model.AvailableCategories = CategoriesService.Instance.GetAllCategories();


            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(EditProductViewModel model)
        {
    

            var existingProduct = ProductsService.Instance.GetProduct(model.ID);
            existingProduct.Name = model.Name;
            existingProduct.Description = model.Description;
            existingProduct.Price = model.Price;
            existingProduct.Category = null;
            existingProduct.CategoryID = model.CategoryID;

            //dont update imageURL if its empty
            if (!string.IsNullOrEmpty(model.ImageURL))
            {
                existingProduct.ImageURl = model.ImageURL;
            }




            ProductsService.Instance.UpdateProducts(existingProduct);

            return RedirectToAction("ProductTable");
}
        #endregion


        #region Deletion
        //////////////////////////////////////////////////////////////////////////////////     Delete Product throw id
        [HttpPost]
        public ActionResult Delete (int ID)
        {
            ProductsService.Instance.DeleteProduct(ID);

            return RedirectToAction("ProductTable");
        }
        #endregion


        [HttpGet]
        public ActionResult Details(int ID)
        {
            ProductViewModel model = new ProductViewModel();
            model.Product = ProductsService.Instance.GetProduct(ID);
            if (model.Product == null)

            {
                return HttpNotFound();
                    }


            return View(model);
        }


    }
}