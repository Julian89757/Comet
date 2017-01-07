using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using ChatData.Common;
using CometServer.Common;

#region 命名空间
using System.Data;
using System.Data.SQLite;
using ChatData.ChatModel;
#endregion

namespace ChatData.ChatDAL
{
    /// <summary>
    /// 用户信息--数据访问类
    /// </summary>
    public class UserDAL
    {
        #region 新增用户记录
        /// <summary>
        /// 新增用户记录
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public int AddUser(UserInfo user)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("insert into UserInfo ([UserId],[UserName],[NikeName],[PassWord],[Sex],[BirthDay],[Phone],[Email],[Status],[IsValid])  values ");
            sbSql.Append("(@UserId,@UserName,@NikeName,@PassWord,@Sex,@BirthDay,@Phone,@Email,@Status,@IsValid )");
            DbParameter[] parameters = 
            { 
                new SqlParameter("@UserId", DbType.String){Value=user.UserId},
                new SqlParameter("@UserName", DbType.String){Value=user.UserName},
                new SqlParameter("@NikeName", DbType.String){Value=user.NikeName },
                new SqlParameter("@PassWord", DbType.String){Value=user.PassWord},
                new SqlParameter("@Sex", DbType.Int32){  Value= user.Sex },
                new SqlParameter("@BirthDay", DbType.Int32){Value=user.BirthDay},
                new SqlParameter("@Phone", DbType.String){Value=user.Phone},
                new SqlParameter("@Email", DbType.String){Value= user.Email},
                new SqlParameter("@Status", DbType.Int32){Value= user.Status },
                new SqlParameter("@IsValid", DbType.String){Value=user.IsValid  },
            };
            var connection = Dbhelper.OpenConnect("ConnectionString");

            return Dbhelper.Execute(sbSql.ToString(), connection, parameters);
        }
        #endregion

        #region 获取所有用户列表
        /// <summary>
        /// 获取所有用户列表
        /// </summary>        
        /// <returns></returns>
        public DataSet GetUserAll()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from UserInfo order by OnlineTime desc");
            return SqliteHelper.ExecuteDataset(sbSql.ToString(), null);
        }
        #endregion

        #region 根据用户名查询用户信息
        /// <summary>
        /// 根据用户id查询用户信息
        /// </summary>
        /// <param name="account">用户信息</param>
        /// <returns></returns>
        public DataTable  GetUserOne(UserInfo user)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from UserInfo where UserName=@UserName");
            DbParameter[] parameters = 
            { 
                new SqlParameter("@UserName", DbType.String){  Value=user.UserName}
            };
            var connection = Dbhelper.OpenConnect("ConnectionString");
            return Dbhelper.ExecuteQueryToDataTable(sbSql.ToString(), connection, parameters);
        }
        #endregion

        public DataTable GetUserOne(Guid userId)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from UserInfo where UserId='"+ userId+"'");
            var connection = Dbhelper.OpenConnect("ConnectionString");
            return Dbhelper.ExecuteQueryToDataTable(sbSql.ToString(), connection, null );
        }

        #region 修改用户记录
        /// <summary>
        /// 修改用户记录
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public int UpdateUser(UserInfo user)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("update UserInfo set ");
            sbSql.Append("UserName=@UserName,PassWord=@PassWord,Sex=@Sex,Age=@Age,Email=@Email,Status=@Status,OnlineTime=@OnlineTime,OfflineTime=@OfflineTime ");
            sbSql.Append("where UserId=@UserId");
            SQLiteParameter[] parameters = 
            { 
                new SQLiteParameter("@UserName", DbType.String){Value=user.UserName},
                new SQLiteParameter("@NikeName", DbType.String){Value=user.NikeName },
                new SQLiteParameter("@PassWord", DbType.String){Value=user.PassWord},
                new SQLiteParameter("@Sex", DbType.Int32){  Value= user.Sex},
                new SQLiteParameter("@BirthDay", DbType.String){Value=user.BirthDay},
                new SQLiteParameter("@Phone", DbType.String){Value=user.Phone},
                new SQLiteParameter("@Email", DbType.String){Value=user.Email},
                new SQLiteParameter("@Status", DbType.Int32){Value = user.Status },
                new SQLiteParameter("@OnlineTime", DbType.DateTime){Value=Convert.ToDateTime(user.OnlineTime)},
                new SQLiteParameter("@OfflineTime", DbType.DateTime){Value=Convert.ToDateTime(user.OfflineTime)},

                new SQLiteParameter("@UserId", DbType.String){Value=user.UserId}
            };
            return SqliteHelper.ExecuteNonQuery(sbSql.ToString(), parameters);
        }
        #endregion
    }
}
