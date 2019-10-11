using test_template.Models.function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test_template.Areas.admin.Models;
using test_template.common;
using test_template.Models.entity;

namespace test_template.Areas.admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: admin/Login
        DbconnectString db = new DbconnectString();
        public ActionResult Index()
        {
            if (Request.Cookies["UserNameAdmin"] != null)
            {
                string cookieUsername = Request.Cookies["UserNameAdmin"].Value;

                //var user = dao.GetUserString(cookieUsername);//lấy user trong db ra và gáng vào cookie
                var user = db.Accounts.SingleOrDefault(a => a.Username == cookieUsername);
                if (user != null)//nếu cookie đó đúng là tài khoản thì 
                {
                    Session.Add("Username", user.Username);
                    Session.Add("UserID", user.ID);
                    Session.Add("Fullname", user.Fullname);
                    Session.Add("UserLogin", user);
                    return RedirectToAction("Index", "Product");
                }
                return RedirectToAction("Index", "Login");

            }
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDao();
                int result = dao.login(model.UserName, model.Passwork);
                if (result == 1)
                {
                    var userSession = new AccountLogin();
                    var user = dao.GetbyId(model.UserName);
                    userSession.AccountName = user.Username;
                    userSession.AccountId = user.ID;
                    Session.Add(CommonConstanr.USER_SESSION, userSession);
                    Session.Add("Username", user.Username);
                    Session.Add("UserID", user.ID);
                    Session.Add("Fullname", user.Fullname);
                    Session.Add("UserLogin", user);
                    return RedirectToAction("Index", "Account");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else if (result == 2)
                {
                    ModelState.AddModelError("", "Tài khoản bị khóa");
                }
                else
                    ModelState.AddModelError("", "Mật khẩu không đúng");
            }
            return View("Index");

        }
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(ChangeModel model)
        {

            Account acc = new Account();

            if (ModelState.IsValid)
            {
                var user = db.Accounts.SingleOrDefault(a => a.Username == model.Username);
                if (user != null)
                {
                    if (user.Password == model.Password)//nếu đúng pass của tài khoản mới cập nhập
                    {
                        user.Password = model.NewPassword;//Hàm cập nhập mật khẩu
                        db.SaveChanges();
                        TempData["Success"] = "Đổi mật khẩu thành công";
                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu cũ không dúng, vui lòng nhập lại!");
                        return View();
                    }
                }
            }
            ModelState.AddModelError("", "Đổi mật khẩu thất bại");
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserLogin"] = null;
            Session["Username"] = null;
            Session["UserID"] = null;
            Session["Fullname"] = null;
            if (Response.Cookies["UserNameAdmin"] != null)
            {
                Response.Cookies["UserNameAdmin"].Expires = DateTime.Now.AddDays(-1);
            }

            return View("Index");
        }
    }
}