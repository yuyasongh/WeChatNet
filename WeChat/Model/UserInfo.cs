using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        public string Uin { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        public string RemarkName { get; set; }
        public string PYInitial { get; set; }
        public string PYQuanPin { get; set; }
        public string RemarkPYInitial { get; set; }
        public string RemarkPYQuanPin { get; set; }
        public string HideInputBarFlag { get; set; }
        public string StarFriend { get; set; }
        public string Sex { get; set; }
        public string Signature { get; set; }
        public string AppAccountFlag { get; set; }
        public string VerifyFlag { get; set; }
        public string ContactFlag { get; set; }
        public string WebWxPluginSwitch { get; set; }
        public string HeadImgFlag { get; set; }
        public string SnsFlag { get; set; }
    }
}
