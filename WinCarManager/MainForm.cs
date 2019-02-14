using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarManagement.Core.Services;

namespace WinCarManager
{
    public partial class MainForm : Form
    {
        private readonly IVehicleStorage vehicleStorage;

        public MainForm(IVehicleStorage vehicleStorage)
        {
            this.vehicleStorage = vehicleStorage;
            InitializeComponent();
        }

    }
}
