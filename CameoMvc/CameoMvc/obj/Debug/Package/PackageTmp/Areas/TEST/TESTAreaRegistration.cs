using System.Web.Mvc;

namespace CameoMvc.Areas.TEST
{
    public class TESTAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TEST";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TEST_default",
                "TEST/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}