using System;
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
        private const string BASE_QUERRY = "SELECT serial, number, engineHorsePower, engineIsStarted, enrollmentId, color FROM vehicle, enrollment" +
            "WHERE enrollmentId = vehicleId";

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
        public IVehicleQuery get()
        {
            return new PrvVehicleQuery(this.connectionString, this.vehicleBuilder);
        }

        public void set(IVehicle vehicle)
        {
            throw new NotImplementedException();
        }

        private class PrvVehicleQuery : IVehicleQuery
        {
            private readonly string connectionString;
            private readonly IVehicleBuilder vehicleBuilder;
            private readonly IDictionary<string, string> filters;
            private string indexEngineHorsePower = "whereHorsePower";

            public PrvVehicleQuery(string connectionString, IVehicleBuilder vehicleBuilder)
            {
                this.connectionString = connectionString;
                this.vehicleBuilder = vehicleBuilder;
                this.filters = new Dictionary<string, string>();
            }

            public IEnumerator<IVehicle> GetEnumerator()
            {
                return enumerate();
            }

            public IVehicleQuery whereColorIs(CarColor color)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereColorIs)));
                this.filters[nameof(whereColorIs)] = " color = " + Convert.ToInt32(color) + " ";
                return this;
            }

            public IVehicleQuery whereEngineIsStarted(bool started)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEngineIsStarted)));
                this.filters[nameof(whereEngineIsStarted)] = " isStarted = " + started + " ";
                return this;
            }

            public IVehicleQuery whereEnrollmentIs(IEnrollment enrollment)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentIs)));
                this.filters[nameof(whereEnrollmentIs)] = " enrollment = " + enrollment + " ";
                return this;
            }

            public IVehicleQuery whereEnrollmentSerialIs(string serial)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(whereEnrollmentSerialIs)));
                this.filters[nameof(whereEnrollmentSerialIs)] = " serial = " + serial + " ";
                return this;
            }

            public IVehicleQuery whereHorsePowerEquals(int horsePower)
            {
                Asserts.isFalse(this.filters.ContainsKey(indexEngineHorsePower));
                this.filters[nameof(indexEngineHorsePower)] = " engineHorsePower = " + horsePower + " ";
                return this;
            }

            public IVehicleQuery whereHorsePowerIsBetween(int min, int max)
            {
                Asserts.isFalse(this.filters.ContainsKey(nameof(indexEngineHorsePower)));
                this.filters[nameof(indexEngineHorsePower)] = " engineHorsePower  >= " + min + " AND engineHorsePower <= " + max + " ";
                return this;
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

            IEnumerator IEnumerable.GetEnumerator()
            {
                return enumerate(filters.Values);
            }

            private IEnumerator<IVehicle> enumerate(IEnumerable<string> filters)
            {
                string finalQuerry = "";
                foreach (string filter in filters)
                {
                    finalQuerry += " AND " + filter;
                }
                if(finalQuerry != "")
                {
                    finalQuerry = " WHERE " + finalQuerry.Substring(4);
                }
                finalQuerry = BASE_QUERRY + finalQuerry;
                SqlConnection connection = new SqlConnection(this.connectionString);
                SqlCommand command = new SqlCommand(finalQuerry, connection);
                connection.Open();
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                }

               

            }

        }
    }
}
