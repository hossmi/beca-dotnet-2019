using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
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
            
            string horsePowerString = this.horsePowerEngineView.Text;

            if (string.IsNullOrEmpty(this.enrollmentView.Text.Trim()) != false)
            {
                string[] enrollmentParam = this.enrollmentView.Text.Split('-');
                string serial = enrollmentParam[0].Trim();
                if (enrollmentParam.Length > 1)
                {
                    int number = Convert.ToInt32( enrollmentParam[1].Trim() );

                    queryBuilder = queryBuilder.whereEnrollmentIs(enrollmentProvider.import( serial, number ));
                }
                else
                {
                    queryBuilder = queryBuilder.whereEnrollmentSerialIs(serial);
                }
            }

            if (string.IsNullOrEmpty(this.carColorView.Text.Trim()) != false)
            {
                if( Enum.TryParse<CarColor>(this.carColorView.Text, out CarColor tryColor) )
                {
                    queryBuilder = queryBuilder.whereColorIs(tryColor);
                }
            }

            if (string.IsNullOrEmpty(this.horsePowerEngineView.Text) != false)
            {
                string[] horsePowerParam = this.enrollmentView.Text.Split('-');
                int horsePower = Convert.ToInt32(horsePowerParam[0].Trim());
                if (horsePowerParam.Length > 1)
                {
                    int horsePowerMax = Convert.ToInt32(horsePowerParam[1].Trim());

                    queryBuilder.whereHorsePowerIsBetween(horsePower, horsePowerMax);
                }
                else
                {
                    queryBuilder.whereHorsePowerEquals(horsePower);
                }
            }

            if(this.startedEngineView.Checked)
            {
                queryBuilder.whereEngineIsStarted(true);
            }

            //this.vehicles = queryBuilder;
        }

        private void exitSearchButt_Click(object sender, EventArgs e)
        {

        }

        private void undoChangesButt_Click(object sender, EventArgs e)
        {

        }

        private void undoAllchangesButt_Click(object sender, EventArgs e)
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

        private static void fullDisplayVehicle(IVehicle vehicle)
        {

        }

        private static void fullDisplayVehicleCollection(IVehicle[] vechicles)
        {

        }
    }
}
