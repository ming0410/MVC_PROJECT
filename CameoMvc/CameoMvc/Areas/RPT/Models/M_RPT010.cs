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
    public class M_RPT010
    {
        public string FUNC { get; } = "RPT010";
        public string FUNC_NAME { get; } = "D-LINK 查詢";

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
        public QM_RPT010 QM { get; set; } = new QM_RPT010();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_RPT010> LIST_RM { get; set; } = new List<RM_RPT010>();
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_RPT010
    {
        /// <summary>
        /// 工單
        /// </summary>
        public string WO { get; set; }
        /// <summary>
        /// MAC(媒體存取控制)位址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// D-LINK NO
        /// </summary>
        public string DLINK { get; set; }
        /// <summary>
        /// LINK_TIME(Start)
        /// </summary>
        public string S_TIME { get; set; }
        /// <summary>
        /// LINK_TIME(End)
        /// </summary>
        public string E_TIME { get; set; }
    }

    /// <summary>
    /// 結果欄位
    /// </summary>
    public class RM_RPT010
    {
        /// <summary>
        /// 工單
        /// </summary>
        public string WO { get; set; }
        /// <summary>
        /// BID
        /// </summary>
        public string BID { get; set; }
        /// <summary>
        /// 料號
        /// </summary>
        public string PART_NO { get; set; }
        /// <summary>
        /// 機種
        /// </summary>
        public string MODEL { get; set; }
        /// <summary>
        /// 客戶序號
        /// </summary>
        public string CSN { get; set; }
        /// <summary>
        /// MAC(媒體存取控制)位址
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// D-LINK NO
        /// </summary>
        public string DLINK_NO { get; set; }
        /// <summary>
        /// P KEY
        /// </summary>
        public string P_KEY { get; set; }
        /// <summary>
        /// PUB KEY
        /// </summary>
        public string PUB_KEY { get; set; }
        /// <summary>
        /// 綁定時間
        /// </summary>
        public string LINK_TIME { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UPDATE_TIME { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string REMARK { get; set; }
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_RPT010
    {
        public M_RPT010 QUERY(M_RPT010 iData, bool IsExport)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;
            string CARTON_NO = string.Empty;
            string SqlWhere = string.Empty;

            M_RPT010 oData = iData;

            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_RPT010>();

            try
            {
                
                DataTable dtQ = M_DB.dsMes.GetTable_SqlCmd(SQL_DAtTA_GET(oData.QM));

                if (dtQ?.Rows?.Count > 0)
                {
                    RM_RPT010 RM = null;
                    foreach (DataRow dr in dtQ.Rows)
                    {
                        RM = new RM_RPT010();
                        RM.WO = dr["WORK_ORDER"].ToString();
                        RM.BID = dr["SERIAL_NUMBER"].ToString();
                        RM.PART_NO = dr["PART_NO"].ToString();
                        RM.MODEL = dr["MODEL_NAME"].ToString();
                        RM.CSN = dr["CUSTOMER_SN"].ToString();
                        RM.MAC = dr["MACID"].ToString();
                        RM.DLINK_NO = dr["MYDLINK"].ToString();
                        RM.P_KEY = dr["P_KEY"].ToString();
                        RM.PUB_KEY = dr["PUB_KEY"].ToString();
                        RM.LINK_TIME = dr["LINK_TIME"].ToString();
                        RM.UPDATE_TIME = dr["UPDATE_TIME"].ToString();
                        RM.REMARK = dr["REMARK"].ToString();
                        oData.LIST_RM.Add(RM);
                    }
                    if (IsExport)
                    {
                        string xlsFile = "CAMEO_D-LINK_MAC" + DateTime.Today.ToString("yyyyMMdd") + "_" + oData.USER_NO + ".XLSX";
                        M_OfficeExcel.ExportDataTableToExcel(dtQ, xlsFile, "D-LINK", true);
                    }
                }
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

        private string SQL_DAtTA_GET(QM_RPT010 QM)
        {
            string SqlWhere = string.Empty;
            if(!string.IsNullOrWhiteSpace(QM.WO))
            {
                SqlWhere = string.Format("AND B.WORK_ORDER LIKE '{0}%' ", QM.WO);
            }
            if (!string.IsNullOrWhiteSpace(QM.MAC))
            {
                SqlWhere = string.Format("AND A.MACID LIKE '{0}%' ", QM.MAC);
            }
            if (!string.IsNullOrWhiteSpace(QM.DLINK))
            {
                SqlWhere = string.Format("AND A.MYDLINK LIKE '{0}%' ", QM.DLINK);
            }
            if (!string.IsNullOrWhiteSpace(QM.S_TIME) && !string.IsNullOrWhiteSpace(QM.E_TIME))
            {
                SqlWhere = string.Format("AND A.LINK_TIME BETWEEN TO_DATE('{0}','YYYY/MM/DD') AND TO_DATE('{1}','YYYY/MM/DD') ", QM.DLINK, QM.E_TIME);
            }
            return string.Format(@"
SELECT B.WORK_ORDER,B.SERIAL_NUMBER,P.PART_NO,M.MODEL_NAME,B.CUSTOMER_SN,A.MACID,A.MYDLINK,A.P_KEY,A.PUB_KEY
,TO_CHAR(A.LINK_TIME,'YYYY/MM/DD HH24:MI') LINK_TIME,TO_CHAR(A.UPDATE_TIME,'YYYY/MM/DD HH24:MI') UPDATE_TIME,A.REMARK 
FROM SAJET.G_MAC_MYDLINK A,SAJET.G_SN_STATUS B,SAJET.SYS_PART P,SYS_MODEL M 
WHERE A.MACID=B.MACID AND B.PART_ID=P.PART_ID(+) AND P.MODEL_ID=M.MODEL_ID {0}
ORDER BY WORK_ORDER,SERIAL_NUMBER 
", SqlWhere);
        }
    }
}