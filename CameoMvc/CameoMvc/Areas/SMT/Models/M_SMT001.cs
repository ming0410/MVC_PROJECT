using CameoMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace CameoMvc.Areas.SMT.Models
{
    /// <summary>
    /// 參數模組
    /// </summary>
    public class M_SMT001
    {
        public string FUNC { get; set; } = "SMT001";
        public string FUNC_NAME { get; } = "SMT標準工時表維護";

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
        public QM_SMT001 QM { get; set; } = new QM_SMT001();

        /// <summary>
        /// 查詢結果清單
        /// </summary>
        public List<RM_SMT001> LIST_RM { get; set; } = new List<RM_SMT001>();

        /// <summary>
        /// 生產線別下拉清單
        /// </summary>
        public IEnumerable<SelectListItem> DDL_LINE { get; set; }

        /// <summary>
        /// 製程面下拉清單
        /// </summary>
        public IEnumerable<SelectListItem> DDL_SIDE { get; set; }

        /// <summary>
        /// Table 之欄位名
        /// </summary>
        public static List<string> COL_NAME = new List<string>()
        {
            "MODEL_NAME",
            "PART_NO",
            "SIDE",
            "PDLINE_NAME",
            "STANDARD_TIME"
        };

        /// <summary>
        /// Table 之標頭
        /// </summary>
        public static List<string> COL_HEAD = new List<string>()
        {
            "機種",
            "料號",
            "製程面",
            "線別",
            "標準工時(秒)"
        };
    }

    /// <summary>
    /// 查詢欄位
    /// </summary>
    public class QM_SMT001
    {
        /// <summary>
        /// 線別
        /// </summary>
        public string[] Line { get; set; }
        /// <summary>
        /// 料號
        /// </summary>
        public string Part { get; set; }
        /// <summary>
        /// 機種
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 製程面
        /// </summary>
        public string Side { get; set; }
        /// <summary>
        /// 標準工時
        /// </summary>
        public string StdWT { get; set; }
        /// <summary>
        /// 更新者
        /// </summary>
        public string UpdUserId { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdTime { get; set; }
    }

    /// <summary>
    /// 查詢/結果欄位
    /// </summary>
    public class RM_SMT001
    {
        /// <summary>
        /// 線別ID
        /// </summary>
        public string LineId { get; set; }
        /// <summary>
        /// 線別
        /// </summary>
        public string Line { get; set; }
        /// <summary>
        /// 料號
        /// </summary>
        public string Part { get; set; }
        /// <summary>
        /// 機種
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 製程面
        /// </summary>
        public string Side { get; set; }
        /// <summary>
        /// 標準工時
        /// </summary>
        public string StdWT { get; set; }
        /// <summary>
        /// 更新者
        /// </summary>
        public string UpdUser { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdTime { get; set; }        
    }

    /// <summary>
    /// 功能函式
    /// </summary>
    public class FM_SMT001
    {
        /// <summary>查詢</summary>
        /// <param name="qData">查詢資料</param>
        /// <returns></returns>
        public M_SMT001 QUERY(M_SMT001 iData, bool IsExport)
        {
            string FUNC = "QUERY";
            string ERR_MSG = string.Empty;
            string CARTON_NO = string.Empty;
            string SqlWhere = string.Empty;

            M_SMT001 oData = iData;

            //執行前先初始化執行結果預設值
            oData.FLAG = false;
            oData.ERR = string.Empty;
            oData.LIST_RM = new List<RM_SMT001>();

            try
            {
                #region 下拉清單
                //新增線別下拉清單
                if (oData.DDL_LINE == null)
                {
                    oData.DDL_LINE = M_MFG.DLL_LINE("SMT", true);
                }

                //新增製程面下拉清單
                if (oData.DDL_SIDE == null)
                {
                    oData.DDL_SIDE = M_MFG.DLL_PCB_SIDE();
                }
                #endregion

                #region 篩選條件
                if (oData.QM.Line != null && oData.QM.Line.Length > 0)
                {
                    string line = string.Empty;
                    if (!oData.QM.Line.Contains("ALL"))
                    {
                        foreach (string _line in oData.QM.Line)
                        {
                            line += (_line + ",");
                        }
                        SqlWhere = string.Format("AND A.PDLINE_ID IN ({0}) ", line.Substring(0, line.Length - 1));
                    }
                }
                if (!string.IsNullOrWhiteSpace(oData.QM.Side))
                {
                    if (oData.QM.Side != M_MFG.PCB_SIDE.NONE.ToString())
                    {
                        SqlWhere += string.Format("AND A.SIDE LIKE '{0}%' ", oData.QM.Side);
                    }
                }
                if (!string.IsNullOrWhiteSpace(oData.QM.Part))
                {
                    SqlWhere += string.Format("AND T.PART_NO LIKE '{0}%' ", oData.QM.Part);
                }
                #endregion


                RM_SMT001 RM = null;
                DataTable dtQ = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_GET(SqlWhere));
                if (dtQ?.Rows?.Count > 0)
                {
                    foreach (DataRow dr in dtQ.Rows)
                    {
                        RM = new RM_SMT001();
                        RM.Part = dr["PART_NO"].ToString();
                        RM.LineId = dr["PDLINE_ID"].ToString();
                        RM.Line = dr["PDLINE_NAME"].ToString();
                        RM.Side = dr["SIDE"].ToString();
                        RM.Model = dr["MODEL_NAME"].ToString();
                        RM.StdWT = dr["STANDARD_TIME"].ToString();
                        RM.UpdUser = dr["UPDATE_USER"].ToString();
                        RM.UpdTime = dr["UPDATE_TIME"].ToString();
                        oData.LIST_RM.Add(RM);
                    }
                    if (IsExport)
                    {
                        DataTable dtE = FM_SYS.DataTable_ColNameChange(dtQ, M_SMT001.COL_NAME, M_SMT001.COL_HEAD);
                        string xlsFile = "CAMEO_SmtStdWorkTime_" + DateTime.Today.ToString("yyyyMMdd") + "_" + oData.USER_NO + ".XLSX";
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

        /// <summary>
        /// 儲存
        /// </summary>
        /// <param name="iQM"></param>
        /// <param name="ERR_MSG"></param>
        /// <returns></returns>
        public bool SAVE(QM_SMT001 iQM, out string ERR_MSG)
        {
            bool res = false;
            string FUNC = "SAVE";
            ERR_MSG = string.Empty;

            try
            {
                #region 確認填入資料完整性
                if (string.IsNullOrWhiteSpace(iQM.Part))
                {
                    ERR_MSG = "資料不完整，無法儲存" + Environment.NewLine + "沒有料號";
                }
                else if (string.IsNullOrWhiteSpace(iQM.StdWT))
                {
                    ERR_MSG = "資料不完整，無法儲存" + Environment.NewLine + "沒有填工時";
                }
                #endregion

                if (string.IsNullOrWhiteSpace(ERR_MSG))
                {
                    #region 取得料號ID
                    string PartId = string.Empty;
                    DataTable dtPID = M_DB.dsMes.GetTable_SqlCmd(SQL_PARTID_GET(iQM.Part));
                    if (dtPID?.Rows?.Count > 0)
                    {
                        PartId = dtPID.Rows[0]["PART_ID"].ToString();
                    }
                    #endregion

                    if (string.IsNullOrWhiteSpace(PartId))
                    {
                        ERR_MSG = string.Format("系統沒有該料號[{0}]", iQM.Part);
                    }
                    else
                    {
                        List<string> _lines = new List<string>();
                        if (iQM.Line.Contains("ALL"))
                        {
                            IEnumerable<SelectListItem> DDL_LINE = M_MFG.DLL_LINE("SMT", false);

                            foreach (SelectListItem s in DDL_LINE)
                            {
                                _lines.Add(s.Value);
                            }
                        }
                        else
                        {
                            foreach (string s in iQM.Line)
                            {
                                _lines.Add(s);
                            }
                        }

                        #region 逐一儲存作業
                        foreach (string sLine in _lines)
                        {
                            //判斷資料是否存在
                            DataTable dtExist = M_DB.dsMes.GetTable_SqlCmd(SQL_DATA_ROWCOUNT(sLine, iQM.Side, PartId));
                            if (int.Parse(dtExist.Rows[0][0].ToString()) > 0)
                            {
                                //執行 Update
                                if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_UPD(sLine, iQM.Side, PartId, iQM.StdWT, iQM.UpdUserId, iQM.UpdTime)) <= 0)
                                {
                                    ERR_MSG = "無法更新紀錄";
                                }
                                else
                                {
                                    res = true;
                                }
                            }
                            else
                            {
                                //執行 Insert
                                if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_INS(sLine, iQM.Side, PartId, iQM.StdWT, iQM.UpdUserId)) <= 0)
                                {
                                    ERR_MSG = "無法新增紀錄";
                                }
                                else
                                {
                                    res = true;
                                }
                            }
                        }
                        #endregion
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
        /// 刪除
        /// </summary>
        /// <param name="iQM"></param>
        /// <param name="ERR_MSG"></param>
        /// <returns></returns>
        public bool DELETE(QM_SMT001 iQM, out string ERR_MSG)
        {
            bool res = false;
            string FUNC = "DELETE";
            ERR_MSG = string.Empty;
            string SqlWhere = string.Empty;
            string PartId = string.Empty;

            try
            {
                #region 確認是否有選擇項目
                if (iQM.Line.Length <= 0)
                {
                    ERR_MSG = "資料不完整，無法刪除" + Environment.NewLine + "沒有任何線別";
                }
                else if (string.IsNullOrWhiteSpace(iQM.Part))
                {
                    ERR_MSG = "資料不完整，無法刪除" + Environment.NewLine + "沒有料號";
                }
                else
                {
                    #region 取得料號ID
                    DataTable dtPID = M_DB.dsMes.GetTable_SqlCmd(SQL_PARTID_GET(iQM.Part));
                    if (dtPID?.Rows?.Count > 0)
                    {
                        PartId = dtPID.Rows[0]["PART_ID"].ToString();
                    }

                    if (string.IsNullOrWhiteSpace(PartId))
                    {
                        ERR_MSG = string.Format("系統沒有該料號[{0}]", iQM.Part);
                    }
                    #endregion
                }
                #endregion

                if(string.IsNullOrWhiteSpace(ERR_MSG))
                {
                    #region 篩選條件
                    if (iQM.Line != null && iQM.Line.Length > 0)
                    {
                        string line = string.Empty;
                        if (!iQM.Line.Contains("ALL"))
                        {
                            foreach (string _line in iQM.Line)
                            {
                                line += (_line + ",");
                            }
                            SqlWhere = string.Format("AND PDLINE_ID IN ({0}) ", line.Substring(0, line.Length - 1));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(iQM.Side))
                    {
                        if (iQM.Side != M_MFG.PCB_SIDE.NONE.ToString())
                        {
                            SqlWhere += string.Format("AND SIDE='{0}' ", iQM.Side);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(PartId))
                    {
                        SqlWhere += string.Format("AND PART_ID={0} ", PartId);
                    }
                    #endregion

                    if (string.IsNullOrWhiteSpace(SqlWhere))
                    {
                        ERR_MSG = "資料不完整，無法刪除" + Environment.NewLine + "沒有任何線別/料號";
                    }
                    else
                    {
                        //刪除第一個 AND 字串
                        SqlWhere = SqlWhere.Substring(4);

                        //執行 Update
                        if (M_DB.dsMes.Execute_SqlCmd(SQL_DATA_DEL(SqlWhere, iQM.UpdUserId, iQM.UpdTime)) <= 0)
                        {
                            ERR_MSG = "無法更新紀錄";
                        }
                        else
                        {
                            res = true;
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


        #region SQL
        /// <summary>
        /// 取得料號ID
        /// </summary>
        /// <param name="sPart"></param>
        /// <returns></returns>
        public string SQL_PARTID_GET(string sPart)
        {
            return string.Format("SELECT PART_ID FROM SYS_PART WHERE PART_NO = '{0}'", sPart);
        }
        
        /// <summary>
        /// 取得表準工時資料，依料號、線別、製程面排序
        /// </summary>
        /// <param name="SqlWhere"></param>
        /// <returns></returns>
        public string SQL_DATA_GET(string SqlWhere)
        {
            return string.Format(@"
SELECT M.MODEL_NAME,T.PART_NO,A.SIDE,A.PDLINE_ID,L.PDLINE_NAME,A.STANDARD_TIME,E.EMP_NO||' '||E.EMP_NAME UPDATE_USER,TO_CHAR(A.UPDATE_TIME,'YYYY/MM/DD HH24:MI:SS') UPDATE_TIME
FROM SAJET.CAMEO_WORKING_TIME A,SAJET.SYS_PDLINE L,SAJET.SYS_PART T,SAJET.SYS_MODEL M,SAJET.SYS_EMP E
WHERE A.PDLINE_ID=L.PDLINE_ID AND A.PART_ID=T.PART_ID AND T.MODEL_ID=M.MODEL_ID AND A.UPDATE_USERID=E.EMP_ID AND A.ENABLED='Y' {0} ORDER BY PART_NO,PDLINE_NAME,SIDE
", SqlWhere);
        }

        /// <summary>
        /// 條件下的資料是否存在
        /// </summary>
        /// <param name="sLine">線別</param>
        /// <param name="sSide">製程面</param>
        /// <param name="sPart">料號</param>
        /// <returns>SQL語法</returns>
        public string SQL_DATA_ROWCOUNT(string sLine, string sSide, string sPart)
        {
            string SQL = string.Empty;
            if(sSide == M_MFG.PCB_SIDE.NONE.ToString())
                SQL = string.Format("SELECT COUNT(PART_ID) CNT FROM SAJET.CAMEO_WORKING_TIME WHERE PDLINE_ID={0} AND SIDE IS NULL AND PART_ID='{2}'", sLine, sSide, sPart);
            else
                SQL = string.Format("SELECT COUNT(PART_ID) CNT FROM SAJET.CAMEO_WORKING_TIME WHERE PDLINE_ID={0} AND SIDE='{1}' AND PART_ID='{2}'", sLine, sSide, sPart);
            return SQL;
        }

        /// <summary>
        /// 新增一筆資料
        /// </summary>
        /// <param name="sLine">線別</param>
        /// <param name="sSide">製程面</param>
        /// <param name="sPart">料號</param>
        /// <param name="sWT">工時</param>
        /// <param name="sUser">新增者ID</param>
        /// <returns>SQL語法</returns>
        public string SQL_DATA_INS(string sLine, string sSide, string sPart, string sWT, string sUser)
        {
            string SQL = string.Empty;
            if (sSide == M_MFG.PCB_SIDE.NONE.ToString())
                SQL = string.Format("INSERT INTO SAJET.CAMEO_WORKING_TIME (PDLINE_ID,PART_ID,STANDARD_TIME,UPDATE_USERID) VALUES ({0},{2},{3},{4})", sLine, sSide, sPart, sWT, sUser);
            else
                SQL = string.Format("INSERT INTO SAJET.CAMEO_WORKING_TIME (PDLINE_ID,SIDE,PART_ID,STANDARD_TIME,UPDATE_USERID) VALUES ({0},'{1}',{2},{3},{4})", sLine, sSide, sPart, sWT, sUser);
            return SQL;
        }

        /// <summary>
        /// 更新條件下的資料
        /// </summary>
        /// <param name="sLine">線別</param>
        /// <param name="sSide">製程面</param>
        /// <param name="sPart">料號</param>
        /// <param name="sWT">工時</param>
        /// <param name="sUser">更新者ID</param>
        /// <param name="sUTime">更新時間</param>
        /// <returns>SQL語法</returns>
        public string SQL_DATA_UPD(string sLine, string sSide, string sPart, string sWT, string sUser, string sUTime)
        {
            string SQL = string.Empty;
            if (sSide == M_MFG.PCB_SIDE.NONE.ToString())
                SQL = string.Format("UPDATE SAJET.CAMEO_WORKING_TIME SET ENABLED='Y',STANDARD_TIME={3},UPDATE_USERID={4},UPDATE_TIME=TO_DATE('{5}','YYYY/MM/DD HH24:MI:SS') WHERE PDLINE_ID={0} AND SIDE IS NULL AND PART_ID={2}", sLine, sSide, sPart, sWT, sUser, sUTime);
            else
                SQL = string.Format("UPDATE SAJET.CAMEO_WORKING_TIME SET ENABLED='Y',STANDARD_TIME={3},UPDATE_USERID={4},UPDATE_TIME=TO_DATE('{5}','YYYY/MM/DD HH24:MI:SS') WHERE PDLINE_ID={0} AND SIDE='{1}' AND PART_ID={2}", sLine, sSide, sPart, sWT, sUser, sUTime);
            return SQL;
        }

        /// <summary>
        /// 更新條件下的資料為刪除標記
        /// </summary>
        /// <param name="SqlWhere">篩選條件</param>
        /// <param name="sUser">更新者ID</param>
        /// <param name="sUTime">更新時間</param>
        /// <returns></returns>
        public string SQL_DATA_DEL(string SqlWhere, string sUser, string sUTime)
        {
            return string.Format("UPDATE SAJET.CAMEO_WORKING_TIME SET ENABLED='N',UPDATE_USERID={1},UPDATE_TIME=TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS') WHERE {0}", SqlWhere, sUser, sUTime);
        }

        #endregion
    }
}