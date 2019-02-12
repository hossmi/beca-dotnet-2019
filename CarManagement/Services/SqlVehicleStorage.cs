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
            List<string> clearDbQuerry = new List<string>();
            clearDbQuerry.Add("DELETE FROM door");
            clearDbQuerry.Add("DELETE FROM wheel");
            clearDbQuerry.Add("DELETE FROM vehicle");
            clearDbQuerry.Add("DELETE FROM enrollment");
            //------------------------------------------------------------------------------------------
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                //------------------------------------------------------------------------------------------
                connection.Open();
                //------------------------------------------------------------------------------------------
                for (int i = 0; i < clearDbQuerry.Count; i++)
                {

                    SqlCommand command = new SqlCommand(clearDbQuerry[i], connection);
                    command.ExecuteNonQuery();

                }
                //------------------------------------------------------------------------------------------
                connection.Close();
                //------------------------------------------------------------------------------------------
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
                connection.Open();
                IDataReader reader = command.ExecuteReader();
                String getFromVehicle = "SELECT  color,engineHorsePower,engineIsStarted FROM wheel WHERE id =" + Convert.ToInt32(reader["id"]);
                command = new SqlCommand(getFromVehicle, connection);
                reader = command.ExecuteReader();
                vehicleDto.Color = (CarColor)Convert.ToInt32(reader["color"]);
                vehicleDto.Engine.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);
                vehicleDto.Engine.IsStarted = Convert.ToBoolean(reader["engineIsStarted"]);

                //------------------------------------------------------------------------------------------
                String getFromWheel = "SELECT  pressure FROM wheel WHERE id =" + Convert.ToInt32(reader["id"]);
                command = new SqlCommand(getFromWheel, connection);
                reader = command.ExecuteReader();
                reader.Read();
                float[] vehiclePressure = null;
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
            
            string getVehiceProperties = "SELECT enrollmentId, serial, number, color, engineHorsePower, engineIsStarted " +
                "FROM enrollment, vehicle WHERE enrollmentid = id ";

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(getVehiceProperties, connection);
                return giveMeValues(command);
            }
        }

        private  List<IVehicle> giveMeValues(SqlCommand command)
        {
            IDataReader reader = command.ExecuteReader();
            List<IVehicle> vehicles = new List<IVehicle>();

            while (reader.Read())
            {

                VehicleDto vehicleDto = new VehicleDto();
                vehicleDto.Enrollment = new EnrollmentDto();
                vehicleDto.Engine = new EngineDto();

                int enrollmentID = Convert.ToInt32(reader["enrollmentid"]);
                vehicleDto.Enrollment.Number = Convert.ToInt32(reader["number"]);
                vehicleDto.Enrollment.Serial = Convert.ToString(reader["serial"]);
                vehicleDto.Engine.IsStarted = Convert.ToBoolean(reader["engineIsStarted"]);
                vehicleDto.Engine.HorsePower = Convert.ToInt32(reader["engineHorsePower"]);
                vehicleDto.Color = (CarColor)(Convert.ToInt32(reader["color"]));

                string getVehicleWheel = $@"SELECT pressure FROM wheel WHERE vehicleId =   {enrollmentID};";
                command = new SqlCommand(getVehicleWheel, command.Connection);
                vehicleDto.Wheels = giveMeWheels(command);

                string getVehicleDoor = $@"SELECT isOpen FROM door WHERE vehicleId =   {enrollmentID};"; ;
                command = new SqlCommand(getVehicleDoor, command.Connection);
                vehicleDto.Doors = giveMeDoors(command);

                IVehicle vehicle = this.vehicleBuilder.import(vehicleDto);
                vehicles.Add(vehicle);
                

            }
            command.Connection.Close();
            return vehicles;
        }

        private static DoorDto[] giveMeDoors(SqlCommand command)
        {
            IDataReader readerDoor = command.ExecuteReader();
            List<DoorDto> doors = new List<DoorDto>();

            while (readerDoor.Read())
            {
                DoorDto door = new DoorDto();
                door.IsOpen = Convert.ToBoolean(readerDoor["isOpen"]);
                doors.Add(door);
            }
           return doors.ToArray();
        }

        private  static WheelDto[] giveMeWheels(SqlCommand command)
        {
            List<WheelDto> wheels = new List<WheelDto>();
            IDataReader readerWheel = command.ExecuteReader();
            while (readerWheel.Read())
            {
                WheelDto wheel = new WheelDto();
                wheel.Pressure = Convert.ToDouble(readerWheel["pressure"]);
                wheels.Add(wheel);
            }

           return wheels.ToArray();
        }

        public void set(IVehicle vehicle)
        {
            //this.vehicleBuilder.export(vehicle);
            string pushToEnrollmentb = "INSERT INTO enrollment(seria,number) output INSERTED.ID VALUES(@serial,@number)";
            String pushToWheel = "INSERT INTO wheel(vehicleid,pressure)" + "VALUES(@vehicleid,@pressure)";
            String pushToDoor = "INSERT INTO door(vehicleid,isopen)" + "VALUES(@vehicleid,@isopen)";
            string pushToVehicle = "INSERT INTO vehicle(color,engineHorsePower,engineIsStarted) VALUES(@color,@engineHorsePower,@engineIsStarted)";
            using (SqlConnection conection = new SqlConnection(this.connectionString))
            {
                //------------------------------------------------------------------------------------------
                conection.Open();
                //------------------------------------------------------------------------------------------
                SqlCommand pusher = new SqlCommand(pushToEnrollmentb, conection);
                pusher = new SqlCommand(pushToEnrollmentb, conection);
                pusher.Parameters.AddWithValue("@serial", vehicle.Enrollment.Serial);
                pusher.Parameters.AddWithValue("@number", vehicle.Enrollment.Number);
                int enrollmentId = (int)pusher.ExecuteScalar();
                //------------------------------------------------------------------------------------------
                pusher = new SqlCommand(pushToVehicle, conection);
                pusher.Parameters.AddWithValue("@enrollmentid", enrollmentId);
                pusher.Parameters.AddWithValue("@color", vehicle.Color);
                pusher.Parameters.AddWithValue("@engineHorsePower", vehicle.Engine.HorsePower);
                pusher.Parameters.AddWithValue("@engineIsStarted", vehicle.Engine.IsStarted);
                pusher.ExecuteNonQuery();
                //------------------------------------------------------------------------------------------


                foreach (IWheel wheels in vehicle.Wheels)
                {
                    pusher = new SqlCommand(pushToWheel, conection);
                    pusher.Parameters.AddWithValue("@pressure", wheels.Pressure);
                    pusher.Parameters.AddWithValue("@vehicleid", enrollmentId);
                    pusher.ExecuteNonQuery();

                }
                //------------------------------------------------------------------------------------------
                foreach (IDoor doors in vehicle.Doors)
                {
                    pusher = new SqlCommand(pushToDoor, conection);
                    pusher.Parameters.AddWithValue("@isopen", doors.IsOpen);
                    pusher.Parameters.AddWithValue("@vehicleid", enrollmentId);
                    pusher.ExecuteNonQuery();
                }
                //------------------------------------------------------------------------------------------
                conection.Close();

                //------------------------------------------------------------------------------------------
            }
        }

    }
}
