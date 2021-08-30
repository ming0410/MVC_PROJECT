using CameoMvc.Areas.TEST.Models;
using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameoMvc.Areas.TEST.Controllers
{
    public class TEST001Controller : Controller
    {
        // GET: TEST/TEST001
        public ActionResult Index(M_TEST001 imodel)
        {
            DataTable data = M_DB.dsMes.GetTable_SqlCmd(M_MFG.SQL_LINE_GET("SMT"));

            List<SelectListItem> mySelectItemList = new List<SelectListItem>();

            foreach (DataRow drLine in data.Rows)
            {
                mySelectItemList.Add(new SelectListItem()
                {
                    Text = drLine["PDLINE_NAME"].ToString(),
                    Value = drLine["PDLINE_ID"].ToString(),
                    Selected = false
                });
            }

            M_TEST001 model = new M_TEST001() //上面的 Model
            {
                MyList = mySelectItemList
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(M_TEST001 imodel, string buutton)
        {
            DataTable data = M_DB.dsMes.GetTable_SqlCmd(M_MFG.SQL_LINE_GET("SMT"));

            List<SelectListItem> mySelectItemList = new List<SelectListItem>();

            foreach (DataRow drLine in data.Rows)
            {
                mySelectItemList.Add(new SelectListItem()
                {
                    Text = drLine["PDLINE_NAME"].ToString(),
                    Value = drLine["PDLINE_ID"].ToString(),
                    Selected = false
                });
            }

            M_TEST001 model = new M_TEST001() //上面的 Model
            {
                MyList = mySelectItemList
            };

            return View(model);
        }
    }
}