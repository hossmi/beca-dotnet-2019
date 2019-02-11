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

        public int Count {
            get
            {
                SqlConnection con = conOpen();
                String query;
                query = "SELECT count(*) AS 'Count' FROM vehicle";
                SqlCommand sentence = new SqlCommand(query, con);
                SqlDataReader reader = sentence.ExecuteReader();
                reader.Read();
                int count = Convert.ToInt32(reader["Count"]);
                return count;
            }
        }

        public void clear()
        {
            SqlConnection con = conOpen();
            String query;
            query = "USE Carmanagement;" +
                "DELETE FROM door;" +
                "DELETE FROM wheel;" + 
                "DELETE FROM vehicle;" + 
                "DELETE FROM enrollment;";
            SqlCommand sentence = new SqlCommand(query, con);
            sentence.ExecuteNonQuery();
        }

        public void Dispose()
        {
            SqlConnection con;
            con = new SqlConnection(this.connectionString);
            con.Dispose();
        }

        public IVehicle get(IEnrollment enrollment)
        {
            SqlConnection con = conOpen();
            string serial = enrollment.Serial;
            int number = enrollment.Number;
            String query;
            query = "USE Carmanagement;" +
                "SELECT id FROM enrollment" +
                "WHERE serial = " + serial + "AND number = " + number;
            SqlCommand sentence = new SqlCommand(query, con);
            SqlDataReader reader = sentence.ExecuteReader();
            VehicleDto vehicleDto = new VehicleDto();
            reader.Read();
            EnrollmentDto enrollmentDto = new EnrollmentDto();
            EngineDto engineDto = new EngineDto();
            List<WheelDto> wheelsDto = new List<WheelDto>();
            List<DoorDto> doorsDto = new List<DoorDto>();
            int id = (int)reader["id"];
            enrollmentDto.Serial = serial;
            enrollmentDto.Number = number;

            String query2 = "SELECT color, engineHorsePower, engineIsStarted " +
                "FROM vehicle " +
                "WHERE enrollmentId =" + id;
            SqlCommand sentence2 = new SqlCommand(query2, con);
            SqlDataReader reader2 = sentence2.ExecuteReader();
            reader2.Read();

            CarColor color = (CarColor)Enum.Parse(typeof(CarColor), reader2["color"].ToString());
            engineDto.HorsePower = Convert.ToInt32(reader2["engineHorsePower"]);
            engineDto.IsStarted = Convert.ToBoolean(reader2["engineIsStarted"]);
            reader2.Close();

            query2 = "SELECT pressure FROM wheel WHERE vehicleId =" + id;

            sentence2 = new SqlCommand(query2, con);
            reader2 = sentence2.ExecuteReader();
            while (reader2.Read())
            {
                WheelDto wheelDto = new WheelDto();
                wheelDto.Pressure = Convert.ToInt32(reader2["pressure"]);
                wheelsDto.Add(wheelDto);
            }
            reader2.Close();
            query2 = "SELECT isOpen FROM door WHERE vehicleId =" + id;
            sentence2 = new SqlCommand(query2, con);
            reader2 = sentence2.ExecuteReader();
            while (reader2.Read())
            {
                DoorDto doorDto = new DoorDto();
                doorDto.IsOpen = Convert.ToBoolean(reader2["isOpen"]);
                doorsDto.Add(doorDto);
            }
            reader2.Close();
            vehicleDto.Color = color;
            vehicleDto.Doors = doorsDto.ToArray();
            vehicleDto.Wheels = wheelsDto.ToArray();
            vehicleDto.Engine = engineDto;
            vehicleDto.Enrollment = enrollmentDto;
            IVehicle vehicle_get = this.vehicleBuilder.import(vehicleDto);
            return vehicle_get;
        }

        private SqlConnection conOpen()
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            return con;
        }

        public IEnumerable<IVehicle> getAll()
        {
            List<IVehicle> lVehicle = new List<IVehicle>();

            SqlConnection con;
            con = new SqlConnection(this.connectionString);
            con.Open();
            string query = "USE Carmanagement;" +
                "SELECT * FROM enrollment";
            SqlCommand sentence = new SqlCommand(query, con);
            SqlDataReader reader = sentence.ExecuteReader();
            VehicleDto vehicleDto = new VehicleDto();
            while (reader.Read())
            {
                EnrollmentDto enrollmentDto = new EnrollmentDto();
                EngineDto engineDto = new EngineDto();
                List<WheelDto> wheelsDto = new List<WheelDto>();
                List<DoorDto> doorsDto = new List<DoorDto>();
                int id = (int)reader["id"];
                enrollmentDto.Serial = (string)reader["serial"];
                enrollmentDto.Number = Convert.ToInt32(reader["number"]);

                string query2 = "SELECT color, engineHorsePower, engineIsStarted " +
                    "FROM vehicle " +
                    "WHERE enrollmentId =" + id;
                SqlCommand sentence2 = new SqlCommand(query2, con);
                SqlDataReader reader2 = sentence2.ExecuteReader();
                reader2.Read();

                CarColor color = (CarColor)Enum.Parse(typeof(CarColor), reader2["color"].ToString());
                engineDto.HorsePower = Convert.ToInt32(reader2["engineHorsePower"]);
                engineDto.IsStarted = Convert.ToBoolean(reader2["engineIsStarted"]);
                reader2.Close();

                query2 = "SELECT pressure FROM wheel WHERE vehicleId =" + id;

                sentence2 = new SqlCommand(query2, con);
                reader2 = sentence2.ExecuteReader();
                while (reader2.Read())
                {
                    WheelDto wheelDto = new WheelDto();
                    wheelDto.Pressure = Convert.ToInt32(reader2["pressure"]);
                    wheelsDto.Add(wheelDto);
                }
                reader2.Close();
                query2 = "SELECT isOpen FROM door WHERE vehicleId =" + id;
                sentence2 = new SqlCommand(query2, con);
                reader2 = sentence2.ExecuteReader();
                while (reader2.Read())
                {
                    DoorDto doorDto = new DoorDto();
                    doorDto.IsOpen = Convert.ToBoolean(reader2["isOpen"]);
                    doorsDto.Add(doorDto);
                }
                reader2.Close();
                vehicleDto.Color = color;
                vehicleDto.Doors = doorsDto.ToArray();
                vehicleDto.Wheels = wheelsDto.ToArray();
                vehicleDto.Engine = engineDto;
                vehicleDto.Enrollment = enrollmentDto;
                IVehicle vehicle_get = this.vehicleBuilder.import(vehicleDto);
                lVehicle.Add(vehicle_get);
            }
            reader.Close();
            con.Close();
            return lVehicle;
        }

        public void set(IVehicle vehicle)
        {
            string serial = vehicle.Enrollment.Serial.ToString();
            int number = vehicle.Enrollment.Number;
            SqlConnection con;
            con = new SqlConnection(this.connectionString);
            con.Open();
            string query = "USE [CarManagement]" +
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

            int color_int = (int)vehicle.Color;
            int engineIsStarted = vehicle.Engine.IsStarted ? 1 : 0;

            int engineHorsePower = vehicle.Engine.HorsePower;
            query = "INSERT INTO [vehicle] (enrollmentId, color, engineHorsePower, engineIsStarted) " +
                "VALUES (" + ressult + ", " + color_int + ", " + engineHorsePower + ", " + engineIsStarted + ")";
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
