using System;
using System.Collections;
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
            List<IVehicle> lVehicle = new List<IVehicle>();
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            String query;
            query = "USE Carmanagement;" +
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
                IVehicle vehicle = this.vehicleBuilder.import(vehicleDto);
                lVehicle.Add(vehicle);
            }
            reader.Close();
            return lVehicle;
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

        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        public void Set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private CarColor color;
            private bool colorHasValue;

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                this.color = color;
                this.colorHasValue = true;
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                throw new NotImplementedException();
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                throw new NotImplementedException();
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                throw new NotImplementedException();
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                throw new NotImplementedException();
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                throw new NotImplementedException();
            }

        }
    }
}