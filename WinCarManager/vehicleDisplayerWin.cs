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
    public partial class VehicleDisplayerWin : Form
    {
        private string connectionString;
        private IEnumerable<IVehicle> vehicles;

        IEnrollmentProvider enrollmentProvider;
        IVehicleStorage vehicleStorage;

        private String currentEnrollment;

        public VehicleDisplayerWin( IEnrollmentProvider enrollmentProvider, IVehicleStorage vehicleStorage)
        {
            InitializeComponent();
            this.connectionString = ConfigurationManager.AppSettings["CarManagerConnectionString"];

            this.enrollmentProvider = enrollmentProvider;
            this.vehicleStorage = vehicleStorage;

            String[] carColorOptionArray = new String[Enum.GetNames(typeof(CarColor)).Length + 1];
            carColorOptionArray[0] = String.Empty;
            Enum.GetNames(typeof(CarColor)).CopyTo(carColorOptionArray, 1);
            this.carColorView.DataSource = carColorOptionArray;
        }

        private void searchCarButt_Click(object sender, EventArgs e)
        {
            IVehicleQuery queryBuilder = this.vehicleStorage.get();
            
            string horsePowerString = this.horsePowerEngineView.Text;

            if (string.IsNullOrEmpty(this.enrollmentView.Text.Trim()) == false)
            {
                string[] enrollmentParam = this.enrollmentView.Text.Split('-');
                string serial = enrollmentParam[0].Trim();
                if (enrollmentParam.Length > 1)
                {
                    int number = Convert.ToInt32( enrollmentParam[1].Trim() );

                    queryBuilder = queryBuilder.whereEnrollmentIs(this.enrollmentProvider.import( serial, number ));
                }
                else
                {
                    queryBuilder = queryBuilder.whereEnrollmentSerialIs(serial);
                }
            }

            if (string.IsNullOrEmpty(this.carColorView.Text.Trim()) == false)
            {
                if( Enum.TryParse<CarColor>(this.carColorView.Text, out CarColor tryColor) )
                {
                    queryBuilder = queryBuilder.whereColorIs(tryColor);
                }
            }

            if (string.IsNullOrEmpty(this.horsePowerEngineView.Text) == false)
            {
                string[] horsePowerParam = this.horsePowerEngineView.Text.Split('-');
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

            this.vehicles = queryBuilder;
            fullDisplayVehicleCollection();
        }

        private void exitSearchButt_Click(object sender, EventArgs e)
        {
            this.vehicles = null;
            this.currentEnrollment = null;
            fullDisplayVehicleCollection();
        }

        private void undoChangesButt_Click(object sender, EventArgs e)
        {
            if (this.currentEnrollment != null)
            {
                if (this.vehicles.Any(vehicle => vehicle.Enrollment.ToString() == this.currentEnrollment))
                {
                    fullDisplayVehicle(this.vehicles.First(vehicle => vehicle.Enrollment.ToString() == this.currentEnrollment));
                }
            }
        }

        private void undoAllchangesButt_Click(object sender, EventArgs e)
        {
            fullDisplayVehicleCollection();
        }

        private void updateCarButt_Click(object sender, EventArgs e)
        {

        }

        private void addCarButt_Click(object sender, EventArgs e)
        {

        }

        private void goToFirstButt_Click(object sender, EventArgs e)
        {
            this.currentEnrollment = this.vehicles.First().Enrollment.ToString();
            fullDisplayVehicle(this.vehicles.First());
        }

        private void goToPreviousButt_Click(object sender, EventArgs e)
        {

        }

        private void goToNextButt_Click(object sender, EventArgs e)
        {

        }

        private void goToLastButt_Click(object sender, EventArgs e)
        {
            this.currentEnrollment = this.vehicles.Last().Enrollment.ToString();
            fullDisplayVehicle(this.vehicles.Last());
        }

        private void storageTabButt_Click(object sender, EventArgs e)
        {

        }

        private void wheelPressureView_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.wheelPressureView.Text) == false)
            {
                if (Double.TryParse(this.wheelPressureView.Text, out double pressure) == false)
                {
                    this.errorProvider.SetError(this.wheelPressureView, "This has to be a number (decimals allowed)");
                }
                else
                {
                    this.errorProvider.SetError(this.wheelPressureView, string.Empty);
                }
            }
        }

        private void carListView_ItemActivate(object sender, EventArgs e)
        {
            this.currentEnrollment = this.carListView.SelectedItems[0].Text;
            fullDisplayVehicle(
                this.vehicles.First
                (vehicle =>
                    vehicle.Enrollment.ToString() == this.currentEnrollment
                )
            );
        }

        private void doorsView_ItemActivate(object sender, EventArgs e)
        {
            fullDisplayDoor(
                this.vehicles.Single
                (vehicle =>
                    vehicle.Enrollment.ToString() == this.currentEnrollment
                )
                .Doors[this.doorsView.SelectedIndices[0]]
                
            );
        }

        private void wheelsView_ItemActivate(object sender, EventArgs e)
        {
            fullDisplayWheel(
                this.vehicles.Single
                (vehicle =>
                    vehicle.Enrollment.ToString() == this.currentEnrollment
                )
                .Wheels[this.wheelsView.SelectedIndices[0]]

            );
        }

        private void fullDisplayVehicleCollection()
        {
            if (this.vehicles != null)
            {
                this.carListView.Items.Clear();
                foreach (IVehicle vehicle in this.vehicles)
                {
                    this.carListView.Items.Add(vehicle.Enrollment.ToString());
                }
                fullDisplayVehicle(this.vehicles.First());
            }
            else
            {
                this.carListView.Items.Clear();
            }
        }

        private void fullDisplayVehicle(IVehicle vehicle)
        {
            this.carColorView.Text = vehicle.Color.ToString();
            this.enrollmentView.Text = vehicle.Enrollment.ToString();
            this.horsePowerEngineView.Text = vehicle.Engine.HorsePower.ToString();
            if (vehicle.Engine.IsStarted)
            {
                this.startedEngineView.Checked = true;
            }
            else
            {
                this.startedEngineView.Checked = false;
            }

            this.wheelsView.Items.Clear();
            foreach (IWheel wheel in vehicle.Wheels)
            {
                this.wheelsView.Items.Add("Wh" + wheel.Pressure.ToString());
            }
            fullDisplayWheel(vehicle.Wheels[0]);

            this.doorsView.Items.Clear();
            foreach (IDoor door in vehicle.Doors)
            {
                this.doorsView.Items.Add("Dr" + door.IsOpen.ToString());
            }
            if (vehicle.Doors.Length > 0)
            {
                fullDisplayDoor(vehicle.Doors[0]);
            }
        }

        private void fullDisplayWheel(IWheel wheel)
        {
            this.wheelPressureView.Text = wheel.Pressure.ToString();
        }

        private void fullDisplayDoor(IDoor door)
        {
            this.isOpenedDoor.Checked = door.IsOpen;
        }

    }
}
