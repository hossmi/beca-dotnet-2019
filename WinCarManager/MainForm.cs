using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarManagement.Core.Services;

namespace WinCarManager
{
    public partial class MainForm : Form
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IEnrollmentProvider enrollmentProvider;

        public MainForm(IVehicleStorage vehicleStorage, IEnrollmentProvider enrollmentProvider)
        {
            this.vehicleStorage = vehicleStorage;
            this.enrollmentProvider = enrollmentProvider;
            InitializeComponent();
        }

        private void vehiclesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VehicleStorageForm vehicleStorageForm = new VehicleStorageForm();
            vehicleStorageForm.Show();
        }
    }
}
