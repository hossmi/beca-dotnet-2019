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
            string clearAllDb = "DELETE * from enrollment,vehicle,door,wheel";
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                SqlCommand command = new SqlCommand(clearAllDb, connection);
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IVehicle get(IEnrollment enrollment)
        {

            VehicleDto vehicleDto = new VehicleDto();
            vehicleDto.Enrollment.Number = enrollment.Number;
            vehicleDto.Enrollment.Serial = enrollment.Serial;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                String getFromEnrollment = "SELECT  id,serial,enrollment FROM enrollment WHERE serial = " + enrollment.Serial + ", number = " + enrollment.Number;
                SqlCommand command = new SqlCommand(getFromEnrollment, connection);
                //------------------------------------------------------------------------------------------
                connection.Open();
                //------------------------------------------------------------------------------------------
                IDataReader reader = command.ExecuteReader();
                reader.Read();
                //------------------------------------------------------------------------------------------
                String getFromVehicle = "SELECT  color,engineHorsePower,engineIsStarted FROM wheel WHERE id =" + Convert.ToInt32(reader["id"]);
                command = new SqlCommand(getFromVehicle, connection);
                reader = command.ExecuteReader();
                reader.Read();
                vehicleDto.Color = (CarColor)Convert.ToInt32(reader["color"]);
                vehicleDto.Engine.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);
                vehicleDto.Engine.IsStarted = Convert.ToBoolean(reader["engineIsStarted"]);

                //------------------------------------------------------------------------------------------
                String getFromWheel = "SELECT  pressure FROM wheel WHERE id =" + Convert.ToInt32(reader["id"]);
                command = new SqlCommand(getFromWheel, connection);
                reader = command.ExecuteReader();
                reader.Read();
                float[] vehiclePressure = null; _
                for (int i = 0; reader.Read(); i++)
                {
                    vehiclePressure[i] = (float)(reader["pressure"]);
                    reader.Read();
                }
                int counter = 0;
                foreach (WheelDto wheels in vehicleDto.Wheels)
                {
                    wheels.Pressure = vehiclePressure[counter];
                    counter++;
                }
                //------------------------------------------------------------------------------------------
                String getFromDoor = "SELECT  isOpen FROM door WHERE id =" + Convert.ToInt32(reader["id"]);
                command = new SqlCommand(getFromDoor, connection);
                reader = command.ExecuteReader();
                reader.Read();
                bool[] vehicleIsOpen = null;
                for (int i = 0; reader.Read(); i++)
                {
                    vehicleIsOpen[i] = (bool)reader["isOpen"];
                    reader.Read();
                }
                counter = 0;
                foreach (DoorDto doors in vehicleDto.Doors)
                {
                    doors.IsOpen = vehicleIsOpen[counter];
                    counter++;
                }

                //------------------------------------------------------------------------------------------
                connection.Close();
            }

            //------------------------------------------------------------------------------------------
            return this.vehicleBuilder.import(vehicleDto);


        }

        public IEnumerable<IVehicle> getAll()
        {
            //------------------------------------------------------------------------------------------
            List<IVehicle> vehicles = null;
            string getVehiceProperties = "SELECT enrollmentid,vehicleid, serial, number, color, engineHorsePower, engineIsStarted" +
                "FROM enrollment, vehicle";
            string getVehicleWheel = "SELECT vehicleId, pressure FROM wheel";
            string getVehicleDoor = "SELECT vehicleId, IsOpen FROM door";
            //------------------------------------------------------------------------------------------
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {

                //------------------------------------------------------------------------------------------
                connection.Open();
                //------------------------------------------------------------------------------------------
                SqlCommand command = new SqlCommand(getVehiceProperties, connection);
                IDataReader reader = command.ExecuteReader();
                reader.Read();
                //------------------------------------------------------------------------------------------
                while (reader.Read())
                {

                    int enrollmentID = Convert.ToInt32(reader["enrollmentid"]);
                    VehicleDto vehicleDto = new VehicleDto();

                    for (int i = 0; reader.Read(); i++)
                    {
                        vehicleDto.Enrollment.Number = Convert.ToInt32(reader["number"]);
                        vehicleDto.Enrollment.Serial = Convert.ToString(reader["serial"]); ;
                        vehicleDto.Engine.IsStarted = Convert.ToBoolean(reader["IsStarted"]);
                        vehicleDto.Engine.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);
                        vehicleDto.Color = (CarColor)(Convert.ToInt32(reader["color"]));
                        reader.Read();
                    }

                    //------------------------------------------------------------------------------------------
                    command = new SqlCommand(getVehicleWheel, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    int vehicleID = Convert.ToInt32(reader["vehicleId"]);
                    float[] vehicleDtoPressure = null;
                    for (int i = 0; enrollmentID == vehicleID; i++)
                    {
                        vehicleDtoPressure[i] = (float)reader["pressure"];
                        int counter = 0;
                        foreach (WheelDto wheel in vehicleDto.Wheels)
                        {
                            wheel.Pressure = vehicleDtoPressure[counter];
                            counter++;

                        }
                    }

                    //------------------------------------------------------------------------------------------
                    command = new SqlCommand(getVehicleDoor, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    vehicleID = Convert.ToInt32(reader["vehicleId"]);
                    bool[] vehicleDtoIsOpen = null;
                    for (int i = 0; enrollmentID == vehicleID; i++)
                    {
                        vehicleDtoIsOpen[i] = Convert.ToBoolean(reader["IsOpen"]);
                        int counter = 0;
                        foreach (DoorDto door in vehicleDto.Doors)
                        {
                            door.IsOpen = vehicleDtoIsOpen[counter];
                            counter++;

                        }
                    }
                    //------------------------------------------------------------------------------------------
                    IVehicle vehicle = vehicleBuilder.import(vehicleDto);
                    vehicles.Add(vehicle);
                    //vehicles = vehicles.Concat(new[] { vehicle });
                    reader.Read();
                    //------------------------------------------------------------------------------------------
                }
                //------------------------------------------------------------------------------------------
                connection.Close();
            }
            return vehicles;
            //------------------------------------------------------------------------------------------

        }

        public void set(IVehicle vehicle)
        {
            string pushToEnrollmentb = "INSERT INTO enrollment(seria,number) output INSERTED.ID VALUES(@serial,@number)";
            String pushToWheel = "INSERT INTO wheel(vehicleid,pressure)" + "VALUES(@vehicleid,@pressure)";
            String pushToDoor = "INSERT INTO door(vehicleid,isopen)" + "VALUES(@vehicleid,@isopen)";
            string pushToVehicle = "INSERT INTO vehicle(color,engineHorsePower,engineIsStarted) VALUES(@color,@engineHorsePower,@engineIsStarted)";
            using (SqlConnection conection = new SqlConnection(this.connectionString))
            {
                SqlCommand pusher = new SqlCommand(pushToEnrollmentb, conection);
                pusher = new SqlCommand(pushToEnrollmentb, conection);
                pusher.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                pusher.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                int enrollmentId = (int)pusher.ExecuteScalar();


                pusher = new SqlCommand(pushToVehicle, conection);
                pusher.Parameters.AddWithValue("@enrollmentid", enrollmentId);
                pusher.Parameters.AddWithValue("@color", vehicle.Color);
                pusher.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
                pusher.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
                pusher.ExecuteNonQuery();


                foreach (IWheel wheels in vehicle.Wheels)
                {
                    pusher = new SqlCommand(pushToWheel, conection);
                    pusher.Parameters.AddWithValue("@pressure", wheels.Pressure);
                    pusher.Parameters.AddWithValue("@vehicleid", enrollmentId);
                    pusher.ExecuteNonQuery();

                }
                foreach (IDoor doors in vehicle.Doors)
                {
                    pusher = new SqlCommand(pushToDoor, conection);
                    pusher.Parameters.AddWithValue("@isopen", doors.IsOpen);
                    pusher.Parameters.AddWithValue("@vehicleid", enrollmentId);
                    pusher.ExecuteNonQuery();
                }

            }
        }
    }
}
