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
    public class M_RPT001
    {
        public string FUNC { get; } = "RPT001";
        public string FUNC_NAME { get; } = "製造流程查詢";

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
        public QM_RPT001 QM { get; set; } = new QM_RPT001();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_RPT001> LIST_RM { get; set; } = new List<RM_RPT001>();
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_RPT001
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
    public class RM_RPT001
    {
        /// <summary>工單
        /// </summary>
        public string WO { get; set; }
        /// <summary>料號
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>線別
        /// </summary>
        public string PdLine { get; set; }
        /// <summary>製程
        /// </summary>
        public string Process { get; set; }
        /// <summary>站別
        /// </summary>
        public string Terminal { get; set; }
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
        /// <summary>檢驗單號
        /// </summary>
        public string QcNo { get; set; }
        /// <summary>送驗單號
        /// </summary>
        public string InspNo { get; set; }
        /// <summary>內彩盒號
        /// </summary>
        public string BoxNo { get; set; }
        /// <summary>外箱箱號
        /// </summary>
        public string CartonNo { get; set; }
        /// <summary>棧板號
        /// </summary>
        public string PalletNo { get; set; }
        /// <summary>重工單號
        /// </summary>
        public string ReworkNo { get; set; }
        /// <summary>連版號
        /// </summary>
        public string PanelNo { get; set; }
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_RPT001
    {
        public M_RPT001 Query(M_RPT001 iData)
        {
            string FUNC = "RPT001";
            string ACTION = "Query";
            string ERR_MSG = string.Empty;
            string sWHERE = string.Empty;

            M_RPT001 oData = iData;
            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_RPT001>();
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

                    RM_RPT001 RowData = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        RowData = new RM_RPT001();
                        RowData.WO = dr["WORK_ORDER"].ToString();
                        RowData.ItemNo = dr["PART_NO"].ToString();
                        RowData.PdLine = dr["PDLINE_NAME"].ToString();
                        RowData.Process = dr["PROCESS_NAME"].ToString();
                        RowData.Terminal = dr["TERMINAL_NAME"].ToString();
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
                        RowData.ReworkNo = dr["REWORK_NO"].ToString();
                        RowData.PanelNo = dr["PANEL_NO"].ToString();
                        oData.LIST_RM.Add(RowData);
                    }
                    oData.FLAG = true;
                }
            }
            catch (Exception ex)
            {
                ERR_MSG = "例外錯誤"
                     + Environment.NewLine + string.Format("功能:{0}/{1}", FUNC, ACTION)
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

        private string Sql_GetTravel(string sWhere)
        {
            return string.Format(@"
SELECT st.SERIAL_NUMBER,st.WORK_ORDER,pt.PART_NO,l.PDLINE_NAME,p.PROCESS_NAME,t.TERMINAL_NAME,DECODE(st.CURRENT_STATUS,'0','GOOD','1','FAIL','3','FAIL','A','FAIL') STATUS
,TO_CHAR(st.OUT_PROCESS_TIME,'YYYY/MM/DD HH24:MI:SS') OUT_PROCESS_TIME,e.EMP_NAME
,st.CUSTOMER_SN,st.MACID,st.QC_NO,st.INSPECTION_NO,st.BOX_NO,st.CARTON_NO,st.PALLET_NO,st.REWORK_NO,st.PANEL_NO
FROM SAJET.G_SN_STATUS ss,SAJET.G_SN_TRAVEL st,SAJET.SYS_PART pt,SAJET.SYS_PDLINE l,SAJET.SYS_PROCESS p,SAJET.SYS_TERMINAL t,SAJET.SYS_EMP e
WHERE ss.SERIAL_NUMBER=st.SERIAL_NUMBER AND st.PART_ID=pt.PART_ID AND st.PDLINE_ID=l.PDLINE_ID AND st.PROCESS_ID=p.PROCESS_ID AND st.TERMINAL_ID=t.TERMINAL_ID AND st.EMP_ID=e.EMP_ID AND {0} 
ORDER BY OUT_PROCESS_TIME
", sWhere);
        }
    }
}