using System.Web.Mvc;

namespace CameoMvc.Areas.MFG
{
    public class MFGAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MFG";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MFG_default",
                "MFG/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}