using CarManagement.Core.Models;
using CarManagement.Core.Services;
using CarManagement.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinCarManager
{
    public partial class vehicleDisplayerWin : Form
    {
        private string connectionString;
        private IVehicle[] vehicles;

        public vehicleDisplayerWin()
        {
            InitializeComponent();
            this.connectionString = ConfigurationManager.AppSettings["CarManagerConnectionString"];
        }

        private void searchCarButt_Click(object sender, EventArgs e)
        {
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            SqlVehicleStorage vehicleStorage = new SqlVehicleStorage(this.connectionString, vehicleBuilder);

            IVehicleQuery queryBuilder = vehicleStorage.get();



            //this.vehicles = queryBuilder;
        }

        private void exitSearchButt_Click(object sender, EventArgs e)
        {

        }

        private void updateCarButt_Click(object sender, EventArgs e)
        {

        }

        private void addCarButt_Click(object sender, EventArgs e)
        {

        }

        private void goToFirstButt_Click(object sender, EventArgs e)
        {

        }

        private void goToPreviousButt_Click(object sender, EventArgs e)
        {

        }

        private void goToNextButt_Click(object sender, EventArgs e)
        {

        }

        private void goToLastButt_Click(object sender, EventArgs e)
        {

        }

        private void storageTabButt_Click(object sender, EventArgs e)
        {

        }

        private void carListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void doorListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void wheelListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
