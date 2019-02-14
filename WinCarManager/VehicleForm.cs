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
            //firstButton_Click.Text = char.ConvertFromUtf32(0x2B70);
            //prevButton_Click.Text = char.ConvertFromUtf32(0x2B60);
            //nextButton_Click.Text = char.ConvertFromUtf32(0x2B62);
            //lastButton_Click.Text = char.ConvertFromUtf32(0x2B72);
        }

        private void firstButton_Click(object sender, EventArgs e)
        {

        }

        private void prevButton_Click(object sender, EventArgs e)
        {

        }

        private void nextButton_Click(object sender, EventArgs e)
        {

        }

        private void lastButton_Click(object sender, EventArgs e)
        {

        }
    }
}
