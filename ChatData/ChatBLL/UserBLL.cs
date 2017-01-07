using System;
using System.Collections.Generic;
using System.Text;
#region 命名空间
using System.Data;
using ChatData.ChatDAL;
using ChatData.ChatModel;
#endregion

namespace ChatData.ChatBLL
{
    /// <summary>
    /// 用户信息--业务逻辑类
    /// </summary>
    public class UserBll
    {

        private readonly UserDAL  _userDal = new UserDAL();

        public bool UserLogin(string userName,string Pwd)
        {
            var   dt =_userDal.GetUserOne(new UserInfo() {UserName = userName});
            if (dt.Rows.Count == 0)
                return false;
            else
            {
                if (dt.Rows[0]["PassWord"].ToString() == Pwd)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region 获取所有用户列表
        /// <summary>
        /// 获取所有用户列表
        /// </summary>        
        /// <returns></returns>
        public List<UserInfo> GetUserAll()
        {
            DataSet ds = _userDal.GetUserAll();
            return DataSetToList(ds);
        }
        #endregion

        #region 根据用户名查询用户信息
        /// <summary>
        /// 根据用户名查询用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public UserInfo GetUserOne(UserInfo user)
        {
            DataTable dt = _userDal.GetUserOne(user);
            var  ds  = new DataSet();
            ds.Tables.Add(dt);

            List<UserInfo> users = DataSetToList(ds);
            if (users == null) return null;

            return users[0];
        }
        #endregion

        public UserInfo GetUserOne(Guid  userId)
        {
            DataTable dt = _userDal.GetUserOne(userId);
            var ds = new DataSet();
            ds.Tables.Add(dt);

            List<UserInfo> users = DataSetToList(ds);
            if (users == null) return null;

            return users[0];
        }

        #region 新增用户记录
        /// <summary>
        /// 新增用户记录
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public bool AddUser(UserInfo user)
        {
            int affectRows = _userDal.AddUser(user);
            return affectRows > 0;
        }
        #endregion

        #region 修改用户记录
        /// <summary>
        /// 修改用户记录
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public bool UpdateUser(UserInfo user)
        {
            int affectRows = _userDal.UpdateUser(user);
            return affectRows > 0;
        }
        #endregion

        #region 私有公共方法
        /// <summary>
        /// 将查询数据集转换成用户集合
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <returns></returns>
        private List<UserInfo> DataSetToList(DataSet ds)
        {
            List<UserInfo> users = new List<UserInfo>();

            Guid guiduserID = Guid.Empty;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Guid.TryParse(item["UserId"].ToString(), out guiduserID);
                users.Add(new UserInfo()
                {
                    UserId = guiduserID,
                    UserName = item["UserName"].ToString(),
                    NikeName =  item["UserName"].ToString(),
                    PassWord = item["PassWord"].ToString(),
                    Sex = Int32.Parse(item["Sex"].ToString()),
                    BirthDay =  Convert.ToDateTime(item["BirthDay"]),  
                    Phone = item["Phone"].ToString(),
                    Email = item["Email"].ToString(),
                    Status = Int32.Parse(item["Status"].ToString())
                });
            }
            if (users.Count > 0) 
                return users;
            return null;
        }
        #endregion
    }
}
