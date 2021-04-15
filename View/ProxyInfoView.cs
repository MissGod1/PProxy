using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AwesomeProject.View
{
    public partial class ProxyInfoView : Form
    {
        private Point point;
        private bool edit;
        public Dictionary<string, object> proxy;
        public string name;
        public ProxyInfoView(Point _point, bool _edit = false)
        {
            InitializeComponent();
            point = _point;
            proxy = new Dictionary<string, object>();
            edit = _edit;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void GameInfoView_Load(object sender, EventArgs e)
        {
            this.Location = point;
            if (edit)
            {
                tbName.Enabled = false;
                tbName.Text = name;

                tbAddress.Text = proxy["server"].ToString();
                nudPort.Value = Decimal.Parse(proxy["port"].ToString());
                tbPwd.Text = proxy["password"].ToString();
                cbbMethod.Text = proxy["method"].ToString();

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbAddress.Text.Trim())
//                || string.IsNullOrEmpty(tbPort.Text.Trim())
                || string.IsNullOrEmpty(tbPwd.Text.Trim())
                || string.IsNullOrEmpty(tbName.Text.Trim())
                || cbbMethod.SelectedItem == null)
            {
                MessageBox.Show("任一内容不能为空");
                return;
            }
            name = tbName.Text.Trim();

            proxy["port"] = Decimal.ToInt32(nudPort.Value);
            proxy["server"] = tbAddress.Text.Trim();
            proxy["password"] = tbPwd.Text.Trim();
            proxy["method"] = cbbMethod.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void cbDisplayPwd_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbDisplayPwd.Checked)
            {
                tbPwd.PasswordChar = '\0';
            } else
            {
                tbPwd.PasswordChar = '*';
            }
        }
    }
}
