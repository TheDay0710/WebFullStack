using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web;
using WEB;

namespace Web.Controllers
{
    public class PaymentController : Controller
    {
        private DBADIDASEntities2 db = new DBADIDASEntities2();

        public ActionResult Index()
        {
            // BẮT BUỘC ĐĂNG NHẬP
            if (Session["IDCus"] == null)
            {
                return RedirectToAction("Login", "Customers");
            }

            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            var cart = Session["Cart"] as List<CartItem>;
            ViewBag.Cart = cart.Select(x => x._shopping_product).ToList();
            ViewBag.Total = cart.Sum(x => x._shopping_product.Price * x._shopping_quantity);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessOrder(OrderPro order)
        {
            if (Session["IDCus"] == null)
            {
                return RedirectToAction("Login", "Customers");
            }

            if (ModelState.IsValid)
            {
                order.IDCus = int.Parse(Session["IDCus"].ToString());
                order.DateOrder = DateTime.Now;

                db.OrderProes.Add(order);
                db.SaveChanges();

                var cart = Session["Cart"] as List<CartItem>;
                foreach (var item in cart)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.IDOrder = order.ID;
                    detail.IDProduct = item._shopping_product.ProductID;
                    detail.Quantity = item._shopping_quantity;
                    detail.UnitPrice = (double)item._shopping_product.Price.GetValueOrDefault(0);
                    db.OrderDetails.Add(detail);
                }
                db.SaveChanges();

                Session["Cart"] = null;

                return RedirectToAction("Success", new { id = order.ID });
            }

            if (Session["Cart"] != null)
            {
                var cart = Session["Cart"] as List<CartItem>;
                ViewBag.Cart = cart.Select(x => x._shopping_product).ToList();
                ViewBag.Total = cart.Sum(x => x._shopping_product.Price * x._shopping_quantity);
            }
            return View("Index", order);
        }

        public ActionResult Success(int? id)
        {
            if (id.HasValue)
            {
                var order = db.OrderProes.Find(id);
                return View(order);
            }
            return View();
        }
    }
}

