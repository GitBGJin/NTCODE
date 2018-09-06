using SmartEP.Core.Enums;
using SmartEP.Core.Interfaces;
using SmartEP.Utilities.DataTypes.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEP.WebControl.CbxRsm
{
    [Serializable]
    public class RsmPoint : IPoint
    {
        #region Name
        private String _pointName;
        public String PointName
        { get { return _pointName; } }
        #endregion

        #region ID
        private String _pointID;
        public String PointID
        { get { return _pointID; } }
        #endregion

        #region Guid
        private String _pointGuid;
        public String PointGuid
        { get { return _pointGuid; } }
        #endregion

        public RsmPoint(String pointName, String pointID, String pointGuid)
        {
            this._pointName = pointName;
            this._pointID = pointID;
            this._pointGuid = pointGuid;
        }

        public override String ToString()
        {
            return String.Format("{1}:{2}:{3}", _pointName, _pointID, _pointGuid);
        }

        public String ToString(String strSpacer = ":")
        {
            return String.Format("{1}{0}{2}{0}{3}", strSpacer, _pointName, _pointID, _pointGuid);
        }
    }
}
