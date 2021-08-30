using System.Web.Mvc;

namespace CameoMvc.Areas.SYS
{
    public class SYSAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SYS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SYS_default",
                "SYS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}