using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AwesomeProject
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var mutex = new Mutex(false, "Global\\AwesomeProject"))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("程序已在运行中", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Environment.Exit(1);
                }
            }
            Directory.SetCurrentDirectory(Application.StartupPath);

            //创建模式目录用于存放游戏配置
            if (!Directory.Exists(Properties.Resources.modePath))
            {
                Directory.CreateDirectory(Properties.Resources.modePath);
            }
            //创建配置目录用于放置服务器配置
            if (!Directory.Exists(Properties.Resources.serverPath))
            {
                Directory.CreateDirectory(Properties.Resources.serverPath);
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new View.MainWindow());
        }
    }
}
