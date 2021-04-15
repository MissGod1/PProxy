using AwesomeProject.Controller;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AwesomeProject.View
{
    public partial class MainWindow : Form
    {
        private FileController fc;
        private ProcessController pc;
        private ContextMenuStrip gMenu;
        private ContextMenuStrip pMenu;
        public MainWindow()
        {
            InitializeComponent();
            fc = new FileController();
            pc = new ProcessController(ProcessExitedHandler);
            gMenu = CreateContextMenu(addGame, editGame);
            pMenu = CreateContextMenu(addProxy, editProxy);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon.Visible = true;
        }

        private void btnGame_Click(object sender, EventArgs e)
        {
            var gamelist = fc.GetFileList(Properties.Resources.modePath, "conf");
            var infoView = new InfoListView(gamelist, new Point(this.Location.X + this.Width, this.Location.Y), true);
            if (infoView.ShowDialog() == DialogResult.OK && infoView.selected != null)
            {
                btnGame.Text = infoView.selected;
            }
        }

        private void btnProxy_Click(object sender, EventArgs e)
        {
            var proxylist = fc.GetFileList(Properties.Resources.serverPath, "json");
            var infoView = new InfoListView(proxylist, new Point(this.Location.X + this.Width, this.Location.Y), false);
            if (infoView.ShowDialog() == DialogResult.OK && infoView.selected != null)
            {
                btnProxy.Text = infoView.selected;
            }
        }

        private void btnGSetting_Click(object sender, EventArgs e)
        {
            gMenu.Show(MousePosition);
        }

        private void addGame(object sender, EventArgs e)
        {
            var dialog = new GameInfoView(new Point(this.Location.X + this.Width, this.Location.Y));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fc.SaveFile($"{Properties.Resources.modePath}/{dialog.name}.conf", dialog.process);
                }
                catch
                {
                    MessageBox.Show("保存失败");
                }

            }
        }
        private void editGame(object sender, EventArgs e)
        {
            var filename = btnGame.Text;
            try
            {
                var process = fc.GetFileLines($"{Properties.Resources.modePath}/{filename}.conf");
                var dialog = new GameInfoView(new Point(this.Location.X + this.Width, this.Location.Y), true)
                {
                    name = filename,
                    process = process
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fc.SaveFile($"{Properties.Resources.modePath}/{dialog.name}.conf", dialog.process, true);
                }
            }
            catch (Exception)
            {
                return;
            }

        }
        private void btnPSetting_Click(object sender, EventArgs e)
        {
            pMenu.Show(MousePosition);
        }

        private void addProxy(object sender, EventArgs e)
        {
            var dialog = new ProxyInfoView(new Point(this.Location.X + this.Width, this.Location.Y));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fc.SaveJsonFile(dialog.proxy, $"{Properties.Resources.serverPath}/{dialog.name}.json");
            }
        }
        private void editProxy(object sender, EventArgs e)
        {
            var filename = btnProxy.Text;
            try
            {
                var proxy = fc.GetJsonFile($"{Properties.Resources.serverPath}/{filename}.json");
                var dialog = new ProxyInfoView(new Point(this.Location.X + this.Width, this.Location.Y), true)
                {
                    name = filename,
                    proxy = proxy
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    fc.SaveJsonFile(dialog.proxy, $"{Properties.Resources.serverPath}/{dialog.name}.json", true);
                }
            }
            catch (Exception)
            {
                return;
            }

        }
        private ContextMenuStrip CreateContextMenu(EventHandler add, EventHandler edit)
        {
            var cMenu = new ContextMenuStrip()
            {
                ImageScalingSize = new System.Drawing.Size(32, 32),
                Size = new System.Drawing.Size(153, 44)
            };
            cMenu.Items.Add(new ToolStripMenuItem("新增", Properties.Resources.add_circle, add) 
            {
               Size = new System.Drawing.Size(152, 40)
            });
            cMenu.Items.Add(new ToolStripMenuItem("修改", Properties.Resources.edit, edit)
            {
                Size = new System.Drawing.Size(152, 40)
            });
            return cMenu;
        }

        Point point = new Point();
        private void topPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - point.X, this.Location.Y + e.Y - point.Y);
            }
        }

        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                point = new Point(e.X, e.Y);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                var game = fc.GetFileLines($"{Properties.Resources.modePath}/{btnGame.Text}.conf");
                var proxy = fc.GetJsonFile($"{Properties.Resources.serverPath}/{btnProxy.Text}.json");
                proxy["apps"] = game;
                fc.SaveJsonFile(proxy, "config.json", true);
                pc.Start();
                btnStop.BringToFront();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void ProcessExitedHandler(object sender, EventArgs e)
        {
            this.Invoke(new EventHandler(delegate {
                btnRun.BringToFront();
            }));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            pc.Stop();
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void tsmMain_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            notifyIcon.Visible = false;
        }
    }
}
