using DA_WebBanHang.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DA_WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLiBanHangDataContext db = new QuanLiBanHangDataContext("Data Source=LAPTOP-AUIVJ6IL;Initial Catalog=QuanLyCuaHang;Integrated Security=True");

        public ActionResult Index()
        {
            var products = db.SanPhams.Take(8).ToList();
            return View(products);
        }

        public ActionResult ProductDetail(int id)
        {
            var product = db.SanPhams.FirstOrDefault(p => p.ID_SanPham == id);
            return View(product);
        }
        public ActionResult Blog()
        {
            var posts = db.BaiViets.OrderByDescending(b => b.ID_BaiViet).ToList();
            ViewBag.Posts = posts;
            return View();
        }
        public ActionResult Cart()
        {
            if (Session["Cart"] == null)
            {
                return View(new List<ChiTietDonHang>());
            }

            var cart = (List<ChiTietDonHang>)Session["Cart"];

            // Load thông tin sản phẩm cho mỗi item trong giỏ hàng
            foreach (var item in cart)
            {
                item.SanPham = db.SanPhams.FirstOrDefault(p => p.ID_SanPham == item.ID_SanPham);
            }

            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            var product = db.SanPhams.FirstOrDefault(p => p.ID_SanPham == productId);
            if (product == null)
                return HttpNotFound();

            var cart = Session["Cart"] as List<ChiTietDonHang> ?? new List<ChiTietDonHang>();

            var cartItem = cart.FirstOrDefault(i => i.ID_SanPham == productId);
            if (cartItem == null)
            {
                cart.Add(new ChiTietDonHang
                {
                    ID_SanPham = productId,
                    SoLuong = quantity,
                    Gia = product.Gia
                });
            }
            else
            {
                cartItem.SoLuong += quantity;
            }

            Session["Cart"] = cart;
            return RedirectToAction("Cart");
        }
    }
}