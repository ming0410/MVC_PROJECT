using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameoMvc.Models
{
    public class M_MFG
    {

        /// <summary>
        /// 印刷電路板之製程面
        /// </summary>
        public enum PCB_SIDE
        {
            TOP,
            BOT,
            NONE
        }

        /// <summary>
        /// 取得生產線清單
        /// </summary>
        /// <param name="LINE_NAME"></param>
        /// <param name="ContainsAll">是否包含所有生產線之選項</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> DLL_LINE(string LINE_NAME, bool ContainsAll)
        {
            List<SelectListItem> Dll = new List<SelectListItem>();

            if (ContainsAll)
            {
                Dll.Add(new SelectListItem
                {
                    Text = "-All LINE-",
                    Value = "ALL",
                    Selected = true
                });
            }

            DataTable dtLine = M_DB.dsMes.GetTable_SqlCmd(SQL_LINE_GET(LINE_NAME));
            if (dtLine?.Rows?.Count > 0)
            {
                foreach (DataRow drLine in dtLine.Rows)
                {
                    Dll.Add(new SelectListItem
                    {
                        Text = drLine["PDLINE_NAME"].ToString(),
                        Value = drLine["PDLINE_ID"].ToString(),
                        Selected = false
                    });
                }
            }
            return Dll;
        }

        /// <summary>
        /// 取得製程面清單
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> DLL_PCB_SIDE()
        {
            IEnumerable<SelectListItem> Dll = new List<SelectListItem>();

            IEnumerable<PCB_SIDE> SmtSides = Enum.GetValues(typeof(PCB_SIDE)).Cast<PCB_SIDE>();
            Dll = from side in SmtSides
                  select new System.Web.Mvc.SelectListItem
                  {
                      Text = side.ToString(),
                      Value = side.ToString()
                  };
            return Dll;
        }


        #region SQL
        /// <summary>
        /// 取得啟用中的線別清單
        /// </summary>
        /// <returns></returns>
        public static string SQL_LINE_GET(string LineName)
        {
            return string.Format("SELECT PDLINE_ID,PDLINE_NAME FROM SAJET.SYS_PDLINE WHERE PDLINE_NAME LIKE '{0}%' AND ENABLED='Y' AND OP_STATUS='Y' ORDER BY PDLINE_NAME", LineName);
        }
        #endregion
    }
}