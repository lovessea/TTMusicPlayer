using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace 网络延迟测试
{
    public class InternetConnected
    {
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        internal bool GetConnectedState()
        {
            int Desc;
            bool result = InternetGetConnectedState(out  Desc, 0);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 获取本机广域网IP地址
        /// </summary>
        /// <returns>返回广域网IP地址</returns>
        public string GetWANAddress()
        {
            try
            {
                string strUrl = "http://www.ip138.com/ip2city.asp";     //获得IP的网址
                Uri uri = new Uri(strUrl);
                WebRequest webreq = WebRequest.Create(uri);
                Stream stss = webreq.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(stss, Encoding.Default);
                string all = sr.ReadToEnd();         //读取网站返回的数据  格式：您的IP地址是：[x.x.x.x]
                int i = all.IndexOf("[", StringComparison.Ordinal) + 1;
                string tempip = all.Substring(i, 15);
                string ip = tempip.Replace("]", "").Replace(" ", "").Replace("<", ""); //去除杂项找出ip
                return ip;
            }
            catch (Exception)
            {
                        
                throw;
            }
            
        }


        /// <summary>
        /// 获取ISP网络服务供应商
        /// </summary>
        /// <returns>返回网络服务供应商名</returns>
        internal string GetISPName()
        {
            try
            {
                string strUrl = "http://1111.ip138.com/ic.asp";     //获得IP的网址
                Uri uri = new Uri(strUrl);
                WebRequest webreq = WebRequest.Create(uri);
                Stream stss = webreq.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(stss, Encoding.Default);
                string all = sr.ReadToEnd();         //读取网站返回的数据  格式：您的IP地址是：
                int i = all.IndexOf("来自：", System.StringComparison.Ordinal) + 3;
                string tempip = all.Substring(i, 15);
                string strIspName = tempip.Replace("</cent", "").Replace(" ", "").Replace("<", ""); //去除杂项找出ip
                return strIspName;
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        /// <returns>返回局域网IP地址</returns>
        internal IPAddress[] GetLANAddress()
        {
            IPAddress[] localhostList = Dns.GetHostAddresses(Dns.GetHostName());

            return localhostList;
        }
    }
}
