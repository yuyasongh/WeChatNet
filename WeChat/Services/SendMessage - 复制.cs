using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Services
{
    public class SendMessage11
    {

        /// <summary>
        /// 发送消息url
        /// </summary>
        private static string _sendmsg_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsg?sid=";
        /// <summary>
        /// 上传图片
        /// </summary>
        private static string _upload_image_url = "https://file2.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json&sid=";
        /// <summary>
        /// 发送聊天图片
        /// </summary>
        private static string _send_image_url = "https://wx2.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsgimg?fun=async&f=json&sid=";

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

            HttpWebRequest conn = (HttpWebRequest)WebRequest.Create("https://file2.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json&sid=");
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;//默认是true，所以导致错误
                //请求头参数
                String boundary = "----WebKitFormBoundary6oVvR66QUmo1TkXD"; //区分每个参数之间
                String freFix = "--";
                String newLine = "\r\n";

                    // 请求主体
                    StringBuilder sb = new StringBuilder();
                
                    sb.Append(freFix + boundary).Append(newLine); //这里注意多了个freFix，来区分去请求头中的参数
                    sb.Append("Content-Disposition: form-data; name=\"id\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append(LoginService.GetTimeStamp()).Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"name\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append(fileinfo.Name.Substring(fileinfo.Name.LastIndexOf("\\") + 1)).Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"type\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append("image/png").Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"lastModifiedDate\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append("Wed Mar 15 2017 18:13:15 GMT+0800").Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"size\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append(fileinfo.Length).Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"mediatype\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append("pic").Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"uploadmediarequest\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append("{ \"UploadType\":2,\"BaseRequest\":{\"Uin\":" + uIn.Value + ",\"Sid\":\"" + sId.Value + "\",\"Skey\":\"" + sKey + "\",\"DeviceID\"://\"e823469202135602\"},\"ClientMediaId\": " + LoginService.GetTimeStamp() + ",\"TotalLen\":" + fileinfo.Length + ",\"StartPos\":0,\"DataLen/\":" + fileinfo.Length + ",\"MediaType\":4,\"FromUserName\":\"" + from + "\",\"ToUserName\":\"" + to + "\",\"FileMd5\":/\"7911968c2371ab9b72c3f42d52776ce9/\"}").Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"webwx_data_ticket\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append(webwx_data_ticket.Value).Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"pass_ticket\"");
                    sb.Append(newLine).Append(newLine);
                    sb.Append(LoginService.pass_Ticket).Append(newLine);
                
                    sb.Append(freFix + boundary).Append(newLine);
                    sb.Append("Content-Disposition: form-data; name=\"filename\"; filename=\"" + fileinfo.Name.Substring(fileinfo.Name.LastIndexOf("\\") + 1)+ "\"");
                    sb.Append(newLine);
                    sb.Append("Content-Type: image/png");
                    sb.Append(newLine).Append(newLine);
                    sb.Append(freFix + boundary + freFix);
                
                    conn.Method = "POST";
                    conn.Accept = "*/*";
                    conn.ContentType = "multipart/form-data; boundary=" + boundary;
                    conn.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    conn.Referer = "https://wx2.qq.com/?&lang=zh_CN";
                    conn.Host = "file2.wx.qq.com";
                    conn.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    conn.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                    conn.Headers.Add("Cache-Control", "no-cache");
                    //conn.Headers.Add("Connection", "Keep-Alive");
                    conn.Headers.Add("Origin", "https://wx2.qq.com");
                    conn.Headers.Add("Pragma", "no-cache");
                
                    byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
                    // 输入文件流数据 
                    byte[] buffers = new Byte[checked((uint)Math.Min(4096, (int)fileinfo.Length))];
                
                    conn.ContentLength = data.Length + buffers.Length;
                    int bytesRead = 0;
                    using (Stream reqStream = conn.GetRequestStream())
                    {
                        reqStream.Write(data, 0, data.Length);
                
                        while ((bytesRead = fileinfo.Read(buffers, 0, buffers.Length)) != 0)
                            reqStream.Write(buffers, 0, bytesRead);
                
                        reqStream.Close();
                    }
                
                  HttpWebResponse resp = (HttpWebResponse)conn.GetResponse();
                  Stream stream = resp.GetResponseStream();
                  string result;
                  //获取响应内容  
                  using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                  {
                      result = reader.ReadToEnd();
                  }
                   return result;

                string url_1 = "https://file.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json&sid=";
                string url_2 = "https://file2.wx.qq.com/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json&sid=";
        //     HttpWebRequest conn11 = (HttpWebRequest)WebRequest.Create(url_1);
        //     conn11.Method = "POST";
        //     conn11.Accept = "*/*";
        //     conn11.ContentType = "multipart/form-data; boundary=" + boundary;
        //     conn11.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
        //     conn11.Referer = "https://wx2.qq.com/?&lang=zh_CN";
        //     conn11.Host = "file2.wx.qq.com";
        //     conn11.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        //     conn11.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
        //     conn11.Headers.Add("Cache-Control", "no-cache");
        //     //conn.Headers.Add("Connection", "Keep-Alive");
        //     conn11.Headers.Add("Origin", "https://wx2.qq.com");
        //     conn11.Headers.Add("Pragma", "no-cache");
        //
        //
        //     // 输入文件流数据 
        //     byte[] buffers11 = new Byte[checked((uint)Math.Min(4096, (int)fileinfo.Length))];
        //
        //     string BaseRequest = "{" +
        //           "\"DeviceID\" : \"e441551176\"," +
        //           "\"Sid\" : \"" + sId.Value + "\"," +
        //           "\"Skey\" : \"" + LoginService.sKey + "\"," +
        //           "\"Uin\" : \"" + uIn.Value + "\"}";
        //     string fdic = "{" +
        //    "\"BaseRequest\" : " + BaseRequest + "," +
        //    "\"ClientMediaId\" : \"" + DateTime.Now.Millisecond + "\"," +
        //    "\"TotalLen\" : \"" + fileinfo.Length + "\"," +
        //    "\"StartPos\" : \"0\"," +
        //    "\"DataLen\" : \"" + fileinfo.Length + "\"," +
        //    "\"MediaType\" : \"4\"}";
        //
        //     string files = "{" +
        //  "\"id\":  \"WU_FILE_0\"," +
        //  "\"name\": \"temp\"," +
        //  "\"type\": \"image/png\"," +
        //  "\"lastModifiedDate\":\"Wed Mar 15 2017 18:13:15 GMT+0800\"," +
        //  "\"size\": \"" + fileinfo.Length + "\"," +
        //  "\"mediatype\":\"pic\"," +
        //  "\"uploadmediarequest\": " + fdic + "," +
        //  "\"webwx_data_ticket\": \"" + webwx_data_ticket.Value + "\"," +
        //  "\"pass_ticket\": \"" + LoginService.pass_Ticket + "\"," +
        //  "\"filename\": \"\"}";
        //     byte[] data11 = Encoding.UTF8.GetBytes(files.ToString());
        //     conn11.ContentLength = data11.Length + buffers11.Length;
        //     int bytesRead = 0;
        //     using (Stream reqStream = conn11.GetRequestStream())
        //     {
        //         reqStream.Write(data11, 0, data11.Length);
        //
        //         while ((bytesRead = fileinfo.Read(buffers11, 0, buffers11.Length)) != 0)
        //             reqStream.Write(buffers11, 0, bytesRead);
        //
        //         reqStream.Close();
        //     }
        //
        //     HttpWebResponse resp = (HttpWebResponse)conn11.GetResponse();
        //     Stream stream = resp.GetResponseStream();
        //     string result;
        //     //获取响应内容  
        //     using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        //     {
        //         result = reader.ReadToEnd();
        //     }
                // return result;

                //    byte[] bytesss = RequstService.SendPostRequest(_upload_image_url + sId.Value + "&lang=zh_CN&pass_ticket=" + LoginService.pass_Ticket, files);


                //        string send_result = Encoding.UTF8.GetString(bytesss);


                //    Stream outputStream = conn.GetRequestStream();
                // outputStream.Write(System.Text.Encoding.UTF8.GetBytes(sb.ToString()), 0, 0);//写入请求参数

          //    BinaryReader dis = new BinaryReader(fileinfo);
          //    int bytes = 0;
          //    byte[] bufferOut = new byte[1024];
          //    while ((bytes = dis.Read(bufferOut, 1, 1)) != -1)
          //    {
          //        outputStream.Write(bufferOut, 0, bytes);//写入图片
          //    }
          //
          //      outputStream.Write(System.Text.Encoding.UTF8.GetBytes(newLine), 0, 0);
          //      outputStream.Write(System.Text.Encoding.UTF8.GetBytes((freFix + boundary + freFix + newLine)), 0, 0);//标识请求数据写入结束
          //
          //     dis.Close();
          //    outputStream.Close();
          //
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
                String str = "";
                StringBuilder buffer = new StringBuilder();
                while ((str = bufferedReader.ToString()) != null)
                {
                    buffer.Append(str);
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
                        bytes = RequstService.SendPostRequest(_sendmsg_url + sid.Value + "&lang=zh_CN&pass_ticket=" + LoginService.pass_Ticket, msg_json);
                        break;
                    case 3:
                        bytes = RequstService.SendPostRequest(_send_image_url + sid.Value + "&lang=zh_CN&pass_ticket=" + LoginService.pass_Ticket, msg_json);
                        break;
                }

                string send_result = Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
