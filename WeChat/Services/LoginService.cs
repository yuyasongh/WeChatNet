using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChat.Services;

namespace WeChat.Services
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginService
    {
        #region 登录
        /// <summary>
        /// 获取到的UUID（参考方法 getUUID）
        /// </summary>
        private static string _uuid = null;
        /// <summary>
        /// 等待登录（参考方法 waitForLogin）这里是微信确认登录   tip : 1:未扫描 0:已扫描 
        /// </summary>
        public static int tip = 1;
        /// <summary>
        /// 登录
        /// </summary>
        public delegate Image MethodDelegate();
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <returns></returns>
        public Image GetQRCode()
        {
            try
            {
                string datetime = GetTimeStamp();
                //获取UUID的URL
                string _appid_url = "https://login.weixin.qq.com/jslogin?appid=wx782c26e4c19acffb&fun=new&lang=zh_CN&_=" + datetime + "";
                //获取二维码的URL
                string _qrcode_url = "https://login.weixin.qq.com/qrcode/"; //后面增加UUID
                byte[] bytes = RequstService.SendGetRequest(_appid_url);
                _uuid = Encoding.UTF8.GetString(bytes).Split(new string[] { "\"" }, StringSplitOptions.None)[1];
                bytes = RequstService.SendPostRequest(_qrcode_url + _uuid, "t=webwx&_=" + datetime + "");
                Image qrCode = Image.FromStream(new MemoryStream(bytes));
                return qrCode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 登录扫描检测
        /// </summary>
        /// <returns></returns>
        public object LoginScanDetection()
        {
            try
            {
                if (_uuid == null) return null;
                string datetime = GetTimeStamp();
                //等待登录 判断二维码扫描情况  408 登陆超时 201 扫描成功 200 确认登录
                string _loginScanDetection_url = "https://login.weixin.qq.com/cgi-bin/mmwebwx-bin/login?loginicon=true&_=" + datetime + "&tip=" + tip + "&uuid="; //后面增加UUID
                byte[] bytes = RequstService.SendGetRequest(_loginScanDetection_url + _uuid);
                if (bytes == null) return null;
                string loginResult = Encoding.UTF8.GetString(bytes);
                if (loginResult.Contains("=201")) // 已扫描二维码但未登录
                {
                    string base64_image = loginResult.Split(new string[] { "\'" }, StringSplitOptions.None)[1].Split(',')[1];
                    byte[] base64_image_bytes = Convert.FromBase64String(base64_image);
                    MemoryStream memoryStream = new MemoryStream(base64_image_bytes, 0, base64_image_bytes.Length);
                    memoryStream.Write(base64_image_bytes, 0, base64_image_bytes.Length);
                    return Image.FromStream(memoryStream);
                }
                else if (loginResult.Contains("=200"))  //已扫描二维码并完成登录
                {
                    string login_redirect_url = loginResult.Split(new string[] { "\"" }, StringSplitOptions.None)[1];
                    return login_redirect_url;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static string pass_Ticket;
        public static string sKey;
        /// <summary>
        /// 获取sid、uin
        /// </summary>
        public void GetSidUin(string loginRedirect)
        {
            byte[] bytes = RequstService.SendGetRequest(loginRedirect + "&fun=new");
            string pass_ticket = Encoding.UTF8.GetString(bytes);
            pass_Ticket = pass_ticket.Split(new string[] { "pass_ticket" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
            sKey = pass_ticket.Split(new string[] { "skey" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
        }
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion
    }
}
