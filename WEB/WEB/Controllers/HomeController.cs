using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity; // <--- Cần thêm dòng này để dùng .Include
using Web; // <--- Cần dùng namespace chứa Model của bạn (check lại namespace file cũ)
using WEB;

namespace Web.Controllers
{
    public class HomeController : Controller
    {

        private DBADIDASEntities4 db = new DBADIDASEntities4();


        public ActionResult Index(string searchString)
        {

            var products = db.Products.Include(p => p.Category);


            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.NamePro.Contains(searchString));
            }


            return View(products.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}