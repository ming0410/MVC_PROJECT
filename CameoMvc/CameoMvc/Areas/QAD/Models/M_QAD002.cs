using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameoMvc.Areas.QAD.Models
{
    /// <summary>
    /// 參數模組
    /// </summary>
    public class M_QAD002
    {
        public string FUNC { get; } = "QAD002";
        public string FUNC_NAME { get; } = "客驗借出/歸還查詢";

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
        public QM_QAD002 QM { get; set; } = new QM_QAD002();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_QAD002> LIST_RM { get; set; } = new List<RM_QAD002>();

        /// <summary>
        /// 借出/歸還下拉清單
        /// </summary>
        public IEnumerable<SelectListItem> DDL_TYPE { get; set; }

        /// <summary>
        /// Table 之欄位名
        /// </summary>
        public static List<string> COL_NAME = new List<string>()
        {
            "TYPE",                 //借出/歸還",
            "WORK_ORDER",           //工單",
            "CARTON_NO",            //箱號",
            "PALLET_NO",            //棧板號",
            "PART_NO",              //料號",
            "MODEL_NAME",           //機種",
            "LEND_USER",            //借出者",
            "LEND_TIME",            //借出時間",
            "LEND_SN",              //借出所刷序號",
            "BACK_USER",            //歸還者",
            "BACK_TIME",            //歸還時間",
            "RETURN_REF_SN"         //歸還所刷序號",
        };

        /// <summary>
        /// Table 之標頭
        /// </summary>
        public static List<string> COL_HEAD = new List<string>()
        {
            "借出/歸還",
            "工單",
            "箱號",
            "棧板號",
            "料號",
            "機種",
            "借出者",
            "借出時間",
            "借出所刷序號",
            "歸還者",
            "歸還時間",
            "歸還所刷序號",
        };
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_QAD002
    {
        /// <summary>
        /// 工單
        /// </summary>
        public string WO { get; set; }

        /// <summary>
        /// 借出的產品序號(Customer S/N)
        /// </summary>
        public string CSN { get; set; }

        /// <summary>
        /// 借出/歸還
        /// </summary>
        public string TYPE { get; set; }

        /// <summary>
        /// 查詢日期(起)
        /// </summary>
        public string SDate { get; set; }

        /// <summary>
        /// 查詢日期(訖)
        /// </summary>
        public string EDate { get; set; }
    }

    /// <summary>
    /// 結果欄位
    /// </summary>
    public class RM_QAD002
    {
        /// <summary>
        /// 借出/歸還
        /// </summary>
        public QAD001_TYPE TYPE { get; set; }
        /// <summary>
        /// 工單
        /// </summary>
        public string WO { get; set; }
        /// <summary>
        /// 箱號
        /// </summary>
        public string CARTON_NO { get; set; }
        /// <summary>
        /// 棧板號
        /// </summary>
        public string PALLET_NO { get; set; }
        /// <summary>
        /// 料號
        /// </summary>
        public string PART_NO { get; set; }
        /// <summary>
        /// 機種
        /// </summary>
        public string MODEL_NAME { get; set; }
        /// <summary>
        /// 借出時所刷的條碼紀錄
        /// </summary>
        public string LEND_SN { get; set; }
        /// <summary>
        /// 借出者工號姓名
        /// </summary>
        public string LEND_USER { get; set; }
        /// <summary>
        /// 借出者ID
        /// </summary>
        public string LEND_USERID { get; set; }
        /// <summary>
        /// 借出時間
        /// </summary>
        public string LEND_TIME { get; set; }
        /// <summary>
        /// 歸還時所刷的條碼紀錄
        /// </summary>
        public string BACK_SN { get; set; }
        /// <summary>
        /// 歸還時所刷的關係條碼
        /// </summary>
        public string BACK_REF_SN { get; set; }
        /// <summary>
        /// 歸還者工號姓名
        /// </summary>
        public string BACK_USER { get; set; }
        /// <summary>
        /// 歸還者ID
        /// </summary>
        public string BACK_USERID { get; set; }
        /// <summary>
        /// 歸還時間
        /// </summary>
        public string BACK_TIME { get; set; }
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_QAD002
    {
        /// <summary>查詢</summary>
        /// <param name="qData">查詢資料</param>
        /// <returns></returns>
        public M_QAD002 QUERY(M_QAD002 iData, bool IsExport)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;
            string CARTON_NO = string.Empty;

            M_QAD002 oData = iData;
            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            //oData.LIST_RM = new List<RM_QAD001>();
            try
            {
                if (oData.DDL_TYPE == null)
                {
                    oData.DDL_TYPE = FM_QAD001.DLL_QAD001_TYPE();
                }

                if (string.IsNullOrWhiteSpace(oData.QM.TYPE) && oData.DDL_TYPE.Count() > 0)
                {
                    oData.QM.TYPE = oData.DDL_TYPE.FirstOrDefault().Value;
                }

                if (!string.IsNullOrWhiteSpace(iData.QM.CSN))
                {
                    //依 產品序號 取得 箱號 及 棧板號
                    DataTable dt = M_DB.dsMes.GetTable_SqlCmd(SQL_STATUS_GET(iData.QM.CSN));
                    if (dt?.Rows?.Count > 0)
                    {
                        CARTON_NO = dt.Rows[0]["CARTON_NO"].ToString();
                    }
                }

                QAD001_TYPE TYPE = (QAD001_TYPE)Enum.Parse(typeof(QAD001_TYPE), oData.QM.TYPE);

                //取得借出的資料
                DataTable dtQ = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(TYPE, iData.QM.WO, CARTON_NO, oData.QM.SDate, oData.QM.EDate));
                if (dtQ?.Rows?.Count > 0)
                {
                    RM_QAD002 RM;
                    foreach (DataRow dr in dtQ.Rows)
                    {
                        RM = new RM_QAD002();
                        RM.TYPE = (QAD001_TYPE)Enum.Parse(typeof(QAD001_TYPE), dr["TYPE"].ToString());
                        RM.WO = dr["WORK_ORDER"].ToString();
                        RM.CARTON_NO = dr["CARTON_NO"].ToString();
                        RM.PALLET_NO = dr["PALLET_NO"].ToString();
                        RM.PART_NO = dr["PART_NO"].ToString();
                        RM.MODEL_NAME = dr["MODEL_NAME"].ToString();
                        RM.LEND_SN = dr["LEND_SN"].ToString();
                        RM.LEND_USER = dr["LEND_USER"].ToString();
                        RM.LEND_TIME = dr["LEND_TIME"].ToString();
                        RM.BACK_SN = dr["RETURN_SN"].ToString();
                        RM.BACK_REF_SN = dr["RETURN_REF_SN"].ToString();
                        RM.BACK_USER = dr["BACK_USER"].ToString();
                        RM.BACK_TIME = dr["BACK_TIME"].ToString();
                        oData.LIST_RM.Add(RM);
                    }
                    if (IsExport)
                    {
                        DataTable dtE = FM_SYS.DataTable_ColNameChange(dtQ, M_QAD002.COL_NAME, M_QAD002.COL_HEAD);
                        string xlsFile = "CAMEO_客驗借出歸還紀錄_" + DateTime.Today.ToString("yyyyMMdd") + "_" + oData.USER_NO + ".XLSX";
                        M_OfficeExcel.ExportDataTableToExcel(dtE, xlsFile, "SMT工時表", true);
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

        #region SQL

        private string SQL_DATA_GET(QAD001_TYPE TYPE, string WO, string CARTON_NO, string S_DATE, string E_DATE)
        {
            string sqlWhere11 = string.IsNullOrWhiteSpace(S_DATE) ? "" : string.Format("AND LEND_TIME >= TO_DATE('{0}','YYYY/MM/DD')", S_DATE);
            string sqlWhere12 = string.IsNullOrWhiteSpace(S_DATE) ? "" : string.Format("AND LEND_TIME <= TO_DATE('{0} 23:59:59','YYYY/MM/DD HH24:MI:SS')", E_DATE);
            string sqlWhere21 = string.IsNullOrWhiteSpace(S_DATE) ? "" : string.Format("AND LEND_TIME >= TO_DATE('{0}','YYYY/MM/DD')", S_DATE);
            string sqlWhere22 = string.IsNullOrWhiteSpace(S_DATE) ? "" : string.Format("AND RETURN_TIME <= TO_DATE('{0} 23:59:59','YYYY/MM/DD HH24:MI:SS')", E_DATE);
            string sqlSrc = string.Empty;
            switch (TYPE)
            {
                case QAD001_TYPE.ALL:
                    sqlSrc = string.Format(@"
SELECT 'LEND' TYPE,CARTON_NO,PALLET_NO,LEND_SN,LEND_USERID,LEND_TIME,null RETURN_SN,null RETURN_REF_SN,null RETURN_USERID,null RETURN_TIME
FROM SAJET.CAMEO_QA_LEND 
WHERE CARTON_NO LIKE '{0}%' {1} {2}
UNION ALL
SELECT 'BACK' TYPE,CARTON_NO,PALLET_NO,LEND_SN,LEND_USERID,LEND_TIME,RETURN_SN,RETURN_REF_SN,RETURN_USERID,RETURN_TIME
FROM SAJET.CAMEO_QA_LEND_RETURN 
WHERE CARTON_NO LIKE '{0}%' {3} {4} 
", CARTON_NO, sqlWhere11, sqlWhere12, sqlWhere21, sqlWhere22);
                    break;
                case QAD001_TYPE.LEND:
                    sqlSrc = string.Format(@"
SELECT 'LEND' TYPE,CARTON_NO,PALLET_NO,LEND_SN,LEND_USERID,LEND_TIME,null RETURN_SN,null RETURN_REF_SN,null RETURN_USERID,null RETURN_TIME
FROM SAJET.CAMEO_QA_LEND 
WHERE CARTON_NO LIKE '{0}%' {1} {2}
", CARTON_NO, sqlWhere11, sqlWhere12, sqlWhere21, sqlWhere22);
                    break;
                case QAD001_TYPE.BACK:
                    sqlSrc = string.Format(@"
SELECT 'BACK' TYPE,CARTON_NO,PALLET_NO,LEND_SN,LEND_USERID,LEND_TIME,RETURN_SN,RETURN_REF_SN,RETURN_USERID,RETURN_TIME
FROM SAJET.CAMEO_QA_LEND_RETURN 
WHERE CARTON_NO LIKE '{0}%' {3} {4} 
", CARTON_NO, sqlWhere11, sqlWhere12, sqlWhere21, sqlWhere22);
                    break;
            }

            string Sql = string.Format(@"
SELECT DISTINCT a.TYPE,c.WORK_ORDER,a.CARTON_NO,a.PALLET_NO,p.PART_NO,m.MODEL_NAME
,a.LEND_SN,a.LEND_USERID,e1.EMP_NO||' '||e1.EMP_NAME LEND_USER,TO_CHAR(a.LEND_TIME,'YYYY/MM/DD HH24:MI:SS') LEND_TIME
,a.RETURN_SN,a.RETURN_REF_SN,a.RETURN_USERID BACK_USERID,NVL2(a.RETURN_USERID,e2.EMP_NO||' '||e2.EMP_NAME,'') BACK_USER,TO_CHAR(a.RETURN_TIME,'YYYY/MM/DD HH24:MI:SS') BACK_TIME
FROM({0}) a,SAJET.SYS_EMP e1,SAJET.SYS_EMP e2,SAJET.G_SN_STATUS c,SAJET.G_WO_BASE d,SAJET.SYS_PART p,SAJET.SYS_MODEL m
WHERE a.LEND_USERID=e1.EMP_ID(+) AND a.RETURN_USERID=e2.EMP_ID(+) 
AND a.CARTON_NO=c.CARTON_NO(+) AND a.PALLET_NO=c.PALLET_NO(+) AND c.WORK_ORDER LIKE '{1}%' AND c.WORK_ORDER=d.WORK_ORDER(+) AND c.PART_ID=p.PART_ID(+) AND p.MODEL_ID=m.MODEL_ID(+)
ORDER BY BACK_TIME,LEND_TIME
", sqlSrc, WO);
            return Sql;
        }

        /// <summary>
        /// 依 [產品序號 或 箱號] 取得 [箱號]及[棧板號]
        /// </summary>
        /// <param name="CSN">產品序號</param>
        /// <returns></returns>
        private string SQL_STATUS_GET(string CSN)
        {
            return string.Format("SELECT DISTINCT PALLET_NO,CARTON_NO,OUT_PROCESS_TIME,IN_PROCESS_TIME FROM SAJET.G_SN_STATUS WHERE (CUSTOMER_SN='{0}' OR CARTON_NO='{0}') ORDER BY OUT_PROCESS_TIME DESC,IN_PROCESS_TIME DESC", CSN);
        }
        #endregion
    }
}