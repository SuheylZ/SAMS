using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

using DBG = System.Diagnostics.Debug;

namespace DataProxy
{
    internal class Storage
    {
        IDbConnection _cnn;
        private static Storage _this=null;
        private string _connStr = "";

        #region Static Functions
        internal static void Initialize(string sIn)
        {
            _this = new Storage(sIn);
        }
        internal static Storage Get()
        {
            return _this;
        }
        internal static void Cleanup()
        {
            _this._cnn.Close();
            _this._cnn.Dispose();
            GC.SuppressFinalize(_this);
            _this = null;
        }

#endregion

        #region Executer
        internal IDataReader ExecuteReader(string sql, params object[] col)
        {
            IDataReader rdr = null;
            using (IDbCommand cmd = CreateCommand2(sql, col))
            {
                rdr = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.CloseConnection);
            }
            return rdr;
        }

        internal object ExecuteObject(string sql, params object[] col)
        {
            object obj = null;
            using (IDbCommand cmd = CreateCommand(sql, col))
            {
                obj = cmd.ExecuteScalar();
            }
            return (obj == DBNull.Value) ? null : obj;
        }

        internal int ExecuteInt(string sql, params object[] col)
        {
            object obj = ExecuteObject(sql, col);
            return  obj==null ? 0 : Convert.ToInt32(obj);
        }

        internal string ExecuteText(string sql, params object[] col)
        {
            object obj = ExecuteObject(sql, col);
            return obj == null ? string.Empty : Convert.ToString(obj);
        }

        internal bool ExecuteBool(string sql, params object[] col)
        {
            object obj = ExecuteObject(sql, col);
            return obj == null ? false : true;
        }

        internal double ExecuteReal(string sql, params object[] col)
        {
            object obj = ExecuteObject(sql, col);
            return obj == null ? 0 : Convert.ToDouble(obj);
        }

        internal void ExecuteNonQuery(string sql, params object[] col)
        {
            using (IDbCommand cmd = CreateCommand(sql, col))
            {
                cmd.ExecuteNonQuery();
            }
        }

        internal int Identity
        {
            get { return ExecuteInt("Select @@identity"); }
        }

        internal bool IsConnected
        {
            get { return _cnn.State == ConnectionState.Open; }
        }
        #endregion

        #region Core
        private Storage(string sIn)
        {
            _cnn = new SqlConnection();
            _connStr = sIn;
            _cnn.ConnectionString = _connStr;
            _cnn.Open();
            DBG.Assert(_cnn.State == ConnectionState.Open);
        }

        private IDbCommand CreateCommand(string sql,params object[] col)
        {
            IDbCommand cmd= new SqlCommand(); 
            cmd.CommandText = sql;
            cmd.CommandTimeout = 30;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = _cnn;

            if (col.Length != 0)
            {
                List<string> paramNames = FindParameters(sql);
                if (paramNames.Count > 0)
                    for (int i = 0; i < paramNames.Count; i++)
                    {
                        IDataParameter param = cmd.CreateParameter();
                        param.ParameterName = paramNames[i];
                        param.Direction = ParameterDirection.Input;
                        //param.DbType = col[i].GetType();
                        param.Value = col[i];
                        cmd.Parameters.Add(param);
                    }
            }
            return cmd;
        }

        private IDbCommand CreateCommand2(string sql, params object[] col)
        {
            IDbCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandTimeout = 30;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = GetDuplicateConnection();

            if (col.Length != 0)
            {
                List<string> paramNames = FindParameters(sql);
                if (paramNames.Count > 0)
                    for (int i = 0; i < paramNames.Count; i++)
                    {
                        IDataParameter param = cmd.CreateParameter();
                        param.ParameterName = paramNames[i];
                        param.Direction = ParameterDirection.Input;
                        //param.DbType = col[i].GetType();
                        param.Value = col[i];
                        cmd.Parameters.Add(param);
                    }
            }
            return cmd;
        }
        private List<string> FindParameters(string sql)
        {
            List<string> names = new List<string>(32);
            CommandType typ = FindSqlType(sql);
            if (typ == CommandType.Text)
            {
                Regex regex = new Regex(@"@[a-zA-Z]+[0-9]*", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                MatchCollection colMatches = regex.Matches(sql);
                foreach (Match item in colMatches)
                {
                    if (!AlreadyPresent(names, item.Value))
                        names.Add(item.Value);
                }
            }
            else if (typ == CommandType.StoredProcedure)
            {


            }
            return names;
        }

        private bool AlreadyPresent(List<string> list, string s)
        {
            foreach (string itm in list)
                if (itm.ToLower() == s.ToLower())
                    return true;
            return false;
        }

        private IDbConnection GetDuplicateConnection()
        {
            SqlConnection cnn = new SqlConnection(_connStr);
            cnn.Open();
            return cnn;
        }

        private CommandType FindSqlType(string sql)
        {
            CommandType tret = CommandType.Text;
            Regex regex = new Regex(@"[select|insert|update|delete|create|drop|add|alter|set]\b", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
            if (!regex.IsMatch(sql))
            {
                tret = CommandType.StoredProcedure;
                //verify further by querying the database
                // so that a table name is not mixed with 
                // a stored procedure. 
            }
            return tret;
        }
        #endregion

    };

    public class Proxy
    {
        public static void Start(string s)
        {
            Storage.Initialize(s); 
        }

        public static void Stop()
        {
            Storage.Cleanup(); 
        }

        public static bool IsConnected
        {
            get {
                bool bRet = false;
                try
                {
                    Storage store = Storage.Get();
                    bRet = store.IsConnected;
                }catch(Exception ex){
                    
                }
                return bRet;
            }
        }

    };
}
