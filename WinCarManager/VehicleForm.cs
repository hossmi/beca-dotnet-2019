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
    public partial class VehicleForm : Form
    {
        private const string CONNECTION_STRING_KEY = "CarManagerConnectionString";
        private List<IVehicle> vehicles;
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;
        private readonly IEnrollmentProvider enrollmentProvider;
        private int position = 0;

        public VehicleForm()
        {
            InitializeComponent();
            this.connectionString = ConfigurationManager.AppSettings[CONNECTION_STRING_KEY];
            this.enrollmentProvider = new DefaultEnrollmentProvider();
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);
        }

        private void ButtonFirst_Click(object sender, EventArgs e)
        {
            if (this.vehicles.Count > 0)
            {
                this.position = 0;
                loadVehicle(this.vehicles[this.position]);
            }
        }

        private void VehicleForm_Load(object sender, EventArgs e)
        {
            this.ButtonFirst.Text = char.ConvertFromUtf32(0x2B70);
            this.ButtonPrev.Text = char.ConvertFromUtf32(0x2B60);
            this.ButtonNext.Text = char.ConvertFromUtf32(0x2B62);
            this.ButtonLast.Text = char.ConvertFromUtf32(0x2B72);

            loadVehicles();

        }

        private void loadVehicles()
        {
            IVehicleStorage databaseVehicleStorage =
                new SqlVehicleStorage(this.connectionString, this.vehicleBuilder);

            this.vehicles = databaseVehicleStorage.get().ToList();

            if (this.vehicles.Count>0)
                loadVehicle(this.vehicles[0]);
        }

        private void loadVehicle(IVehicle vehicle)
        {
            this.EnrollmentSerial.Text = vehicle.Enrollment.Serial;
            this.EnrollmentNumber.Text = vehicle.Enrollment.Number.ToString();

            this.Color.Text = ((CarColor)vehicle.Color).ToString();

            this.EngineHorsePower.Text = vehicle.Engine.HorsePower.ToString();
            this.EngineIsStarted.Text = vehicle.Engine.IsStarted.ToString();

            this.ListWheels.Items.Clear();
            this.ListDoors.Items.Clear();
            
            int i = 1;
            foreach (IWheel wheel in vehicle.Wheels)
            {
                this.ListWheels.Items.Add("Wheel "+i.ToString()+ " Pressure: " + wheel.Pressure.ToString());
                i++;
            }

            i = 1;
            foreach (IDoor door in vehicle.Doors)
            {
                this.ListDoors.Items.Add("Door "+i.ToString()+": " + door.IsOpen.ToString());
                i++;
            }
            this.LabelPosition.Text = (this.position + 1).ToString();
        }

        private void ButtonLast_Click(object sender, EventArgs e)
        {
            if (this.vehicles.Count > 0)
            {
                this.position = this.vehicles.Count-1;
                loadVehicle(this.vehicles[this.position]);
            }
        }

        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            if (this.vehicles.Count > 0)
            {
                this.position = this.position -1;
                if (this.position < 0)
                    this.position = 0;
                loadVehicle(this.vehicles[this.position]);
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if (this.vehicles.Count > 0)
            {
                this.position = this.position + 1;
                if (this.position > this.vehicles.Count-1 )
                    this.position = this.vehicles.Count - 1;
                loadVehicle(this.vehicles[this.position]);
            }
        }
    }
}
