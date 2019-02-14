using CarManagement.Core.Models;
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
    public partial class VehicleForm : Form
    {
        private readonly List<IVehicle> vehicles;
        public VehicleForm(IVehicleStorage vehicleStorage)
        {
            InitializeComponent();
        }

        private void VehicleForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
