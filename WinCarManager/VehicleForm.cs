using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;
using CarManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinCarManager
{
    public partial class VehicleForm : Form
    {
        private List<IVehicle> vehicles;
        private List<IEnrollment> enrollments;
        IVehicleStorage vehicleStorage;
        IEnrollmentProvider enrollmentProvider;
        private int position = 0;
        private bool refreshing;
        private bool refreshed;

        public VehicleForm(IVehicleStorage vehicleStorage, IEnrollmentProvider enrollmentProvider)
        {
            this.vehicleStorage = vehicleStorage;
            this.enrollmentProvider = enrollmentProvider;
            InitializeComponent();
            this.Color.DataSource = Enum.GetValues(typeof(CarColor));
        }

        private void VehicleForm_Load(object sender, EventArgs e)
        {
            this.ButtonFirst.Text = char.ConvertFromUtf32(0x2B70);
            this.ButtonPrev.Text = char.ConvertFromUtf32(0x2B60);
            this.ButtonNext.Text = char.ConvertFromUtf32(0x2B62);
            this.ButtonLast.Text = char.ConvertFromUtf32(0x2B72);

            RefreshEnrollments();
        }

        private void ButtonFirst_Click(object sender, EventArgs e)
        {
            if (this.EnrollmentsGridView.Rows.Count > 0)
            {
                this.position = 0;
                this.EnrollmentsGridView.Rows[this.position].Selected = true;
            }
        }

        private void ButtonLast_Click(object sender, EventArgs e)
        {
            if (this.EnrollmentsGridView.Rows.Count > 0)
            {
                this.position = this.EnrollmentsGridView.Rows.Count-1;
                this.EnrollmentsGridView.Rows[this.position].Selected = true;
            }
            
        }

        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            if (this.EnrollmentsGridView.Rows.Count > 0)
            {
                this.position = this.position -1;
                if (this.position < 0)
                    this.position = 0;
                this.EnrollmentsGridView.Rows[this.position].Selected = true;
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if (this.EnrollmentsGridView.Rows.Count > 0)
            {
                this.position = this.position + 1;
                if (this.position > this.EnrollmentsGridView.Rows.Count-1 )
                    this.position = this.EnrollmentsGridView.Rows.Count - 1;
                this.EnrollmentsGridView.Rows[this.position].Selected = true;
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            this.ButtonOK.Visible = true;
            this.ButtonCancel.Visible = true;

            this.ButtonAdd.Visible = false;
            this.ButtonModify.Visible = false;
            this.ButtonFirst.Visible = false;
            this.ButtonPrev.Visible = false;
            this.ButtonNext.Visible = false;
            this.ButtonLast.Visible = false;

            this.EnrollmentSerial.ReadOnly = false;
            this.EnrollmentSerial.Clear();
            this.EnrollmentNumber.ReadOnly = false;
            this.EnrollmentNumber.Clear();

            this.Color.Enabled = true;
            this.Color.SelectedItem = this.Color.Items[0];

            this.EngineHorsePower.ReadOnly = false;
            this.EngineHorsePower.Clear();
            this.EngineIsStarted.Enabled = true;
            this.EngineIsStarted.Checked = false;

            this.DoorGridView.ReadOnly = false;
            this.DoorGridView.AllowUserToAddRows = true;
            this.DoorGridView.Rows.Clear();

            this.WheelGridView.ReadOnly = false;
            this.WheelGridView.AllowUserToAddRows = true;
            this.WheelGridView.Rows.Clear();

        }

        private void ButtonModify_Click(object sender, EventArgs e)
        {
            this.ButtonOK.Visible = true;
            this.ButtonCancel.Visible = true;

            this.ButtonAdd.Visible = false;
            this.ButtonModify.Visible = false;
            this.ButtonFirst.Visible = false;
            this.ButtonPrev.Visible = false;
            this.ButtonNext.Visible = false;
            this.ButtonLast.Visible = false;

            this.Color.Enabled = true;
            this.EngineHorsePower.ReadOnly = false;
            this.EngineIsStarted.Enabled = true;
            this.DoorGridView.ReadOnly = false;
            this.DoorGridView.AllowUserToAddRows = false;
            this.WheelGridView.ReadOnly = false;
            this.WheelGridView.AllowUserToAddRows = false;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.ButtonOK.Visible = false;
            this.ButtonCancel.Visible = false;

            this.ButtonAdd.Visible = true;
            this.ButtonModify.Visible = true;
            this.ButtonFirst.Visible = true;
            this.ButtonPrev.Visible = true;
            this.ButtonNext.Visible = true;
            this.ButtonLast.Visible = true;

            this.EnrollmentSerial.ReadOnly = true;
            this.EnrollmentNumber.ReadOnly = true;
            this.Color.Enabled = false;
            this.EngineHorsePower.ReadOnly = true;
            this.EngineIsStarted.Enabled =false;
            this.DoorGridView.ReadOnly = true;
            this.DoorGridView.AllowUserToAddRows = false;
            this.WheelGridView.ReadOnly = true;
            this.WheelGridView.AllowUserToAddRows = false;

            LoadVehicle();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            SetVehicle();

            this.ButtonOK.Visible = false;
            this.ButtonCancel.Visible = false;

            this.ButtonAdd.Visible = true;
            this.ButtonModify.Visible = true;
            this.ButtonFirst.Visible = true;
            this.ButtonPrev.Visible = true;
            this.ButtonNext.Visible = true;
            this.ButtonLast.Visible = true;

            this.EnrollmentSerial.ReadOnly = true;
            this.EnrollmentNumber.ReadOnly = true;
            this.Color.Enabled = false;
            this.EngineHorsePower.ReadOnly = true;
            this.EngineIsStarted.Enabled = false;
            this.DoorGridView.ReadOnly = true;
            this.DoorGridView.AllowUserToAddRows = false;
            this.WheelGridView.ReadOnly = true;
            this.WheelGridView.AllowUserToAddRows = false;
        }

        private void EnrollmentsGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.EnrollmentsGridView.SelectedRows.Count > 0 && this.refreshing == false)
            {
                this.position = this.EnrollmentsGridView.SelectedRows[0].Index;
                LoadVehicle();
            }

            if (this.refreshing && this.EnrollmentsGridView.Rows.Count > this.position)
            {
                this.refreshing = false;
                this.EnrollmentsGridView.Rows[this.position].Selected = true;
                
            }
        }

        private void RefreshEnrollments()
        {
            this.refreshing = true;
            this.EnrollmentsGridView.Rows.Clear();
            this.enrollments = this.vehicleStorage.get().Keys.ToList();
            int i = 1;
            foreach (IEnrollment enrollment in this.enrollments)
            {
                this.refreshing = true;
                this.EnrollmentsGridView.Rows.Add(enrollment.ToString(), enrollment.Serial, enrollment.Number);
                this.EnrollmentsGridView.Rows[i - 1].HeaderCell.Value = i.ToString();
                i++;
            }
            this.EnrollmentsGridView.Rows[this.position].Selected = true;
            this.refreshing = false;
        }

        private void loadVehicle(IVehicle vehicle)
        {
            this.EnrollmentSerial.Text = vehicle.Enrollment.Serial;
            this.EnrollmentNumber.Text = vehicle.Enrollment.Number.ToString();

            this.Color.SelectedItem  = (CarColor)vehicle.Color;

            this.EngineHorsePower.Text = vehicle.Engine.HorsePower.ToString();
            this.EngineIsStarted.Checked = vehicle.Engine.IsStarted;

            this.DoorGridView.Rows.Clear();
            this.WheelGridView.Rows.Clear();
            
            int i = 1;
            foreach (IWheel wheel in vehicle.Wheels)
            {
                this.WheelGridView.Rows.Add(wheel.Pressure.ToString());
                this.WheelGridView.Rows[i - 1].HeaderCell.Value = i.ToString();
                i++;
            }

            i = 1;
            foreach (IDoor door in vehicle.Doors)
            {
                this.DoorGridView.Rows.Add(door.IsOpen.ToString());             
                this.DoorGridView.Rows[i - 1].HeaderCell.Value = i.ToString();
                i++;
            }
            this.LabelPosition.Text = (this.position + 1).ToString();
        }

        private void SetVehicle()
        {
            IVehicleBuilder builder = new VehicleBuilder(this.enrollmentProvider);
            VehicleDto vehicleDto = new VehicleDto();
            EngineDto engineDto = new EngineDto();
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            CarColor color;
            List<WheelDto> wheelsDto = new List<WheelDto>();
            List<DoorDto> doorsDto = new List<DoorDto>();

            enrollmentDto.Serial = this.EnrollmentSerial.Text;
            enrollmentDto.Number = Convert.ToInt32(this.EnrollmentNumber.Text);
            vehicleDto.Enrollment = enrollmentDto;
            Enum.TryParse<CarColor>(this.Color.SelectedValue.ToString(), out color);
            vehicleDto.Color = color;
            engineDto.HorsePower = Convert.ToInt32(this.EngineHorsePower.Text);
            engineDto.IsStarted = this.EngineIsStarted.Checked;
            vehicleDto.Engine = engineDto;

            foreach (DataGridViewRow row in this.WheelGridView.Rows)
            {
                WheelDto wheelDto = new WheelDto();
                wheelDto.Pressure = Convert.ToDouble(row.Cells[0].Value);
                if (wheelDto.Pressure > 1.0)
                    wheelsDto.Add(wheelDto);
            }
            vehicleDto.Wheels = wheelsDto.ToArray();

            foreach (DataGridViewRow row in this.DoorGridView.Rows)
            {
                DoorDto doorDto = new DoorDto();
                doorDto.IsOpen = Convert.ToBoolean(row.Cells[0].Value);
                doorsDto.Add(doorDto);
            }
            vehicleDto.Doors = doorsDto.ToArray();

            IVehicle vehicle = builder.import(vehicleDto);
            this.vehicleStorage.set(vehicle);

            RefreshEnrollments();
        }

        private void LoadVehicle()
        {
            if (this.EnrollmentsGridView.SelectedRows.Count > 0)
            {
                string serial = this.EnrollmentsGridView.SelectedCells[1].Value.ToString();
                int number = Convert.ToInt32(this.EnrollmentsGridView.SelectedCells[2].Value);
                IVehicleBuilder builder = new VehicleBuilder(this.enrollmentProvider);
                IVehicle vehicle = this.vehicleStorage.get().whereEnrollmentIs(builder.import(serial, number)).Single();

                loadVehicle(vehicle);
            }
        }
    }
}
