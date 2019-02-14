using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinCarManager
{
    public partial class MainForm : Form
    {
        private VehicleForm ShowVehicles;

        public MainForm()
        {
            InitializeComponent();
        }

        private void vehiclesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ShowVehicles == null || this.ShowVehicles.IsDisposed)
            {
                this.ShowVehicles = new VehicleForm();
                this.ShowVehicles.MdiParent = this;
                this.ShowVehicles.Show();
            }
        }
    }
}
