using CameoMvc.Areas.RPT.Models;
using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameoMvc.Areas.RPT.Controllers
{
    public class RPT010Controller : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            string MSG = string.Empty;
            M_RPT010 iData = new M_RPT010();
            M_RPT010 oData = null;
            FM_RPT010 EXE = new FM_RPT010();
            string ACTION = "Index";
            string FUNC = iData.FUNC;
            string FUNC_NAME = iData.FUNC_NAME;

            #region Cookie 基本資料
            //錯誤資料清空
            iData.FLAG = false;
            iData.ERR = string.Empty;

            //取得 Cookie 紀錄資料
            iData.FAB_ID = M_Cookie.GetCookie(SessionKey._FAB_ID);
            iData.USER_ID = M_Cookie.GetCookie(SessionKey._USER_ID);
            iData.USER_NO = M_Cookie.GetCookie(SessionKey._USER_NO);
            iData.USER_NAME = M_Cookie.GetCookie(SessionKey._USER_NAME);

            //Cookie沒紀錄，改取 Session 紀錄資料
            if (string.IsNullOrWhiteSpace(iData.FAB_ID) && Session[SessionKey._FAB_ID] != null)
                iData.FAB_ID = Session[SessionKey._FAB_ID].ToString();
            if (string.IsNullOrWhiteSpace(iData.USER_ID) && Session[SessionKey._USER_ID] != null)
                iData.USER_ID = Session[SessionKey._USER_ID].ToString();
            if (string.IsNullOrWhiteSpace(iData.USER_NO) && Session[SessionKey._USER_NO] != null)
                iData.USER_NO = Session[SessionKey._USER_NO].ToString();
            if (string.IsNullOrWhiteSpace(iData.USER_NAME) && Session[SessionKey._USER_NAME] != null)
                iData.USER_NAME = Session[SessionKey._USER_NAME].ToString();

            //Cookie 或 Session 過期，或沒有資料，導引到登入畫面
            if (string.IsNullOrWhiteSpace(iData.USER_ID))
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            //Debug用來 預設廠別ID 及 預設使用者ID
            if (string.IsNullOrWhiteSpace(iData.FAB_ID)) iData.FAB_ID = "0";
            if (string.IsNullOrWhiteSpace(iData.USER_ID)) iData.USER_ID = "0"; //MES(Auto)

            //是否為行動裝置
            iData.PHON = FM_SYS.IsPhoneDevice(Request);
            iData.MOBL = FM_SYS.IsMobileDevice(Request);

            //是否為測試環境
            iData.ENV_T = M_DB.dsMes.ConnectionString.Contains("mesdbtest");

            //確認是否有權限，沒有資料，導引到登入畫面
            iData.AUTHORITY = FM_SYS.QueryAuthority(iData.USER_NO, FUNC);
            if (iData.AUTHORITY.Count <= 0)
            {
                //清空最後登入畫面
                M_Cookie.SetCookie(SessionKey._LAST_ACTN, "", TimeSpan.FromDays(30));
                M_Cookie.SetCookie(SessionKey._LAST_CTRL, "", TimeSpan.FromDays(30));
                M_Cookie.SetCookie(SessionKey._LAST_AREA, "", TimeSpan.FromDays(30));

                MSG = string.Format("您沒有開啟『{1}({0})』之權限", FUNC, FUNC_NAME);
                TempData["msg"] = string.Format("<script>alert('{0}');</script>", MSG);
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            //紀錄登入畫面
            M_Cookie.SetCookie(SessionKey._LAST_ACTN, ACTION, TimeSpan.FromDays(30));
            M_Cookie.SetCookie(SessionKey._LAST_CTRL, iData.FUNC, TimeSpan.FromDays(30));
            M_Cookie.SetCookie(SessionKey._LAST_AREA, iData.FUNC.Substring(0, 3), TimeSpan.FromDays(30));
            #endregion

            try
            {
                oData = iData;
            }
            catch (Exception ex)
            {
                MSG = "例外錯誤"
                    + Environment.NewLine + string.Format("功能:{0}/{1}", FUNC, ACTION)
                    + Environment.NewLine + string.Format("訊息:{0}", ex.Message.Replace("'", "").Replace("\r\n", ""));
            }
            finally
            {
                if (MSG?.Length > 0)
                {
                    MSG = MSG.Replace("\r\n", "\\r\\n");
                    TempData["msg"] = string.Format("<script>alert('{0}');</script>", MSG);
                }
                else
                {
                    TempData["msg"] = "";
                }
            }

            ModelState.Clear();
            return View(oData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(M_RPT010 iData, string button)
        {
            string MSG = string.Empty;
            M_RPT010 oData = null;
            FM_RPT010 EXE = new FM_RPT010();
            string ACTION = "Index";
            string FUNC = iData.FUNC;
            string FUNC_NAME = iData.FUNC_NAME;

            #region Cookie 基本資料
            //錯誤資料清空
            iData.FLAG = false;
            iData.ERR = string.Empty;

            //取得 Cookie 紀錄資料
            iData.FAB_ID = M_Cookie.GetCookie(SessionKey._FAB_ID);
            iData.USER_ID = M_Cookie.GetCookie(SessionKey._USER_ID);
            iData.USER_NO = M_Cookie.GetCookie(SessionKey._USER_NO);
            iData.USER_NAME = M_Cookie.GetCookie(SessionKey._USER_NAME);

            //Cookie沒紀錄，改取 Session 紀錄資料
            if (string.IsNullOrWhiteSpace(iData.FAB_ID) && Session[SessionKey._FAB_ID] != null)
                iData.FAB_ID = Session[SessionKey._FAB_ID].ToString();
            if (string.IsNullOrWhiteSpace(iData.USER_ID) && Session[SessionKey._USER_ID] != null)
                iData.USER_ID = Session[SessionKey._USER_ID].ToString();
            if (string.IsNullOrWhiteSpace(iData.USER_NO) && Session[SessionKey._USER_NO] != null)
                iData.USER_NO = Session[SessionKey._USER_NO].ToString();
            if (string.IsNullOrWhiteSpace(iData.USER_NAME) && Session[SessionKey._USER_NAME] != null)
                iData.USER_NAME = Session[SessionKey._USER_NAME].ToString();

            //Cookie 或 Session 過期，或沒有資料，導引到登入畫面
            if (string.IsNullOrWhiteSpace(iData.USER_ID))
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            //Debug用來 預設廠別ID 及 預設使用者ID
            if (string.IsNullOrWhiteSpace(iData.FAB_ID)) iData.FAB_ID = "0";
            if (string.IsNullOrWhiteSpace(iData.USER_ID)) iData.USER_ID = "0"; //MES(Auto)

            //是否為行動裝置
            iData.PHON = FM_SYS.IsPhoneDevice(Request);
            iData.MOBL = FM_SYS.IsMobileDevice(Request);

            //是否為測試環境
            iData.ENV_T = M_DB.dsMes.ConnectionString.Contains("mesdbtest");

            //確認是否有權限，沒有資料，導引到登入畫面
            iData.AUTHORITY = FM_SYS.QueryAuthority(iData.USER_NO, FUNC);
            if (iData.AUTHORITY.Count <= 0)
            {
                //清空最後登入畫面
                M_Cookie.SetCookie(SessionKey._LAST_ACTN, "", TimeSpan.FromDays(30));
                M_Cookie.SetCookie(SessionKey._LAST_CTRL, "", TimeSpan.FromDays(30));
                M_Cookie.SetCookie(SessionKey._LAST_AREA, "", TimeSpan.FromDays(30));

                MSG = string.Format("您沒有開啟『{1}({0})』之權限", FUNC, FUNC_NAME);
                TempData["msg"] = string.Format("<script>alert('{0}');</script>", MSG);
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            //紀錄登入畫面
            M_Cookie.SetCookie(SessionKey._LAST_ACTN, ACTION, TimeSpan.FromDays(30));
            M_Cookie.SetCookie(SessionKey._LAST_CTRL, iData.FUNC, TimeSpan.FromDays(30));
            M_Cookie.SetCookie(SessionKey._LAST_AREA, iData.FUNC.Substring(0, 3), TimeSpan.FromDays(30));
            #endregion

            try
            {
                string errMsg = string.Empty;
                iData.QM.S_TIME = string.Empty;
                iData.QM.E_TIME = string.Empty;

                if (string.IsNullOrWhiteSpace(iData.QM.WO) && string.IsNullOrWhiteSpace(iData.QM.MAC) && string.IsNullOrWhiteSpace(iData.QM.DLINK))
                {
                    oData = iData;
                    MSG = "查詢錯誤" 
                        + Environment.NewLine + string.Format("功能:{0}/{1}", FUNC, button)
                        + Environment.NewLine + string.Format("訊息:{0}", "至少要有一個查詢條件");
                }
                else
                {
                    //執行查詢
                    oData = EXE.QUERY(iData, button == "EXPORT");

                    if (!oData.FLAG)
                    {
                        MSG = oData.ERR;
                    }
                }
            }
            catch (Exception ex)
            {
                MSG = "例外錯誤"
                    + Environment.NewLine + string.Format("功能:{0}/{1}", FUNC, ACTION)
                    + Environment.NewLine + string.Format("訊息:{0}", ex.Message.Replace("'", "").Replace("\r\n", ""));
            }
            finally
            {
                if (MSG?.Length > 0)
                {
                    MSG = MSG.Replace("\r\n", "\\r\\n");
                    TempData["msg"] = string.Format("<script>alert('{0}');</script>", MSG);
                }
                else
                {
                    TempData["msg"] = "";
                }

                //操作紀錄
                FM_SYS.Operation_Record(new OperationRecord
                {
                    PROGRAM = FUNC.Substring(0, 3),
                    FUNCTION = FUNC,
                    DLL_FILENAME = string.Format("{0}/{1}/{2}", FUNC.Substring(0, 3), FUNC, ACTION),
                    USER_ID = oData.USER_ID,
                    USE_TYPE = OperationType.Q,
                    REMORK = oData == null || oData.FLAG ? string.Empty : oData.ERR
                });
            }

            ModelState.Clear();
            return View(oData);
        }
    }
}