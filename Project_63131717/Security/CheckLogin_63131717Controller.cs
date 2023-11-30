using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project_63131717.Security
{
    public class CheckLogin_63131717Controller : Controller
    {
        // GET: CheckLogin_63131717
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["NameUser"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Login_63131717",
                    action = "Index",
                    area = ""
                }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}