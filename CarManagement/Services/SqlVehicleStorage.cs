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
            private List<string> queryPartsVehicle;
            private string query;
            private List<string> queryParts;
            private string queryId = "USE CarManagement; " +
                "SELECT id " +
                "FROM enrollment ";
            private string queryvehicleHead = "SELECT * FROM vehicle ";

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.queryPartsVehicle = new List<string>();
                this.queryParts = new List<string>();
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isTrue(color != null);
                this.queryPartsVehicle.Add("carColor = " + Convert.ToInt32(color) + " ");
                this.color = color;
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isTrue(started != null);
                this.queryPartsVehicle.Add("isStarted = " + Convert.ToInt16(started) + " ");
                this.engineIsStarted = started;
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.queryParts.Add("number = " + enrollment.Number);
                this.enrollment = enrollment;
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.enrollmentSerial = serial;
                this.queryParts.Add("serial = " + serial);
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isTrue(horsePower != null);
                this.queryPartsVehicle.Add("horsePower = " + horsePower + " ");
                this.horsePower = horsePower;
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min != null);
                Asserts.isTrue(max != null);
                this.queryPartsVehicle.Add("horsePower BETWEEN " + min + " AND " + max + " ");
                this.max = max;
                this.min = min;
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate();
            }

            private IEnumerator<IVehicle> enumerate()
            {
                Asserts.isFalse(this.queryParts.Count == 0);
                queryCreator(this.queryParts);
                string idQuery = this.queryId + this.query;

                queryCreator(this.queryPartsVehicle);
                string mainQuery = this.queryvehicleHead + this.query;

                VehicleDto vehicleDto = new VehicleDto();
                EnrollmentDto enrollmentDto = new EnrollmentDto();
                EngineDto engineDto = new EngineDto();
                List<WheelDto> wheelsDto = new List<WheelDto>();
                List<DoorDto> doorsDto = new List<DoorDto>();
                int id;
                SqlConnection con = new SqlConnection(this.connectionString);
                con.Open();

                SqlCommand sentence = new SqlCommand(this.queryId, con);
                SqlDataReader reader = sentence.ExecuteReader();
                reader.Read();
                id = (int)reader["id"];
                mainQuery = mainQuery + " AND vehicleId = " + (int)reader["id"];
                enrollmentDto.Serial = this.enrollmentSerial;
                enrollmentDto.Number = this.enrollment.Number;

                SqlCommand sentence2 = new SqlCommand(this.query, con);
                SqlDataReader reader2 = sentence2.ExecuteReader();
                reader2.Read();
                CarColor color = (CarColor)Enum.Parse(typeof(CarColor), reader2["color"].ToString());
                engineDto.HorsePower = Convert.ToInt32(reader2["engineHorsePower"]);
                engineDto.IsStarted = Convert.ToBoolean(reader2["engineIsStarted"]);
                reader2.Close();
                this.query = "SELECT pressure FROM wheel WHERE vehicleId =" + id;
                SqlCommand sentence3 = new SqlCommand(this.query, con);
                SqlDataReader reader3 = sentence3.ExecuteReader();
                while (reader3.Read())
                {
                    WheelDto wheelDto = new WheelDto();
                    wheelDto.Pressure = Convert.ToInt32(reader2["pressure"]);
                    wheelsDto.Add(wheelDto);
                }
                reader3.Close();
                this.query = "SELECT isOpen FROM door WHERE vehicleId =" + id;
                SqlCommand sentence4 = new SqlCommand(this.query, con);
                SqlDataReader reader4 = sentence4.ExecuteReader();
                while (reader4.Read())
                {
                    DoorDto doorDto = new DoorDto();
                    doorDto.IsOpen = Convert.ToBoolean(reader2["isOpen"]);
                    doorsDto.Add(doorDto);
                }
                reader4.Close();
                vehicleDto.Color = color;
                vehicleDto.Doors = doorsDto.ToArray();
                vehicleDto.Wheels = wheelsDto.ToArray();
                vehicleDto.Engine = engineDto;
                vehicleDto.Enrollment = enrollmentDto;
                IVehicle vehicle_get = this.vehicleBuilder.import(vehicleDto);
                yield return vehicle_get;
            }

            private void queryCreator(List<string> listcondition)
            {
                int counter = 0;
                foreach (string partcondition in listcondition)
                {
                    if (counter == 0)
                    {
                        this.query += " WHERE " + partcondition;
                    }
                    else
                    {
                        this.query += " AND " + partcondition;
                    }
                    counter++;
                }
            }
        }
    }
}
