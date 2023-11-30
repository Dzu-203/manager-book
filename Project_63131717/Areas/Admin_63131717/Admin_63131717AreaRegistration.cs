using System.Web.Mvc;

namespace Project_63131717.Areas.Admin_63131717
{
    public class Admin_63131717AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin_63131717";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_63131717_default",
                "Admin_63131717/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}