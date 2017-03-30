using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WeChatDLL.Model;
using WeChatDLL.Services;

namespace WeChatDLL
{
    /// <summary>
    /// 微信操作类
    /// </summary>
    public class WeChats
    {
        /// <summary>
        /// 登录服务
        /// </summary>
        private static LoginService _loginService = new LoginService();
        /// <summary>
        /// 序列和反序列化
        /// </summary>
        private static JavaScriptSerializer _jsonSerialize = new JavaScriptSerializer();
        /// <summary>
        /// 二维码扫描 获取二维码图片Byte
        /// </summary>
        public Byte[] QRCode()
        {
            try
            {
                Image imageQRCode = _loginService.GetQRCode();
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Bitmap bmp = new Bitmap(imageQRCode))
                    {
                        bmp.Save(ms, imageQRCode.RawFormat);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 登录扫描检测 返回非空字符串为登录成功,该返回字符串用于请求 GetSidUinAndCookie 参数
        /// </summary>
        public string LoginScanDetection()
        {
            try
            {
                WXResult wxResult = new WXResult();
                object _object = _loginService.LoginScanDetection();
                if (_object is string)  //已扫描二维码并完成登录
                {
                    _loginService.tip = 0;
                    //登录获取sid、uin
                    _loginService.GetSidUin(_object as string);
                    return _object as string;
                }
                return "";
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取sid、uin及相关Cookie
        /// </summary>
        /// <param name="loginRedirect">扫描登录成功返回的string</param>
        /// <returns></returns>
        public string GetSidUinAndCookie(string loginRedirect)
        {
            try
            {
                byte[] bytes = RequstService.SendGetRequest(loginRedirect + "&fun=new");
                string pass_ticket = Encoding.UTF8.GetString(bytes);
                string pass_Ticket = pass_ticket.Split(new string[] { "pass_ticket" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
                string sKey = pass_ticket.Split(new string[] { "skey" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
                /// 登录后sid
                Cookie sid = RequstService.GetCookie("wxsid");
                /// 登录后uin
                Cookie uin = RequstService.GetCookie("wxuin");
                Cookie webwx_data_ticket = RequstService.GetCookie("webwx_data_ticket");
                WXResult wxResult = new WXResult();

                wxResult.Status = "OK";
                SidUinAndCookie sidUinAndCookie = new SidUinAndCookie();
                sidUinAndCookie.pass_Ticket = pass_Ticket;
                sidUinAndCookie.sId = sid.Value;
                sidUinAndCookie.sKey = sKey;
                sidUinAndCookie.uIn = uin.Value;
                sidUinAndCookie.webwx_Data_Ticket = webwx_data_ticket.Value;
                wxResult.Result = sidUinAndCookie;
                return _jsonSerialize.Serialize(wxResult);
            }
            catch (Exception ex)
            {
                WXResult wxResult = new WXResult();
                wxResult.Exception = ex.Message;
                return _jsonSerialize.Serialize(wxResult);
            }
        }
        /// <summary>
        /// 初始化并返回最近联系人
        /// </summary>
        /// <returns></returns>
        public List<object> Load()
        {
            try
            {
                return MessageSync.Message();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 好友列表
        /// </summary>
        public List<object> FriendLists()
        {
            try
            {
                //加载好友列表数据
                return MessageSync.FriendsList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 微信同步检测(检查是否有新消息)，返回非空为有新消息
        /// </summary>
        /// <returns></returns>
        public string WxSyncCheck()
        {
            try
            {
                return MessageSync.WxSyncCheck();  //同步检查
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 微信同步(获取最新消息)
        /// </summary>
        /// <returns></returns>
        public List<object> WxSync()
        {
            try
            {
                WXResult wxResult = new WXResult();
                JObject sync_result = MessageSync.WxSync();  //同步检查
                List<object> msgList = new List<object>();
                if (sync_result != null)
                {
                    if (sync_result["AddMsgCount"] != null && sync_result["AddMsgCount"].ToString() != "0")
                    {
                        foreach (JObject m in sync_result["AddMsgList"])
                        {
                            WXMsg msg = new WXMsg();
                            msg.From = m["FromUserName"].ToString();
                            msg.To = m["ToUserName"].ToString();
                            msg.Msg = m["Content"].ToString();
                            msg.Type = int.Parse(m["MsgType"].ToString());
                            msgList.Add(msg);
                        }
                        return msgList;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取需要发送用户UserName
        /// </summary>
        /// <param name="nickName">用户昵称 从好友列表中获取</param>
        /// <returns></returns>
        public string GetUserName(string nickName)
        {
            try
            {
                WXResult wxResult = new WXResult();
                // JObject friends = MessageSync.Friends();
                JObject friends = MessageSync.friends_result;
                if (friends != null)
                {
                    if (friends["MemberList"] != null)
                    {
                        foreach (var item in friends["MemberList"])
                        {
                            if (item["NickName"].ToString() == nickName)
                            {
                                return item["UserName"].ToString();
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="uin"></param>
        /// <param name="pass_Ticket"></param>
        /// <returns></returns>
        public UserInfo UserInfo()
        {
            try
            {
                return MessageSync.userInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 指定某人发送指定消息
        /// </summary>
        /// <param name="FromUserName">自己ID</param>
        /// <param name="ToUserName">好友ID</param>
        /// <param name="Msg">消息内容</param>
        public bool SpecifiedReply(string ToUserName, string Msg)
        {
            try
            {
                WXMsg assignMsg = new WXMsg();
                WXUser assignUser = new WXUser();
                assignMsg.From = MessageSync.userInfo.UserName;
                assignMsg.Msg = Msg;
                assignMsg.Readed = false;
                assignMsg.To = ToUserName;
                assignMsg.Type = 1;
                assignMsg.Time = DateTime.Now;
                assignUser.SendMsg(assignMsg, false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
