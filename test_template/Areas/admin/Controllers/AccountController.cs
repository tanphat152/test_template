using test_template.Models.function;
using test_template.Models.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace test_template.Areas.admin.Controllers
{
    public class AccountController : BaseController
    {
        // GET: admin/Account
        public ActionResult Index(string searchString, int page = 1, int pagesixe = 10)
        {

            var dao = new AccountDao();
            var model = dao.ListALLPaging(searchString, page, pagesixe);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new AccountDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Account accont)
        {
            if (ModelState.IsValid)
            {
                var dao = new AccountDao();
                int id = dao.insert(accont);
                if (id > 0)
                {
                    return RedirectToAction("Index", "Account");
                }
                else if (id == -1)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại, vui lòng nhập lại!");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công");
                }
            }

            return View(accont);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = new AccountDao().ViewDeTaill(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(Account user)
        {
            var dao = new AccountDao();
            var result = dao.uplate(user);
            if (result)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                ModelState.AddModelError("", "cập nhật user ko thành công");
            }
            return View("Index");
        }
    }
}