using _24DH113182_LTW.Models;
using _24DH113182_LTW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _24DH113182_LTW.Areas.Customer.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Customer/Order
        public ActionResult Index()
        {
            return View();
        }

        // GET: Customer/Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            if(customer == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = new CheckoutVM
            {
                CartItems = cart,
                TotalAmount = cart.Sum(item => item.TotalPrice),
                OrderDate = DateTime.Now,
                ShippingAddress = customer.CustomerAddress,
                CustomerID = customer.CustomerID,
                Username = customer.Username,
            };
            return View(model);
        }

        // POST: Customer/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            if(ModelState.IsValid)
            {
                var cart = Session["Cart"] as List<CartItem>;
                if(cart == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var user = db.Users.SingleOrDefault(u => u.Username== User.Identity.Name);
                if (user == null) return RedirectToAction("Login", "Account");
                var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                if(customer == null) return RedirectToAction("Login", "Account");

                // Nếu người dùng chọn PayPal chuyển hướng trang thanh toán đến PaymentWithPaypal
                if (model.PaymentMethod == "Paypal") return RedirectToAction("PaymentWithPaypal", "PayPal", model);


                // Đặt trạng thái đơn hàng
                string paymentStatus = "Chưa thanh toán";
                switch(model.PaymentMethod)
                {
                    case "Tiền mặt":
                        paymentStatus = "Thanh toán thành công"; 
                        break;
                    case "PayPal":
                        paymentStatus = "Thanh toán PayPal";
                        break;
                    case "Mua trước trả sau":
                        paymentStatus = "Trả góp";
                        break;
                    default:
                        break;
                }

                // Tạo đơn hàng và chi tiết đơn hàng
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = DateTime.Now,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    ShippingMethod = model.ShippingMethod,
                    ShippingAddress = model.ShippingAddress,
                    OrderDetails = cart.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,
                    }).ToList()
                };
                db.Orders.Add(order);
                db.SaveChanges();
                // Xóa giỏ hàng sau khi đặt hàng thành công
                Session["Cart"] = null;
                // Điều hướng tới trang xác nhận đơn hàng
                return RedirectToAction("OrderSuccess", new {id = order.OrderID});
            }
            return View(model);
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }
        public ActionResult MyOrder()
        {
            return View();
        }
    }
}