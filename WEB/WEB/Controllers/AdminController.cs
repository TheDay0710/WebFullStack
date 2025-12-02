using System;
using System.Linq;
using System.Web.Mvc;

using WEB;

namespace Web.Controllers
{
    public class AdminController : Controller
    {
        DBADIDASEntities8 db = new DBADIDASEntities8();
   
        public ActionResult Index()
        {
          
            if (Session["AdminUser"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.SoLuongSanPham = db.Products.Count();
            ViewBag.SoLuongDonHang = db.OrderProes.Count();
            ViewBag.SoLuongKhach = db.Customers.Count();
          
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Login(string User, string Pass)
        {
        
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
       
   
        public ActionResult Logout()
        {
            Session["AdminUser"] = null;
            return RedirectToAction("Login");
        }
    }
}