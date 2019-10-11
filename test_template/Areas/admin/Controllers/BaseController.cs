using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using test_template.common;

namespace test_template.Areas.admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: admin/Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sess = (AccountLogin)Session[CommonConstanr.USER_SESSION];
            if (sess == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new
                    {
                        controller = "Login",
                        action = "Index",
                        Areas = "Admin"
                    }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}