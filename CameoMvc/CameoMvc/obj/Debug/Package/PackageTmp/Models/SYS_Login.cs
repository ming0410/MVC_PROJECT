using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace CameoMvc.Models
{
    public class SYS_Login
    {
        public bool FLAG { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ERR { get; set; }

       

        public string ERR_KEY { get; set; }
        public string ERR_MSG { get; set; }
        public LoginUser UserInfo { get; set; } = new LoginUser();
        public List<LoginUser> oUSER_LIST { get; set; } = new List<LoginUser>();

        public SYS_Login CheckLogin(M_LOGIN IN_INFO)
        {
            string sErrKey = string.Empty;
            string sErrMsg = string.Empty;
            SYS_Login OUT_INFO = new SYS_Login();
            try
            {
                OUT_INFO.FLAG = false;

                if (string.IsNullOrEmpty(IN_INFO.USER_NO))
                {
                    sErrKey = "USER_NO";
                    sErrMsg = "使用者工號不可為空值";
                    goto LABEL_FINAL;
                }
                // 判斷是否為訪客
                if (IN_INFO.USER_NO.ToUpper() == User_Guest.UserNo.ToUpper() && IN_INFO.PASSWORD == User_Guest.Password)
                {
                    OUT_INFO.UserInfo = new LoginUser()
                    {
                        FabId = User_Guest.FabId,
                        UserId = User_Guest.UserId,
                        UserNo = User_Guest.UserNo,
                        UserName = User_Guest.User,
                        Password = User_Guest.Password,
                        RoleId = User_Guest.RoleId
                    };
                    goto LABEL_FINAL;
                }
                // 判斷是否為最高權限
                if (IN_INFO.USER_NO.ToUpper() == User_Admin.UserNo.ToUpper() && IN_INFO.PASSWORD == User_Admin.Password)
                {
                    OUT_INFO.UserInfo = new LoginUser()
                    {
                        FabId = User_Admin.FabId,
                        UserId = User_Admin.UserId,
                        UserNo = User_Admin.UserNo,
                        UserName = User_Admin.User,
                        Password = User_Admin.Password,
                        RoleId = User_Admin.RoleId
                    };
                    goto LABEL_FINAL;
                }

                DataTable dt = M_DB.dsMes.GetTable_SqlCmd(sql_emp_get(IN_INFO.USER_NO.ToUpper()));

                if (dt?.Rows.Count > 0)
                {
                    List<RoleAuthority> roles = new List<RoleAuthority>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace(dr["PROGRAM"].ToString()) && !string.IsNullOrWhiteSpace(dr["FUNCTION"].ToString()) && !string.IsNullOrWhiteSpace(dr["AUTHORITYS"].ToString()))
                        {
                            roles.Add(new RoleAuthority()
                            {
                                Program = dr["PROGRAM"].ToString(),
                                WebpageFun = dr["FUNCTION"].ToString(),
                                Authority = dr["AUTHORITYS"].ToString()
                            });
                        }
                    }

                    OUT_INFO.UserInfo = new LoginUser()
                    {
                        FabId = SystemDefault.FabId,
                        UserId = int.Parse(dt.Rows[0]["EMP_ID"].ToString()),
                        UserNo = dt.Rows[0]["EMP_NO"].ToString(),
                        UserName = dt.Rows[0]["EMP_NAME"].ToString(),
                        Password = dt.Rows[0]["PWD"].ToString(),
                        RoleId = int.Parse(dt.Rows[0]["ROLE_ID"].ToString()),
                        Authoritys = roles
                    };
                }
                else
                {
                    sErrKey = "USER_NO";
                    sErrMsg = "沒有該使用者";
                    goto LABEL_FINAL;
                }

                if (OUT_INFO.UserInfo.Password != IN_INFO.PASSWORD)
                {
                    sErrKey = "PASSWORD";
                    sErrMsg = "密碼錯誤";
                    goto LABEL_FINAL;
                }
                
                //if (OUT_INFO.UserInfo.Authoritys?.Count <= 0)
                //{
                //    sErrKey = "UserNo";
                //    sErrMsg = "該使用者尚未設定權限";
                //    goto LABEL_FINAL;
                //}

            LABEL_FINAL:
                if (sErrMsg.Length <= 0)
                {
                    OUT_INFO.FLAG = true;
                }
                else
                {
                    OUT_INFO.ERR_KEY = sErrKey;
                    OUT_INFO.ERR_MSG = sErrMsg;
                }
            }
            catch (Exception ex)
            {
                sErrKey = "USER_NO";
                sErrMsg = ex.Message;
            }
            finally
            {
                if (sErrMsg.Length > 0)
                {
                    OUT_INFO.ERR_MSG = sErrMsg;
                }
            }
            return OUT_INFO;
        }

        /// <summary>修改密碼</summary>
        /// <param name="IN_INFO"></param>
        /// <returns></returns>
        public SYS_Login ModifyPwd(M_PasswordChg IN_INFO)
        {
            string sFuncName = "ModifyPwd";
            string sErrMsg = string.Empty;
            SYS_Login OUT_INFO = new SYS_Login();
            try
            {
                OUT_INFO.FLAG = false;

                // 判斷是否為最高權限
                if (IN_INFO.UserNo.ToUpper() == User_Admin.UserNo.ToUpper())
                {
                    sErrMsg = string.Format("錯誤 \\r\\n 功能:{0} \\r\\n 訊息:{1}", sFuncName, "系統預設最高權限無法修改");
                }
                else
                {
                    //取得現行帳號密碼
                    DataTable dt = M_DB.dsMes.GetTable_SqlCmd(sql_emp_get(IN_INFO.UserNo.ToUpper()));
                    if (dt.Rows.Count <= 0)
                    {
                        sErrMsg = string.Format("錯誤 \\r\\n 功能:{0} \\r\\n 訊息:{1}", sFuncName, "查詢無使用者");
                    }
                    else
                    {
                        OUT_INFO.UserInfo.Password = dt.Rows[0]["PWD"].ToString();
                        if (OUT_INFO.UserInfo.Password != IN_INFO.PasswordOld)
                        {
                            sErrMsg = string.Format("錯誤 \\r\\n 功能:{0} \\r\\n 訊息:{1}", sFuncName, "密碼錯誤");
                        }
                        else
                        {
                            if(IN_INFO.PasswordNew != IN_INFO.PasswordChk)
                            {
                                sErrMsg = string.Format("錯誤 \\r\\n 功能:{0} \\r\\n 訊息:{1}", sFuncName, "新密碼與新密碼確認不符");
                            }
                            else
                            {
                                if (M_DB.dsMes.Execute_SqlCmd(sql_pw_upd(IN_INFO.UserNo.ToUpper(), IN_INFO.PasswordNew)) <= 0)
                                {
                                    sErrMsg = string.Format("錯誤 \\r\\n 功能:{0} \\r\\n 訊息:{1}", sFuncName, "修改密碼失敗");
                                }
                            }
                        }
                    }
                }
                if (sErrMsg.Length <= 0)
                {
                    OUT_INFO.FLAG = true;
                }
            }
            catch (Exception ex)
            {
                sErrMsg = string.Format("例外錯誤 \\r\\n 功能:{0} \\r\\n 訊息:{1}", sFuncName, ex.Message.Replace("'", "").Replace("\r\n", ""));
            }
            finally
            {
                if (sErrMsg.Length > 0)
                {
                    OUT_INFO.ERR_MSG = sErrMsg;
                }
            }
            return OUT_INFO;
        }

        /// <summary>取得左側選單內容</summary>
        /// <param name="UserNo"></param>
        /// <returns></returns>
        public M_Menu QueryMenuList(string UserNo)
        {
            M_Menu OUT_INFO = new M_Menu();
            OUT_INFO.Menus = new List<M_Webpage>();
            try
            {
                DataTable dt = M_DB.dsMes.GetTable_SqlCmd(sql_menu_get(UserNo));

                if (dt?.Rows.Count > 0)
                {
                    List<M_Webpage> meuns = new List<M_Webpage>();
                    M_Webpage web = null;


                    string PRO_IDX = string.Empty, TYPE_IDX = string.Empty, FUN_IDX = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        web = new M_Webpage();
                        web.Url = dr["DLL_FILENAME"].ToString();
                        string[] urls = web.Url.Split('/');
                        if (urls.Length == 3)
                        {
                            web.AreaName = urls[0];
                            web.CtrlName = urls[1];
                            web.ActnName = urls[2];
                        }
                        else if (urls.Length == 2)
                        {
                            web.CtrlName = urls[0];
                            web.ActnName = urls[1];
                        }
                        else if (urls.Length == 1)
                        {
                            web.CtrlName = "Home";
                            web.ActnName = urls[0];
                        }

                        string authority = "0";
                        if (dr["AUTHORITYS"].ToString().Contains("2"))
                        {
                            authority = "2";
                        }
                        else
                        {
                            string[] authoritys = dr["AUTHORITYS"].ToString().Split(',');
                            authority = authoritys[authoritys.Length - 1];
                        }   
                        web.Authority = (Authority)Enum.Parse(typeof(Authority), authority);

                        web.Name = dr["FUN_TW"].ToString();         //多語 FUN_ENG
                        web.Type1 = dr["PROGRAM_TW"].ToString();    //多語 PROGRAM_ENG
                        web.Type2 = dr["FUN_TYPE_TW"].ToString();   //多語 FUN_TYPE_ENG
                        OUT_INFO.Menus.Add(web);
                    }
                }
            }
            catch (Exception ex)
            {
                OUT_INFO = null;
            }
            return OUT_INFO;
        }

        private string sql_emp_get(string UserNo)
        {
            return string.Format(@"
SELECT TRIM(SAJET.PASSWORD.DECRYPT(E.PASSWD)) PWD,E.EMP_ID,E.EMP_NO,E.EMP_NAME,E.EMAIL,R.ROLE_ID,P.PROGRAM,P.FUNCTION,P.AUTHORITYS 
FROM SAJET.SYS_EMP E,SYS_ROLE_EMP R,SYS_ROLE_PRIVILEGE P 
WHERE E.EMP_ID=R.EMP_ID(+) AND R.ROLE_ID=P.ROLE_ID(+) AND E.FACTORY_ID={0} AND E.ENABLED='{1}' AND E.EMP_NO='{2}' 
ORDER BY PROGRAM,FUNCTION,AUTHORITYS"
, SystemDefault.FabId, SystemDefault.Enable, UserNo);
        }

        private string sql_pw_upd(string UserNo, string Password)
        {
            return string.Format(@"UPDATE SAJET.SYS_EMP SET PASSWD=TRIM(SAJET.PASSWORD.ENCRYPT('{3}')) WHERE FACTORY_ID={0} AND ENABLED='{1}' AND EMP_NO='{2}'", SystemDefault.FabId, SystemDefault.Enable, UserNo, Password);
        }

        private string sql_menu_get(string UserNo)
        {
            return string.Format(@"
SELECT t.PRO_IDX,t.TYPE_IDX,t.FUN_IDX,t.PROGRAM_TW,t.PROGRAM_ENG,t.FUN_TYPE,t.FUN_TYPE_TW,t.FUN_TYPE_ENG,t.FUN_TW,t.FUN_ENG,t.DLL_FILENAME,
LISTAGG (t.AUTHORITY,',') within GROUP (order by t.AUTHORITY) as AUTHORITYS
FROM(
SELECT P.FUN_TYPE_IDX PRO_IDX, PF.FUN_TYPE_IDX TYPE_IDX, PF.FUN_IDX,P.PROGRAM_TW,P.PROGRAM_ENG,PF.FUN_TYPE,PF.FUN_TYPE_TW,PF.FUN_TYPE_ENG,PF.FUN_TW,PF.FUN_ENG,PF.DLL_FILENAME
,DECODE(RP.AUTHORITYS,'Read Only',0,'Allow To Change',1,'Full Control',2,'Allow To Execute',3) AUTHORITY
FROM SAJET.SYS_EMP E, SAJET.SYS_ROLE_EMP R, SAJET.SYS_ROLE_PRIVILEGE RP, SAJET.SYS_PROGRAM_FUN_NAME PF, SAJET.SYS_PROGRAM_NAME P
WHERE E.EMP_ID = R.EMP_ID(+) AND R.ROLE_ID = RP.ROLE_ID(+) AND RP.PROGRAM = PF.PROGRAM(+) AND RP.FUNCTION = PF.FUNCTION(+) AND RP.PROGRAM = P.PROGRAM(+)
AND E.FACTORY_ID={0} AND E.ENABLED='{1}' AND E.EMP_NO = '{2}' AND PF.ENABLED = 'Y' AND PF.WEB_FLAG = 'Y'
UNION
SELECT P.FUN_TYPE_IDX PRO_IDX, PF.FUN_TYPE_IDX TYPE_IDX, PF.FUN_IDX,P.PROGRAM_TW,P.PROGRAM_ENG,PF.FUN_TYPE,PF.FUN_TYPE_TW,PF.FUN_TYPE_ENG,PF.FUN_TW,PF.FUN_ENG,PF.DLL_FILENAME
,0 AUTHORITY 
FROM SAJET.SYS_PROGRAM_FUN_NAME PF, SAJET.SYS_PROGRAM_NAME P
WHERE PF.PROGRAM = P.PROGRAM(+) AND PF.SHOW_FLAG='1' AND PF.ENABLED = 'Y' AND PF.WEB_FLAG = 'Y'
) t
GROUP BY t.PRO_IDX,t.TYPE_IDX,t.FUN_IDX,t.PROGRAM_TW,t.PROGRAM_ENG,t.FUN_TYPE,t.FUN_TYPE_TW,t.FUN_TYPE_ENG,t.FUN_TW,t.FUN_ENG,t.DLL_FILENAME
ORDER BY PRO_IDX,TYPE_IDX,FUN_IDX"
, SystemDefault.FabId, SystemDefault.Enable, UserNo);
        }
    }
}