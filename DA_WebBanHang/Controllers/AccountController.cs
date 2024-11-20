using DA_WebBanHang.Models;
using System;
using System.Linq;
using System.Web.Mvc;
namespace DA_WebBanHang.Controllers
{
    public class AccountController : Controller
    {
        QuanLiBanHangDataContext db = new QuanLiBanHangDataContext("Data Source=LAPTOP-AUIVJ6IL;Initial Catalog=QuanLyCuaHang;Integrated Security=True");

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var customer = db.KhachHangs.FirstOrDefault(k => k.Email == email && k.matkhau == password);
            if (customer != null)
            {
                Session["UserID"] = customer.ID_KhachHang;
                Session["UserName"] = customer.HoTen;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Email hoặc mật khẩu không đúng";
            return View();
        }

        [HttpPost]
        public ActionResult Register(KhachHang customer)
        {
            if (ModelState.IsValid)
            {
                var check = db.KhachHangs.FirstOrDefault(k => k.Email == customer.Email);
                if (check == null)
                {
                    // Có thể thêm mã hóa mật khẩu ở đây nếu cần
                    db.KhachHangs.InsertOnSubmit(customer);
                    db.SubmitChanges();
                    return RedirectToAction("Login");
                }
                ViewBag.Error = "Email đã tồn tại";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}