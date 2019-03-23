using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClothBazar.Services;
using ClothBazar.web.Code;
using ClothBazar.web.ViewModels;

namespace ClothBazar.web.Controllers
{
    //[Authorize]
    public class ShopController : Controller
    {
        public ActionResult Index(string searchTerm, int? minimumPrice,int? maximumPrice,int? categoryID, int? sortBy, int? pageNo)
        {
            var pageSize = ConfigurationsService.Instance.ShopPageSize();
            ShopViewModel model = new ShopViewModel();
            model.SearchTerm = searchTerm;


            model.FeaturedCategories = CategoriesService.Instance.GetFeaturedCategories();

            model.MaximumPrice = ProductsService.Instance.GetMaximumPrice();

            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            model.sortBy = sortBy;

            model.CategoryID = categoryID;
            int totalCount = ProductsService.Instance.SearchProductsCount(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy);


            model.Products = ProductsService.Instance.SearchProducts(searchTerm,minimumPrice,maximumPrice,categoryID,sortBy,pageNo.Value, pageSize);
            model.Pager = new Pager(totalCount,pageNo, pageSize);
            
            return View(model);
        }


        public ActionResult FilterProducts(string searchTerm, int? minimumPrice, int? maximumPrice, int? categoryID, int? sortBy,int? pageNo)
        {
            var pageSize = ConfigurationsService.Instance.ShopPageSize();

            FilterProductsViewModel model = new FilterProductsViewModel();
            pageNo = pageNo.HasValue ? pageNo.Value > 0 ? pageNo.Value : 1 : 1;

            model.SearchTerm = searchTerm;
            model.SortBy = sortBy;
            model.CategoryID = categoryID;
            int totalCount = ProductsService.Instance.SearchProductsCount(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy);


            model.Products = ProductsService.Instance.SearchProducts(searchTerm, minimumPrice, maximumPrice, categoryID, sortBy, pageNo.Value, pageSize);
            model.Pager = new Pager(totalCount, pageNo, pageSize);

            return PartialView(model);
        }



        // ProductsService productsService = new ProductsService();
        public ActionResult Checkout()
        {
            CheckoutViewModels model = new CheckoutViewModels();


            var CartProductsCookie = Request.Cookies["CartProducts"];
            if (CartProductsCookie != null)
            {
                //var productIDs = CartProductsCookie.Value;
                //var ids = productIDs.Split('-');
                //List<int> pIDs = ids.Select(x => int.Parse(x)).ToList();

                model.CartProductIDs = CartProductsCookie.Value.Split('-').Select(x => int.Parse(x)).ToList();
               model.CartProducts = ProductsService.Instance.GetProducts(model.CartProductIDs);
            }
            return View(model);
        }
    }
}