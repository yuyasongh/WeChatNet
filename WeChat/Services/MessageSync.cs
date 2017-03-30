using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WeChat.Model;

namespace WeChat.Services
{
    /// <summary>
    /// 信息同步类
    /// 好友列表、最近联系人、消息同步
    /// </summary>
    public class MessageSync
    {
        #region 好友列表
        /// <summary>
        /// 登录后sid
        /// </summary>
        public static Cookie sid = RequstService.GetCookie("wxsid");
        /// <summary>
        /// 登录后uin
        /// </summary>
        public static Cookie uin = RequstService.GetCookie("wxuin");
        /// <summary>
        /// 好友列表数据
        /// </summary>
        public static JObject friends_result = new JObject();
        /// <summary>
        /// 获取好友列表数据
        /// </summary>
        public static JObject Friends()
        {
            try
            {
                if (sid != null && uin != null)
                {
                    string init_json = string.Format("{{\"BaseRequest\":{{\"Uin\":\"{0}\",\"Sid\":\"{1}\",\"Skey\":\"{2}\",\"DeviceID\":\"e1615250492\"}}}}", uin.Value, sid.Value, LoginService.sKey);

                    byte[] bytesFriends = RequstService.SendPostRequest(InterFaceURL._friends_url + "&pass_ticket=" + LoginService.pass_Ticket, init_json);
                    if (bytesFriends==null)
                    {
                        return null;
                    }
                    string friends_str = Encoding.UTF8.GetString(bytesFriends);
                    friends_result = JsonConvert.DeserializeObject(friends_str) as JObject;
                    return friends_result;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 好友列表绑定
        /// </summary>
        public static List<object> FriendsList()
        {
            try
            {
                List<object> contact_all = new List<object>();
                List<Object> _contact_all = new List<object>();
                JObject contact_result = new JObject();
                if (friends_result == null || friends_result.ToString() == "{}")
                    contact_result = MessageSync.Friends(); //通讯录
                else contact_result = friends_result;
                if (contact_result != null)
                {
                    contact_all = new List<object>();
                    foreach (JObject contact in contact_result["MemberList"])  //完整好友名单
                    {
                        if (contact["Sex"].ToString() != "0")
                        {
                            WXUser user = new WXUser();
                            user.UserName = contact["UserName"].ToString();
                            user.City = contact["City"].ToString();
                            user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                            user.NickName = contact["NickName"].ToString();
                            user.Province = contact["Province"].ToString();
                            user.PYQuanPin = contact["PYQuanPin"].ToString();
                            user.RemarkName = contact["RemarkName"].ToString();
                            user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                            user.Sex = contact["Sex"].ToString();
                            user.Signature = contact["Signature"].ToString();

                            contact_all.Add(user);
                        }
                    }
                }
                IOrderedEnumerable<object> list_all = contact_all.OrderBy(ee => (ee as WXUser).ShowPinYin);

                WXUser wx; string start_char;
                foreach (object o in list_all)
                {
                    wx = o as WXUser;
                    start_char = wx.ShowPinYin == "" ? "" : wx.ShowPinYin.Substring(0, 1);
                    if (!_contact_all.Contains(start_char.ToUpper()))
                    {
                        _contact_all.Add(start_char.ToUpper());
                    }
                    _contact_all.Add(o);
                }
                return _contact_all;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        #region 最近联系人
        /// <summary>
        /// 保存联系人消息列表
        /// </summary>
        public static List<object> _contact_latest = new List<object>();
        private static JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
        /// <summary>
        /// 存放SyncKey的key、val
        /// </summary>
        public static Dictionary<string, string> _syncKey = new Dictionary<string, string>();
        /// <summary>
        /// 用户信息
        /// </summary>
        public static UserInfo userInfo = new UserInfo();
        /// <summary>
        /// 初始化
        /// </summary>
        public static JObject Load()
        {
            try
            {
                // await Task.Factory.StartNew(() =>
                // {
                if (sid != null && uin != null)
                {
                    string init_json = string.Format("{{\"BaseRequest\":{{\"Uin\":\"{0}\",\"Sid\":\"{1}\",\"Skey\":\"{2}\",\"DeviceID\":\"e1615250492\"}}}}", uin.Value, sid.Value, LoginService.sKey);
                    byte[] bytes = RequstService.SendPostRequest(InterFaceURL._init_url + "&pass_ticket=" + LoginService.pass_Ticket, init_json);
                    string init_str = Encoding.UTF8.GetString(bytes);
                    JObject init_result = JsonConvert.DeserializeObject(init_str) as JObject;
                    _syncKey = new Dictionary<string, string>();
                    foreach (JObject synckey in init_result["SyncKey"]["List"])  //同步键值
                    {
                        if (!_syncKey.ContainsKey(synckey["Key"].ToString()))
                        {
                            _syncKey.Add(synckey["Key"].ToString(), synckey["Val"].ToString());
                        }
                    }
                    userInfo = jsonSerialize.Deserialize<UserInfo>(init_result["User"].ToString());

                    return init_result;
                }
                return null;
                //  });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 初始化最近联系人
        /// </summary>
        public static List<object> Message()
        {
            try
            {
                JObject init_result = Load();  //初始化

                List<object> contact_all = new List<object>();
                if (init_result != null)
                {
                    _contact_latest = new List<object>();
                    PublicWindows.messageBoxs.MeUser = userInfo;
                    foreach (JObject contact in init_result["ContactList"])  //部分好友名单
                    {
                        if (contact["VerifyFlag"].ToString() == "0")
                        {
                            WXUser user = new WXUser();
                            user.UserName = contact["UserName"].ToString();
                            user.City = contact["City"].ToString();
                            user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                            user.NickName = contact["NickName"].ToString();
                            user.Province = contact["Province"].ToString();
                            user.PYQuanPin = contact["PYQuanPin"].ToString();
                            user.RemarkName = contact["RemarkName"].ToString();
                            user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                            user.Sex = contact["Sex"].ToString();
                            user.Signature = contact["Signature"].ToString();

                            _contact_latest.Add(user);
                        }
                    }
                    return _contact_latest;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region 消息同步
        /// <summary>
        /// 微信同步检测(检查是否有新消息)
        /// </summary>
        /// <returns></returns>
        public static string WxSyncCheck()
        {
            string datetime = LoginService.GetTimeStamp();
            string sync_key = "";
            foreach (KeyValuePair<string, string> p in _syncKey)
            {
                sync_key += p.Key + "_" + p.Value + "%7C";
            }
            sync_key = sync_key.TrimEnd('%', '7', 'C');

            Cookie sid = RequstService.GetCookie("wxsid");
            Cookie uin = RequstService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                //同步检查url
                string _synccheckUrl = InterFaceURL._synccheck_url + string.Format("&sid={0}&uin={1}&synckey={2}&skey={3}&deviceid={4}", sid.Value, uin.Value, sync_key, LoginService.sKey, "e1615250492");

                byte[] bytes = RequstService.SendGetRequest(_synccheckUrl + "&pass_ticket=" + LoginService.pass_Ticket + "&_=" + datetime);
                if (bytes != null)
                {
                    return Encoding.UTF8.GetString(bytes);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 微信同步(获取最新消息)
        /// </summary>
        /// <returns></returns>
        public static JObject WxSync()
        {
            string sync_json = "{{\"BaseRequest\" : {{\"DeviceID\":\"e1615250492\",\"Sid\":\"{1}\", \"Skey\":\"{5}\", \"Uin\":\"{0}\"}},\"SyncKey\" : {{\"Count\":{2},\"List\":[{3}]}},\"rr\" :{4}}}";
            Cookie sid = RequstService.GetCookie("wxsid");
            Cookie uin = RequstService.GetCookie("wxuin");

            string sync_keys = "";
            foreach (KeyValuePair<string, string> p in _syncKey)
            {
                sync_keys += "{\"Key\":" + p.Key + ",\"Val\":" + p.Value + "},";
            }
            sync_keys = sync_keys.TrimEnd(',');
            sync_json = string.Format(sync_json, uin.Value, sid.Value, _syncKey.Count, sync_keys, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, LoginService.sKey);

            if (sid != null && uin != null)
            {
                byte[] bytes = RequstService.SendPostRequest(InterFaceURL._sync_url + sid.Value + "&lang=zh_CN&skey=" + LoginService.sKey + "&pass_ticket=" + LoginService.pass_Ticket, sync_json);
                string sync_str = Encoding.UTF8.GetString(bytes);

                JObject sync_resul = JsonConvert.DeserializeObject(sync_str) as JObject;

                if (sync_resul["SyncKey"]["Count"].ToString() != "0")
                {
                    _syncKey.Clear();
                    foreach (JObject key in sync_resul["SyncKey"]["List"])
                    {
                        if (!_syncKey.ContainsKey(key["Key"].ToString()))
                        {
                            _syncKey.Add(key["Key"].ToString(), key["Val"].ToString());
                        }
                    }
                }
                return sync_resul;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 接收聊天图片base64
        /// </summary>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        public static string Image(string MsgId)
        {
            Cookie sid = RequstService.GetCookie("wxsid");
            Cookie uin = RequstService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                byte[] bytes = RequstService.SendGetRequest(InterFaceURL._image_url + sid.Value + "&lang=zh_CN&skey=" + LoginService.sKey + "&pass_ticket=" + LoginService.pass_Ticket + "&username=" + userInfo.UserName + "&type=slave&MsgID=" + MsgId);
                return Convert.ToBase64String(bytes);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
