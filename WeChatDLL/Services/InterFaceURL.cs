using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatDLL.Services
{
    /// <summary>
    /// 接口地址
    /// </summary>
    public class InterFaceURL
    {
        /// <summary>
        /// 微信初始化url
        /// </summary>
        public static string _init_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxinit?r=1377482058764";
        /// <summary>
        /// 好友列表url
        /// </summary>
        public static string _friends_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxgetcontact?r=1377482058764";

        /// <summary>
        /// 消息检查（参考方法 syncCheck）
        /// </summary>
        public static string _synccheck_url = "https://webpush2.weixin.qq.com/cgi-bin/mmwebwx-bin/synccheck?r=1377482058764";
        /// <summary>
        /// 获取最新消息（参考方法 webwxsync）
        /// </summary>
        public static string _sync_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsync?sid=";

        /// <summary>
        /// 聊天图片获取
        /// </summary>
        public static string _image_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxgetmsgimg?sid=";


        /// <summary>
        /// 获取好友头像
        /// </summary>
        public static string _geticon_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxgeticon?username=";
        /// <summary>
        /// 获取群聊（组）头像
        /// </summary>
        public static string _getheadimg_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxgetheadimg?username=";

        /// <summary>
        /// 发送消息url
        /// </summary>
        public static string _sendmsg_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsg?sid=";
        /// <summary>
        /// 上传图片
        /// </summary>
        public static string _upload_image_url = "https://file2.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json&sid=";
        /// <summary>
        /// 发送聊天图片
        /// </summary>
        public static string _send_image_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsgimg?fun=async&f=json&sid=";
    }
}
