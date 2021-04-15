using System;
using System.Drawing;
using System.Windows.Forms;

namespace AwesomeProject.View
{
    public partial class InfoListView : Form
    {
        private Point point;
        public string selected;
        private string[] list;
        public InfoListView(string[] _list, Point _point)
        {
            InitializeComponent();
            point = _point;
            list = _list;
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
    }
}
