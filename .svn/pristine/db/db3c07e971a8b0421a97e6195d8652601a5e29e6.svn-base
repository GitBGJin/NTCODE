using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace SmartEP.Utilities.Web.NetWork
{
    public class Networks
    {
        /// <summary>  
        /// 使用ARP获取MAC地址  
        /// </summary>  
        /// <param name="DestIP">目标IP</param>  
        /// <param name="SrcIP">0</param>  
        /// <param name="pMacAddr">两个uint 都是255</param>  
        /// <param name="PhyAddrLen">长度6</param>  
        /// <returns>返回错误信息</returns>  
        [DllImport("Iphlpapi.dll")]
        public static extern uint SendARP(uint DestIP, uint SrcIP, ref ulong pMacAddr, ref uint PhyAddrLen);
        /// <summary>  
        /// 调用API获取MAC地址  
        /// </summary>  
        /// <param name="p_Id">IP地址</param>  
        /// <returns>MAC地址</returns>  
        public static string GetMac(string p_Id)
        {
            IPAddress _Address;
            if (!IPAddress.TryParse(p_Id, out _Address)) return "";
            uint DestIP = System.BitConverter.ToUInt32(_Address.GetAddressBytes(), 0);
            ulong pMacAddr = 0;
            uint PhyAddrLen = 6;
            uint error_code = SendARP(DestIP, 0, ref pMacAddr, ref PhyAddrLen);
            byte[] _Bytes1 = BitConverter.GetBytes(pMacAddr);
            return BitConverter.ToString(_Bytes1, 0, 6);
        }
        /// <summary>  
        /// 判断指定的IP地址是否是内部网络  
        /// </summary>  
        /// <param name="p_IPAddress">IP地址</param>  
        /// <returns></returns>  
        public static bool IsInnerNetWork(string p_IPAddress)
        {
            string m_MAC = GetMac(p_IPAddress);
            return !(m_MAC == "00-00-00-00-00-00");
        }
        /// <summary>  
        /// 判断指定的IP地址是否是内部网络  
        /// </summary>  
        /// <param name="p_IPAddress">IP地址</param>  
        /// <returns></returns>  
        public static bool IsInnerNetWork()
        {
            String m_IpAdd = GetRealIP();
            return IsInnerNetWork(m_IpAdd);
        }
        public static string GetRealIP()
        {
            string ip;
            try
            {
                HttpRequest request = HttpContext.Current.Request;

                if (request.ServerVariables["HTTP_VIA"] != null)
                {
                    ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
                }
                else
                {
                    ip = request.UserHostAddress;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ip;
        } 
    }
}
