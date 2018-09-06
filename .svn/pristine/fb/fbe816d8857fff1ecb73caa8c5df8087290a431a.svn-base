using SmartEP.Utilities.Caching;
using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Service.Frame
{
    public class UserService
    {
        private static readonly UserService m_instance = new UserService();

        public static UserService Instance
        {
            get
            {
                return m_instance;
            }
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public IList<User> GetAllUser()
        {
            IList<User> userList;

            string FrameUserServiceUrl = System.Configuration.ConfigurationManager.AppSettings["FrameUserServiceUrl"].ToString();
            userList = new List<User>();
            object ret = WebServiceHelper.InvokeWebService(FrameUserServiceUrl, "SelectAllUser", new object[] { "", "", "sinoyd12345678" });
            DataTable userDT = ret != null ? (DataTable)ret : new DataTable();
            for (int i = 0; i < userDT.Rows.Count; i++)
            {
                DataRow dr = userDT.Rows[i];
                User user = new User();
                user.Id = new Guid(dr["RowGuid"].ToString());
                user.LoginName = dr["LoginID"].ToString();
                user.CName = dr["DisplayName"].ToString();
                user.DeptId = new Guid(dr["DeptGuid"].ToString());
                user.Theme = dr["Theme"].ToString();
                userList.Add(user);
            }

            return userList;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public User GetUserByUserId(Guid userId)
        {
            #region 太复杂 用Lambda表达式，简洁明了
            //IList<User> userList = GetAllUser();
            //var query = from IUser in userList where IUser.Id == userId select IUser;
            //try
            //{
            //    return query.Single<User>();
            //}
            //catch
            //{
            //    return null;
            //}
            #endregion
            IList<User> userList = GetAllUser();
            return userList.Single<User>(p => p.Id == userId).ThrowIfNullOrDBNull("没有该用户数据！");
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public User GetCurrentUser()
        {
            return GetUserByUserId(GetCurrentUserId());
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns></returns>
        public Guid GetCurrentUserId()
        {
            if (System.Web.HttpContext.Current.Session["UserId"] != null)
                return new Guid(System.Web.HttpContext.Current.Session["UserId"].ToString());
            else
                return Guid.Empty;
        }

        /// <summary>
        /// 获取超级管理员用户
        /// </summary>
        /// <returns></returns>
        public User GetAdministratorUser()
        {
            return GetUserByUserId(new Guid("4ce5bed9-78bd-489f-8b3f-a830098759c4"));
        }

        /// <summary>
        /// 取得认证用户
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetRegisterUser(string userGuid)
        {
            string FrameUserServiceUrl = System.Configuration.ConfigurationManager.AppSettings["FrameUserServiceUrl"].ToString();
            object ret = WebServiceHelper.InvokeWebService(FrameUserServiceUrl, "SelectByUserGuid", new object[] { userGuid });
            if (ret == null)
                return null;
            try
            {
                Dictionary<string, object> user = SmartEP.Utilities.DataTypes.ExtensionMethods.ValueTypeExtensions.ObjectToDic(ret);
                return user;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public static bool RegisterUser(string userGuid)
        {
            Dictionary<string, object> user = GetRegisterUser(userGuid);
            //当前用户存在：用户姓名不为空
            if (user == null
                || !user.ContainsKey("DisplayName")
                || !user.ContainsKey("DeptGuid")
                || !user.ContainsKey("DeptName")
                || !user.ContainsKey("RowGuid")
                || !user.ContainsKey("LoginID")
                || user["DisplayName"] == null
                || user["DeptGuid"] == null
                || user["DeptName"] == null
                || user["RowGuid"] == null
                || user["LoginID"] == null
                || string.IsNullOrEmpty(user["DisplayName"].ToString())
                || string.IsNullOrEmpty(user["DeptGuid"].ToString())
                || string.IsNullOrEmpty(user["DeptName"].ToString())
                || string.IsNullOrEmpty(user["RowGuid"].ToString())
                || string.IsNullOrEmpty(user["LoginID"].ToString()))
            {
                return false;
            }
            //部门标识
            SessionHelper.Add("DeptGuid", user["DeptGuid"].ToString());
            //部门名称
            SessionHelper.Add("DeptName", user["DeptName"].ToString());
            //用户标识
            SessionHelper.Add("UserGuid", user["RowGuid"].ToString());
            //姓名
            SessionHelper.Add("DisplayName", user["DisplayName"].ToString());
            //用户名
            SessionHelper.Add("LoginID", user["LoginID"].ToString());
            return true;
        }
    }

    public class User
    {
        public Guid Id
        {
            get;
            set;
        }
        public string LoginName
        {
            get;
            set;
        }
        public string CName
        {
            get;
            set;
        }
        public string EName
        {
            get;
            set;
        }
        public bool Sex
        {
            get;
            set;
        }
        public DateTime DateOfBirth
        {
            get;
            set;
        }
        public string PhotoUrl
        {
            get;
            set;
        }
        public Guid DeptId
        {
            get;
            set;
        }
        public Guid ApplicationId
        {
            get;
            set;
        }
        public Guid NavigationId
        {
            get;
            set;
        }
        public string Theme
        {
            get;
            set;
        }
        public string Extension
        {
            get;
            set;
        }
    }
}
