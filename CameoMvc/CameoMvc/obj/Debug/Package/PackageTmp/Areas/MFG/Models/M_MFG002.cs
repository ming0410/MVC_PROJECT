using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CameoMvc.Areas.MFG.Models
{
    /// <summary>
    /// 參數模組
    /// </summary>
    public class M_MFG002
    {
        public string FUNC { get; set; } = "MFG002";
        public string FUNC_NAME { get; } = "非生產之大分類定義";

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
        public QM_MFG002 QM { get; set; } = new QM_MFG002();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_MFG002> LIST_RM { get; set; } = new List<RM_MFG002>();
        public string UPDATE_TIME { get; set; }
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_MFG002
    {
        /// <summary>
        /// 大類
        /// </summary>
        public string TYPE_NAME { get; set; }
    }

    /// <summary>
    /// 結果欄位
    /// </summary>
    public class RM_MFG002
    {
        /// <summary>
        /// 大類
        /// </summary>
        public string TYPE_NAME { get; set; }
        /// <summary>
        /// 異動人員
        /// </summary>
        public string UPDATE_USERID { get; set; }
        /// <summary>
        /// 異動時間
        /// </summary>
        public string UPDATE_TIME { get; set; }       
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_MFG002
    {
        /// <summary>查詢</summary>
        /// <param name="qData">查詢資料</param>
        /// <returns></returns>
        public M_MFG002 QUERY(M_MFG002 iData)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;

            M_MFG002 oData = iData;
            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_MFG002>();
            try
            {
                DataTable dtQ = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(iData.QM.TYPE_NAME));

                RM_MFG002 rm = null;
                if (dtQ?.Rows?.Count > 0)
                {
                    foreach (DataRow dr in dtQ.Rows)
                    {
                        rm = new RM_MFG002();
                        rm.TYPE_NAME = dr["TYPE_NAME"].ToString();
                        rm.UPDATE_USERID = dr["UPDATE_USERID"].ToString();
                        rm.UPDATE_TIME = dr["UPDATE_TIME"].ToString();
                        oData.LIST_RM.Add(rm);
                    }
                }
            }
            catch (Exception ex)
            {
                ERR_MSG = "例外錯誤"
                    + Environment.NewLine + string.Format("功能:{0}", FUNC)
                    + Environment.NewLine + string.Format("訊息:{0}", ex.Message.Replace("'", "").Replace("\r\n", ""));
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

        /// <summary>
        /// 儲存
        /// </summary>
        /// <param name="iData"></param>
        /// <returns></returns>
        public bool SAVE(QM_MFG002 iQM, string UserId, out string ERR_MSG)
        {
            bool res = false;
            string FUNC = "SAVE";
            ERR_MSG = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(iQM.TYPE_NAME))
                {
                    ERR_MSG = "未輸入[大類名稱]";
                }
                else
                {
                    //是否存在
                    DataTable dtExist = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(iQM.TYPE_NAME));
                    if (dtExist?.Rows?.Count > 0)
                    {
                        ERR_MSG = "此異常已存在";
                    }
                    else
                    {
                        //不存在 Insert
                        string sql = SQL_DATA_INS(iQM.TYPE_NAME, UserId);

                        int 新增的筆數 = M_DB.dsMes.Execute_SqlCmd(sql);

                        if (新增的筆數 <= 0)
                        {
                            ERR_MSG = "新增失敗";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ERR_MSG = "例外錯誤"
                    + Environment.NewLine + string.Format("功能:{0}", FUNC)
                    + Environment.NewLine + string.Format("訊息:{0}", ex.Message.Replace("'", "").Replace("\r\n", ""));
            }

            return res;
        }


        /// <summary>
        /// 歸還
        /// </summary>
        /// <param name="iData"></param>
        /// <returns></returns>
        public bool DELETE(QM_MFG002 iQM, out string ERR_MSG)
        {
            bool res = false;
            string FUNC = "DELETE";
            ERR_MSG = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(iQM.TYPE_NAME))
                {
                    ERR_MSG = "未輸入[大類名稱]";
                }
                else
                {
                    //是否存在
                    DataTable dtExist = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(iQM.TYPE_NAME));

                    if (dtExist?.Rows?.Count > 0)
                    {
                        //存在 才可以刪除
                        string sql = SQL_DATA_DEL(iQM.TYPE_NAME);

                        int 異動的筆數 = M_DB.dsMes.Execute_SqlCmd(sql);

                        if (異動的筆數 <= 0)
                        {
                            ERR_MSG = "Fail";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ERR_MSG = "例外錯誤"
                    + Environment.NewLine + string.Format("功能:{0}", FUNC)
                    + Environment.NewLine + string.Format("訊息:{0}", ex.Message.Replace("'", "").Replace("\r\n", ""));
            }

            return res;
        }

        /// <summary>
        /// 依 產品序號 取得 箱號 及 棧板號
        /// </summary>
        /// <param name="TYPE_NAME">大類名稱</param>
        /// <param name="ERR_MSG">錯誤訊息</param>
        /// <returns></returns>
        private bool ItemInfo_Get(string TYPE_NAME)
        {
            TYPE_NAME = string.Empty;
            string ERR_MSG = string.Empty;
            DataTable dt = M_DB.dsMes.GetTable_SqlCmd(SQL_QUERY_GET());
            if (dt?.Rows?.Count > 0)
            {
                if (dt.Rows.Count > 1)
                {
                    ERR_MSG = "有多筆資料";
                }
                TYPE_NAME = dt.Rows[0]["TYPE_NAME"].ToString();

                if (string.IsNullOrWhiteSpace(TYPE_NAME))
                {
                    ERR_MSG = "查無[大類名稱]資訊";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                ERR_MSG = string.Format("查無[大類名稱：{0}]紀錄", TYPE_NAME);
                return false;
            }
        }
        #region SQL

        /// <summary>
        /// GET 大類清單
        /// </summary>
        /// <param name="TYPE_NAME">大類</param>
        /// <returns></returns>
        public string SQL_DATA_GET(string TYPE_NAME)
        {
            string Where = "";
            if (!string.IsNullOrWhiteSpace(TYPE_NAME))
                Where = string.Format("WHERE TYPE_NAME='{0}'", TYPE_NAME);
            return "SELECT TYPE_NAME,SAJET.F_GET_EMP_NAME(UPDATE_USERID) as UPDATE_USERID , to_char(UPDATE_TIME,'yyyy/mm/dd hh24:mi:ss') as UPDATE_TIME FROM SAJET.CAMEO_MSWH_TYPE " + Where;
        }

        /// <summary>
        /// 新增大類
        /// </summary>
        /// <param name="TYPE_NAME">大類</param>
        /// <param name="UPDATE_USERID">更新人員</param>
        /// <returns></returns>
        public string SQL_DATA_INS(string TYPE_NAME, string UPDATE_USERID)
        {
            return string.Format("INSERT INTO SAJET.CAMEO_MSWH_TYPE (TYPE_NAME,UPDATE_USERID) VALUES ('{0}','{1}')", TYPE_NAME, UPDATE_USERID);
        }

        /// <summary>
        /// 刪除大類
        /// </summary>
        /// <param name="TYPE_NAME">大類</param>
        /// <returns></returns>
        public string SQL_DATA_DEL(string TYPE_NAME)
        {
            return string.Format("DELETE FROM SAJET.CAMEO_MSWH_TYPE WHERE TYPE_NAME='{0}'", TYPE_NAME);
        }

        /// <summary>
        /// 查詢所有大類
        /// </summary>
        /// <returns></returns>
        public string SQL_QUERY_GET()
        {
            return "SELECT TYPE_NAME,SAJET.F_GET_EMP_NAME(UPDATE_USERID) UPDATE_USERID ,TO_DATE(UPDATE_TIME,'YYYY/MM/DD HH24:MI:SS') UPDATE_TIME FROM SAJET.CAMEO_MSWH_TYPE";
        }

        #endregion
        
    }
}