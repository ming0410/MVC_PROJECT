using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace CameoMvc.Areas.SMT.Models
{
    enum RecordMode
    {
        /// <summary>
        /// 定時紀錄
        /// </summary>
        Timing = 0,
        /// <summary>
        /// 異常紀錄
        /// </summary>
        Exception = 1
    }

    /// <summary>
    /// 參數模組
    /// </summary>
    public class M_SMT002
    {
        public string FUNC { get; set; } = "SMT002";
        public string FUNC_NAME { get; } = "防潮櫃溫濕度";

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
        public QM_SMT002 QM { get; set; } = new QM_SMT002();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_SMT002> LIST_RM { get; set; } = new List<RM_SMT002>();
        public List<M_CHART> LIST_CHART { get; set; } = new List<M_CHART>();

        /// <summary>
        /// 下拉清單-防潮櫃設備
        /// </summary>
        public IEnumerable<SelectListItem> DLL_DEVS { get; set; }
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_SMT002
    {
        /// <summary>
        /// 防潮櫃設備
        /// </summary>
        public string Device { get; set; }
        /// <summary>
        /// 定時紀錄模式
        /// </summary>
        public bool Timing { get; set; }
        /// <summary>
        /// 異常紀錄模式
        /// </summary>
        public bool Exception { get; set; }
        /// <summary>
        /// 查詢日期(起)
        /// </summary>
        public string sDate { get; set; }
        /// <summary>
        /// 查詢日期(訖)
        /// </summary>
        public string eDate { get; set; }
    }

    /// <summary>
    /// 查詢/結果欄位
    /// </summary>
    public class RM_SMT002
    {
        /// <summary>
        /// 防潮櫃設備
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 溫度
        /// </summary>
        public string Tp { get; set; }
        /// <summary>
        /// 濕度
        /// </summary>
        public string Rh { get; set; }
        /// <summary>
        /// 紀錄模式
        /// </summary>
        public string Md { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string Tm { get; set; }
    }

    public class M_CHART
    {
        public string[] X_NAME { get; set; }
        public string Y_NAME { get; set; }
        public string DC_ID { get; set; }
        public string DC_NAME { get; set; }
        public string DC_UNIT { get; set; }
        public double DC_LCL { get; set; }
        public double DC_UCL { get; set; }
        public double DC_LSL { get; set; }
        public double DC_USL { get; set; }
        public double[] VALUE { get; set; }
        public double[] LCL { get; set; }
        public double[] UCL { get; set; }
        public double[] LSL { get; set; }
        public double[] USL { get; set; }
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_SMT002
    {
        /// <summary>查詢</summary>
        /// <param name="qData">查詢資料</param>
        /// <returns></returns>
        public M_SMT002 QUERY(M_SMT002 iData, bool IsExport)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;
            string CARTON_NO = string.Empty;
            string SqlWhere = string.Empty;

            M_SMT002 oData = iData;

            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_SMT002>();

            try
            {
                #region 防潮櫃設備IP 下拉清單
                if (oData.DLL_DEVS == null)
                {
                    List<SelectListItem> Dll = new List<SelectListItem>();

                    DataTable dtLine = M_DB.dsMes.GetTable_SqlCmd(SQL_DEVICES_GET());
                    if (dtLine?.Rows?.Count > 0)
                    {
                        foreach (DataRow drLine in dtLine.Rows)
                        {
                            Dll.Add(new SelectListItem
                            {
                                Text = drLine["IP"].ToString(),
                                Value = drLine["IP"].ToString(),
                                Selected = false
                            });
                        }
                    }
                    oData.DLL_DEVS = Dll;
                }
                #endregion

                #region 紀錄模式
                string SelMode = string.Empty;

                if (oData.QM.Timing)
                    SelMode += string.Format("'{0}',", ((int)RecordMode.Timing).ToString());
                if (oData.QM.Exception)
                    SelMode += string.Format("'{0}',", ((int)RecordMode.Exception).ToString());

                SelMode = SelMode.Substring(0, SelMode.Length - 1);
                #endregion

                RM_SMT002 RM = null;
                DataTable dtQ = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(oData.QM.sDate, oData.QM.eDate, SelMode));
                if (dtQ?.Rows?.Count > 0)
                {
                    string[] UTime = new string[dtQ.Rows.Count];
                    double[] ValRh = new double[dtQ.Rows.Count];
                    double[] ValHL = new double[dtQ.Rows.Count];
                    double[] ValHU = new double[dtQ.Rows.Count];
                    double[] ValTm = new double[dtQ.Rows.Count];
                    double[] ValTL = new double[dtQ.Rows.Count];
                    double[] ValTU = new double[dtQ.Rows.Count];

                    for (int i = 0; i < dtQ.Rows.Count; i++)
                    {
                        RM = new RM_SMT002();
                        RM.IP = dtQ.Rows[i]["IP"].ToString();
                        RM.Rh = dtQ.Rows[i]["HUMIDITY"].ToString();
                        RM.Tp = dtQ.Rows[i]["TEMPERATURE"].ToString();
                        RM.Tm = dtQ.Rows[i]["UPDATE_TIME"].ToString();
                        RM.Md = dtQ.Rows[i]["RECORD_MODE"].ToString();
                        oData.LIST_RM.Add(RM);
                        UTime[i] = dtQ.Rows[i]["UPDATE_TIME"].ToString();
                        ValRh[i] = double.Parse(dtQ.Rows[i]["HUMIDITY"].ToString());
                        ValHU[i] = 25;
                        ValHL[i] = 0;
                        ValTm[i] = double.Parse(dtQ.Rows[i]["TEMPERATURE"].ToString());
                        ValTL[i] = 30;
                        ValTU[i] = 15;
                    }

                    M_CHART cRH = new M_CHART();
                    cRH.DC_NAME = "RH";
                    cRH.VALUE = ValRh;
                    cRH.LCL = ValHL;
                    cRH.UCL = ValHU;
                    cRH.DC_UNIT = "%RH";
                    cRH.DC_USL = 25;
                    cRH.DC_LSL = 0;
                    cRH.Y_NAME = "濕度";
                    cRH.X_NAME = UTime;
                    oData.LIST_CHART.Add(cRH);

                    M_CHART cTP = new M_CHART();
                    cTP.DC_NAME = "TP";
                    cTP.VALUE = ValTm;
                    cTP.LCL = ValTL;
                    cTP.UCL = ValTU;
                    cTP.DC_UNIT = "℃";
                    cTP.DC_USL = 25;
                    cTP.DC_LSL = 0;
                    cTP.Y_NAME = "溫度";
                    cTP.X_NAME = UTime;
                    oData.LIST_CHART.Add(cTP);
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

        #region SQL

        /// <summary>
        /// 取得所有防潮櫃IP
        /// </summary>
        /// <returns></returns>
        private string SQL_DEVICES_GET()
        {
            return "SELECT DISTINCT IP FROM SAJET.CAMEO_DRY_BOX ORDER BY IP";
        }

        /// <summary>
        /// 取得溫溼度資料
        /// </summary>
        /// <param name="sDate">查詢日期(起)</param>
        /// <param name="eDate">查詢日期(訖)</param>
        /// <returns></returns>
        private string SQL_DATA_GET(string sDate, string eDate, string modes)
        {
            return string.Format(@"
SELECT IP,HUMIDITY,TEMPERATURE,TO_CHAR(UPDATE_TIME,'{3}') UPDATE_TIME,RECORD_MODE 
FROM SAJET.CAMEO_DRY_BOX 
WHERE UPDATE_TIME >= TO_DATE('{0}','YYYY/MM/DD') AND UPDATE_TIME <= TO_DATE('{1} 23:59:59','YYYY/MM/DD HH24:MI:SS') AND RECORD_MODE IN ({2}) 
ORDER BY UPDATE_TIME
", sDate, eDate, modes, modes.Contains("1") ? "HH24:MI:SS" : "HH24");
        }
        #endregion
    }
}