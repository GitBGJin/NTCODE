using SmartEP.Utilities.Web.WebServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SmartEP.Utilities.DataTypes.ExtensionMethods;

namespace SmartEP.Service.Frame
{
    public class RoleService
    {
        private static readonly RoleService m_instance = new RoleService();

        public static RoleService Instance
        {
            get
            {
                return m_instance;
            }
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetAllRole()
        {
            IList<Role> roleList;
            roleList = new List<Role>();

            string FrameRoleServiceUrl = System.Configuration.ConfigurationManager.AppSettings["FrameRoleServiceUrl"].ToString();
            object ret = WebServiceHelper.InvokeWebService(FrameRoleServiceUrl, "GetAllRole", new object[] { "", "sinoyd12345678" });
            DataTable roleDT = ret != null ? (DataTable)ret : new DataTable();
            for (int i = 0; i < roleDT.Rows.Count; i++)
            {
                DataRow dr = roleDT.Rows[i];
                Role role = new Role();
                role.Id = new Guid(dr["RowGuid"].ToString());
                role.RoleName = dr["RoleName"].ToString();
                roleList.Add(role);
            }
            return roleList;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public Role GetRoleByRoleId(Guid roleId)
        {
            IList<Role> userRoleList = GetAllRole();
            //var query = from Role in userList where Role.Id == roleId select Role;
            //try
            //{
            //    return query.Single<Role>();
            //}
            //catch
            //{
            //    return null;
            //}

            return userRoleList.Single<Role>(p => p.Id == roleId).ThrowIfNullOrDBNull("没有该用户角色数据！");
        }

        /// <summary>
        /// 获取角色Ids
        /// </summary>
        /// <param name="userId">用户ID，多个角色ID用';'隔开</param>
        /// <returns></returns>
        public string[] GetRoleIdsByUserId(Guid userId)
        {
            string FrameRoleServiceUrl = System.Configuration.ConfigurationManager.AppSettings["FrameRoleServiceUrl"].ToString();
            object ret = WebServiceHelper.InvokeWebService(FrameRoleServiceUrl, "GetRoleGuidList", new object[] { userId.ToString(), "sinoyd12345678" });
            string roleIds = ret != null ? ret.ToString() : string.Empty;
            return roleIds.Trim(';').Split(';');
        }

        /// <summary>
        /// 获取超级管理员角色
        /// </summary>
        /// <returns></returns>
        public Role GetAdministratorRole()
        {
            return GetRoleByRoleId(new Guid("4f9ab883-86a0-4d86-9fca-7223b78edbb8"));
        }
    }

    public class Role
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public Guid ApplicationId { get; set; }

    }
}
