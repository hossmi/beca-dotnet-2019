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
        private int currentDoor;
        private int currentWheel;
        List<DtoChangeableItem> modifiableVehicles;

        public VehicleDisplayerWin( IEnrollmentProvider enrollmentProvider, IVehicleStorage vehicleStorage)
        {
            InitializeComponent();

            this.vehicles = new List<IVehicle>();

            this.connectionString = ConfigurationManager.AppSettings["CarManagerConnectionString"];

            this.enrollmentProvider = enrollmentProvider;
            this.vehicleStorage = vehicleStorage;
            this.vehicleBuilder = new VehicleBuilder(this.enrollmentProvider);

            this.currentVehicle = -1;
            this.currentDoor = -1;
            this.currentWheel = -1;
            this.modifiableVehicles = new List<DtoChangeableItem>();

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
            this.currentVehicle = -1;
            this.currentDoor = -1;
            this.currentWheel = -1;

            this.carColorView.Text = "";
            this.enrollmentView.Text = "";
            this.horsePowerEngineView.Text = "";
            this.startedEngineView.Checked = false;
            this.doorsView.Clear();
            this.isOpenedDoorView.Checked = false;
            this.wheelsView.Clear();
            this.wheelPressureView.Text = "";
        }
        private void exitSearchButt_Click(object sender, EventArgs e)
        {
            this.vehicles = new List<IVehicle>();
            this.modifiableVehicles = new List<DtoChangeableItem>();
            this.currentVehicle = -1;
            this.currentDoor = -1;
            this.currentWheel = -1;
            this.fullDisplayVehicleCollection();
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
                    this.fullDisplayVehicle(this.modifiableVehicles[this.currentVehicle].vehicle);
                }
                else
                {
                    this.modifiableVehicles.RemoveAt(this.currentVehicle);
                }
            }
            this.fullDisplayVehicleCollection();
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

        private void saveChangesButt_Click(object sender, EventArgs e)
        {
            foreach (DtoChangeableItem dtoChangeable in this.modifiableVehicles)
            {
                switch (dtoChangeable.action)
                {
                    case ActionPerformed.unchanged:
                        break;
                    case ActionPerformed.changed:
                        this.vehicleStorage.set( this.vehicleBuilder.import(dtoChangeable.vehicle) );
                        break;
                    case ActionPerformed.erased:
                        this.vehicleStorage.remove( 
                            this.vehicleBuilder.import(
                                dtoChangeable.vehicle.Enrollment.Serial,
                                dtoChangeable.vehicle.Enrollment.Number
                            ) 
                        );
                        break;
                    case ActionPerformed.added:
                        this.vehicleStorage.set(this.vehicleBuilder.import(dtoChangeable.vehicle));
                        break;
                    default:
                        break;
                }
                
            }

            this.fullDisplayVehicleCollection();
        }
        private void addCarButt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.enrollmentView.Text.Trim()) == false  & this.currentVehicle != -1)
            {
                string[] enrollmentParam = this.enrollmentView.Text.Split('-');
                string serial = enrollmentParam[0].Trim();
                if (enrollmentParam.Length > 1)
                {
                    if (int.TryParse(enrollmentParam[1].Trim(), out int parsedNumber))
                    {
                        if (this.modifiableVehicles.All(dtoChangeableItem => dtoChangeableItem.vehicle.Enrollment.Serial != serial
                            | dtoChangeableItem.vehicle.Enrollment.Number != parsedNumber))
                        {
                            int engineHorsePower = this.modifiableVehicles[this.currentVehicle].vehicle.Engine.HorsePower;
                            CarColor carColor = this.modifiableVehicles[this.currentVehicle].vehicle.Color;

                            int.TryParse(this.horsePowerEngineView.Text, out engineHorsePower);
                            Enum.TryParse<CarColor>(this.carColorView.Text, out carColor);

                            List<DoorDto> doorDtos = new List<DoorDto>();
                            foreach (DoorDto doorDto in this.modifiableVehicles[this.currentVehicle].vehicle.Doors)
                            {
                                doorDtos.Add(new DoorDto { IsOpen = doorDto.IsOpen, });
                            }

                            doorDtos[this.currentDoor].IsOpen = this.isOpenedDoorView.Checked;

                            List<WheelDto> wheelDtos = new List<WheelDto>();
                            foreach (WheelDto wheelDto in this.modifiableVehicles[this.currentVehicle].vehicle.Wheels)
                            {
                                wheelDtos.Add(new WheelDto { Pressure = wheelDto.Pressure, });
                            }
                            if (double.TryParse(this.wheelPressureView.Text, out double pressure))
                            {
                                wheelDtos[this.currentWheel].Pressure = pressure;
                            }

                            this.modifiableVehicles
                            .Add(
                                new DtoChangeableItem
                                {
                                    action = ActionPerformed.added,
                                    vehicle = new VehicleDto
                                    {
                                        Engine = new EngineDto
                                        {
                                            HorsePower = engineHorsePower,
                                            IsStarted = this.startedEngineView.Checked,
                                        },
                                        Enrollment = new EnrollmentDto
                                        {
                                            Serial = serial,
                                            Number = parsedNumber,
                                        },
                                        Color = carColor,
                                        Doors = doorDtos.ToArray(),
                                        Wheels = wheelDtos.ToArray(),
                                    },
                                }
                            );
                        }
                    }
                }
            }

            fullDisplayVehicleCollection();
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
                this.fullDisplayVehicle(this.modifiableVehicles.First().vehicle);
            }
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
            if (Enum.TryParse<CarColor>(this.carColorView.Text, out CarColor parsedCarColor))
            {
                if (this.modifiableVehicles[this.currentVehicle].vehicle.Color != parsedCarColor)
                {
                    this.modifiableVehicles[this.currentVehicle].vehicle.Color = parsedCarColor;
                    this.flagCurrentVehicleAsChanged();
                }
            }

            if (int.TryParse(this.horsePowerEngineView.Text, out int parsedHorsePower))
            {
                if (this.modifiableVehicles[this.currentVehicle].vehicle.Engine.HorsePower != parsedHorsePower)
                {
                    this.modifiableVehicles[this.currentVehicle].vehicle.Engine.HorsePower = parsedHorsePower;
                    this.flagCurrentVehicleAsChanged();
                }
            }

            if (bool.TryParse(this.startedEngineView.Text, out bool parsedStartedEngine))
            {
                if (this.modifiableVehicles[this.currentVehicle].vehicle.Engine.IsStarted != parsedStartedEngine)
                {
                    this.modifiableVehicles[this.currentVehicle].vehicle.Engine.IsStarted = parsedStartedEngine;
                    this.flagCurrentVehicleAsChanged();
                }
            }

            this.currentVehicle = this.carListView.SelectedIndices[0];
            fullDisplayVehicle(this.modifiableVehicles[this.currentVehicle].vehicle);
        }
        private void doorsView_ItemActivate(object sender, EventArgs e)
        {
            if (bool.TryParse(this.wheelPressureView.Text, out bool parsedOpenedDoor))
            {
                if (this.modifiableVehicles[this.currentVehicle].vehicle.Doors[this.currentWheel].IsOpen != parsedOpenedDoor)
                {
                    this.modifiableVehicles[this.currentVehicle].vehicle.Doors[this.currentWheel].IsOpen = parsedOpenedDoor;
                    this.flagCurrentVehicleAsChanged();
                }
            }

            this.currentDoor = this.doorsView.SelectedIndices[0];
            fullDisplayDoor(this.modifiableVehicles[this.currentVehicle].vehicle.Doors[this.currentDoor]);
        }
        private void wheelsView_ItemActivate(object sender, EventArgs e)
        {
            if (double.TryParse(this.wheelPressureView.Text, out double parsedWheelPressure))
            {
                if (this.modifiableVehicles[this.currentVehicle].vehicle.Wheels[this.currentWheel].Pressure != parsedWheelPressure)
                {
                    this.modifiableVehicles[this.currentVehicle].vehicle.Wheels[this.currentWheel].Pressure = parsedWheelPressure;

                    this.flagCurrentVehicleAsChanged();
                }
            }

            this.currentWheel = this.wheelsView.SelectedIndices[0];
            fullDisplayWheel(this.modifiableVehicles[this.currentVehicle].vehicle.Wheels[this.currentWheel]);
        }

        private void fullDisplayVehicleCollection()
        {
            if (this.modifiableVehicles.Count > 0)
            {
                this.carListView.Items.Clear();
                foreach (DtoChangeableItem dtoChangeableItem in this.modifiableVehicles)
                {
                    this.carListView.Items.Add(dtoChangeableItem.vehicle.Enrollment.Serial + dtoChangeableItem.vehicle.Enrollment.Number.ToString());
                    if (dtoChangeableItem.action == ActionPerformed.erased)
                    {
                        this.carListView.Items[this.carListView.Items.Count - 1].BackColor = Color.Red;
                    }
                    else if (dtoChangeableItem.action == ActionPerformed.added)
                    {
                        this.carListView.Items[this.carListView.Items.Count - 1].BackColor = Color.Green;
                    }
                    else
                    {
                        this.carListView.Items[this.carListView.Items.Count - 1].BackColor = Color.Transparent;
                    }
                }
                this.currentVehicle = 0;
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
            this.enrollmentView.Text = vehicle.Enrollment.Serial + "-" + vehicle.Enrollment.Number.ToString();
            this.horsePowerEngineView.Text = vehicle.Engine.HorsePower.ToString();
            this.startedEngineView.Checked = vehicle.Engine.IsStarted;

            this.wheelsView.Items.Clear();
            foreach (WheelDto wheel in vehicle.Wheels)
            {
                this.wheelsView.Items.Add("Wh" + wheel.Pressure.ToString());
            }
            this.currentWheel = 0;
            fullDisplayWheel(vehicle.Wheels[0]);

            this.doorsView.Items.Clear();
            foreach (DoorDto door in vehicle.Doors)
            {
                this.doorsView.Items.Add("Dr" + door.IsOpen.ToString());
            }
            this.currentDoor = -1;
            if (vehicle.Doors.Length > 0)
            {
                fullDisplayDoor(vehicle.Doors[0]);
                this.currentDoor = 0;
            }
        }
        private void fullDisplayWheel(WheelDto wheel)
        {
            this.wheelPressureView.Text = wheel.Pressure.ToString();
        }
        private void fullDisplayDoor(DoorDto door)
        {
            this.isOpenedDoorView.Checked = door.IsOpen;
        }

        private void flagCurrentVehicleAsChanged()
        {
            if (this.modifiableVehicles[this.currentVehicle].action != ActionPerformed.erased
                & this.modifiableVehicles[this.currentVehicle].action != ActionPerformed.added)
            {
                this.modifiableVehicles[this.currentVehicle] =
                    new DtoChangeableItem
                    {
                        action = ActionPerformed.changed,
                        vehicle = this.modifiableVehicles[this.currentVehicle].vehicle,
                    };

                this.carListView.Items[this.currentVehicle].BackColor = Color.Yellow;
            }
        }
    }
}
