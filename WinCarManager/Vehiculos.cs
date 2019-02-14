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
    public partial class Vehiculos : Form
    {
        private readonly IVehicleStorage vehicleStorage;
        public Vehiculos(IVehicleStorage vehicleStorage)
        {
            InitializeComponent();
            this.vehicleStorage = vehicleStorage;
        }


        private void Vehiculos_Load(object sender, EventArgs e)
        {

        }

        private void vehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VehicleForm newMDIChild = new VehicleForm(this.vehicleStorage);
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }
    }
}
