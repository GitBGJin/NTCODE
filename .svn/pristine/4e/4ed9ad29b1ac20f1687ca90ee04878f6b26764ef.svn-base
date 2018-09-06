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
    public class RsmFactor : IPollutant
    {
        #region Name
        private String _pollutantName;
        public String PollutantName
        { get { return _pollutantName; } }
        #endregion

        #region Code
        private String _pollutantCode;
        public String PollutantCode
        { get { return _pollutantCode; } }
        #endregion

        #region DecimalNum
        private String _pollutantDecimalNum;
        public String PollutantDecimalNum
        { get { return _pollutantDecimalNum; } }
        #endregion

        #region MeasureUnit
        private String _pollutantMeasureUnit;
        public String PollutantMeasureUnit
        { get { return _pollutantMeasureUnit; } }
        #endregion

        #region Guid
        private String _pollutantGuid;
        public String PollutantGuid
        { get { return _pollutantGuid; } }
        #endregion

        #region OrderNumber
        private Int32 _orderNumber;
        public Int32 OrderNumber
        { get { return _orderNumber; } }
        #endregion

        public RsmFactor(String pollutantName, String pollutantCode, String pollutantDecimalNum, String pollutantMeasureUnit, String pollutantGuid)
        {
            this._pollutantName = pollutantName;
            this._pollutantCode = pollutantCode;
            this._pollutantDecimalNum = pollutantDecimalNum;
            this._pollutantMeasureUnit = pollutantMeasureUnit;
            this._pollutantGuid = pollutantGuid;
            this._orderNumber = 99999;
        }

        public override String ToString()
        {
            return String.Format("{0}:{1}:{2}:{3}:{4}", _pollutantName, _pollutantCode, _pollutantDecimalNum, _pollutantMeasureUnit, _pollutantGuid);
        }

        public String ToString(String strSpacer = ":")
        {
            return String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", strSpacer
                , _pollutantName, _pollutantCode, _pollutantDecimalNum, _pollutantMeasureUnit, _pollutantGuid);
        }
    }
}
