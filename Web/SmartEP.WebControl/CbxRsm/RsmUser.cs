using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEP.WebControl.CbxRsm
{
    public class RsmUser
    {
        #region Name
        private String _userName;
        public String UserName
        { get { return _userName; } }
        #endregion

        #region ID
        private String _userID;
        public String UserID
        { get { return _userID; } }
        #endregion

        #region Guid
        private String _userGuid;
        public String UserGuid
        { get { return _userGuid; } }
        #endregion

        #region Gender
        private String _userGender;
        public String UserGender
        { get { return _userGender; } }
        #endregion

        #region Mobile
        private String _userMobile;
        public String UserMobile
        { get { return _userMobile; } }
        #endregion

        #region Email
        private String _userEmail;
        public String UserEmail
        { get { return _userEmail; } }
        #endregion

        public RsmUser(String userName, String userID, String userGuid, String userGender, String userMobile, String userEmail)
        {
            this._userName = userName;
            this._userID = userID;
            this._userGuid = userGuid;
            this._userGender = userGender;
            this._userMobile = userMobile;
            this._userEmail = userEmail;
        }

        public override String ToString()
        {
            return String.Format("{1}:{2}:{3}:{4}:{5}:{6}", _userName, _userID, _userGuid, _userGender, _userMobile, _userEmail);
        }

        public String ToString(String strSpacer = ":")
        {
            return String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", strSpacer
                , _userName, _userID, _userGuid, _userGender, _userMobile, _userEmail);
        }
    }
}
