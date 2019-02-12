using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarManagement.Core;
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

        private SqlConnection conOpen()
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            return con;
        }
        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        public void set(IVehicle vehicle)
        {
            string serial = vehicle.Enrollment.Serial.ToString();
            int number = vehicle.Enrollment.Number;
            SqlConnection con;
            con = new SqlConnection(this.connectionString);
            con.Open();
            string query;
            query = "USE CarManagement;" +
                "SELECT count(*) AS 'count' FROM enrollment " +
                "WHERE serial = '" + serial + "' AND number = " + number;
            SqlCommand sentence = new SqlCommand(query, con);
            SqlDataReader reader = sentence.ExecuteReader();
            reader.Read();
            int count = Convert.ToInt32(reader["count"]);
            if (count > 0)
            {
                query = "SELECT id " +
                    "FROM [enrollment]" +
                    "WHERE (serial = '" + serial + "' AND number = " + number + ")";
                sentence = new SqlCommand(query, con);
                reader = sentence.ExecuteReader();
                reader.Read();
                int ressult = (int)reader["id"];
                reader.Close();

                int color_int = (int)vehicle.Color;
                int engineIsStarted = vehicle.Engine.IsStarted ? 1 : 0;

                int engineHorsePower = vehicle.Engine.HorsePower;
                query = "UPDATE vehicle" +
                    "SET enrollmentId = " + ressult + ", color = " + color_int + ", engineHorsePower = " + engineHorsePower + ", engineIsStarted = " + engineIsStarted;
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();

                IWheel[] wheels = vehicle.Wheels;
                foreach (IWheel wheel in wheels)
                {
                    double pressure = wheel.Pressure;
                    query = "UPDATE wheel" +
                        "SET vehicleId = " + ressult + ", pressure = " + pressure;
                    sentence = new SqlCommand(query, con);
                    sentence.ExecuteNonQuery();
                }

                IDoor[] doors = vehicle.Doors;
                foreach (IDoor door in doors)
                {
                    string isOpen = (door.IsOpen ? 1 : 0).ToString();
                    query = "UPDATE door" +
                        "SET vehicleId = " + ressult + ", isOpen = '" + isOpen + "'";
                    sentence = new SqlCommand(query, con);
                    sentence.ExecuteNonQuery();

                }
                con.Close();
            }
            else
            {
                query = "USE [CarManagement]" +
                "INSERT INTO [enrollment] (serial, number) " +
                "VALUES ('" + serial + "', " + number + ")";
                sentence = new SqlCommand(query, con);
                sentence.ExecuteNonQuery();

                query = "SELECT id " +
                    "FROM [enrollment]" +
                    "WHERE (serial = '" + serial + "' AND number = " + number + ")";
                sentence = new SqlCommand(query, con);
                reader = sentence.ExecuteReader();
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
                    string pressure = wheel.Pressure.ToString().Replace(",", ".");
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

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private CarColor color;
            private bool engineIsStarted;
            private IEnrollment enrollment;
            private string enrollmentSerial;
            private int horsePower;
            private int min;
            private int max;
            private List<string> queryParts;
            private string query;
            private string queryId = "USE CarManagement; " +
                "SELECT id " +
                "FROM enrollment ";
            private string queryHeadvehicle = "USE CarManagement; " +
                "SELECT e.serial AS 'serial', " +
                "e.number AS 'number', " +
                "v.engineHorsePower AS 'horsePower', " +
                "v.engineIsStarted AS 'isStarted', " +
                "v.color AS 'carColor', " +
                "d.isOpen AS 'isOpen', " +
                "w.pressure AS 'pressure' " +
                "FROM enrollment e " +
                "INNER JOIN vehicle v " +
                "ON e.id = v.enrollmentId " +
                "INNER JOIN door d " +
                "ON e.id = d.vehicleId " +
                "INNER JOIN wheel w " +
                "ON e.id = w.vehicleId ";
            private string queryHeaddoorwheel = "USE CarManagement; " +
                "SELECT e.serial AS 'serial', " +
                "e.number AS 'number', " +
                "v.engineHorsePower AS 'horsePower', " +
                "v.engineIsStarted AS 'isStarted', " +
                "v.color AS 'carColor', " +
                "d.isOpen AS 'isOpen', " +
                "w.pressure AS 'pressure' " +
                "FROM enrollment e " +
                "INNER JOIN vehicle v " +
                "ON e.id = v.enrollmentId " +
                "INNER JOIN door d " +
                "ON e.id = d.vehicleId " +
                "INNER JOIN wheel w " +
                "ON e.id = w.vehicleId ";

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.queryParts = new List<string>();
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isTrue(color != null);
                this.queryParts.Add("carColor = " + Convert.ToInt32(color) + " ");
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isTrue(started != null);
                this.queryParts.Add("isStarted = " + Convert.ToInt16(started) + " ");
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                //this.queryParts.Add("number = " + enrollment.Number + " ");
                this.enrollment = enrollment;
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.enrollmentSerial = serial;
                //this.queryParts.Add("serial = " + serial + " ");
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isTrue(horsePower != null);
                this.queryParts.Add("horsePower = " + horsePower + " ");
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min != null);
                Asserts.isTrue(max != null);
                this.queryParts.Add("horsePower BETWEEN " + min + " AND " + max + " ");
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                /*foreach (string partcondition in this.queryParts)
                {
                    this.query += " AND " + partcondition;
                }
                this.query = this.query.Remove(0, 4);
                this.query = " WHERE " + this.query;
                this.query = this.queryHead + this.query;*/
                this.queryId += "WHERE serial = '" + this.enrollmentSerial + "'";

                SqlConnection con = new SqlConnection(this.connectionString);
                con.Open();
                SqlCommand sentence = new SqlCommand(this.queryId, con);
                SqlDataReader reader = sentence.ExecuteReader();
                VehicleDto vehicleDto = new VehicleDto();
                reader.Read();
                EnrollmentDto enrollmentDto = new EnrollmentDto();
                EngineDto engineDto = new EngineDto();
                List<WheelDto> wheelsDto = new List<WheelDto>();
                List<DoorDto> doorsDto = new List<DoorDto>();
                int id = (int)reader["id"];
                enrollmentDto.Serial = this.enrollmentSerial;
                enrollmentDto.Number = this.enrollment.Number;

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
                yield return vehicle_get;
            }

        }
    }
}
