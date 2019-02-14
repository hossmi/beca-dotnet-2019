﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
            private IDictionary<string, string> queryParts;
            private string query;
            private string query2;
            private string queryHead = "USE CarManagement; " +
                "SELECT e.serial, " +
                "e.number, " +
                "e.id, " +
                "v.color, " +
                "v.engineIsStarted, " +
                "v.engineHorsePower " +
                "FROM enrollment e" +
                "INNER JOIN vehicle v ";

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.queryParts = new Dictionary<string, string>();
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isTrue(color != null);
                this.queryParts.Add("COLOR" ,"carColor = @COLOR");
                this.color = color;
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isTrue(started != null);
                this.queryParts.Add("STARTED", "isStarted = @STARTED");
                this.engineIsStarted = started;
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isTrue(enrollment != null);
                this.queryParts.Add("NUMBER", "number = @NUMBER");
                this.enrollment = enrollment;
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isTrue(serial != null);
                this.enrollmentSerial = serial;
                this.queryParts.Add("NUMBER", "serial = @SERIAL");
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isTrue(horsePower != null);
                this.queryParts.Add("HORSEPOWER", "horsePower = @HORSEPOWER");
                this.horsePower = horsePower;
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isTrue(min != null);
                Asserts.isTrue(max != null);
                this.queryParts.Add("BETWEEN", "horsePower BETWEEN @MIN AND @MAX");
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
                queryCreator(this.queryParts);
                string query = this.queryHead + this.query;

                VehicleDto vehicleDto = new VehicleDto();
                EnrollmentDto enrollmentDto = new EnrollmentDto();
                EngineDto engineDto = new EngineDto();
                List<WheelDto> wheelsDto = new List<WheelDto>();
                List<DoorDto> doorsDto = new List<DoorDto>();
                SqlCommand sentence = genericSentence(this.query, this.connectionString);
                SqlDataReader reader = sentence.ExecuteReader();
                while (reader.Read())
                {
                    sentence.Parameters["@ID"].Value = (int)reader["id"];
                    enrollmentDto.Serial = reader["serial"].ToString();
                    enrollmentDto.Number = Convert.ToInt32(reader["number"]);
                    CarColor color = (CarColor)Enum.Parse(typeof(CarColor), reader["color"].ToString());
                    engineDto.IsStarted = Convert.ToBoolean(reader["engineIsStarted"]);
                    engineDto.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);

                    this.query2  = "USE CarManagement; " +
                    "SELECT pressure " +
                    "FROM wheel " +
                    "WHERE vehicleId = @ID";

                    SqlCommand sentence2 = genericSentence(this.query2, this.connectionString);
                    sentence2.Parameters["@ID"].Value = sentence.Parameters["@ID"];
                    SqlDataReader reader2 = sentence2.ExecuteReader();
                    while (reader2.Read())
                    {
                        WheelDto wheelDto = new WheelDto();
                        wheelDto.Pressure = Convert.ToDouble(reader2["pressure"]);
                        wheelsDto.Add(wheelDto);
                    }
                    reader2.Close();
                    this.query = "USE CarManagement; " +
                        "SELECT isOpen FROM door WHERE vehicleId = @ID";
                    sentence2 = genericSentence(this.query, this.connectionString);
                    sentence2.Parameters.Add("@ID", SqlDbType.Int);
                    sentence2.Parameters["@ID"].Value = sentence.Parameters["@ID"].Value;
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
                reader.Close();
            }

            private SqlCommand genericSentence(string query, string con)
            {
                SqlCommand sentence = new SqlCommand(query, connection(con));
                sentence.Parameters.Add("@ID", SqlDbType.Int);

                sentence.Parameters.Add("@SERIAL", SqlDbType.VarChar);
                sentence.Parameters["@SERIAL"].Value = this.enrollmentSerial;
                sentence.Parameters.Add("@NUMBER", SqlDbType.Int);
                sentence.Parameters["@NUMBER"].Value = this.enrollment.Number;
                sentence.Parameters.Add("@COLOR", SqlDbType.Int);
                sentence.Parameters["@COLOR"].Value = Convert.ToInt32(this.color);
                sentence.Parameters.Add("@STARTED", SqlDbType.Int);
                sentence.Parameters["@STARTED"].Value = this.engineIsStarted;
                sentence.Parameters.Add("@HORSEPOWER", SqlDbType.Int);
                sentence.Parameters["@HORSEPOWER"].Value = this.horsePower;
                sentence.Parameters.Add("@MIN", SqlDbType.Int);
                sentence.Parameters["@SERIAL"].Value = this.min;
                sentence.Parameters.Add("@MAX", SqlDbType.Int);
                sentence.Parameters["@SERIAL"].Value = this.max;
                return sentence;
            }

            private SqlConnection connection(string connection)
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                return con;
            }
            private void queryCreator(IDictionary<string, string> listcondition)
            {
                int counter = 0;
                foreach (string partcondition in listcondition.Values)
                {
                    if (counter == 0)
                    {
                        this.query += " WHERE " + partcondition;
                        listcondition.ContainsKey(nameof(partcondition));
                        counter++;
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
