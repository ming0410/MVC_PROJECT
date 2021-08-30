using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web;
using System.Data;

namespace CameoMvc.Models
{

    /// <summary>
    /// 權限
    /// </summary>
    public enum Authority
    {
        Read_Only = 0,
        Allow_To_Change = 1,
        Full_Control = 2,
        Allow_To_Execute = 3
    }

    /// <summary>
    /// 操作型態
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 登入/查詢/執行
        /// </summary>
        Q,
        /// <summary>
        /// 刪除
        /// </summary>
        D,
        /// <summary>
        /// 儲存
        /// </summary>
        S,
        /// <summary>
        /// 編輯
        /// </summary>
        E,
        /// <summary>
        /// 新增
        /// </summary>
        I,
        /// <summary>
        /// 更新
        /// </summary>
        U
    }

    /// <summary>
    /// 操作紀錄
    /// </summary>
    public class OperationRecord
    {
        /// <summary>
        /// 模組名稱
        /// </summary>
        public string PROGRAM { get; set; }
        /// <summary>
        /// 子功能名稱
        /// </summary>
        public string FUNCTION { get; set; }
        /// <summary>
        /// 子功能DLL名稱
        /// </summary>
        public string DLL_FILENAME { get; set; }
        /// <summary>
        /// 參數
        /// </summary>
        public string FUN_PARAM { get; set; }
        /// <summary>
        /// 使用者ID
        /// </summary>
        public string USER_ID { get; set; }
        /// <summary>
        /// 操作型態,Q:查詢/執行,D:刪除,S:儲存,E:編輯,I:新增,U:更新
        /// </summary>
        public OperationType USE_TYPE { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string REMORK { get; set; }
    }

    public class LoginUser
    {
        /// <summary>廠別ID
        /// </summary>
        public int FabId { get; set; }
        /// <summary>使用者ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>使用者工號
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>使用者姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>登入密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>登入角色
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>角色權限
        /// </summary>
        public List<RoleAuthority> Authoritys { get; set; }
    }

    public class RoleAuthority
    {
        /// <summary>程式名稱
        /// </summary>
        public string Program { get; set; }
        /// <summary>功能分類
        /// </summary>
        public string FunType { get; set; }
        /// <summary>網頁功能
        /// </summary>
        public string WebpageFun { get; set; }
        /// <summary>權限
        /// </summary>
        public string Authority { get; set; }
        /// <summary>網頁網址
        /// </summary>
        public string WebpageUrl { get; set; }
    }

    public static class SystemDefault
    {
        public const int FabId = 0;
        public const string Enable = "Y";
    }

    public static class User_Admin
    {
        public const int FabId = 0;
        public const int UserId = 0;
        public const string UserNo = "admin";
        public const string User = "admin";
        public const string Password = "cameo4admin";
        public const int RoleId = 0;
    }

    /// <summary>
    /// 訪客登入帳密
    /// </summary>
    public static class User_Guest
    {
        public const int FabId = 0;
        public const int UserId = 1;
        public const string UserNo = "GUEST";
        public const string User = "訪客";
        public const string Password = "123";
        public const int RoleId = 1;
    }

    /// <summary>
    /// 共用的 SessionKey 或 CookieKey
    /// </summary>
    public static class SessionKey
    {
        public static readonly string _LOGIN_IP = "LoginIP";
        public static readonly string _LOGIN_CLIENT = "LoginClient";
        public static readonly string _LOGIN_DATE_TIME = "LoginDateTime";

        public static readonly string _FAB_ID = "FID";
        public static readonly string _USER_ID = "UID";
        public static readonly string _USER_NO = "UNO";
        public static readonly string _USER_NAME = "UNM";
        public static readonly string _PASSWORD = "UPW";
        public static readonly string _ROLE_ID = "RID";
        public static readonly string _AUTHORITY = "AUT";
        /// <summary>
        /// 最後登入頁面的 Action
        /// </summary>
        public static readonly string _LAST_ACTN = "LAN";
        /// <summary>
        /// 最後登入頁面的 Control
        /// </summary>
        public static readonly string _LAST_CTRL = "LCL";
        /// <summary>
        /// 最後登入頁面的 Area
        /// </summary>
        public static readonly string _LAST_AREA = "LAA";
    }

    public class M_LOGIN
    {
        /// <summary>
        /// 廠別
        /// </summary>
        public int FAB_ID { get; set; }
        /// <summary>
        /// 使用者工號
        /// </summary>
        public string USER_NO { get; set; }
        /// <summary>
        /// 登入密碼
        /// </summary>
        public string PASSWORD { get; set; }

        /// <summary>
        /// 是否為手機，非平板
        /// </summary>
        public bool PHON { get; set; }

        /// <summary>
        /// 是否為行動裝置，手機及平板
        /// </summary>
        public bool MOBL { get; set; }

        /// <summary>
        /// 是否為測試環境
        /// </summary>
        public bool ENV_T { get; set; }


        public string UserAnget { get; set; }
    }

    /// <summary>修改密碼
    /// </summary>
    public class M_PasswordChg
    {
        /// <summary>
        /// 廠別
        /// </summary>
        public string FabId { get; set; }
        /// <summary>
        /// 使用者ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>使用者工號
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>登入密碼
        /// </summary>
        public string PasswordOld { get; set; }
        /// <summary>新密碼
        /// </summary>
        public string PasswordNew { get; set; }
        /// <summary>新密碼確認
        /// </summary>
        public string PasswordChk { get; set; }
    }

    /// <summary>功能清單
    /// </summary>
    public class M_Menu
    {
        public List<M_Webpage> Menus { get; set; }
    }

    /// <summary>功能網址
    /// </summary>
    public class M_Webpage
    {
        /// <summary>網址
        /// </summary>
        public string Url { get; set; }
        /// <summary>Area Name
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>Controller Name
        /// </summary>
        public string CtrlName { get; set; }
        /// <summary>Action Name
        /// </summary>
        public string ActnName { get; set; }
        /// <summary>網頁名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>類型一
        /// </summary>
        public string Type1 { get; set; }
        /// <summary>類型二
        /// </summary>
        public string Type2 { get; set; }
        /// <summary>權限
        /// </summary>
        public Authority Authority { get; set; }
    }

    public static class FM_SYS
    {
        /// <summary>判斷是否為手機裝置，非平板</summary>
        /// <returns></returns>
        public static bool IsPhoneDevice(HttpRequestBase Request)
        {
            if (Request.ServerVariables["HTTP_USER_AGENT"] == null) return true;
            string u = Request.ServerVariables["HTTP_USER_AGENT"];

            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return (b.IsMatch(u) || v.IsMatch(u.Substring(0, 4)));
        }

        /// <summary>判斷是否為行動裝置，含平板</summary>
        /// <returns></returns>
        public static bool IsMobileDevice(HttpRequestBase Request)
        {
            if (Request.ServerVariables["HTTP_USER_AGENT"] == null) return true;
            string UserAnget = Request.ServerVariables["HTTP_USER_AGENT"];

            string[] mobiles = new string[]
            {
                "midp", "j2me", "avant", "docomo", "novarra", "palmos", "palmsource",
                "240x320", "opwv", "chtml","pda", "windows ce", "mmp/",
                "blackberry", "mib/", "symbian", "wireless", "nokia", "hand", "mobi",
                "phone", "cdm", "up.b", "audio", "sie-", "sec-", "samsung", "htc",
                "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx",
                "NEC", "philips", "mmm", "xx", "panasonic", "sharp", "wap", "sch",
                "rover", "pocket", "benq", "java", "pt", "pg", "vox", "amoi",
                "bird", "compal", "kg", "voda","sany", "kdd", "dbt", "sendo",
                "sgh", "gradi", "jb", "dddi", "moto", "iphone", "android",
                "iPod", "incognito", "webmate", "dream", "cupcake", "webos",
                "s8000", "bada", "googlebot-mobile"
            };

            if (string.IsNullOrEmpty(UserAnget))
                return false;

            foreach (var item in mobiles)
            {
                if (UserAnget.ToLower().IndexOf(item) != -1) return true;
            }
            return false;
        }

        /// <summary>檢測瀏覽器</summary>
        /// <returns>瀏覽器資訊字串</returns>
        public static string DetectBrowser(HttpRequestBase Request)
        {
            var browser = Request.Browser;
            //string s = "Browser Capabilities\n"
            //    + "Type = " + browser.Type + "\n"
            //    + "Name = " + browser.Browser + "\n"
            //    + "Version = " + browser.Version + "\n"
            //    + "Major Version = " + browser.MajorVersion + "\n"
            //    + "Minor Version = " + browser.MinorVersion + "\n"
            //    + "Platform = " + browser.Platform + "\n"
            //    + "Is Beta = " + browser.Beta + "\n"
            //    + "Is Crawler = " + browser.Crawler + "\n"
            //    + "Is AOL = " + browser.AOL + "\n"
            //    + "Is Win16 = " + browser.Win16 + "\n"
            //    + "Is Win32 = " + browser.Win32 + "\n"
            //    + "Supports Frames = " + browser.Frames + "\n"
            //    + "Supports Tables = " + browser.Tables + "\n"
            //    + "Supports Cookies = " + browser.Cookies + "\n"
            //    + "Supports VBScript = " + browser.VBScript + "\n"
            //    + "Supports JavaScript = " +
            //        browser.EcmaScriptVersion.ToString() + "\n"
            //    + "Supports Java Applets = " + browser.JavaApplets + "\n"
            //    + "Supports ActiveX Controls = " + browser.ActiveXControls
            //          + "\n"
            //    + "Supports JavaScript Version = " +
            //        browser["JavaScriptVersion"] + "\n";
            string s = "Browser Capabilities" + Environment.NewLine
                + "Type = " + browser.Type + Environment.NewLine
                + "Name = " + browser.Browser + Environment.NewLine
                + "Version = " + browser.Version + Environment.NewLine
                + "Major Version = " + browser.MajorVersion + Environment.NewLine
                + "Minor Version = " + browser.MinorVersion + Environment.NewLine
                + "Platform = " + browser.Platform + Environment.NewLine
                + "Is Beta = " + browser.Beta + Environment.NewLine
                + "Is Crawler = " + browser.Crawler + Environment.NewLine
                + "Is AOL = " + browser.AOL + Environment.NewLine
                + "Is Win16 = " + browser.Win16 + Environment.NewLine
                + "Is Win32 = " + browser.Win32 + Environment.NewLine
                + "Supports Frames = " + browser.Frames + Environment.NewLine
                + "Supports Tables = " + browser.Tables + Environment.NewLine
                + "Supports Cookies = " + browser.Cookies + Environment.NewLine
                + "Supports VBScript = " + browser.VBScript + Environment.NewLine
                + "Supports JavaScript = " + browser.EcmaScriptVersion.ToString() + Environment.NewLine
                + "Supports Java Applets = " + browser.JavaApplets + Environment.NewLine
                + "Supports ActiveX Controls = " + browser.ActiveXControls + Environment.NewLine
                + "Supports JavaScript Version = " + browser["JavaScriptVersion"];
            return s;
        }

        /// <summary>取得操作權限</summary>
        /// <param name="UserNo">工號</param>
        /// <param name="Function">網頁名稱</param>
        /// <returns></returns>
        public static List<Authority> QueryAuthority(string UserNo, string Function)
        {
            List<Authority> oData = new List<Authority>();
            try
            {
                string SQL = string.Format(@"SELECT DISTINCT AUTHORITYS,AUTH_SEQ FROM(
SELECT P.FUNCTION,P.AUTHORITYS,A.AUTH_SEQ FROM SAJET.SYS_EMP E,SAJET.SYS_ROLE_EMP R,SAJET.SYS_ROLE_PRIVILEGE P,SAJET.SYS_PROGRAM_FUN_AUTHORITY A
WHERE E.EMP_ID = R.EMP_ID(+) AND R.ROLE_ID = P.ROLE_ID(+) AND P.PROGRAM=A.PROGRAM(+) AND P.FUNCTION=A.FUNCTION(+) AND P.AUTHORITYS=A.AUTHORITYS(+) 
AND E.EMP_NO='{0}' AND P.PROGRAM LIKE 'WEB%' AND P.FUNCTION='{1}'
UNION 
SELECT PF.FUNCTION,'Read Only' AUTHORITYS,0 AUTH_SEQ FROM SAJET.SYS_PROGRAM_FUN_NAME PF
WHERE PF.SHOW_FLAG='1' AND PF.WEB_FLAG = 'Y' AND PF.FUNCTION='{1}'
) ORDER BY AUTH_SEQ
", UserNo, Function);
                DataTable dt = M_DB.dsMes.GetTable_SqlCmd(SQL);

                if (dt?.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace(dr["AUTH_SEQ"].ToString()))
                        {
                            Authority _auth = (Authority)Enum.Parse(typeof(Authority), dr["AUTH_SEQ"].ToString());
                            oData.Add(_auth);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return oData;
        }

        /// <summary>
        /// 操作紀錄
        /// </summary>
        /// <param name="OR"></param>
        /// <returns></returns>
        public static bool Operation_Record(OperationRecord OR)
        {
            if (!string.IsNullOrWhiteSpace(OR.PROGRAM) && !string.IsNullOrWhiteSpace(OR.FUNCTION) && !string.IsNullOrWhiteSpace(OR.USER_ID))
            {
                string sql = string.Format(@"INSERT INTO CAMEO_PROGRAM_FUN_TRAVEL (PROGRAM,FUNCTION,DLL_FILENAME,FUN_PARAM,USER_ID,USE_TYPE,REMORK) 
VALUES('{0}','{1}','{2}','{3}',{4},'{5}','{6}')", OR.PROGRAM, OR.FUNCTION, OR.DLL_FILENAME, OR.FUN_PARAM, OR.USER_ID, OR.USE_TYPE.ToString(), OR.REMORK);

                return M_DB.dsMes.Execute_SqlCmd(sql) > 0;
            }
            else return false;
        }

        /// <summary>
        /// 轉換 DataTable 之標頭
        /// </summary>
        /// <param name="dtOriginal">資料來源</param>
        /// <param name="ColNameOriginal">要轉換的欄位名稱</param>
        /// <param name="ColNameNewly">新的欄位名稱</param>
        /// <returns>轉換後的 DataTable</returns>
        public static DataTable DataTable_ColNameChange(DataTable dtOriginal, List<string> ColNameOriginal, List<string> ColNameNewly)
        {
            DataTable dtNewly = dtOriginal.Clone();

            List<int> RemoveIndex = new List<int>();
            try
            {
                for (int i = dtNewly.Columns.Count - 1; i >= 0; i--)
                {
                    bool remove = true;
                    foreach (string ColName in ColNameOriginal)
                    {
                        if (dtNewly.Columns[i].ColumnName == ColName)
                        {
                            remove = false;
                            break;
                        }
                    }
                    if (remove) RemoveIndex.Add(i);
                }
                foreach (int _index in RemoveIndex)
                {
                    dtNewly.Columns.RemoveAt(_index);
                }
                foreach (DataRow dr in dtOriginal.Rows)
                {
                    dtNewly.ImportRow(dr);
                }
                for (int i = 0; i < ColNameOriginal.Count; i++)
                {
                    dtNewly.Columns[ColNameOriginal[i]].ColumnName = ColNameNewly[i];
                }
            }
            catch { dtNewly = dtOriginal.Copy(); }
            return dtNewly;
        }
    }
}