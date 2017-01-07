using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CometServer.Common
{
    public class Dbhelper
    {
        public Dbhelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            
        }

        /// <summary>
        /// 得到连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        private static string getConnString(string key)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings[key].ToString();
            if (connStr == null || connStr == "")
            {
                Dbhelper.ErrLog("Dbhelper.getConnString(string key):[" + key + "]所指定的连接类型为空");
            }
            return connStr;
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="connName">连接名</param>
        /// <returns></returns>
        public static System.Data.Common.DbConnection OpenConnect(string connName)
        {

            System.Data.Common.DbConnection Conn;
            //得到配置文件中的连接信息
            System.Configuration.ConnectionStringSettings s =
                System.Configuration.ConfigurationManager.ConnectionStrings[connName];
            //得到驱动类型
            System.Data.Common.DbProviderFactory f = System.Data.Common.DbProviderFactories.GetFactory(s.ProviderName);
            Conn = f.CreateConnection();
            //得到连接字符串
            Conn.ConnectionString = s.ConnectionString;
            Conn.Open();
            return Conn;
        } //OpenConnect(string connName)

        /// <summary>
        /// 执行查询返回DataTable
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>成功返回DataTable,失败则返回 null</returns>
        public static System.Data.DataTable ExecuteQueryToDataTable(string sql, System.Data.Common.DbConnection Conn, System.Data.Common.DbParameter[] param =null )
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.IDataReader reader = ExecuteQuery(sql, Conn,param);
            dt.Load(reader);
            return dt;

        } //ExecuteQueryToDataTable(string sql)


        /// <summary>
        /// 执行查询返回DataReader
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="Conn">连接对象</param>
        /// <returns>成功时返回Reader对象，失败时返回null</returns>
        public static System.Data.IDataReader ExecuteQuery(string sql, System.Data.Common.DbConnection Conn, System.Data.Common.DbParameter[] param =null)
        {
            System.Data.IDataReader reader = null;
            if (Conn == null)
            {
                return null;
            }
            try
            {
                if (Conn.State == System.Data.ConnectionState.Closed)
                {
                    Conn.Open();
                }
                System.Data.IDbCommand cmd = Conn.CreateCommand();
                cmd.CommandText = sql;
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        cmd.Parameters.Add(param[i]);
                    }
                }

                reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                Dbhelper.ErrLog("Dbhelper.ExecuteQuery(string sql, System.Data.Common.DbConnection Conn):" + ex.Message);
                return null;
            }

        } //ExecuteQuery(string sql)

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <returns>返回受影响行数</returns>
        public static int Execute(string sql, System.Data.Common.DbConnection Conn)
        {
            if (Conn == null)
            {
                Dbhelper.ErrLog("Dbhelper.Execute(string sql, System.Data.Common.DbConnection Conn):连接对象为空!");
                return 0;
            }
            System.Data.IDbCommand cmd = Conn.CreateCommand();

            cmd.CommandText = sql;
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Dbhelper.ErrLog("Dbhelper.ExecuteQuery(string sql, System.Data.Common.DbConnection Conn):" + ex.Message +
                             "/nsql=" + sql);
                return 0;
            }
        } //Execute(string sql)

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="Conn">数据库连接对象</param>
        /// <param name="param">参数</param>
        /// <returns>返回受影响行数</returns>
        public static int Execute(string sql, System.Data.Common.DbConnection Conn,System.Data.Common.DbParameter[] param)
        {
            if (Conn == null)
            {
                Dbhelper.ErrLog(
                    "Dbhelper.Execute(string sql, System.Data.Common.DbConnection Conn, System.Data.Common.DbParameter[] param):连接对象为空!");
                return 0;
            }
            System.Data.IDbCommand cmd = Conn.CreateCommand();
            cmd.CommandText = sql;
            for (int i = 0; i < param.Length; i++)
            {
                cmd.Parameters.Add(param[i]);
            }
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Dbhelper.ErrLog(
                    "Dbhelper.Execute(string sql, System.Data.Common.DbConnection Conn, System.Data.Common.DbParameter[] param):" +
                    ex.Message + "/nsql=" + sql);
                return 0;
            }
        } //Execute(string sql,System.Data.IDataParameter[] param)

        /// <summary>
        /// 执行一个事务
        /// </summary>
        /// <param name="sqls">Sql语句组</param>
        /// <returns>成功时返回true</returns>
        public static bool ExecuteTrans(string[] sqls, System.Data.Common.DbConnection Conn)
        {
            System.Data.IDbTransaction myTrans;
            if (Conn == null)
            {
                Dbhelper.ErrLog("Dbhelper.ExecuteTrans(string[] sqls):连接对象为空!");
                return false;
            }
            System.Data.IDbCommand cmd = Conn.CreateCommand();
            myTrans = Conn.BeginTransaction();
            cmd.Transaction = myTrans;
            try
            {
                foreach (string sql in sqls)
                {
                    if (sql != null)
                    {
                        cmd.CommandText = sql;

                        cmd.ExecuteNonQuery();
                    }
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                Dbhelper.ErrLog("Dbhelper.ExecuteTrans(string[] sqls):" + ex.Message);
                return false;
            }
            return true;
        } //Execute(string sql)

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="ErrInfo">错误信息</param>
        private static void ErrLog(string ErrInfo)
        {
            string fileName = System.Configuration.ConfigurationManager.AppSettings["ErrorLogFile"];
            string isWrite = System.Configuration.ConfigurationManager.AppSettings["ErrorLog"].Trim().ToLower();
            if (isWrite == "yes")
            {
                //将错误信息写入系统日志
                //System.Diagnostics.EventLog log = new System.Diagnostics.EventLog();
                //log.Source = "NewFrame";
                //log.WriteEntry(ErrInfo);
                //将错误信息写入记录文件
                StreamWriter sw = new StreamWriter(fileName, true);
                sw.WriteLine(System.DateTime.Now);
                sw.WriteLine(ErrInfo);
                sw.WriteLine();
                sw.Close();
            }
        } //ErrLog(string ErrInfo)
    }
}