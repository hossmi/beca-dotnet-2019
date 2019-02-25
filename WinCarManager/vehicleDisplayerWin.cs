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
        private enum ActionPerformed
        {
            unchanged,
            changed,
            erased,
            added,
        }
        private struct DtoChangeableItem
        {
            public ActionPerformed action;
            public VehicleDto vehicle;
        }

        private IEnumerable<IVehicle> vehicles;

        private string connectionString;
        IEnrollmentProvider enrollmentProvider;
        IVehicleStorage vehicleStorage;
        IVehicleBuilder vehicleBuilder;

        private int currentVehicle;
        List<DtoChangeableItem> modifiableVehicles;

        public VehicleDisplayerWin( IEnrollmentProvider enrollmentProvider, IVehicleStorage vehicleStorage)
        {
            InitializeComponent();
            this.connectionString = ConfigurationManager.AppSettings["CarManagerConnectionString"];

            this.enrollmentProvider = enrollmentProvider;
            this.vehicleStorage = vehicleStorage;
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);

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

            this.modifiableVehicles = new List<DtoChangeableItem>();
            foreach (IVehicle vehicle in this.vehicles)
            {
                this.modifiableVehicles.Add(new DtoChangeableItem { action = ActionPerformed.unchanged, vehicle = this.vehicleBuilder.export(vehicle), });
            }

            fullDisplayVehicleCollection();
        }

        private void clearInputButton_Click(object sender, EventArgs e)
        {

        }

        private void exitSearchButt_Click(object sender, EventArgs e)
        {
            this.vehicles = null;
            this.currentVehicle = -1;
            fullDisplayVehicleCollection();
        }

        private void undoChangesButt_Click(object sender, EventArgs e)
        {
            if ( this.currentVehicle != -1)
            {
                if (this.currentVehicle < this.vehicleStorage.Count)
                {
                    this.modifiableVehicles[this.currentVehicle]= new DtoChangeableItem
                    {
                        action = ActionPerformed.unchanged,
                        vehicle = this.vehicleBuilder.export(this.vehicles.ElementAtOrDefault(this.currentVehicle)),
                    };
                    fullDisplayVehicle(this.modifiableVehicles[this.currentVehicle].vehicle);
                }
            }
            else
            {
                //
            }
        }

        private void undoAllchangesButt_Click(object sender, EventArgs e)
        {
            this.modifiableVehicles = new List<DtoChangeableItem>();
            foreach (IVehicle vehicle in this.vehicles)
            {
                this.modifiableVehicles.Add(new DtoChangeableItem { action = ActionPerformed.unchanged, vehicle = this.vehicleBuilder.export(vehicle), });
            }

            fullDisplayVehicleCollection();
        }

        private void updateCarButt_Click(object sender, EventArgs e)
        {

        }

        private void addCarButt_Click(object sender, EventArgs e)
        {

        }

        private void removeVehicleButt_Click(object sender, EventArgs e)
        {
            if (this.currentVehicle != -1)
            {
                if (this.currentVehicle < this.vehicleStorage.Count)
                {
                    this.modifiableVehicles[this.currentVehicle] = new DtoChangeableItem
                    {
                        action = ActionPerformed.erased,
                        vehicle = this.modifiableVehicles[this.currentVehicle].vehicle,
                    };
                }
                else
                {
                    this.modifiableVehicles.RemoveAt(this.currentVehicle);
                }

                this.fullDisplayVehicleCollection();
            }
        }

        private void goToFirstButt_Click(object sender, EventArgs e)
        {
            if (this.currentVehicle != -1)
            {
                this.currentVehicle = 0;
                this.fullDisplayVehicle(this.vehicleBuilder.export(this.vehicles.First()));
            }
            this.fullDisplayVehicle(this.modifiableVehicles[this.currentVehicle].vehicle);
        }

        private void goToPreviousButt_Click(object sender, EventArgs e)
        {
            if(this.currentVehicle != -1)
            {
                if (--this.currentVehicle < 0)
                {
                    this.currentVehicle = this.modifiableVehicles.Count - 1;
                }
                this.fullDisplayVehicle(this.modifiableVehicles[this.currentVehicle].vehicle);
            }
        }

        private void goToNextButt_Click(object sender, EventArgs e)
        {
            if (this.currentVehicle != -1)
            {
                if (++this.currentVehicle >= this.modifiableVehicles.Count)
                {
                    this.currentVehicle = 0;
                }
                this.fullDisplayVehicle(this.modifiableVehicles[this.currentVehicle].vehicle);
            }
        }

        private void goToLastButt_Click(object sender, EventArgs e)
        {
            if (this.currentVehicle != -1)
            {
                this.currentVehicle = this.modifiableVehicles.Count -1;
                this.fullDisplayVehicle(this.modifiableVehicles.Last().vehicle);
            }
            this.fullDisplayVehicle(this.modifiableVehicles[this.currentVehicle].vehicle);
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
            this.currentVehicle = this.carListView.SelectedIndices[0];
            fullDisplayVehicle(this.modifiableVehicles[this.currentVehicle].vehicle);
        }

        private void doorsView_ItemActivate(object sender, EventArgs e)
        {
            fullDisplayDoor(this.modifiableVehicles[this.currentVehicle].vehicle.Doors[this.doorsView.SelectedIndices[0]]);
        }

        private void wheelsView_ItemActivate(object sender, EventArgs e)
        {
            fullDisplayWheel(this.modifiableVehicles[this.currentVehicle].vehicle.Wheels[this.wheelsView.SelectedIndices[0]]);
        }

        private void fullDisplayVehicleCollection()
        {
            if (this.modifiableVehicles != null)
            {
                this.carListView.Items.Clear();
                foreach (DtoChangeableItem dtoChangeableItem in this.modifiableVehicles)
                {
                    this.carListView.Items.Add(dtoChangeableItem.vehicle.Enrollment.Serial + dtoChangeableItem.vehicle.Enrollment.Number.ToString());
                }
                fullDisplayVehicle(this.modifiableVehicles.First().vehicle);
            }
            else
            {
                this.carListView.Items.Clear();
            }
        }

        private void fullDisplayVehicle(VehicleDto vehicle)
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
            foreach (WheelDto wheel in vehicle.Wheels)
            {
                this.wheelsView.Items.Add("Wh" + wheel.Pressure.ToString());
            }
            fullDisplayWheel(vehicle.Wheels[0]);

            this.doorsView.Items.Clear();
            foreach (DoorDto door in vehicle.Doors)
            {
                this.doorsView.Items.Add("Dr" + door.IsOpen.ToString());
            }
            if (vehicle.Doors.Length > 0)
            {
                fullDisplayDoor(vehicle.Doors[0]);
            }
        }

        private void fullDisplayWheel(WheelDto wheel)
        {
            this.wheelPressureView.Text = wheel.Pressure.ToString();
        }

        private void fullDisplayDoor(DoorDto door)
        {
            this.isOpenedDoor.Checked = door.IsOpen;
        }
    }
}
