using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AwesomeProject.View
{
    public partial class GameInfoView : Form
    {
        private Point point;
        public string name;
        public string[] process;
        public bool edit;
        public GameInfoView(Point _point, bool _edit = false)
        {
            InitializeComponent();
            point = _point;
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
                foreach (var line in process)
                {
                    tbProcess.Text += $"{line}\r\n";
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbName.Text.Trim() == "" || tbProcess.Text.Trim() == "")
            {
                MessageBox.Show("任一信息不能为空！");
                return;
            }
            name = tbName.Text.Trim();
            process = tbProcess.Text.Trim()
                .Split(new char[] { '\n', ';', '，', '\r' })
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s)).ToArray();
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }
    }
}
