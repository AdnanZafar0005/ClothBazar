using ClothBazar.Services;
using ClothBazar.web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClothBazar.web.Controllers
{
    public class WidgetsController : Controller
    {
        // GET: Widgets
        public ActionResult Products(bool isLatestProducts, int? CategoryID = 0)
        {
            ProductsWidgetViewModels model = new ProductsWidgetViewModels();
            model.IsLatestProducts = isLatestProducts;
            if(isLatestProducts)
            {
                model.Products = ProductsService.Instance.GetLatestProducts(4);
            }
            else if(CategoryID.HasValue && CategoryID.Value > 0)
            {
                model.Products = ProductsService.Instance.GetProductsByCategory(CategoryID.Value,4); ;
            }
            else
            {
                model.Products = ProductsService.Instance.GetProducts(1,8);

            }

            return PartialView(model);
        }
    }
}