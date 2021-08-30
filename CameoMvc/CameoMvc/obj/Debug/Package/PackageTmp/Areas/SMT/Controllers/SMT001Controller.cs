using CameoMvc.Areas.SMT.Models;
using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameoMvc.Areas.SMT.Controllers
{
    public class SMT001Controller : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            string MSG = string.Empty;
            M_SMT001 oData = null;
            M_SMT001 iData = new M_SMT001();
            FM_SMT001 EXE = new FM_SMT001();
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

                #region 下拉清單
                //新增線別下拉清單
                if (oData.DDL_LINE == null)
                {
                    oData.DDL_LINE = M_MFG.DLL_LINE("SMT", true);
                }

                //新增製程面下拉清單
                if (oData.DDL_SIDE == null)
                {
                    oData.DDL_SIDE = M_MFG.DLL_PCB_SIDE();
                }
                #endregion
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
        public ActionResult Index(M_SMT001 iData, string button)
        {
            string MSG = string.Empty;
            M_SMT001 oData = null;
            FM_SMT001 EXE = new FM_SMT001();
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
                iData.QM.UpdUserId = iData.USER_ID;
                iData.QM.UpdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                if (button == "Save")
                {
                    if (!EXE.SAVE(iData.QM, out errMsg))
                    {
                        MSG = "儲存失敗！" + Environment.NewLine + errMsg;
                    }
                }
                else if (button == "Delete")
                {
                    if (!EXE.DELETE(iData.QM, out errMsg))
                    {
                        MSG = "刪除失敗！" + Environment.NewLine + errMsg;
                    }
                }
                else if (button.Contains("DelRow"))
                {
                    QM_SMT001 iQM = new QM_SMT001();
                    string[] Valeus = button.Split('|');
                    iQM.Part = Valeus[1];
                    iQM.Side = Valeus[2];
                    iQM.Line = new string[] { Valeus[3] };
                    iQM.UpdUserId = iData.USER_ID;
                    iQM.UpdTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    if (!EXE.DELETE(iQM, out errMsg))
                    {
                        MSG = "刪除失敗！" + Environment.NewLine + errMsg;
                    }
                }
                else if (button.Contains("SelRow"))
                {
                    string[] Valeus = button.Split('|');

                    iData.QM.Part = Valeus[1];
                    iData.QM.Side = Valeus[2];
                    iData.QM.Line = new string[] { Valeus[3] };
                }

                //執行查詢
                oData = EXE.QUERY(iData, button == "Export");

                if (button.Contains("SelRow"))
                {
                    if (oData.LIST_RM.Count > 0)
                    {
                        oData.QM.Model = oData.LIST_RM[0].Model;
                        oData.QM.StdWT = oData.LIST_RM[0].StdWT;
                    }
                }

                if (!oData.FLAG)
                {
                    MSG = oData.ERR;
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
                if (button == "DelRow") button = "DELETE";
                button = button.ToUpper();
                FM_SYS.Operation_Record(new OperationRecord
                {
                    PROGRAM = FUNC.Substring(0, 3),
                    FUNCTION = FUNC,
                    DLL_FILENAME = string.Format("{0}/{1}/{2}", FUNC.Substring(0, 3), FUNC, ACTION),
                    USER_ID = oData.USER_ID,
                    USE_TYPE = button == "SAVE" ? OperationType.S : button == "DELETE" ? OperationType.D : OperationType.Q,
                    REMORK = oData == null || oData.FLAG ? "" : oData.ERR
                });
            }

            ModelState.Clear();
            return View(oData);
        }
    }
}