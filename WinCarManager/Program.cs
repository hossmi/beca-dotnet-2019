using System;
using System.Configuration;
using System.Windows.Forms;
using CarManagement.Core.Services;
using CarManagement.Services;

namespace WinCarManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string connectionString = ConfigurationManager.AppSettings["CarManagerConnectionString"];
            IEnrollmentProvider enrollmentProvider = new DefaultEnrollmentProvider();
            IVehicleBuilder vehicleBuilder = new VehicleBuilder(enrollmentProvider);
            IVehicleStorage vehicleStorage = new SqlVehicleStorage(connectionString, vehicleBuilder);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(vehicleStorage, enrollmentProvider));
        }
    }
}
