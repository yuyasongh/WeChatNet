using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChat.Model;
using System.IO;
using WeChat.NET.Controls;
using WeChat.Services;
using System.Collections;

namespace WeChat.Controls
{
    public partial class MessageBoxs : UserControl
    {
        public event NET.Controls.FriendInfoViewEventHandler FriendInfoView;

        //聊天好友
        private WXUser _friendUser;
        public WXUser FriendUser
        {
            get
            {
                return _friendUser;
            }
            set
            {
                if (value != _friendUser)
                {
                    _friendUser = value;
                    if (_friendUser != null)
                    {
                        PublicWindows.messageBoxs.Show();
                        _friendUser.MsgRecved += new MsgRecvedEventHandler(_friendUser_MsgRecved);
                        _friendUser.MsgSent += new MsgSentEventHandler(_friendUser_MsgSent);

                        webBrowser1.DocumentText = _totalHtml = "";
                        lblInfo.Text = "" + _friendUser.ShowName + "";
                        lblInfo.Location = new Point((plTop.Width - lblInfo.Width) / 2, lblInfo.Location.Y);
                        IEnumerable<KeyValuePair<DateTime, WXMsg>> dic = _friendUser.RecvedMsg.Concat(_friendUser.SentMsg);
                        dic = dic.OrderBy(i => i.Key);
                        foreach (KeyValuePair<DateTime, WXMsg> p in dic)  //恢复聊天记录
                        {
                            if (p.Value.From == _friendUser.UserName)
                            {
                                ShowReceiveMsg(p.Value);
                            }
                            else
                            {
                                ShowSendMsg(p.Value);
                            }
                            p.Value.Readed = true;
                        }
                    }
                }
            }
        }
        //自己
        private UserInfo _meUser;
        public UserInfo MeUser
        {
            get
            {
                return _meUser;
            }
            set
            {
                _meUser = value;
            }
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public MessageBoxs()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WChatBox_Load(object sender, EventArgs e)
        {
            webBrowser1.IsWebBrowserContextMenuEnabled = false;
        }
        /// <summary>
        /// 发送消息完成
        /// </summary>
        /// <param name="msg"></param>
        void _friendUser_MsgSent(WXMsg msg)
        {
            ShowSendMsg(msg);
        }
        /// <summary>
        /// 收到新消息
        /// </summary>
        /// <param name="msg"></param>
        void _friendUser_MsgRecved(WXMsg msg)
        {
            ShowReceiveMsg(msg);
        }
        /// <summary>
        /// 点击发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != null && _friendUser != null && _meUser != null)
            {
                WXMsg msg = new WXMsg();
                msg.From = _meUser.UserName;
                msg.Msg = txtInput.Text;
                msg.Readed = false;
                msg.To = _friendUser.UserName;
                msg.Type = 1;
                msg.Time = DateTime.Now;

                _friendUser.SendMsg(msg, false);
            };
        }
        /// <summary>
        /// 消息输入框 回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSend_Click(null, null);
                e.Handled = true;
            }
        }

        #region  消息框操作相关
        private static WXMsg LastSendMsg = new WXMsg();
        private static WXMsg LastReceiveMsg = new WXMsg();
        /// <summary>
        /// UI界面显示发送消息
        /// </summary>
        private void ShowSendMsg(WXMsg msg)
        {
            if (_meUser == null || _friendUser == null)
            {
                return;
            }
            if (msg == LastSendMsg)
                return;
            LastSendMsg = msg;
            string str = @"<script type=""text/javascript""> </script> 
            <div  name=""content"" id=""content"" class=""chat_content_group self"">   
            <p class=""chat_nick"">" + ((_meUser.RemarkName == null || _meUser.RemarkName == "") ? _meUser.NickName : _meUser.RemarkName) + @"</p>   
            <p class=""chat_content"">" + msg.Msg + @"</p>
            </div><a id='ok'></a>";
            if (_totalHtml == "")
            {
                _totalHtml = MessageHtml._baseHtml;
            }
            _totalHtml = _totalHtml.Replace("<a id='ok'></a>", "") + str;
            webBrowser1.DocumentText = _totalHtml;
            txtInput.Text = "";
        }
        /// <summary>
        /// UI界面显示接收消息
        /// </summary>
        private void ShowReceiveMsg(WXMsg msg)
        {
            if (_meUser == null || _friendUser == null)
            {
                return;
            }
            if (msg == LastReceiveMsg)
                return;
            LastReceiveMsg = msg;
            string str = @"<script type=""text/javascript""></script> 
            <div name=""content"" id=""content"" class=""chat_content_group buddy"">   
            <p class=""chat_nick"">" + _friendUser.ShowName + @"</p>   
            <p class=""chat_content"">" + msg.Msg + @"</p>
            </div><a id='ok'></a>";
            if (_totalHtml == "")
            {
                _totalHtml = MessageHtml._baseHtml;
            }
            msg.Readed = true;
            webBrowser1.DocumentText = _totalHtml = _totalHtml.Replace("<a id='ok'></a>", "") + str;
        }
        private string _totalHtml = "";

        #endregion
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFile_Click(object sender, EventArgs e)
        {
            //初始化一个OpenFileDialog类
            OpenFileDialog fileDialog = new OpenFileDialog();

            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //获取用户选择文件的后缀名
                string extension = Path.GetExtension(fileDialog.FileName);
                //声明允许的后缀名
                string[] str = new string[] { ".jpge", ".jpg", ".png" };
                if (!((IList)str).Contains(extension))
                {
                    MessageBox.Show("仅能上传jpge,jpg,png格式的图片！");
                }
                else
                {
                    //获取用户选择的文件，并判断文件大小不能超过20K，fileStream.Length是以字节为单位的
                    FileStream fileStream = new FileStream(fileDialog.FileName, FileMode.Open);
                    if (fileStream.Length > 20480)
                    {
                        MessageBox.Show("上传的图片不能大于20K");
                    }
                    else
                    {
                        WXMsg msg = new WXMsg();
                        msg.From = _meUser.UserName;
                        msg.Readed = false;
                        msg.To = _friendUser.UserName;
                        msg.Type = 3;
                        msg.Time = DateTime.Now;
                        SendMessage send = new SendMessage();
                        msg.Msg = send.UploadImg(msg.From, msg.To, fileStream);
                        _friendUser.SendMsg(msg, false);
                    }
                }
            }
        }
        /// <summary>
        /// 图片转为Byte字节数组
        /// </summary>
        ///<param name="FilePath">路径
        /// <returns>字节数组</returns>
        private byte[] imageToByteArray(string FilePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Image imageIn = Image.FromFile(FilePath))
                {
                    using (Bitmap bmp = new Bitmap(imageIn))
                    {
                        bmp.Save(ms, imageIn.RawFormat);
                    }
                }
                return ms.ToArray();
            }
        }

    }
}