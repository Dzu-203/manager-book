using Project_63131717.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_63131717.Controllers
{
    public class Home_63131717Controller : Controller
    {
        Project_63131717Entities db = new Project_63131717Entities();
        public ActionResult Index()
        {
            return View();
        }
    }
}