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
        private DBADIDASEntities8 db = new DBADIDASEntities8();
        // 1. TRANG THANH TOÁN (CHO PHÉP KHÁCH VÃNG LAI)
        public ActionResult Index()
        {
            // Chỉ cần kiểm tra giỏ hàng có đồ không thôi
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            var cart = Session["Cart"] as List<CartItem>;
            ViewBag.Cart = cart.Select(x => x._shopping_product).ToList();
            ViewBag.Total = cart.Sum(x => x._shopping_product.Price * x._shopping_quantity);

            return View();
        }

        // 2. XỬ LÝ ĐẶT HÀNG (KHÔNG BẮT ĐĂNG NHẬP)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessOrder(OrderPro order)
        {
            if (ModelState.IsValid)
            {
                // --- XỬ LÝ LOGIC NGƯỜI MUA ---
                if (Session["IDCus"] != null)
                {
                    // Nếu ĐÃ đăng nhập -> Gán ID khách hàng vào đơn
                    order.IDCus = int.Parse(Session["IDCus"].ToString());
                }
                else
                {
                    // Nếu CHƯA đăng nhập (Khách lẻ) -> Để trống ID khách (NULL)
                    // Lưu ý: Trong SQL bảng OrderPro cột IDCus phải cho phép NULL
                    order.IDCus = null;
                }

                // Gán ngày đặt
                order.DateOrder = DateTime.Now;

                // Lưu đơn hàng
                db.OrderProes.Add(order);
                db.SaveChanges();

                // Lưu chi tiết đơn hàng
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

                // Xóa giỏ hàng sau khi mua xong
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

        // 3. THÀNH CÔNG
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