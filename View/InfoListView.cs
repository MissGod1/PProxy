using PProxy.Controller;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PProxy.View
{
    public partial class InfoListView : Form
    {
        private Point point;
        public string selected;
        private string[] list;
        private bool state;
        public InfoListView(string[] _list, Point _point, bool _state)
        {
            InitializeComponent();
            point = _point;
            list = _list;
            state = _state;
        }

        private void InfoListView_Load(object sender, EventArgs e)
        {
            this.Location = point;
            lbList.Items.AddRange(list);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lbList.SelectedItem != null)
            {
                selected = lbList.SelectedItem.ToString();
            }
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void tsmDel_Click(object sender, EventArgs e)
        {
            if (lbList.SelectedItem == null)
            {
                return;
            }
            bool success;
            if (state)
            {
                success = new FileController().DeleteFile($"{Properties.Resources.modePath}/{lbList.SelectedItem.ToString()}.conf");
            }
            else
            {
                success = new FileController().DeleteFile($"{Properties.Resources.serverPath}/{lbList.SelectedItem.ToString()}.json");
            }
            if (success)
            {
                list = list.Where(s => s != lbList.SelectedItem.ToString()).ToArray();
                lbList.Items.Clear();
                lbList.Items.AddRange(list);
            }

        }
    }
}
