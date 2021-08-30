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
    public class M_MFG001
    {
        public string FUNC { get; set; } = "MFG001";
        public string FUNC_NAME { get; } = "非生產之大項定義";

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
        public QM_MFG001 QM { get; set; } = new QM_MFG001();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_MFG001> LIST_RM { get; set; } = new List<RM_MFG001>();

        /// <summary>
        /// 大類下拉清單
        /// </summary>
        public IEnumerable<SelectListItem> DLL_TYPE { get; set; }
        /// <summary>
        /// 小類下拉清單
        /// </summary>
        public IEnumerable<SelectListItem> DLL_ITEM { get; set; }
        /// <summary>
        /// 線別下拉清單
        /// </summary>
        public IEnumerable<SelectListItem> DDL_PDLINE { get; set; }
        public string PDLINE { get; set; }
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_MFG001
    {
        /// <summary>
        /// 工單
        /// </summary>
        public string WO { get; set; }
        /// <summary>
        /// 大類
        /// </summary>
        public string TYPE_NAME { get; set; }
        /// <summary>
        /// 小類
        /// </summary>
        public string ITEM_NAME { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public string DATETIME_S { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public string DATETIME_E { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
        public string REMARK { get; set; }

        /// <summary>
        /// 異常時間(秒)
        /// </summary>
        public string SPANTIME { get; set; }
        /// <summary>
        /// 報工人數
        /// </summary>
        public string INPUT_MAN { get; set; }
        public string DDL_PDLINE { get; set; }
        public string PDLINE { get; set; }
        public string END_TIME { get; set; }
        public string START_TIME { get; set; }
    }

    /// <summary>
    /// 結果欄位
    /// </summary>
    public class RM_MFG001
    {
        public string WO { get; set; }
        public string TYPE_NAME { get; set; }
        public string ITEM_NAME { get; set; }
        public object START_TIME { get; set; }
        public string END_TIME { get; set; }
        public string REMARK { get; set; }    
        public string SPANTIME { get; set; }
        public string PDLINE { get; set; }
        public string INPUT_MAN { get; set; }
        public string UPDATE_USERID { get; set; }
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_MFG001
    {
        public string TYPE_NAME { get; set; }
        public string ITEM_NAME { get; set; }
        public string DATETIME_S { get; set; }
        public string DATETIME_E { get; set; }
        public string SPANTIME { get; set; }

        /// <summary>查詢</summary>
        /// <param name="qData">查詢資料</param>
        /// <returns></returns>
        public M_MFG001 QUERY(M_MFG001 iData)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;

            M_MFG001 oData = iData;
            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_MFG001>();
            try
            {
                #region 大類下拉清單
                if (oData.DLL_TYPE == null)
                {
                    List<SelectListItem> Dll = new List<SelectListItem>();

                    DataTable dtLine = M_DB.dsMes.GetTable_SqlCmd(SQL_TYPE_GET());
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

                    if (Dll.Count > 0 && string.IsNullOrWhiteSpace(oData.QM.TYPE_NAME))
                        oData.QM.TYPE_NAME = Dll[0].Value;
                }
            
                #endregion

                #region 小類下拉清單
                if (oData.DLL_ITEM == null)
                {
                    List<SelectListItem> Dll2 = new List<SelectListItem>();

                    DataTable dtLine = M_DB.dsMes.GetTable_SqlCmd(SQL_ITEM_GET(oData.QM.TYPE_NAME));
                    if (dtLine?.Rows?.Count > 0)
                    {
                        foreach (DataRow drLine in dtLine.Rows)
                        {
                            Dll2.Add(new SelectListItem
                            {
                                Text = drLine["ITEM_NAME"].ToString(),
                                Value = drLine["ITEM_NAME"].ToString(),
                                Selected = false
                            });
                        }
                    }
                    oData.DLL_ITEM = Dll2;
                }
                #endregion

                #region 檢查工單是否為當天有生產
                if (!string.IsNullOrWhiteSpace(oData.QM.WO))
                {
                    DataTable dtQ1 = M_DB.dsMes.GetTable_SqlCmd(SQL_WO_CHECK(oData.QM.WO));
                    if (int.Parse(dtQ1.Rows[0][0].ToString()) == 0)
                    {
                        ERR_MSG = string.Format("今天沒有生產此工單:[{0}]", oData.QM.WO);
                    }

                    #region 線別下拉清單
                    if (oData.DDL_PDLINE == null)
                    {
                        List<SelectListItem> Dll = new List<SelectListItem>();

                        DataTable dtLine = M_DB.dsMes.GetTable_SqlCmd(SQL_PDLINE_GET(oData.QM.WO));
                        if (dtLine?.Rows?.Count > 0)
                        {
                            foreach (DataRow drLine in dtLine.Rows)
                            {
                                Dll.Add(new SelectListItem
                                {
                                    Text = drLine["PDLINE_NAME"].ToString(),
                                    Value = drLine["PDLINE_NAME"].ToString(),
                                    Selected = false
                                });
                            }
                        }
                        oData.DDL_PDLINE = Dll;

                        if (Dll.Count > 0 && string.IsNullOrWhiteSpace(oData.QM.DDL_PDLINE))
                            oData.QM.DDL_PDLINE = Dll[0].Value;
                    }

                    #endregion                    
                }
                else
                {
                  
                }
                
                #endregion

                #region 計算異常時間(秒)
                DateTime dStart = Convert.ToDateTime(oData.QM.DATETIME_S);
                DateTime dEnd = Convert.ToDateTime(oData.QM.DATETIME_E);
                TimeSpan span = dEnd.Subtract(dStart); //算法是 dEnd 减去 dStart
                oData.QM.SPANTIME = span.TotalSeconds +"";
                #endregion

                DataTable dtQ = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(oData.QM.TYPE_NAME, oData.QM.ITEM_NAME));
                if (dtQ?.Rows?.Count > 0)
                {
                    RM_MFG001 rm = null;
                    if (dtQ?.Rows?.Count > 0)
                    {
                        foreach (DataRow dr in dtQ.Rows)
                        {
                            rm = new RM_MFG001();
                            rm.TYPE_NAME = dr["TYPE_NAME"].ToString();
                            rm.ITEM_NAME = dr["ITEM_NAME"].ToString();
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
        /// <param name="QM"></param>
        /// <returns></returns>
        public bool SAVE(QM_MFG001 QM, string UserId, out string ERR_MSG)
        {
            bool res = false;
            string FUNC = "SAVE";
            ERR_MSG = string.Empty;

            try
            {
                #region  確認填入資料完整性
                if (string.IsNullOrWhiteSpace(QM.WO))
                {
                    ERR_MSG = "未輸入[工單]";
                }
                else if (string.IsNullOrWhiteSpace(QM.PDLINE))
                {
                    ERR_MSG = "未選擇[線別]";
                }
                else if (string.IsNullOrWhiteSpace(QM.TYPE_NAME))
                {
                    ERR_MSG = "未選擇[線別]";
                }
                else if (string.IsNullOrWhiteSpace(QM.ITEM_NAME))
                {
                    ERR_MSG = "未選擇[線別]";
                }
                else if (string.IsNullOrWhiteSpace(QM.DATETIME_S))
                {
                    ERR_MSG = "未輸入[開始時間]";
                }
                else if (string.IsNullOrWhiteSpace(QM.DATETIME_E))
                {
                    ERR_MSG = "未輸入[結束時間]";
                }
                else
                {

                }
                #endregion

                if (string.IsNullOrWhiteSpace(ERR_MSG))
                {
                    #region 儲存作業
                    ////判斷資料是否存在
                    //DataTable dtExist = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_INS(QM.TYPE_NAME, QM.ITEM_NAME));
                    //if (int.Parse(dtExist.Rows[0][0].ToString()) > 0)
                    //{
                    //    ERR_MSG = "此異常已存在";
                    //}
                    //else
                    //{
                        //執行 Insert
                        if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_INS(QM, UserId)) <= 0)
                        {
                            ERR_MSG = "無法新增紀錄";
                        }
                        else
                        {
                            res = true;
                        }
                    //}

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



        #region SQL

        /// <summary>
        /// 異常大類下拉清單
        /// </summary>
        /// <returns></returns>
        public string SQL_TYPE_GET()
        {
            return "SELECT TYPE_NAME FROM SAJET.CAMEO_MSWH_TYPE";
        }
        /// <summary>
        /// 異常小類下拉清單
        /// </summary>
        /// <returns></returns>
        public string SQL_ITEM_GET(string TYPE_NAME)
        {
            return string.Format("SELECT ITEM_NAME FROM SAJET.CAMEO_MSWH_ITEM WHERE TYPE_NAME='{0}'", TYPE_NAME);
        }
        /// <summary>
        /// 檢查工單是否今天作業
        /// </summary>
        /// <param name="WO"></param>
        /// <returns></returns>
        public string SQL_WO_CHECK(string WO)
        {
            return string.Format(@"SELECT count(OUTPUT_QTY) cnt FROM G_SN_COUNT WHERE WORK_ORDER='{0}'", WO);
        }
        /// <summary>
        /// 工單取得線別
        /// </summary>
        /// <param name="WO"></param>
        /// <returns></returns>
        public string SQL_PDLINE_GET(string WO)
        {
            return string.Format(@"SELECT DISTINCT B.PDLINE_NAME,NVL(SAJET.F_GET_WF_BY_DAY_HOUR(A.PDLINE_ID,A.WORK_DATE,A.WORK_TIME),0) INPUT_MAN
                                   FROM SAJET.G_SN_COUNT A ,SAJET.SYS_PDLINE B ,SAJET.G_PDLINE_EMP C WHERE A.PDLINE_ID=B.PDLINE_ID AND A.PDLINE_ID=C.PDLINE_ID 
                                   AND A.WORK_ORDER='{0}' AND A.WORK_DATE > '20200128' ORDER BY B.PDLINE_NAME", WO);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="WO"></param>
        /// <returns></returns>
        public string SQL_DATA_GET(string TYPE_NAME, string ITEM_NAME)
        {
            return string.Format(@"select WORK_ORDER,TYPE_NAME,ITEM_NAME,START_TIME,END_TIME,REMARK,SPANTIME,PDLINE,INPUT_MAN,SAJET.F_GET_EMP_NAME(UPDATE_USERID) UPDATE_USER
                                    from CAMEO_MSWH WHERE TYPE_NAME='{0}' AND ITEM_NAME LIKE '%{1}%'", TYPE_NAME, ITEM_NAME);
        }
        /// <summary>
        /// 儲存
        /// </summary>
        /// <param name="RM"></param>
        /// <returns></returns>
        public string SQL_DATA_INS(QM_MFG001 iQM, string UPDATE_USERID)
        {
            return string.Format(@"INSERT INTO SAJET.CAMEO_MSWH(WORK_ORDER,TYPE_NAME,ITEM_NAME,START_TIME,END_TIME,REMARK,SPANTIME,PDLINE,INPUT_MAN,UPDATE_USERID) 
                            VALUES('{0}', '{1}','{2}', '{3}','{4}', '{5}','{6}', '{7}','{8}', '{9}')", 
                                    iQM.WO, iQM.TYPE_NAME, iQM.ITEM_NAME, iQM.START_TIME, iQM.END_TIME, iQM.REMARK, iQM.SPANTIME, iQM.PDLINE, iQM.INPUT_MAN, UPDATE_USERID);
        }

        #endregion



    }
}