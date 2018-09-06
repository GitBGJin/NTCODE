using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.Data.SqlServer.MonitoringBusiness.Water;
using System.Data;

namespace SmartEP.MonitoringBusinessRepository.Water
{
    public class MaintainFileManageRepository
    {
        MaintainFileManageDAL w_MaintainFileManageDAL = new MaintainFileManageDAL();

        /// <summary>
        /// 获取水质自动监测站运行维护文件数据
        /// </summary>
        /// <returns></returns>
        public DataView GetMaintainFile(string startTime, string endTime)
        {
            return w_MaintainFileManageDAL.GetMaintainFile(startTime, endTime);
        }

        public DataView GetMaintainFile(int id)
        {
            return w_MaintainFileManageDAL.GetMaintainFile(id);
        }

        public void ModifyRoute(string DocRoute, string HtmlRoute, string IsUpload, int id)
        {
            w_MaintainFileManageDAL.ModifyRoute(DocRoute, HtmlRoute, IsUpload, id);
        }

        public void AddFile(string fileName, string createTime, string docRoute, string htmlRoute, string isUpload, string person)
        {
            w_MaintainFileManageDAL.AddFile(fileName, createTime, docRoute, htmlRoute, isUpload, person);
        }

        public void DeleteFile(int id)
        {
            w_MaintainFileManageDAL.DeleteFile(id);
        }
    }
}
