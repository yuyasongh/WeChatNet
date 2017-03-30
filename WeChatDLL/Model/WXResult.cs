using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatDLL.Model
{
    /// <summary>
    /// 返回结果状态
    /// </summary>
   public class WXResult
    {
        /// <summary>
        /// 请求状态 OK：成功 NO失败
        /// </summary>
        public string Status = "NO";
        /// <summary>
        /// 消息结果列表
        /// </summary>
        public object Result=new object();
        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception = "";
    }
    /// <summary>
    /// 返回结果列表
    /// </summary>
    public class SidUinAndCookie
    {
        public string pass_Ticket = "";
        public string sKey = "";
        public string sId = "";
        public string uIn = "";
        public string webwx_Data_Ticket = "";
    }
}
