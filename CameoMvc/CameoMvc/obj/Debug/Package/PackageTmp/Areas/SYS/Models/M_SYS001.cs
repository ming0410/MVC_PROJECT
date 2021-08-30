using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CameoMvc.Areas.SYS.Models
{

    /// <summary>
    /// 參數模組
    /// </summary>
    public class M_SYS001
    {
        public string FUNC { get; } = "SYS001";
        public string FUNC_NAME { get; } = "個人基本資料";

        #region 基本參數
        public bool FLAG { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ERR { get; set; }

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

        /// <summary>
        /// 廠別ID
        /// </summary>
        public string FAB_ID { get; set; }

        /// <summary>
        /// 使用者ID
        /// </summary>
        public string USER_ID { get; set; }

        /// <summary>
        /// 使用者工號
        /// </summary>
        public string USER_NO { get; set; }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        public string USER_NAME { get; set; }

        /// <summary>
        /// 使用者權限
        /// </summary>
        public List<Authority> AUTHORITY { get; set; }
        #endregion

        /// <summary>
        /// 查詢欄位內容
        /// </summary>
        public RM_SYS001 QM { get; set; } = new RM_SYS001();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_SYS001> LIST_RM { get; set; } = new List<RM_SYS001>();
    }

    /// <summary>
    /// 結果欄位
    /// </summary>
    public class RM_SYS001
    {

        /// <summary>使用者ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>使用者工號
        /// </summary>
        public string NUMBER { get; set; }
        /// <summary>使用者姓名
        /// </summary>
        public string NAME { get; set; }
        /// <summary>登入密碼
        /// </summary>
        public string PW_OLD { get; set; }
        /// <summary>新密碼
        /// </summary>
        public string PW_NEW { get; set; }
        /// <summary>新密碼確認
        /// </summary>
        public string PW_CHK { get; set; }
        /// <summary>
        /// 部門
        /// </summary>
        public string DEPT { get; set; }
        /// <summary>
        /// 使用者影像
        /// </summary>
        public string PHOTO { get; set; }
        /// <summary>
        /// 電子信箱
        /// </summary>
        public string EMAIL { get; set; }
        /// <summary>使用者權限
        /// </summary>
        public Authority AUTH { get; set; }
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_SYS001
    {
        private M_SYS001 Initial(M_SYS001 IN_INFO)
        {
            M_SYS001 OUT_INFO = new M_SYS001();
            return OUT_INFO;
        }

        /// <summary>查詢</summary>
        /// <param name="qData">查詢資料</param>
        /// <returns></returns>
        public M_SYS001 QUERY(M_SYS001 iData)
        {
            string FUNC = "Query";
            string ERR_MSG = string.Empty;
            M_SYS001 oData = new M_SYS001();
            try
            {
                DataTable dt = M_DB.dsMes.GetTable_SqlCmd(sql_get(iData.FAB_ID, iData.USER_NO, iData.USER_NAME));

                oData.QM = new RM_SYS001();
                oData.QM.ID = dt.Rows[0]["EMP_ID"].ToString();
                oData.QM.NUMBER = dt.Rows[0]["EMP_NO"].ToString();
                oData.QM.NAME = dt.Rows[0]["EMP_NAME"].ToString();
                oData.QM.DEPT = dt.Rows[0]["DEPT_NAME"].ToString();
                oData.QM.EMAIL = dt.Rows[0]["EMAIL"].ToString();
                oData.QM.AUTH = Authority.Full_Control;

                RM_SYS001 _data = null;
                foreach (DataRow dr in dt.Rows)
                {
                    _data = new RM_SYS001();
                    _data.ID = dr["EMP_ID"].ToString();
                    _data.NUMBER = dr["EMP_NO"].ToString();
                    _data.NAME = dr["EMP_NAME"].ToString();
                    _data.EMAIL = dr["EMAIL"].ToString();
                    if (oData.LIST_RM == null) oData.LIST_RM = new List<RM_SYS001>();
                    oData.LIST_RM.Add(_data);
                }
                oData.FLAG = true;
            }
            catch (Exception ex)
            {
                ERR_MSG = string.Format("例外錯誤 \\r\\n 功能:{0} \\r\\n 訊息:{1}", FUNC, ex.Message.Replace("'", "").Replace("\r\n", ""));
            }
            finally
            {
                if (ERR_MSG.Length > 0)
                {
                    oData.ERR = ERR_MSG;
                }
                else
                {
                    oData.FLAG = true;
                }
            }
            return oData;
        }
        public M_SYS001 Save(M_SYS001 IN_INFO)
        {
            M_SYS001 OUT_INFO = new M_SYS001();
            return OUT_INFO;
        }
        public M_SYS001 Delete(M_SYS001 IN_INFO)
        {
            M_SYS001 OUT_INFO = new M_SYS001();
            return OUT_INFO;
        }

        private string sql_get(string FabId, string UserNo, string Name)
        {
            return string.Format("SELECT E.EMP_ID,E.EMP_NO,E.EMP_NAME,E.EMAIL,P.DEPT_NAME,E.REMARK FROM SAJET.SYS_EMP E, SYS_DEPT P WHERE E.ENABLED = 'Y' AND E.FACTORY_ID = {0} AND E.EMP_NO LIKE '{1}%' AND E.EMP_NAME LIKE '{2}%' AND E.DEPT_ID=P.DEPT_ID", FabId, UserNo, Name);
        }
    }
}