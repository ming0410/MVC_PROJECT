using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameoMvc.Areas.MFG.Models
{
    /// <summary>
    /// 參數模組
    /// </summary>
    public class M_MFG003
    {
        public string FUNC { get; set; } = "MFG003";
        public string FUNC_NAME { get; } = "非生產之小分類定義";

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
        public QM_MFG003 QM { get; set; } = new QM_MFG003();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_MFG003> LIST_RM { get; set; } = new List<RM_MFG003>();

        /// <summary>
        /// 大類下拉清單
        /// </summary>
        public IEnumerable<SelectListItem> DLL_TYPE { get; set; }
        public string UPDATE_TIME { get; set; }
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_MFG003
    {
        /// <summary>
        /// 異常大類
        /// </summary>
        public string TYPE_NAME { get; set; }
        /// <summary>
        /// 異常小類
        /// </summary>
        public string ITEM_NAME { get; set; }
    }

    /// <summary>
    /// 結果欄位
    /// </summary>
    public class RM_MFG003
    {        
        /// <summary>
        /// 大類名稱
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
        /// <summary>
        /// 小類名稱
        /// </summary>
        public string ITEM_NAME { get; set; }
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_MFG003
    {
        /// <summary>查詢</summary>
        /// <param name="qData">查詢資料</param>
        /// <returns></returns>
        public M_MFG003 QUERY(M_MFG003 iData)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;

            M_MFG003 oData = iData;
            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_MFG003>();

            try
            {
                #region 大類下拉清單
                if (oData.DLL_TYPE == null)
                {
                    List<SelectListItem> Dll = new List<SelectListItem>();

                    DataTable dtLine = M_DB.dsMes.GetTable_SqlCmd(SQL_QUERY_GET());
                    if (dtLine?.Rows?.Count > 0)
                    {
                        foreach (DataRow drLine in dtLine.Rows)
                        {
                            Dll.Add(new SelectListItem
                            {
                                Text = drLine["TYPE_NAME"].ToString(),
                                Value = drLine["TYPE_NAME"].ToString(),
                                Selected = false
                            });
                        }
                    }
                    oData.DLL_TYPE = Dll;

                    if (string.IsNullOrWhiteSpace(oData.QM.TYPE_NAME) && Dll.Count > 0)
                        oData.QM.TYPE_NAME = Dll[0].Value;
                }
                #endregion

                DataTable dtQ = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(oData.QM.TYPE_NAME, oData.QM.ITEM_NAME));
                if (dtQ?.Rows?.Count > 0)
                {
                    RM_MFG003 rm = null;
                    if (dtQ?.Rows?.Count > 0)
                    {
                        foreach (DataRow dr in dtQ.Rows)
                        {
                            rm = new RM_MFG003();
                            rm.TYPE_NAME = dr["TYPE_NAME"].ToString();
                            rm.ITEM_NAME = dr["ITEM_NAME"].ToString();
                            rm.UPDATE_USERID = dr["UPDATE_USERID"].ToString();
                            rm.UPDATE_TIME = dr["UPDATE_TIME"].ToString();
                            oData.LIST_RM.Add(rm);
                        }
                    }                
                    return oData;
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
        /// <param name="iQM"></param>
        /// <returns></returns>
        public bool SAVE(QM_MFG003 iQM, string UserId, out string ERR_MSG)
        {
            bool res = false;
            string FUNC = "SAVE";
            ERR_MSG = string.Empty;

            try
            {
                #region 確認填入資料完整性
                if (string.IsNullOrWhiteSpace(iQM.TYPE_NAME))
                {
                    ERR_MSG = "資料不完整，無法儲存" + Environment.NewLine + "沒有選擇異常大類";
                }
                else if (string.IsNullOrWhiteSpace(iQM.ITEM_NAME))
                {
                    ERR_MSG = "資料不完整，無法儲存" + Environment.NewLine + "沒有填異常小類";
                }
                #endregion

                if (string.IsNullOrWhiteSpace(ERR_MSG))
                {                    
                    #region 儲存作業
                    //判斷資料是否存在
                    DataTable dtExist = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_ROWCOUNT(iQM.TYPE_NAME, iQM.ITEM_NAME));
                    if (int.Parse(dtExist.Rows[0][0].ToString()) > 0)
                    {                        
                        ERR_MSG = "此異常已存在";
                    }
                    else
                    {
                        //執行 Insert
                        if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_INS(iQM.TYPE_NAME, iQM.ITEM_NAME, UserId)) <= 0)
                        {
                            ERR_MSG = "無法新增紀錄";
                        }
                        else
                        {
                            res = true;
                        }
                    }
                        
                    #endregion
                    
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
        /// 刪除
        /// </summary>
        /// <param name="iData"></param>
        /// /// <param name="ERR_MSG"></param>
        /// <returns></returns>
        public bool DELETE(QM_MFG003 iQM, out string ERR_MSG)
        {
            bool res = false;
            string FUNC = "DELETE";
            ERR_MSG = string.Empty;
            string SqlWhere = string.Empty;
            string PartId = string.Empty;

            try
            {
                #region 確認是否有選擇項目
                if (string.IsNullOrWhiteSpace(iQM.TYPE_NAME))
                {
                    ERR_MSG = "資料不完整，無法儲存" + Environment.NewLine + "沒有選擇異常大類";
                }
                else if (string.IsNullOrWhiteSpace(iQM.ITEM_NAME))
                {
                    ERR_MSG = "資料不完整，無法儲存" + Environment.NewLine + "沒有填異常小類";
                }
                else
                {
                    #region 判斷是否存在
                    DataTable dt = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_ROWCOUNT(iQM.TYPE_NAME, iQM.ITEM_NAME));
                    if (int.Parse(dt.Rows[0][0].ToString()) > 0)
                    {
                        //存在
                        #region 判斷是否已使用
                        DataTable dt1 = M_DB.dsMes.GetTable_SqlCmd(SQL_USE_ROWCOUNT(iQM.TYPE_NAME, iQM.ITEM_NAME));
                        if (int.Parse(dt1.Rows[0][0].ToString()) > 0)
                        {
                            ERR_MSG = "此關聯已使用不可刪除";
                        }
                        else
                        {
                            //存在且未使用可執行 刪除
                            if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_DEL(iQM.TYPE_NAME, iQM.ITEM_NAME)) <= 0)
                            {
                                ERR_MSG = "無法刪除紀錄";
                            }
                            else
                            {
                                res = true;
                            }
                        }
                        #endregion

                    }
                    else
                    {
                        ERR_MSG = string.Format("系統沒有此關聯", iQM.ITEM_NAME);
                    }

                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                ERR_MSG = "例外錯誤"
                    + Environment.NewLine + string.Format("功能:{0}", FUNC)
                    + Environment.NewLine + string.Format("訊息:{0}", ex.Message.Replace("'", "").Replace("\r\n", ""));
            }

            return res;
        }

        #region SQL

        /// <summary>
        /// GET 小類清單
        /// </summary>
        /// <param name="TYPE_NAME">大類</param>
        /// <param name="ITEM_NAME">小類</param>
        /// <returns></returns>
        public string SQL_DATA_GET(string TYPE_NAME, string ITEM_NAME)
        {
            return string.Format(@"
SELECT TYPE_NAME,ITEM_NAME,SAJET.F_GET_EMP_NAME(UPDATE_USERID) AS UPDATE_USERID , TO_CHAR(UPDATE_TIME,'YYYY/MM/DD HH24:MI:SS') AS UPDATE_TIME 
FROM SAJET.CAMEO_MSWH_ITEM WHERE TYPE_NAME LIKE '{0}%' AND ITEM_NAME LIKE '{1}%'
", TYPE_NAME, ITEM_NAME);
        }

        /// <summary>
        /// 新增小類關聯
        /// </summary>
        /// <param name="TYPE_NAME">大類</param>
        /// <param name="ITEM_NAME">小類</param>
        /// <param name="UPDATE_USERID">更新人員</param>
        /// <returns></returns>
        public string SQL_DATA_INS(string TYPE_NAME, string ITEM_NAME,string UPDATE_USERID)
        {
            return string.Format("INSERT INTO SAJET.CAMEO_MSWH_ITEM (TYPE_NAME,ITEM_NAME,UPDATE_USERID) VALUES ('{0}','{1}','{2}')", TYPE_NAME, ITEM_NAME, UPDATE_USERID);
        }

        /// <summary>
        /// 刪除小類關聯
        /// </summary>
        /// <param name="TYPE_NAME">大類</param>
        /// <param name="ITEM_NAME">小類</param>
        /// <returns></returns>
        public string SQL_DATA_DEL(string TYPE_NAME, string ITEM_NAME)
        {
            return string.Format("DELETE FROM SAJET.CAMEO_MSWH_ITEM WHERE TYPE_NAME='{0}' AND ITEM_NAME='{1}'", TYPE_NAME, ITEM_NAME);
        }

        /// <summary>
        /// 異常大類下拉清單
        /// </summary>
        /// <returns></returns>
        public string SQL_QUERY_GET()
        {
            return "SELECT TYPE_NAME FROM SAJET.CAMEO_MSWH_TYPE";
        }

        /// <summary>
        /// 條件下的資料是否存在
        /// </summary>
        /// <param name="TYPE_NAME">大類</param>
        /// <param name="ITEM_NAME">小類</param>
        /// <returns>SQL語法</returns>
        public string SQL_DATA_ROWCOUNT(string TYPE_NAME, string ITEM_NAME)
        {
            return string.Format("SELECT COUNT(ITEM_NAME) CNT FROM SAJET.CAMEO_MSWH_ITEM WHERE TYPE_NAME='{0}' AND ITEM_NAME='{1}'", TYPE_NAME, ITEM_NAME);
        }

        /// <summary>
        /// 此異常分類是否使用
        /// </summary>
        /// <param name="TYPE_NAME">大類</param>
        /// <param name="ITEM_NAME">小類</param>
        /// <returns>SQL語法</returns>
        public string SQL_USE_ROWCOUNT(string TYPE_NAME, string ITEM_NAME)
        {
            return string.Format("SELECT COUNT(ITEM_NAME) CNT FROM SAJET.CAMEO_MSWH WHERE TYPE_NAME='{0}' AND ITEM_NAME='{1}'", TYPE_NAME, ITEM_NAME);
        }


        #endregion
    }
}