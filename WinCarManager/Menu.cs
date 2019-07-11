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
    public partial class Menu : Base
    {
        private readonly IVehicleStorage vehicleStorage;
        private readonly IEnrollmentProvider enrollmentProvider;
        public Menu(IVehicleStorage vehicleStorage, IEnrollmentProvider enrollmentProvider)
        {
            this.vehicleStorage = vehicleStorage;
            this.enrollmentProvider = enrollmentProvider;
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }
    }
}
