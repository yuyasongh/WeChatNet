using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WeChat.Controls;
using WeChat.Model;
using WeChat.NET.Controls;
using WeChat.Properties;
using WeChat.Services;
using static WeChat.QRCode;

namespace WeChat
{
    public partial class Main : Form
    {
        private static JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
        /// <summary>
        /// 好友列表控件
        /// </summary>
        private WFriendsList wFriendsList = new WFriendsList();
        /// <summary>
        /// 消息列表控件
        /// </summary>
        private WChatList wChatList = new WChatList();
        /// <summary>
        /// 指定发送联系人
        /// </summary>
        private string userName = "";
        private delegate void MethodDelegate();
        public Main()
        {

            InitializeComponent();
            pbTop.BringToFront();
            ((Action)(delegate ()
            {
                //加载好友列表数据
                MessageSync.Friends();
                //加载最近消息
                pbMessage_Click(null, null);
                if (MessageSync.friends_result["MemberList"] != null)
                {
                    foreach (var item in MessageSync.friends_result["MemberList"])
                    {
                        if (item["NickName"].ToString() == "yuyasong")
                        {
                            userName = item["UserName"].ToString();
                            break;
                        }
                    }
                }
            })).BeginInvoke(null, null);

            PublicWindows.messageBoxs.Width = 540;
            PublicWindows.messageBoxs.Height = 419;
            panelChat.Controls.Add(PublicWindows.messageBoxs);
            PublicWindows.messageBoxs.Hide();
        }
        #region 好友列表
        /// <summary>
        /// 好友列表点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbFriends_Click(object sender, EventArgs e)
        {
            try
            {
                ((Action)(delegate ()
                {
                    this.pbFriends.BackgroundImage = Resources.haoyou2;
                    pbMessage.BackgroundImage = Resources.message1_2_;
                    List<object> data = MessageSync.FriendsList();
                    this.BeginInvoke((Action)delegate ()
                    {
                        wFriendsList.Items.Clear();
                        wFriendsList.Width = 215;
                        wFriendsList.Height = 446;
                        wFriendsList.Items.AddRange(data.ToArray());  //好友列表
                        this.panelList.Controls.Clear();
                        this.panelList.Controls.Add(wFriendsList);
                        wFriendsList.Show();
                    });
                })).BeginInvoke(null, null);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 最近联系人
        /// <summary>
        /// 最近联系人、消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbMessage_Click(object sender, EventArgs e)
        {
            ((Action)(delegate ()
            {
                bool _start = false;
                pbFriends.BackgroundImage = Resources.haoyou;
                pbMessage.BackgroundImage = Resources.message2_1_;
                List<object> data = MessageSync.Message();
                this.BeginInvoke((Action)delegate ()
                {
                    wChatList.Items.Clear();
                    wChatList.Width = 215;
                    wChatList.Height = 446;
                    wChatList.Items.AddRange(data.ToArray());  //通讯录
                    this.panelList.Controls.Clear();
                    this.panelList.Controls.Add(wChatList);
                    wChatList.Show();
                });
                string sync_flag = "";
                JObject sync_result;
                _start = true;
                while (true)
                {
                    #region 定时回复指定人
                    this.BeginInvoke((Action)delegate ()
                    {
                        //指定时间回复指定人
                        if (DateTime.Now.ToString("HH:mm:ss:ff") == "15:30:00:00")  //如果当前时间是XXXX
                        {
                            WXMsg assignMsg = new WXMsg();
                            WXUser assignUser = new WXUser();
                            assignMsg.From = MessageSync.userInfo.UserName;
                            assignMsg.Msg = "时间到了";
                            assignMsg.Readed = false;
                            assignMsg.To = userName;
                            assignMsg.Type = 1;
                            assignMsg.Time = DateTime.Now;
                            assignUser.SendMsg(assignMsg, false);
                        }
                    });
                    #endregion
                    sync_flag = MessageSync.WxSyncCheck();  //同步检查
                    if (sync_flag == null)
                    {
                        continue;
                    }
                    //这里应该判断 sync_flag中selector的值
                    else //有消息
                    {
                        sync_result = MessageSync.WxSync();  //进行同步
                        if (sync_result != null)
                        {
                            if (sync_result["AddMsgCount"] != null && sync_result["AddMsgCount"].ToString() != "0")
                            {
                                foreach (JObject m in sync_result["AddMsgList"])
                                {
                                    string from = m["FromUserName"].ToString();
                                    string to = m["ToUserName"].ToString();
                                    string content = m["Content"].ToString();
                                    string type = m["MsgType"].ToString();
                                    string MsgId = m["MsgId"].ToString();
                                    WXMsg msg = new WXMsg();
                                    msg.From = from;
                                    #region 指定回复某人
                                    this.BeginInvoke((Action)delegate ()
                                    {
                                        //指定回复某人消息
                                        WXMsg assignMsg = new WXMsg();
                                        WXUser assignUser = new WXUser();
                                        if (msg.From == userName)
                                        {
                                            assignMsg.From = MessageSync.userInfo.UserName;
                                            assignMsg.Msg = "指定消息";
                                            assignMsg.Readed = false;
                                            assignMsg.To = msg.From;
                                            assignMsg.Type = 1;
                                            assignMsg.Time = DateTime.Now;
                                            assignUser.SendMsg(assignMsg, false);
                                        }
                                    });
                                    #endregion
                                    if (type == "3")
                                    {
                                        string imgBase64 = MessageSync.Image(MsgId);
                                        content = "<msg><img length=\"6503\" src=\"data:image/png;base64," + imgBase64 + "\" hdlength = \"0\" /> <commenturl></commenturl></msg>";
                                    }
                                    msg.Msg = (type == "1" || type == "3") ? content : "请在手机上查看消息";  //只接受文本消息
                                    msg.Readed = false;
                                    msg.Time = DateTime.Now;
                                    msg.To = to;
                                    msg.Type = int.Parse(type);

                                    if (msg.Type == 51)  //屏蔽一些系统数据
                                    {
                                        continue;
                                    }
                                    this.BeginInvoke((Action)delegate ()
                                    {
                                        WXUser user; bool exist_latest_contact = false;
                                        foreach (Object u in wChatList.Items)
                                        {
                                            user = u as WXUser;
                                            if (user != null)
                                            {
                                                if (user.UserName == msg.From && msg.To == MessageSync.userInfo.UserName)  //接收别人消息
                                                    {
                                                    wChatList.Items.Remove(user);
                                                    wChatList.Items.Insert(0, user);
                                                    exist_latest_contact = true;
                                                    user.ReceiveMsg(msg);
                                                    break;
                                                }
                                                else if (user.UserName == msg.To && msg.From == MessageSync.userInfo.UserName)  //同步自己在其他设备上发送的消息
                                                    {
                                                    wChatList.Items.Remove(user);
                                                    wChatList.Items.Insert(0, user);
                                                    exist_latest_contact = true;
                                                    user.SendMsg(msg, true);
                                                    break;
                                                }
                                            }
                                        }
                                        if (!exist_latest_contact)
                                        {
                                            foreach (object o in wFriendsList.Items)
                                            {
                                                WXUser friend = o as WXUser;
                                                if (friend != null && friend.UserName == msg.From && msg.To == MessageSync.userInfo.UserName)
                                                {
                                                    wChatList.Items.Insert(0, friend);
                                                    friend.ReceiveMsg(msg);
                                                    break;
                                                }
                                                if (friend != null && friend.UserName == msg.To && msg.From == MessageSync.userInfo.UserName)
                                                {
                                                    wChatList.Items.Insert(0, friend);
                                                    friend.SendMsg(msg, true);
                                                    break;
                                                }
                                            }
                                        }
                                        wChatList.Invalidate();
                                            // System.Threading.Thread.Sleep(5);
                                        });
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(10);
                }
            })).BeginInvoke(null, null);
        }
        #endregion


        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbClose_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        #region 窗体拖动
        private Point _mPoint;
        private void pbTop_MouseDown(object sender, MouseEventArgs e)
        {
            _mPoint.X = e.X;
            _mPoint.Y = e.Y;
        }
        private void pbTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-_mPoint.X, -_mPoint.Y);
                Location = myPosittion;
            }
        }
        #endregion


    }
}
