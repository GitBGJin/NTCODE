using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
///MySoapHeader 的摘要说明
/// </summary>
public class MySoapHeader : System.Web.Services.Protocols.SoapHeader
{
  private string _uname = string.Empty;//webservice访问用户名

  public string Uname
  {
    get { return _uname; }
    set { _uname = value; }
  }
  private string _password = string.Empty;//webservice访问密码

  public string Password
  {
    get { return _password; }
    set { _password = value; }
  }


  public MySoapHeader()
  {
    //
    //TODO: 在此处添加构造函数逻辑
    //
  }
  public MySoapHeader(string uname, string upass)
  {
    init(uname, upass);
  }
  private void init(string uname, string upass)
  {
    this._password = upass;
    this._uname = uname;
  }
  //验证用户是否有权访问内部接口
  private bool isValid(string uname, string upass, out string msg)
  {
    msg = "";
    string loginName= System.Configuration.ConfigurationManager.AppSettings["loginName"];
    string password = System.Configuration.ConfigurationManager.AppSettings["password"];
    if (uname == loginName && upass == password)
    {
      return true;
    }
    else
    {
      msg = "对不起！您无权调用此WebService！";
      return false;
    }
  }
  //验证用户是否有权访问外部接口
  public bool isValid(out string msg)
  {
    return isValid(_uname, _password, out msg);
  }
}

