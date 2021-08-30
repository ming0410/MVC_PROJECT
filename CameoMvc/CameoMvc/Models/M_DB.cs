using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace CameoMvc.Models
{

    public class M_DB
    {
        //public static DataSource dsMes = new DataSource();
        public static DataSource dsMes = new DataSource("DATA SOURCE=");
        public static DataSource dsWms = new DataSource("DATA SOURCE=");
    }

    /// <summary>
    /// 資料庫連結參數之結構
    /// </summary>
    public struct DbField
    {
        /// <summary>資料庫伺服器</summary>
        public string ServerIP;
        /// <summary>資料庫伺服器連結埠</summary>
        public string Port;
        /// <summary>資料庫服務</summary>
        public string Service;
        /// <summary>登入帳號</summary>
        public string User;
        /// <summary>登入密碼</summary>
        public string Password;
    };

    [Serializable]
    public class DataSource
    {
        [NonSerialized]
        private string _ConnectionString = "DATA SOURCE=";
        /// <summary>資料庫連結字串，預設值為 MES正式環境資料庫</summary>
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        [NonSerialized]
        private OracleConnection _DBConn;
        /// <summary>資料庫連線物件</summary>
        public OracleConnection DBConn
        {
            get { return _DBConn; }
            set { _DBConn = value; }
        }

        [NonSerialized]
        private DbField _DBField;
        /// <summary>資料庫連線物件</summary>
        public DbField DBField
        {
            get { return _DBField; }
            set { _DBField = value; }
        }

        /// <summary>資料來源物件</summary>
        public DataSource()
        {

        }

        /// <summary>資料來源物件</summary>
        /// <param name="ConnectionString">連結字串</param>
        public DataSource(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
            _DBField = SetDBField();
        }

        /// <summary>資料來源物件</summary>
        /// <param name="dBField">資料庫連結參數</param>
        public DataSource(DbField dBField)
        {
            _DBField = dBField;
            _ConnectionString = SetConnectionString();
        }

        /// <summary>釋放資料庫來源資源
        /// </summary>
        public void Dispose()
        {
            if (DBConn != null)
            {
                DBConn.Close();
                DBConn.Dispose();
                DBConn = null;
            }
        }

        private string SetConnectionString()
        {
            string ConnString = string.Format("DATA SOURCE={0}:{1}/{2};PERSIST SECURITY INFO=True;USER ID={3};password={4}", _DBField.ServerIP, _DBField.Port, _DBField.Service, _DBField.User, _DBField.Password);
            return ConnString;
        }

        private DbField SetDBField()
        {
            DbField df = new DbField();
            df.ServerIP = GetValueFromConnString(_ConnectionString, "DATA SOURCE=", ":");
            df.Port = GetValueFromConnString(_ConnectionString, ":", "/");
            df.Service = GetValueFromConnString(_ConnectionString, "/", ";");
            df.User = GetValueFromConnString(_ConnectionString, "USER ID=", ";");
            df.Password = GetValueFromConnString(_ConnectionString, "password=", ";");
            return df;
        }

        private string GetValueFromConnString(string oText, string sText, string eText)
        {
            string Val = string.Empty;

            int sIndex = oText.IndexOf(sText);
            sIndex = sIndex + sText.Length;
            int eIndex = oText.IndexOf(eText, sIndex);
            if (eIndex == -1)
            {
                Val = oText.Substring(sIndex);
            }
            else
            {
                Val = oText.Substring(sIndex, eIndex - sIndex);
            }
            return Val;
        }

        /// <summary>設定資料庫連線</summary>
        /// <param name="Connection_str">資料庫連線字串</param>
        private void SetConnection(string ConnString)
        {
            DBConn = new OracleConnection(_ConnectionString);
            try
            {
                DBConn.Open();
            }
            catch
            {
                if (DBConn != null) DBConn.Dispose();
            }
            finally { GC.Collect(); }
        }

        /// <summary>以SQL語法，取得 DataTable</summary>
        /// <param name="SqlCmd">SQL語法</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable_SqlCmd(string SqlCmd)
        {
            DataSet ds = GetDataSet_SqlCmd(SqlCmd);
            if (ds == null) return null;
            else return ds.Tables[0];
        }

        /// <summary>以SQL語法，取得 DataSet</summary>
        /// <param name="SqlCmd">SQL語法</param>
        /// <returns>DataTable</returns>
        public DataSet GetDataSet_SqlCmd(string SqlCmd)
        {
            try
            {
                if (DBConn == null) SetConnection(_ConnectionString);
                if (DBConn.State == System.Data.ConnectionState.Closed) DBConn.Open();
                DataSet myDataSet = new DataSet();
                OracleDataAdapter DataAdapter = new OracleDataAdapter(SqlCmd, DBConn);
                DataAdapter.Fill(myDataSet);
                return myDataSet;
            }
            catch (Exception exp)
            {
                return null;
                throw;
            }
            finally
            {
                if (DBConn != null) DBConn.Close();
            }
        }

        /// <summary>以 Store Procedure，取得 DataTable</summary>
        /// <param name="Procedure">SQL語法或程序名稱</param>
        /// <param name="Parms">參數</param>
        /// <returns></returns>
        public DataTable GetTable_Procedure(string Procedure, List<string[]> Parms)
        {
            DataSet ds = GetDataSet_Procedure(Procedure, Parms);
            if (ds == null) return null;
            else return ds.Tables[0];
        }

        /// <summary>以Store Procedure，取得 DataSet</summary>
        /// <param name="Procedure">Store Procedure Name</param>
        /// <param name="Parms">參數</param>
        /// <returns></returns>
        public DataSet GetDataSet_Procedure(string Procedure, List<string[]> Parms)
        {
            try
            {
                if (DBConn == null) SetConnection(_ConnectionString);
                if (DBConn.State == System.Data.ConnectionState.Closed) DBConn.Open();
                DataSet myDataSet = new DataSet();

                OracleParameter oParm;
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = DBConn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Procedure;
                foreach (string[] parm in Parms)
                {
                    oParm = new OracleParameter(parm[0], parm[1]);
                    cmd.Parameters.Add(oParm);
                }

                OracleDataAdapter DataAdapter = new OracleDataAdapter();
                DataAdapter.Fill(myDataSet);
                return myDataSet;
            }
            catch (Exception exp)
            {
                return null;
            }
            finally
            {
                DBConn.Close();
            }
        }

        /// <summary>執行SQL語法</summary>
        /// <param name="SqlCmd">SQL語法</param>
        /// <returns></returns>
        public int Execute_SqlCmd(string SqlCmd)
        {
            int cnt = -1;
            try
            {
                if (DBConn == null) SetConnection(_ConnectionString);
                if (DBConn.State == System.Data.ConnectionState.Closed) DBConn.Open();

                OracleCommand cmd = new OracleCommand(SqlCmd, DBConn);
                cnt = cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                cnt = -1;
            }
            finally
            {
                DBConn.Close();
            }
            return cnt;
        }

        /// <summary>執行SQL語法</summary>
        /// <param name="SqlCmd">SQL語法</param>
        /// <param name="ConnString">連結字串</param>
        /// <returns></returns>
        public int Execute_SqlCmd(string SqlCmd, string ConnString)
        {
            _ConnectionString = ConnString;
            return Execute_SqlCmd(SqlCmd);
        }
    }
}