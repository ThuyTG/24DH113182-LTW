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
        private CartService GetCartService()
        {
            return new CartService(Session);
        }
        // GET: Customer/Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            var cartService = GetCartService();
            var cart = cartService.GetCart();
            var cartItems = cart.Items.ToList();
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
                CartItems = cartItems,
                TotalAmount = cartItems.Sum(item => item.TotalPrice),
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
                var cartService = GetCartService();
                var cart = cartService.GetCart();
                var cartItems = cart.Items.ToList();
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
                    OrderDetails = cartItems.Select(item => new OrderDetail
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
        public ActionResult OrderSuccess(int? id)
        {
            var order = db.Orders.Include("OrderDetails").SingleOrDefault(o => o.OrderID == id);
            if(order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Customer/MyOrder
        public ActionResult MyOrder(string status = "All", string search = "")
        {
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return RedirectToAction("Login", "Account");
            var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            if (customer == null) return RedirectToAction("Login", "Account");

            // Include product navigation for searching by product name and for display
            var query = db.Orders
                          .Include("OrderDetails.Product")
                          .Where(o => o.CustomerID == customer.CustomerID)
                          .AsQueryable();
            // Filter by status tab (best-effort mapping to existing PaymentStatus field)
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                switch (status)
                {
                    case "ChoThanhToan": // Chờ thanh toán
                        query = query.Where(o => o.PaymentStatus.Contains("Chưa") || o.PaymentStatus.Contains("chưa"));
                        break;
                    case "DangXuLy": // Đang xử lý
                        query = query.Where(o => o.PaymentStatus.Contains("xử lý") || o.PaymentStatus.Contains("đang xử lý"));
                        break;
                    case "DangVanChuyen": // Đang vận chuyển
                        query = query.Where(o => o.PaymentStatus.Contains("vận chuyển") || o.PaymentStatus.Contains("đang vận chuyển"));
                        break;
                    case "DaGiao": // Đã giao
                        query = query.Where(o => o.PaymentStatus.Contains("giao") || o.PaymentStatus.Contains("Đã giao"));
                        break;
                    case "DaHuy": // Đã hủy
                        query = query.Where(o => o.PaymentStatus.Contains("hủy") || o.PaymentStatus.Contains("Hủy"));
                        break;
                    default:
                        break;
                }
            }
            // Search by order id or product name
            if (!string.IsNullOrWhiteSpace(search))
            {
                int orderId;
                var trimmed = search.Trim();
                var byOrderId = int.TryParse(trimmed, out orderId);
                if (byOrderId)
                {
                    query = query.Where(o => o.OrderID == orderId);
                }
                else
                {
                    query = query.Where(o => o.OrderDetails.Any(d => d.Product.ProductName.Contains(trimmed)));
                }
            }

            var orders = query.OrderByDescending(o => o.OrderDate).ToList();

            var vm = new _24DH113182_LTW.Models.ViewModel.MyOrderVM
            {
                Orders = orders,
                CurrentTab = status,
                SearchTerm = search
            };

            return View(vm);
        }

        // POST: Customer/MyOrder
    }
}