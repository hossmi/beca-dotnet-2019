using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public IEnumerable<IVehicle> getAll()
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            String query;
            query = "SELECT *  " +
                "FROM enrollment";
            SqlCommand sentence = new SqlCommand(query, con);
            SqlDataReader reader = sentence.ExecuteReader();
            while (reader.Read())
            {
                EnrollmentDto enrollmentDto = new EnrollmentDto();
                EngineDto engineDto = new EngineDto();
                WheelDto wheelDto = new WheelDto();
                DoorDto doorDto = new DoorDto();
                VehicleDto vehicleDto = new VehicleDto();
                int id = (int)reader["e.id"];
                String query2 = "SELECT color, engineHorsePower, engineIsStarted " +
                    "FROM vehicle " +
                    "WHERE vehicleId =" + id;
                SqlCommand sentence2 = new SqlCommand(query2, con);
                SqlDataReader reader2 = sentence2.ExecuteReader();
                reader2.Read();
                int color_int;
                color_int = (int)reader2["color"];
                CarColor color = new CarColor();
                color = color_int;
                //https://stackoverflow.com/questions/29482/cast-int-to-enum-in-c-sharp




                enrollmentDto.Serial = (string)reader["e.serial"];
                enrollmentDto.Number = (int)reader["e.number"];
                engineDto.HorsePower = (int)reader["v.engineHorsePower"];
                int isStarted_int = (int)reader["v.engineIsStarted"];
                bool isStarted = false;
                if (isStarted_int == 1)
                {
                    isStarted = true;
                }
                engineDto.IsStarted = isStarted;
                wheelDto.Pressure = (int)reader["w.pressure"];


            }


            throw new NotImplementedException();
        }

        public void set(IVehicle vehicle)
        {
            string serial = vehicle.Enrollment.Serial.ToString();
            int number = vehicle.Enrollment.Number;
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            String query;
            query = "USE [CarManagement]" +
                "INSERT INTO [enrollment] (serial, number) " +
                "VALUES ('" + serial + "', " + number + ")";
            SqlCommand sentence = new SqlCommand(query, con);
            sentence.ExecuteNonQuery();

            query = "SELECT id " +
                "FROM [enrollment]" +
                "WHERE (serial = '" + serial + "' AND number = " + number + ")";
            sentence = new SqlCommand(query, con);
            SqlDataReader reader = sentence.ExecuteReader();
            reader.Read();
            int ressult = (int)reader["id"];
            reader.Close();

            int color = (int)vehicle.Color;
            int engineIsStarted = vehicle.Engine.IsStarted ? 1 : 0;

            int engineHorsePower = vehicle.Engine.HorsePower;
            query = "INSERT INTO [vehicle] (enrollmentId, color, engineHorsePower, engineIsStarted) " +
                "VALUES (" + ressult + ", " + color + ", " + engineHorsePower + ", " + engineIsStarted + ")";
            sentence = new SqlCommand(query, con);
            sentence.ExecuteNonQuery();

            IWheel[] wheels = vehicle.Wheels;
            foreach (IWheel wheel in wheels)
            {
                double pressure = wheel.Pressure;
                query = "INSERT INTO wheel (vehicleId, pressure) " +
                    "VALUES (" + ressult + ", " + pressure + ")";
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();
            }

            IDoor[] doors = vehicle.Doors;
            foreach (IDoor door in doors)
            {
                string isOpen = (door.IsOpen ? 1 : 0).ToString();
                query = "INSERT INTO door (vehicleId, isOpen) " +
                    "VALUES (" + ressult + ", " + isOpen + ")";
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();

            }
            con.Close();
        }
    }
}
