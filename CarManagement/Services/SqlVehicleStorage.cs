using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core.Models;
using CarManagement.Core.Models.DTOs;
using CarManagement.Core.Services;

namespace CarManagement.Services
{
    public class SqlVehicleStorage : IVehicleStorage
    {
        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count { get; }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IVehicle get(IEnrollment enrollment)
        {
            VehicleDto vehicleDto = new VehicleDto();
            int number = enrollment.Number;
            string serial = enrollment.Serial;
            String getFromEnrollment = "SELECT  id,serial,enrollment FROM enrollment WHERE serial = " + serial + ", number = " + number.ToString();
            SqlConnection connection = new SqlConnection(this.connectionString);
            SqlCommand command = new SqlCommand(getFromEnrollment, connection);
            //------------------------------------------------------------------------------------------
            connection.Open();
            //------------------------------------------------------------------------------------------
            IDataReader reader = command.ExecuteReader();
            reader.Read();
            int vehicleID = (int)reader["id"];
            String getFromVehicle = "SELECT  color,engineHorsePower,engineIsStarted FROM wheel WHERE id =" + vehicleID.ToString();
            reader.Read();
            command = new SqlCommand(getFromVehicle, connection);
            reader = command.ExecuteReader();
            reader.Read();
            CarColor vehicleColor = (CarColor)(int)reader["color"];
            int vehicleHorsePower = (int)reader["engineHorsePower"];
            bool vehicleIsStarted = (bool)reader["engineIsStarted"];
            //------------------------------------------------------------------------------------------
            String getFromWheel = "SELECT  pressure FROM wheel WHERE id =" + vehicleID.ToString();
            command = new SqlCommand(getFromWheel, connection);
            reader = command.ExecuteReader();
            float[] vehiclePressure = null;
            for (int i = 0; reader.Read(); i++)
            {
                vehiclePressure[i] = (float)(reader["pressure"]);
            }
            int counter = 0;
            foreach (WheelDto wheels in vehicleDto.Wheels)
            {
                wheels.Pressure = vehiclePressure[counter];
            }
            //------------------------------------------------------------------------------------------
            String getFromDoor = "SELECT  isOpen FROM door WHERE id =" + vehicleID.ToString();
            command = new SqlCommand(getFromDoor, connection);
            reader = command.ExecuteReader();
            reader.Read();
            bool[] vehicleIsOpen = null;
            for (int i = 0; reader.Read(); i++)
            {
                vehicleIsOpen[i] = (bool)reader["isOpen"];
            }
            counter = 0;
            foreach (DoorDto doors in vehicleDto.Doors)
            {
                doors.IsOpen = vehicleIsOpen[counter];
            }
            //------------------------------------------------------------------------------------------
            vehicleDto.Color = vehicleColor;
            vehicleDto.Engine.HorsePower = vehicleHorsePower;
            vehicleDto.Engine.IsStarted = vehicleIsStarted;
            vehicleDto.Enrollment.Number = number;
            vehicleDto.Enrollment.Serial = serial;
            //------------------------------------------------------------------------------------------
            connection.Close();
            //------------------------------------------------------------------------------------------
            return this.vehicleBuilder.import(vehicleDto);


        }

        public IEnumerable<IVehicle> getAll()
        {

            string getAllDB = "SELECT id, serial, number, color, engineHorsePower, engineIsStarted, pressure, IsOpen " +
                "FROM enrollment, vehicle, wheel, door";
            SqlConnection connection = new SqlConnection(this.connectionString);
            SqlCommand command = new SqlCommand(getAllDB, connection);
            //------------------------------------------------------------------------------------------
            connection.Open();
            //------------------------------------------------------------------------------------------

            IDataReader reader = command.ExecuteReader();
            reader.Read();

            //------------------------------------------------------------------------------------------
            connection.Close();
            //------------------------------------------------------------------------------------------
            
            int[] id = null;
            string[] serial = null;
            int[] number = null;
            CarColor[] color = null;
            int[] horsePower = null;
            bool[] isStarted  = null;
            for (int i = 0; reader.Read(); i++)
            {
                id[i] = Convert.ToInt32(reader["id"]);
                serial[i] = Convert.ToString(reader["serial"]);
                number[i] = Convert.ToInt32(reader["number"]);
                color[i] = (CarColor)(Convert.ToInt32(reader["color"]));
                horsePower[i] = Convert.ToInt32(reader["engineHorsePower"]);
                isStarted [i] = Convert.ToBoolean(reader["engineHorsePower"]);
            }





        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}
