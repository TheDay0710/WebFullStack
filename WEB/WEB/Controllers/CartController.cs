using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class CartController : Controller
    {
        DBADIDASEntities db = new DBADIDASEntities();


        public List<CartItem> GetCart()
        {
            List<CartItem> myCart = Session["Cart"] as List<CartItem>;
            if (myCart == null)
            {
                myCart = new List<CartItem>();
                Session["Cart"] = myCart;
            }
            return myCart;
        }


        public ActionResult AddToCart(int id)
        {

            List<CartItem> myCart = GetCart();


            CartItem currentItem = myCart.FirstOrDefault(p => p._shopping_product.ProductID == id);

            if (currentItem == null)
            {

                Product product = db.Products.Find(id);
                if (product != null)
                {
                    myCart.Add(new CartItem
                    {
                        _shopping_product = product,
                        _shopping_quantity = 1
                    });
                }
            }
            else
            {

                currentItem._shopping_quantity++;
            }


            Session["Cart"] = myCart;


            return RedirectToAction("Index");
        }


        public ActionResult Index()
        {
            List<CartItem> myCart = GetCart();


            if (myCart.Count > 0)
            {
                ViewBag.Total = myCart.Sum(x => x._shopping_product.Price * x._shopping_quantity);
            }
            else
            {
                ViewBag.Total = 0;
            }

            return View(myCart);
        }


        public ActionResult UpdateCart(int id, FormCollection f)
        {
            List<CartItem> myCart = GetCart();
            CartItem currentItem = myCart.FirstOrDefault(p => p._shopping_product.ProductID == id);

            if (currentItem != null)
            {
                currentItem._shopping_quantity = int.Parse(f["Quantity"].ToString());
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveCart(int id)
        {
            List<CartItem> myCart = GetCart();
            CartItem currentItem = myCart.FirstOrDefault(p => p._shopping_product.ProductID == id);

            if (currentItem != null)
            {
                myCart.Remove(currentItem);
            }
            return RedirectToAction("Index");
        }
    }
}