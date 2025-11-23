using System;
using System.Linq;
using System.Web.Mvc;

using WEB;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        DBADIDASEntities db = new DBADIDASEntities();

        // 1. DASHBOARD (Trang chủ Admin)
        public ActionResult Index()
        {
            // Kiểm tra xem đã đăng nhập Admin chưa
            if (Session["AdminUser"] == null)
            {
                return RedirectToAction("Login");
            }

            // Thống kê cơ bản để hiện lên Dashboard
            ViewBag.SoLuongSanPham = db.Products.Count();
            ViewBag.SoLuongDonHang = db.OrderProes.Count();
            ViewBag.SoLuongKhach = db.Customers.Count();

            return View();
        }

        // 2. ĐĂNG NHẬP ADMIN (GET)
        public ActionResult Login()
        {
            return View();
        }

        // 3. XỬ LÝ ĐĂNG NHẬP (POST)
        [HttpPost]
        public ActionResult Login(string User, string Pass)
        {
            // Check bảng AdminUser
            var admin = db.AdminUsers.FirstOrDefault(a => a.UserName == User && a.PasswordUser == Pass);

            if (admin != null)
            {
                Session["AdminUser"] = admin.UserName;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
                return View();
            }
        }
       
        // 4. ĐĂNG XUẤT
        public ActionResult Logout()
        {
            Session["AdminUser"] = null;
            return RedirectToAction("Login");
        }
    }
}