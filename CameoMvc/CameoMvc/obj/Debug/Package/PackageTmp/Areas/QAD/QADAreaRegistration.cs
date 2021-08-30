using System.Web.Mvc;

namespace CameoMvc.Areas.QAD
{
    public class QADAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QAD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QAD_default",
                "QAD/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}