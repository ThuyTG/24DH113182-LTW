using _24DH113182_LTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _24DH113182_LTW.Models.ViewModel;
using PagedList;
using System.Net;
namespace _24DH113182_LTW.Areas.Customer.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Customer/Home
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                products = products.Where(p => p.ProductName.Contains(searchTerm) ||
                                               p.ProductDescription.Contains(searchTerm) || 
                                               p.Category.CategoryName.Contains(searchTerm));
            }
            int pageNumber = page ?? 1;
            int pageSize = 6;

            // Lấy 10 sản phẩm nổi bật
            model.FeaturedProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(10).ToList();

            // Lấy 20 sản phẩm bán chậm nhất
            model.NewProducts = products.OrderBy(p => p.OrderDetails.Count()).Take(20).ToPagedList(pageNumber, pageSize);
            return View(model);
        }
        public ActionResult ProductDetail(int? id, int? quantity, int? page)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro = db.Products.Find(id);
            if(pro == null)
            {
                return HttpNotFound();
            }
            // Lấy sản phẩm cùng danh mục
            var products = db.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();
            ProductDetailVM model = new ProductDetailVM();

            // Lấy ra số trang hiện tại
            int pageNumber = page ?? 1;
            int pageSize = model.PageSize;
            model.product = pro;
            model.RelatedProducts = products.OrderBy(p => p.ProductID).Take(8).ToList();
            model.TopProducts = products.OrderByDescending(p => p.OrderDetails.Count()).Take(8).ToPagedList(pageNumber, pageSize);

            if (quantity.HasValue)
            {
                model.quantity = quantity.Value;
            }

            return View(model);
        }
        public ActionResult ProductList()
        {
            return View();
        }
    }
}