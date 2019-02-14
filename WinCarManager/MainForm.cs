using CarManagement.Core.Services;
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
        private VehicleForm vehicleForm;
        private readonly IVehicleStorage vehicleStorage;
        private readonly IEnrollmentProvider enrollmentProvider;


        public MainForm(IVehicleStorage vehicleStorage, IEnrollmentProvider enrollmentProvider)
        {
            this.vehicleStorage = vehicleStorage;
            this.enrollmentProvider = enrollmentProvider;
            InitializeComponent();
        }

        private void mainFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.vehicleForm == null || this.vehicleForm.IsDisposed)
            {
                this.vehicleForm = new VehicleForm(this.vehicleStorage);
                this.vehicleForm.MdiParent = this;
                this.vehicleForm.Show();
            }
        }
    }
}
