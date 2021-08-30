using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CameoMvc.Areas.RPT.Models
{
    /// <summary>
    /// 參數模組
    /// </summary>
    public class M_RPT002
    {
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

        /// <summary>
        /// 查詢欄位內容
        /// </summary>
        public QM_RPT002 QM { get; set; } = new QM_RPT002();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_RPT002> LIST_RM { get; set; } = new List<RM_RPT002>();
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_RPT002
    {
        /// <summary>序號(BID/VID/PID)
        /// </summary>
        public string SN { get; set; }

        /// <summary>客戶序號
        /// </summary>
        public string CSN { get; set; }

        /// <summary>MAC
        /// </summary>
        public string MAC { get; set; }
    }

    /// <summary>
    /// 結果欄位
    /// </summary>
    public class RM_RPT002
    {
        /// <summary>工單
        /// </summary>
        public string WO { get; set; }
        /// <summary>機種
        /// </summary>
        public string Model { get; set; }
        /// <summary>料號
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>製造序號
        /// </summary>
        public string SN { get; set; }
        /// <summary>線別
        /// </summary>
        public string PdLine { get; set; }
        /// <summary>製程
        /// </summary>
        public string Process { get; set; }
        /// <summary>站別
        /// </summary>
        public string Terminal { get; set; }
        /// <summary>製程
        /// </summary>
        public string NextProcess { get; set; }
        /// <summary>狀態
        /// </summary>
        public string Status { get; set; }
        /// <summary>作業時間
        /// </summary>
        public string WTime { get; set; }
        /// <summary>作業員
        /// </summary>
        public string WUser { get; set; }
        /// <summary>出貨序號
        /// </summary>
        public string CSN { get; set; }
        /// <summary>MAC(媒體存取控制)位址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>送驗單號
        /// </summary>
        public string InspNo { get; set; }
        /// <summary>檢驗單號
        /// </summary>
        public string QcNo { get; set; }
        /// <summary>內彩盒號
        /// </summary>
        public string BoxNo { get; set; }
        /// <summary>外箱箱號
        /// </summary>
        public string CartonNo { get; set; }
        /// <summary>棧板號
        /// </summary>
        public string PalletNo { get; set; }
        /// <summary>連版號
        /// </summary>
        public string PanelNo { get; set; }
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_RPT002
    {
        public M_RPT002 Query(M_RPT002 iData)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;
            string sWHERE = string.Empty;

            M_RPT002 oData = iData;
            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_RPT002>();
            try
            {
                if (!string.IsNullOrWhiteSpace(oData.QM.SN))
                {
                    sWHERE = string.Format("ss.SERIAL_NUMBER = '{0}'", oData.QM.SN);
                }
                if (!string.IsNullOrWhiteSpace(oData.QM.CSN))
                {
                    if (!string.IsNullOrWhiteSpace(sWHERE)) sWHERE += " AND ";
                    sWHERE += string.Format("ss.CUSTOMER_SN = '{0}'", oData.QM.CSN);
                }
                if (!string.IsNullOrWhiteSpace(oData.QM.MAC))
                {
                    if (!string.IsNullOrWhiteSpace(sWHERE)) sWHERE += " AND ";
                    sWHERE += string.Format("ss.MACID = '{0}'", oData.QM.MAC);
                }
                DataTable dt = M_DB.dsMes.GetTable_SqlCmd(Sql_GetTravel(sWHERE));

                if (dt == null || dt.Rows.Count <= 0)
                {

                }
                else
                {
                    if (string.IsNullOrWhiteSpace(oData.QM.SN))
                        oData.QM.SN = dt.Rows[0]["SERIAL_NUMBER"].ToString();

                    RM_RPT002 RowData = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        RowData = new RM_RPT002();
                        RowData.WO = dr["WORK_ORDER"].ToString();
                        RowData.Model = dr["MODEL_NAME"].ToString();
                        RowData.ItemNo = dr["PART_NO"].ToString();
                        RowData.SN = dr["SERIAL_NUMBER"].ToString();
                        RowData.PdLine = dr["PDLINE_NAME"].ToString();
                        RowData.Process = dr["PROCESS_NAME"].ToString();
                        RowData.Terminal = dr["TERMINAL_NAME"].ToString();
                        RowData.NextProcess = dr["NEXT_PROCESS"].ToString();
                        RowData.Status = dr["STATUS"].ToString();
                        RowData.WTime = dr["OUT_PROCESS_TIME"].ToString();
                        RowData.WUser = dr["EMP_NAME"].ToString();
                        RowData.CSN = dr["CUSTOMER_SN"].ToString();
                        RowData.MAC = dr["MACID"].ToString();
                        RowData.QcNo = dr["QC_NO"].ToString();
                        RowData.InspNo = dr["INSPECTION_NO"].ToString();
                        RowData.BoxNo = dr["BOX_NO"].ToString();
                        RowData.CartonNo = dr["CARTON_NO"].ToString();
                        RowData.PalletNo = dr["PALLET_NO"].ToString();
                        RowData.PanelNo = dr["PANEL_NO"].ToString();
                        oData.LIST_RM.Add(RowData);
                    }
                    oData.FLAG = true;
                }
            }
            catch (Exception ex)
            {
                ERR_MSG = string.Format("例外錯誤 \\r\\n 功能:{0} \\r\\n 訊息:{1}", FUNC, ex.Message.Replace("'", "").Replace("\r\n", ""));
            }
            finally
            {
                if (ERR_MSG?.Length > 0)
                {
                    oData.FLAG = false;
                    oData.ERR = ERR_MSG;
                }
            }
            return oData;
        }

        private string Sql_GetTravel(string sWhere)
        {
            return string.Format(@"
SELECT SN.WORK_ORDER,M.MODEL_NAME,PT.PART_NO,SN.SERIAL_NUMBER,SN.CUSTOMER_SN,PS1.PROCESS_NAME,T.TERMINAL_NAME,SN.OUT_PROCESS_TIME,E.EMP_NAME,NVL(NPC.PROCESS_NAME,(CASE WHEN SN.OUT_PROCESS_TIME IS NULL THEN '未投入' ELSE '已完成' END)) NEXT_PROCESS,SN.PANEL_NO,SN.INSPECTION_NO,SN.QC_NO
FROM SAJET.G_SN_STATUS SN,SAJET.SYS_PART PT,SAJET.SYS_PROCESS PS1,SAJET.SYS_PROCESS NPC,SAJET.SYS_TERMINAL T,SAJET.SYS_MODEL M,SAJET.SYS_EMP E
WHERE SN.PART_ID=PT.PART_ID(+) AND SN.PROCESS_ID=PS1.PROCESS_ID(+) AND SN.WIP_PROCESS=NPC.PROCESS_ID(+) AND SN.TERMINAL_ID = T.TERMINAL_ID(+) AND SN.EMP_ID = E.EMP_ID(+) AND PT.MODEL_ID=M.MODEL_ID
AND SN.SERIAL_NUMBER LIKE 'V60213231393%' AND SN.WORK_ORDER LIKE '236276%' AND PT.PART_NO LIKE '7600DG200520ZA1-T%' AND PS1.PROCESS_NAME LIKE 'IQPQ000-XP%' 
AND SN.OUT_PROCESS_TIME >= TO_DATE('20210414','YYYYMMDD') AND SN.OUT_PROCESS_TIME <= TO_DATE('20210415','YYYYMMDD')
ORDER BY WORK_ORDER,SERIAL_NUMBER
", sWhere);
        }
    }
}