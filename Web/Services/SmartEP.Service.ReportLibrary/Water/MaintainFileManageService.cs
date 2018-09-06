using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartEP.MonitoringBusinessRepository.Water;
using System.Data;

namespace SmartEP.Service.ReportLibrary.Water
{
    public class MaintainFileManageService
    {
        MaintainFileManageRepository w_MaintainFileManageRepository = new MaintainFileManageRepository();

        public DataView GetMaintainFile(string startTime, string endTime)
        {
            return w_MaintainFileManageRepository.GetMaintainFile(startTime, endTime);
        }

        public DataView GetMaintainFile(int id)
        {
            return w_MaintainFileManageRepository.GetMaintainFile(id);
        }

        public void ModifyRoute(string DocRoute, string HtmlRoute, string IsUpload, int id)
        {
            w_MaintainFileManageRepository.ModifyRoute(DocRoute, HtmlRoute, IsUpload, id);
        }

        public void AddFile(string fileName, string createTime, string docRoute, string htmlRoute, string isUpload, string person)
        {
            w_MaintainFileManageRepository.AddFile(fileName, createTime, docRoute, htmlRoute, isUpload, person);
        }

        public void DeleteFile(int id)
        {
            w_MaintainFileManageRepository.DeleteFile(id);
        }
    }
}
