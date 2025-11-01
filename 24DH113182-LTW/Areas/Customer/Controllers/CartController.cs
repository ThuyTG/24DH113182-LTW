using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _24DH113182_LTW.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        // GET: Customer/Cart
        public ActionResult Index()
        {
            return View();
        }
    }
}