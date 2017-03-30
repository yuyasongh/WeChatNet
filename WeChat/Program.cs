using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChat;
using WeChat.Services;

namespace WeChat
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                QRCode qRCode = new QRCode();
                qRCode.ShowDialog();
                if (qRCode.DialogResult == DialogResult.OK)
                {
                    Application.Run(new Main());
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                 Application.Run(new Main());
            }
        }
    }
}
