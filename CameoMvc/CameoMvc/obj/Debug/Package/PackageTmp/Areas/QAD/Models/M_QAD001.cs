using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameoMvc.Areas.QAD.Models
{
    /// <summary>操作模式：借出/歸還
    /// </summary>
    public enum QAD001_TYPE
    {
        /// <summary>所有
        /// </summary>
        ALL = -1,
        /// <summary>借出
        /// </summary>
        LEND = 0,
        /// <summary>歸還
        /// </summary>
        BACK = 1
    }

    /// <summary>
    /// 參數模組
    /// </summary>
    public class M_QAD001
    {
        public string FUNC { get; } = "QAD001";
        public string FUNC_NAME { get; } = "客驗借出/歸還作業";

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
        public QM_QAD001 QM { get; set; } = new QM_QAD001();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_QAD001> LIST_RM { get; set; } = new List<RM_QAD001>();
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_QAD001
    {
        /// <summary>
        /// 借出的產品序號(Customer S/N)
        /// </summary>
        public string CSN { get; set; }

        /// <summary>
        /// 關聯的產品序號(Reference CSN)
        /// </summary>
        public string REF { get; set; }

        /// <summary>借出/歸還
        /// </summary>
        public QAD001_TYPE TYPE { get; set; }
    }

    /// <summary>
    /// 結果欄位
    /// </summary>
    public class RM_QAD001
    {
        /// <summary>
        /// 借出/歸還
        /// </summary>
        public QAD001_TYPE TYPE { get; set; }
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
    public class FM_QAD001
    {
        /// <summary>查詢</summary>
        /// <param name="qData">查詢資料</param>
        /// <returns></returns>
        public M_QAD001 QUERY(M_QAD001 iData)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;
            string CARTON_NO = string.Empty;

            M_QAD001 oData = iData;
            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_QAD001>();
            try
            {
                if (!string.IsNullOrWhiteSpace(iData.QM.CSN))
                {
                    //依 產品序號 取得 箱號 及 棧板號
                    DataTable dtQ = M_DB.dsMes.GetTable_SqlCmd(SQL_STATUS_GET(iData.QM.CSN));
                    if (dtQ?.Rows?.Count > 0)
                    {
                        CARTON_NO = dtQ.Rows[0]["CARTON_NO"].ToString();
                    }
                }

                //取得借出的資料
                DataTable dt = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(CARTON_NO, iData.QM.TYPE));
                if (dt?.Rows?.Count > 0)
                {
                    RM_QAD001 RM;
                    foreach (DataRow dr in dt.Rows)
                    {
                        RM = new RM_QAD001();
                        RM.TYPE = (QAD001_TYPE)Enum.Parse(typeof(QAD001_TYPE), dr["TYPE"].ToString());
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
        /// 借出
        /// </summary>
        /// <param name="iData"></param>
        /// <returns></returns>
        public string LEND(M_QAD001 iData)
        {
            string ERR_MSG = string.Empty;
            RM_QAD001 RM = new RM_QAD001();
            RM.TYPE = QAD001_TYPE.LEND;
            RM.LEND_SN = iData.QM.CSN;
            RM.LEND_USERID = iData.USER_ID;
            RM.LEND_USER = iData.USER_NO;
            RM.LEND_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            if (string.IsNullOrWhiteSpace(iData.QM.CSN))
            {
                ERR_MSG = "未輸入[產品序號]";
            }
            else
            {
                //依 產品序號 取得 箱號 及 棧板號
                string CARTON_NO = string.Empty;
                string PALLET_NO = string.Empty;
                if (PackInfo_Get(iData.QM.CSN, out CARTON_NO, out PALLET_NO, out ERR_MSG))
                {
                    //判斷該箱是否可以繼續作業
                    if (!CartonCanWork_Chk(CARTON_NO))
                    {
                        ERR_MSG = string.Format("箱號[{0}]，已經{1}", CARTON_NO, RM.TYPE == QAD001_TYPE.LEND ? "借出" : "歸還");
                    }
                    else
                    {
                        RM.CARTON_NO = CARTON_NO;
                        RM.PALLET_NO = PALLET_NO;
                        if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_INS(RM)) <= 0)
                        {
                            ERR_MSG = "無法新增紀錄";
                        }
                    }
                }
            }
            return ERR_MSG;
        }

        /// <summary>
        /// 歸還
        /// </summary>
        /// <param name="iData"></param>
        /// <returns></returns>
        public string BACK(M_QAD001 iData)
        {
            string ERR_MSG = string.Empty;
            RM_QAD001 RM = new RM_QAD001();
            RM.TYPE = QAD001_TYPE.BACK;
            RM.BACK_SN = iData.QM.CSN;
            RM.BACK_REF_SN = iData.QM.REF;
            RM.BACK_USERID = iData.USER_ID;
            RM.BACK_USER = iData.USER_NO;
            RM.BACK_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            RM.CARTON_NO = string.Empty;
            RM.PALLET_NO = string.Empty;
            if (string.IsNullOrWhiteSpace(iData.QM.CSN))
            {
                ERR_MSG = "未輸入[產品序號]";
            }
            else
            {
                //檢查是否可通行
                bool PASSPORT = false;
                //依 產品序號 取得 箱號 及 棧板號
                string LEND_CARTON_NO = string.Empty;
                string LEND_PALLET_NO = string.Empty;
                string BACK_CARTON_NO = string.Empty;
                string BACK_PALLET_NO = string.Empty;

                //取得借出的箱號及棧板號
                if (PackInfo_Get(iData.QM.CSN, out LEND_CARTON_NO, out LEND_PALLET_NO, out ERR_MSG))
                {
                    //判斷該箱是否有在借出清單上
                    DataTable dt_lend = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(LEND_CARTON_NO, QAD001_TYPE.LEND));
                    if (dt_lend.Rows?.Count <= 0)
                    {
                        //沒有在借出清單上
                        ERR_MSG = string.Format("[{0}] 之箱號[{1}] 未借出", iData.QM.CSN, LEND_CARTON_NO);
                    }
                    else
                    {
                        //有在借出清單上
                        //以棧板號取得所有箱號清單，以判斷該箱棧板是否只有一箱
                        DataTable dtList = M_DB.dsMes.GetTable_SqlCmd(SQL_CARTON_LIST_GET(LEND_PALLET_NO));

                        if (int.Parse(dtList.Rows[0][0].ToString()) <= 1)
                        {
                            if (int.Parse(dtList.Rows[0][0].ToString()) == 1)
                            {
                                //棧板是否只有一箱
                                iData.QM.REF = iData.QM.CSN;
                                BACK_CARTON_NO = LEND_CARTON_NO;
                                BACK_PALLET_NO = LEND_PALLET_NO;
                                PASSPORT = true;
                            }
                        }
                        else
                        {
                            //棧板是否不只一箱
                            //判斷是否有輸入另一箱之產品序號或箱號
                            if (string.IsNullOrWhiteSpace(iData.QM.REF))
                            {
                                ERR_MSG = "未輸入[棧板上任一箱產品序號]";
                            }
                            else
                            {
                                //取得同一棧板未借出的箱號及棧板號
                                if (PackInfo_Get(iData.QM.REF, out BACK_CARTON_NO, out BACK_PALLET_NO, out ERR_MSG))
                                {
                                    if (LEND_CARTON_NO == BACK_CARTON_NO)
                                    {
                                        ERR_MSG = "箱號不可相同"
                                           + Environment.NewLine + string.Format("[{0}] 之箱號為[{1}] 棧板為[{2}]", iData.QM.CSN, LEND_CARTON_NO, LEND_PALLET_NO)
                                           + Environment.NewLine + string.Format("[{0}] 之箱號為[{1}] 棧板為[{2}]", iData.QM.REF, BACK_CARTON_NO, BACK_PALLET_NO);
                                    }
                                    else if (LEND_PALLET_NO != BACK_PALLET_NO)
                                    {
                                        ERR_MSG = "棧板不同"
                                           + Environment.NewLine + string.Format("[{0}] 之箱號為[{1}] 棧板為[{2}]", iData.QM.CSN, LEND_CARTON_NO, LEND_PALLET_NO)
                                           + Environment.NewLine + string.Format("[{0}] 之箱號為[{1}] 棧板為[{2}]", iData.QM.REF, BACK_CARTON_NO, BACK_PALLET_NO);
                                    }
                                    else
                                    {
                                        PASSPORT = true;
                                    }
                                }
                            }
                        }

                        if (PASSPORT)
                        {
                            //之前借的資料
                            RM.LEND_SN = dt_lend.Rows[0]["LEND_SN"].ToString();
                            RM.LEND_USERID = dt_lend.Rows[0]["LEND_USERID"].ToString();
                            RM.LEND_USER = dt_lend.Rows[0]["LEND_USER"].ToString();
                            RM.LEND_TIME = dt_lend.Rows[0]["LEND_TIME"].ToString();
                            //此次規還的資料，因為此次刷的借出條碼有可能不一樣
                            RM.BACK_SN = iData.QM.CSN;
                            RM.BACK_REF_SN = iData.QM.REF;
                            RM.CARTON_NO = LEND_CARTON_NO;
                            RM.PALLET_NO = LEND_PALLET_NO;

                            if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_UPD(RM)) <= 0)
                            {
                                ERR_MSG = "無法更新紀錄";
                            }
                            else
                            {
                                if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_DEL(RM)) <= 0)
                                {
                                    ERR_MSG = "無法更新紀錄";
                                }
                            }
                        }
                    }
                }
            }
            return ERR_MSG;
        }

        /// <summary>
        /// 依 產品序號 取得 箱號 及 棧板號
        /// </summary>
        /// <param name="CSN">產品序號</param>
        /// <param name="CARTON_NO">箱號</param>
        /// <param name="PALLET_NO">棧板號</param>
        /// <param name="ERR_MSG">錯誤訊息</param>
        /// <returns></returns>
        private bool PackInfo_Get(string CSN, out string CARTON_NO, out string PALLET_NO, out string ERR_MSG)
        {
            CARTON_NO = string.Empty;
            PALLET_NO = string.Empty;
            ERR_MSG = string.Empty;
            DataTable dt = M_DB.dsMes.GetTable_SqlCmd(SQL_STATUS_GET(CSN));
            if (dt?.Rows?.Count > 0)
            {
                if (dt.Rows.Count > 1)
                {
                    //ERR_MSG = "有多筆資料";
                }
                CARTON_NO = dt.Rows[0]["CARTON_NO"].ToString();
                PALLET_NO = dt.Rows[0]["PALLET_NO"].ToString();

                if (string.IsNullOrWhiteSpace(CARTON_NO))
                {
                    ERR_MSG = "查無[箱號]資訊";
                    return false;
                }
                else if (string.IsNullOrWhiteSpace(PALLET_NO))
                {
                    ERR_MSG = "查無[棧板]資訊";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                ERR_MSG = string.Format("查無[生產序號：{0}]紀錄", CSN);
                return false;
            }
        }

        /// <summary>
        /// 該箱是否可以繼續作業，資訊存在表示該箱已經借出，無法繼續作業
        /// </summary>
        /// <param name="CARTON_NO">箱號</param>
        /// <returns></returns>
        private bool CartonCanWork_Chk(string CARTON_NO)
        {
            DataTable dt = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(CARTON_NO, QAD001_TYPE.LEND));
            if(dt == null)
            {
                return true;
            }
            else
            {
                if (dt.Rows == null)
                {
                    return true;
                }
                else
                {
                    if (dt.Rows.Count <= 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        #region SQL

        private string SQL_DATA_GET(string CARTON_NO, QAD001_TYPE TYPE)
        {
            if(TYPE == QAD001_TYPE.ALL)
            {
                return string.Format(@"
SELECT DISTINCT a.TYPE,a.CARTON_NO,a.PALLET_NO,p.PART_NO,m.MODEL_NAME
,a.LEND_SN,a.LEND_USERID,e1.EMP_NO||' '||e1.EMP_NAME LEND_USER,TO_CHAR(a.LEND_TIME,'YYYY/MM/DD HH24:MI:SS') LEND_TIME
,a.RETURN_SN,a.RETURN_REF_SN,a.RETURN_USERID BACK_USERID,NVL2(a.RETURN_USERID,e2.EMP_NO||' '||e2.EMP_NAME,'') BACK_USER,TO_CHAR(a.RETURN_TIME,'YYYY/MM/DD HH24:MI:SS') BACK_TIME
FROM(
SELECT 'LEND' TYPE,CARTON_NO,PALLET_NO,LEND_SN,LEND_USERID,LEND_TIME,null RETURN_SN,null RETURN_REF_SN,null RETURN_USERID,null RETURN_TIME
FROM SAJET.CAMEO_QA_LEND 
WHERE CARTON_NO LIKE '{0}%' 
UNION ALL
SELECT 'BACK' TYPE,CARTON_NO,PALLET_NO,LEND_SN,LEND_USERID,LEND_TIME,RETURN_SN,RETURN_REF_SN,RETURN_USERID,RETURN_TIME
FROM SAJET.CAMEO_QA_LEND_RETURN 
WHERE CARTON_NO LIKE '{0}%'
) a,SAJET.SYS_EMP e1,SAJET.SYS_EMP e2,SAJET.G_SN_STATUS c,SAJET.G_WO_BASE d,SAJET.SYS_PART p,SAJET.SYS_MODEL m
WHERE a.LEND_USERID=e1.EMP_ID(+) AND a.RETURN_USERID=e2.EMP_ID(+) 
AND a.CARTON_NO=c.CARTON_NO(+) AND a.PALLET_NO=c.PALLET_NO(+) AND c.WORK_ORDER=d.WORK_ORDER(+) AND c.PART_ID=p.PART_ID(+) AND p.MODEL_ID=m.MODEL_ID(+)
ORDER BY LEND_TIME DESC"
, CARTON_NO);
            }
            else
            {
                return string.Format(@"
SELECT DISTINCT a.TYPE,a.CARTON_NO,a.PALLET_NO,p.PART_NO,m.MODEL_NAME
,a.LEND_SN,a.LEND_USERID,e1.EMP_NO||' '||e1.EMP_NAME LEND_USER,TO_CHAR(a.LEND_TIME,'YYYY/MM/DD HH24:MI:SS') LEND_TIME
,a.RETURN_SN,a.RETURN_REF_SN,a.RETURN_USERID BACK_USERID,NVL2(a.RETURN_USERID,e2.EMP_NO||' '||e2.EMP_NAME,'') BACK_USER,TO_CHAR(a.RETURN_TIME,'YYYY/MM/DD HH24:MI:SS') BACK_TIME
FROM(
SELECT 'LEND' TYPE,CARTON_NO,PALLET_NO,LEND_SN,LEND_USERID,LEND_TIME,null RETURN_SN,null RETURN_REF_SN,null RETURN_USERID,null RETURN_TIME
FROM SAJET.CAMEO_QA_LEND 
WHERE CARTON_NO LIKE '{0}%' 
) a,SAJET.SYS_EMP e1,SAJET.SYS_EMP e2,SAJET.G_SN_STATUS c,SAJET.G_WO_BASE d,SAJET.SYS_PART p,SAJET.SYS_MODEL m
WHERE a.LEND_USERID=e1.EMP_ID(+) AND a.RETURN_USERID=e2.EMP_ID(+) 
AND a.CARTON_NO=c.CARTON_NO(+) AND a.PALLET_NO=c.PALLET_NO(+) AND c.WORK_ORDER=d.WORK_ORDER(+) AND c.PART_ID=p.PART_ID(+) AND p.MODEL_ID=m.MODEL_ID(+)
ORDER BY LEND_TIME DESC"
, CARTON_NO);
            }            
        }

        private string SQL_DATA_INS(RM_QAD001 RM)
        {
            return string.Format(@"INSERT INTO SAJET.CAMEO_QA_LEND (LEND_SN,CARTON_NO,PALLET_NO,LEND_USERID) VALUES ('{0}','{1}','{2}',{3})"
, RM.LEND_SN, RM.CARTON_NO, RM.PALLET_NO, RM.LEND_USERID);
        }

        private string SQL_DATA_UPD(RM_QAD001 RM)
        {
            return string.Format(@"INSERT INTO SAJET.CAMEO_QA_LEND_RETURN (CARTON_NO,PALLET_NO,LEND_SN,LEND_USERID,LEND_TIME,RETURN_SN,RETURN_REF_SN,RETURN_USERID) VALUES ('{0}','{1}','{2}',{3},TO_DATE('{4}','YYYY/MM/DD HH24:MI:SS'),'{5}','{6}',{7})"
, RM.CARTON_NO, RM.PALLET_NO, RM.LEND_SN, RM.LEND_USERID, RM.LEND_TIME, RM.BACK_SN, RM.BACK_REF_SN, RM.BACK_USERID);
        }

        private string SQL_DATA_DEL(RM_QAD001 RM)
        {
            return string.Format("DELETE FROM SAJET.CAMEO_QA_LEND WHERE CARTON_NO='{0}'", RM.CARTON_NO);
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

        /// <summary>
        /// 依 [棧板號] 取得 該棧版上的箱號清單
        /// </summary>
        /// <param name="PALLET"></param>
        /// <returns></returns>
        private string SQL_CARTON_LIST_GET(string PALLET)
        {
            return string.Format("SELECT COUNT(CARTON_NO) CNT FROM (SELECT DISTINCT CARTON_NO FROM SAJET.G_SN_STATUS WHERE PALLET_NO='{0}')", PALLET);
        }

        #endregion



        /// <summary>
        /// 取得 借出/歸還 清單
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> DLL_QAD001_TYPE()
        {
            IEnumerable<SelectListItem> Dll = new List<SelectListItem>();

            IEnumerable<QAD001_TYPE> Types = Enum.GetValues(typeof(QAD001_TYPE)).Cast<QAD001_TYPE>();
            Dll = from type in Types
                  select new System.Web.Mvc.SelectListItem
                  {
                      Text = type.ToString() == "ALL" ? "全部" : type.ToString() == "LEND" ? "借出" : "歸還",
                      Value = type.ToString()
                  };
            return Dll;
        }
    }
}