using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChat.Services;

namespace WeChat
{
    public partial class QRCode : Form
    {
        public QRCode()
        {
            InitializeComponent();
            Login();
        }
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
        /// 返回二维码扫描界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblReturn_Click(object sender, EventArgs e)
        {
            _start = false;
            lblReturn.Visible = false;
            GC.Collect();
            Login();
        }

        #region 登录
        /// <summary>
        /// 获取到的UUID（参考方法 getUUID）
        /// </summary>
        private static string _uuid = null;
        /// <summary>
        /// 循环判断手机扫描二维码结果状态
        /// </summary>
        private static bool _start;
        private LoginService loginService= new LoginService();
        /// <summary>
        /// 登录
        /// </summary>
        public delegate Image MethodDelegate();
        /// <summary>
        /// 扫码登录判断
        /// </summary>
        /// <param name="ar"></param>
        public void TakesLoginOpera(IAsyncResult ar)
        {
            if (ar != null)
            {
                _start = true;
                MethodDelegate methodDelegate = ar.AsyncState as MethodDelegate;
                if (methodDelegate != null)
                {
                    Image result = methodDelegate.EndInvoke(ar);
                    pbQRCode.Image = result;
                    object loginStatus = null;
                    while (_start && result != null)  //循环判断手机扫描二维码结果
                    {
                        loginStatus = loginService.LoginScanDetection();
                        if (loginStatus is Image) //已扫描二维码但未登录
                        {
                            LoginService.tip = 0;
                            this.BeginInvoke((Action)delegate ()
                            {
                                lblPrompt.Text = "请点击手机登录按钮";
                                pbQRCode.SizeMode = PictureBoxSizeMode.CenterImage;  //用户扫描后用户头像
                                pbQRCode.Image = loginStatus as Image;
                                lblReturn.Visible = true;
                            });
                        }
                        if (loginStatus is string)  //已扫描二维码并完成登录
                        {
                            LoginService.tip = 0;
                            //登录获取Cookie（参考方法 login）
                            loginService.GetSidUin(loginStatus as string);
                            //打开主界面
                            this.BeginInvoke((Action)delegate ()
                            {
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            });
                            break;
                        }
                    }
                }
            }
        }
        public void Login()
        {
            pbQRCode.Image = null;
            pbQRCode.SizeMode = PictureBoxSizeMode.Zoom;
            lblPrompt.Text = "手机微信扫一扫登录";
            MethodDelegate methodDelegate = loginService.GetQRCode;
            methodDelegate.BeginInvoke(TakesLoginOpera, methodDelegate);
            #region test
            //   _task = new Task(() =>
            //   {
            //       Image qrCode = GetQRCode();
            //       if (qrCode != null)
            //       {
            //           this.BeginInvoke((Action)delegate ()
            //           {
            //               pbQRCode.Image = qrCode;
            //           });
            //           object loginStatus = null;
            //           while (true)  //循环判断手机扫描二维码结果
            //           {
            //               loginStatus = LoginScanDetection();
            //               if (loginStatus is Image) //已扫描二维码但未登录
            //               {
            //                   this.BeginInvoke((Action)delegate ()
            //                   {
            //                       lblPrompt.Text = "请点击手机登录按钮";
            //                       pbQRCode.SizeMode = PictureBoxSizeMode.CenterImage;  //用户扫描后用户头像
            //                       pbQRCode.Image = loginStatus as Image;
            //                       lblReturn.Visible = true;
            //                   });
            //               }
            //               if (loginStatus is string)  //已扫描二维码并完成登录
            //               {
            //                   //登录获取Cookie（参考方法 login）
            //                   GetSidUin(loginStatus as string);
            //                   //打开主界面
            //                   this.BeginInvoke((Action)delegate ()
            //                   {
            //                       this.DialogResult = DialogResult.OK;
            //                       this.Close();
            //                   });
            //                   break;
            //               }
            //           }
            //       }
            //   });
            //   _task.Start();
            #endregion
        }

        #endregion
    }
}
