using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using CameoMvc.Models;

namespace CameoMvc.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>登入</summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            M_LOGIN iData = new M_LOGIN();

            //第一次登入 Session 是空的
            iData.USER_NO = M_Cookie.GetCookie(SessionKey._USER_NO);
            if (string.IsNullOrWhiteSpace(iData.USER_NO) && Session[SessionKey._USER_NO] != null)
                iData.USER_NO = Session[SessionKey._USER_NO].ToString();

            //每次登出會跳轉登入頁面，所以每次登入前要清空使用者ID，用來之後的功能頁面登入前驗證是否有登入成功，以避免有心人士利用暫存權限來操作
            M_Cookie.SetCookie(SessionKey._USER_ID, string.Empty, TimeSpan.FromDays(1));
            Session[SessionKey._USER_ID] = string.Empty;

            bool isMobile = FM_SYS.IsPhoneDevice(Request);

            //是否為行動裝置
            iData.PHON = FM_SYS.IsPhoneDevice(Request);
            iData.MOBL = FM_SYS.IsMobileDevice(Request);

            if (Request.ServerVariables["HTTP_USER_AGENT"] != null)
                iData.UserAnget = Request.ServerVariables["HTTP_USER_AGENT"];
            
            iData.ENV_T = M_DB.dsMes.ConnectionString.Contains("mesdbtest");

            return View(iData);
        }

        /// <summary>登入</summary>
        /// <param name="iData"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(M_LOGIN iData, string button)
        {
            if (button == "Guest")
            {
                iData.USER_NO = User_Guest.UserNo;
                iData.PASSWORD = User_Guest.Password;
            }
            //先透過 ModelState.IsValid 檢查是否必要的欄位都有輸入
            if (button != "Guest" && !ModelState.IsValid)
            {
                return RedirectToAction("Login");
            }
            else
            {
                SYS_Login EXE = new SYS_Login();

                var oData = EXE.CheckLogin(iData);

                if (!oData.FLAG)
                {
                    //透過 ModelState.AddModelError 自訂錯誤訊息
                    ModelState.AddModelError(oData.ERR_KEY, oData.ERR_MSG);
                    return View(iData);
                }
                else
                {
                    // 登入完成,導到目錄頁前，將登入資訊存於 Cookie
                    M_Cookie.SetCookie(SessionKey._FAB_ID, oData.UserInfo.FabId.ToString(), TimeSpan.FromDays(30));
                    M_Cookie.SetCookie(SessionKey._USER_NO, oData.UserInfo.UserNo.ToString(), TimeSpan.FromDays(30));
                    M_Cookie.SetCookie(SessionKey._USER_ID, oData.UserInfo.UserId.ToString(), TimeSpan.FromDays(1));
                    M_Cookie.SetCookie(SessionKey._USER_NAME, oData.UserInfo.UserName.ToString(), TimeSpan.FromDays(1));
                    M_Cookie.SetCookie(SessionKey._AUTHORITY, oData.UserInfo.UserName.ToString(), TimeSpan.FromDays(1));

                    Session[SessionKey._FAB_ID] = oData.UserInfo.FabId;
                    Session[SessionKey._USER_NO] = oData.UserInfo.UserNo;
                    Session[SessionKey._USER_ID] = oData.UserInfo.UserId;
                    Session[SessionKey._USER_NAME] = oData.UserInfo.UserName;

                    //由網頁讀取IP，存於 Cookie
                    string sCMD_IP = string.Empty;
                    if (Request.ServerVariables["HTTP_VIA"] == null)
                    {
                        //沒有使用 代理伺服器(Proxy)，取得 Client 真實 IP
                        sCMD_IP = Request.ServerVariables["REMOTE_ADDR"] == null ? string.Empty : Request.ServerVariables["REMOTE_ADDR"].ToString();
                        if (string.IsNullOrWhiteSpace(sCMD_IP))
                            sCMD_IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null ? string.Empty : Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        //使用 代理伺服器(Proxy)，取得 Client 真實 IP
                        sCMD_IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null ? string.Empty : Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    M_Cookie.SetCookie(SessionKey._LOGIN_IP, sCMD_IP, TimeSpan.FromDays(30));
                    Session[SessionKey._LOGIN_IP] = sCMD_IP;

                    //return RedirectToAction("Index", "Home");

                    //取得最後登入的畫面
                    string ACYN = M_Cookie.GetCookie(SessionKey._LAST_ACTN);
                    string CTRL = M_Cookie.GetCookie(SessionKey._LAST_CTRL);
                    string AREA = M_Cookie.GetCookie(SessionKey._LAST_AREA);
                    if (string.IsNullOrWhiteSpace(ACYN)) ACYN = "Index";
                    if (string.IsNullOrWhiteSpace(CTRL)) CTRL = "RPT001";
                    if (string.IsNullOrWhiteSpace(AREA)) AREA = "RPT";

                    return RedirectToAction(ACYN, CTRL, new { area = AREA });
                }
            }
            
        }

        /// <summary>修改新密碼</summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ModifyPassword()
        {
            M_PasswordChg iINFO = new M_PasswordChg();

            iINFO.FabId = M_Cookie.GetCookie(SessionKey._FAB_ID);
            iINFO.UserId = M_Cookie.GetCookie(SessionKey._USER_ID);
            iINFO.UserNo = M_Cookie.GetCookie(SessionKey._USER_NO);
            iINFO.PasswordOld = M_Cookie.GetCookie(SessionKey._PASSWORD);

            if (string.IsNullOrWhiteSpace(iINFO.FabId))
                iINFO.FabId = Session[SessionKey._FAB_ID].ToString();
            if (string.IsNullOrWhiteSpace(iINFO.UserId))
                iINFO.UserNo = Session[SessionKey._USER_ID].ToString();
            if (string.IsNullOrWhiteSpace(iINFO.UserNo))
                iINFO.UserNo = Session[SessionKey._USER_NO].ToString();
            if (string.IsNullOrWhiteSpace(iINFO.PasswordOld))
                iINFO.PasswordOld = Session[SessionKey._PASSWORD].ToString();
            iINFO.PasswordNew = "";
            iINFO.PasswordChk = "";
            return View(iINFO);
        }
        
        /// <summary>修改新密碼</summary>
        /// <param name="iINFO"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyPassword(M_PasswordChg iINFO)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(iINFO.PasswordNew))
                {
                    ModelState.AddModelError("", string.Format("請輸入新密碼"));
                    iINFO.PasswordNew = "";
                    iINFO.PasswordChk = "";
                    return View(iINFO);
                }
                else if (string.IsNullOrEmpty(iINFO.PasswordChk))
                {
                    ModelState.AddModelError("", string.Format("請輸入確認新密碼"));
                    iINFO.PasswordNew = "";
                    iINFO.PasswordChk = "";
                    return View(iINFO);
                }
                else if (iINFO.PasswordNew != iINFO.PasswordChk)
                {
                    ModelState.AddModelError("", string.Format("輸入的新密碼與確認新密碼不同"));
                    iINFO.PasswordNew = "";
                    iINFO.PasswordChk = "";
                    return View(iINFO);
                }
                SYS_Login oFUNC = new SYS_Login();
                var oINFO = oFUNC.ModifyPwd(iINFO);
                if (!oINFO.FLAG)
                {
                    ModelState.AddModelError("", string.Format(oINFO.ERR_MSG));
                    return View(iINFO);
                }
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }

        /// <summary>功能清單</summary>
        /// <returns></returns>
        public ActionResult Menu()
        {
            string UserNo = M_Cookie.GetCookie(SessionKey._USER_NO);
            if (string.IsNullOrWhiteSpace(UserNo))
                UserNo = Session[SessionKey._USER_NO].ToString();

            //Session Timeout -> 重新登入
            if (string.IsNullOrWhiteSpace(UserNo))
            {
                return RedirectToAction("Login");
            }


            SYS_Login oFUNC = new SYS_Login();
            var OutInfo = oFUNC.QueryMenuList(UserNo);

            return PartialView("_LayoutMenuPartial", OutInfo);
        }        
    }
}