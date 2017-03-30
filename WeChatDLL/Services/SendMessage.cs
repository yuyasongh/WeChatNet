using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WeChatDLL.Services
{
    /// <summary>
    /// 消息发送类
    /// </summary>
    public class SendMessage
    {
        public String UploadImg(string from, string to, FileStream fileinfo)
        {
            Cookie sId = RequstService.GetCookie("wxsid");
            Cookie uIn = RequstService.GetCookie("wxuin");
            Cookie webwx_data_ticket = RequstService.GetCookie("webwx_data_ticket");
            string sKey = LoginService.sKey;
            String response = "";
            Stream inputStream = null;
            Stream inputStreamReader = null;
            BufferedStream bufferedReader = null;

            HttpWebRequest conn = (HttpWebRequest)WebRequest.Create("https://file2.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json");
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;//默认是true，所以导致错误
                //请求头参数
                String boundary = "----WebKitFormBoundary6oVvR66QUmo1TkXD"; //区分每个参数之间
                String freFix = "--";
                String newLine = "\r\n";

                // 请求主体
                StringBuilder sb = new StringBuilder();
                string str = "";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"id\"";
                str += "";
                str += "WU_FILE_0";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"name\"";
                str += "";
                str += "1.png";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"type\"";
                str += "";
                str += "image / png";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"lastModifiedDate\"";
                str += "";
                str += "Wed Mar 15 2017 18:13:15 GMT + 0800(中国标准时间)";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"size\"";
                str += "";
                str += "3195";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"mediatype\"";
                str += "";
                str += "pic";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"uploadmediarequest\"";
                str += "";
                str += "{ \"UploadType\":2,\"BaseRequest\":{ \"Uin\":"+uIn+",\"Sid\":\""+sId+"\",\"Skey\":\""+sKey+"\",\"DeviceID\":\"e772864972883278\"},\"ClientMediaId\":"+ DateTime.Now.Millisecond + ",\"TotalLen\":3195,\"StartPos\":0,\"DataLen\":3195,\"MediaType\":4,\"FromUserName\":\""+from+"\",\"ToUserName\":\""+to+"\",\"FileMd5\":\"7911968c2371ab9b72c3f42d52776ce9\"}";
                str += "  ------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"webwx_data_ticket\"";
                str += "";
                str += ""+ webwx_data_ticket + "";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"pass_ticket\"";
                str += "";
                str += ""+LoginService.pass_Ticket+"";
                str += "------WebKitFormBoundary42gviyclJo9GfHd7";
                str += "Content - Disposition: form - data; name = \"filename\"; filename = \"1.png\"";
                str += "Content - Type: image / png";
                str += "";
                str += "";
                str += "  ------WebKitFormBoundary42gviyclJo9GfHd7--";


                byte[] data = Encoding.UTF8.GetBytes(str);
                conn.Method = "POST";
                conn.Accept = "*/*";
                conn.ContentType = "multipart/form-data;";
                conn.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36;";
                conn.Referer = "https://wx2.qq.com/?&lang=zh_CN";
                conn.Host = "file2.wx.qq.com";
                conn.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                conn.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                conn.Headers.Add("Cache-Control", "no-cache");
                //conn.Headers.Add("Connection", "Keep-Alive");
                conn.Headers.Add("Origin", "https://wx2.qq.com");
                conn.Headers.Add("Pragma", "no-cache");

                string url_1 = "https://file.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json&sid=";
                string url_2 = "https://file2.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json&sid=";
                //     HttpWebRequest conn11 = (HttpWebRequest)WebRequest.Create(url_1);
                //     // 输入文件流数据 
                //     byte[] buffers11 = new Byte[checked((uint)Math.Min(4096, (int)fileinfo.Length))];
                //
   //            string BaseRequest = "{" +
   //                  "\"DeviceID\" : \"e441551176\"," +
   //                  "\"Sid\" : \"" + sId.Value + "\"," +
   //                  "\"Skey\" : \"" + LoginService.sKey + "\"," +
   //                  "\"Uin\" : \"" + uIn.Value + "\"}";
   //            string fdic = "{" +
   //           "\"BaseRequest\" : " + BaseRequest + "," +
   //           "\"ClientMediaId\" : \"" + DateTime.Now.Millisecond + "\"," +
   //           "\"TotalLen\" : \"" + fileinfo.Length + "\"," +
   //           "\"StartPos\" : \"0\"," +
   //           "\"DataLen\" : \"" + fileinfo.Length + "\"," +
   //           "\"MediaType\" : \"4\"}";
   //        
   //            string files = "{" +
   //         "\"id\":  \"WU_FILE_0\"," +
   //         "\"name\": \"temp\"," +
   //         "\"type\": \"image/png\"," +
   //         "\"lastModifiedDate\":\"Wed Mar 15 2017 18:13:15 GMT+0800\"," +
   //         "\"size\": \"" + fileinfo.Length + "\"," +
   //         "\"mediatype\":\"pic\"," +
   //         "\"uploadmediarequest\": " + fdic + "," +
   //         "\"webwx_data_ticket\": \"" + webwx_data_ticket.Value + "\"," +
   //         "\"pass_ticket\": \"" + LoginService.pass_Ticket + "\"," +
   //         "\"filename\": \"\"}";


               // byte[] data = Encoding.UTF8.GetBytes(files);
                byte[] bArr = new byte[fileinfo.Length];
                conn.ContentLength = data.Length + bArr.Length;
                fileinfo.Read(bArr, 0, bArr.Length);
                fileinfo.Close();
                using (Stream datasteam = conn.GetRequestStream())
                {
                    datasteam.Write(data, 0, data.Length);
                    datasteam.Write(bArr, 0, bArr.Length);
                    datasteam.Close();
                }

                HttpWebResponse myResponse = conn.GetResponse() as HttpWebResponse;
                using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    response = reader.ReadToEnd();
                }
                
                HttpWebResponse response1 = (HttpWebResponse)conn.GetResponse();
                Stream response_stream = response1.GetResponseStream();

                int count = (int)response1.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf.ToString();


                if (RequstService.CookiesContainer == null)
                {
                    RequstService.CookiesContainer = new CookieContainer();
                }
                conn.CookieContainer = RequstService.CookiesContainer;  //启用cookie

                //读取响应信息
                inputStream = conn.GetRequestStream();
                inputStreamReader = inputStream;
                bufferedReader = new BufferedStream(inputStreamReader);
                String strs = "";
                StringBuilder buffer = new StringBuilder();
                while ((strs = bufferedReader.ToString()) != null)
                {
                    buffer.Append(strs);
                }

                response = buffer.ToString();
            }
            catch (Exception e)
            {
                //  e.printStackTrace();
            }
            finally
            {
                if (conn != null)
                {
                    //   conn.cloas();
                }
                try
                {
                    bufferedReader.Close();
                    inputStreamReader.Close();
                    inputStream.Close();
                }
                catch (IOException execption)
                {

                }
            }
            return response;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        public void SendMsg(string msg, string from, string to, int type)
        {
            string msg_json = "";
            msg_json = "{{" +
           "\"BaseRequest\":{{" +
               "\"DeviceID\" : \"e441551176\"," +
               "\"Sid\" : \"{0}\"," +
               "\"Skey\" : \"{6}\"," +
               "\"Uin\" : \"{1}\"" +
           "}}," +
           "\"Msg\" : {{" +
               "\"ClientMsgId\" : {8}," +
               "\"Content\" : \"{2}\"," +
               "\"FromUserName\" : \"{3}\"," +
               "\"LocalID\" : {9}," +
               "\"ToUserName\" : \"{4}\"," +
               "\"Type\" : {5}" +
           "}}," +
           "\"rr\" : {7}" +
           "}}";
            Cookie sid = RequstService.GetCookie("wxsid");
            Cookie uin = RequstService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                msg_json = string.Format(msg_json, sid.Value, uin.Value, msg, from, to, type, LoginService.sKey, DateTime.Now.Millisecond, DateTime.Now.Millisecond, DateTime.Now.Millisecond);
                byte[] bytes = null;
                switch (type)
                {
                    case 1:
                        bytes = RequstService.SendPostRequest(InterFaceURL._sendmsg_url + sid.Value + "&lang=zh_CN&pass_ticket=" + LoginService.pass_Ticket, msg_json);
                        break;
                    case 3:
                        bytes = RequstService.SendPostRequest(InterFaceURL._send_image_url + sid.Value + "&lang=zh_CN&pass_ticket=" + LoginService.pass_Ticket, msg_json);
                        break;
                }

                string send_result = Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
