using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WeChatDLL.Services
{
    /// <summary>
    /// 头像获取
    /// </summary>
   public class GetIcons
    {
        /// <summary>
        /// 获取好友头像
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Image GetIcon(string username)
        {
            byte[] bytes = RequstService.SendGetRequest(InterFaceURL._geticon_url + username);

            return Image.FromStream(new MemoryStream(bytes));
        }
        /// <summary>
        /// 获取微信讨论组头像
        /// </summary>
        /// <param name="usename"></param>
        /// <returns></returns>
        public Image GetHeadImg(string usename)
        {
            byte[] bytes = RequstService.SendGetRequest(InterFaceURL._getheadimg_url + usename);

            return Image.FromStream(new MemoryStream(bytes));
        }
    }
}
