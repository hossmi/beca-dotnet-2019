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
        private const string SELECT_FROM_VEHICLE = @"
                        SELECT v.[enrollmentId]
                              ,v.[color]
                              ,v.[engineHorsePower]
                              ,v.[engineIsStarted]
	                          ,e.serial
	                          ,e.number
                          FROM [vehicle] v
                          INNER JOIN enrollment e ON v.enrollmentId = e.id ;";

        private const string SELECT_COUNT = "SELECT count(*) FROM vehicle";

        private readonly string connectionString;
        private readonly IVehicleBuilder vehicleBuilder;

        public SqlVehicleStorage(string connectionString, IVehicleBuilder vehicleBuilder)
        {
            this.connectionString = connectionString;
            this.vehicleBuilder = vehicleBuilder;
        }

        public int Count
        {
            get
            {
                using (SqlConnection connection = new SqlConnection(this.connectionString))
                using (SqlCommand command = new SqlCommand(SELECT_COUNT, connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public void clear()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                string query = @"DELETE FROM wheel;
                                DELETE FROM door;
                                DELETE FROM vehicle;
                                DELETE FROM enrollment;";
                connection.Open();
                command.Connection = connection;
                command.CommandText = query;

                int afectedRows = command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Dispose()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.Dispose();

                connection.Close();
            }
        }

        //public IVehicle get(IEnrollment enrollment)
        //{
        //    bool exist = false;

        //    using (SqlConnection connection = new SqlConnection(this.connectionString))
        //    using (SqlCommand command = new SqlCommand())
        //    {
        //        string selectEnrollment = $@"
        //                SELECT count(*)
        //                FROM enrollment                        
        //                WHERE serial = {enrollment.Serial} 
        //                AND number = {enrollment.Number};";
        //        connection.Open();
        //        command.Connection = connection;
        //        command.CommandText = selectEnrollment;

        //        int filas = (int)command.ExecuteScalar();
        //        exist = filas > 0;
        //    }

        //    Asserts.isTrue(exist);

        //    IVehicle vehicle = null;

        //    string query = $@"
        //                SELECT v.enrollmentId
        //                    , v.color
        //                    , v.engineHorsePower
        //                    , v.engineIsStarted
        //                    , e.serial
        //                    , e.number
        //                FROM vehicle v
        //                INNER JOIN enrollment e ON v.enrollmentId = e.id
        //                WHERE serial = {enrollment.Serial} 
        //                AND number = {enrollment.Number};";

        //    using (SqlConnection connection = new SqlConnection(this.connectionString))
        //    using (SqlCommand command = new SqlCommand())
        //    {
        //        connection.Open();
        //        command.Connection = connection;
        //        command.CommandText = query;
        //        using (SqlDataReader readerVehicle = command.ExecuteReader())
        //        {
        //            if (readerVehicle.Read())
        //            {
        //                VehicleDto vehicleDto = new VehicleDto();

        //                int enrollmentId = (int)readerVehicle["enrollmentId"];
        //                vehicleDto.Color = (CarColor)Convert.ToInt32(readerVehicle["color"]);
        //                vehicleDto.Engine = new EngineDto
        //                {
        //                    HorsePower = Convert.ToInt16(readerVehicle["engineHorsePower"]),
        //                    IsStarted = Convert.ToBoolean(readerVehicle["engineIsStarted"]),
        //                };
        //                vehicleDto.Enrollment = new EnrollmentDto
        //                {
        //                    Serial = readerVehicle["serial"].ToString(),
        //                    Number = Convert.ToInt16(readerVehicle["number"]),
        //                };

        //                List<WheelDto> wheelDtos = readWheels(connection, enrollmentId);
        //                List<DoorDto> doorDtos = readDoors(connection, enrollmentId);

        //                vehicleDto.Wheels = wheelDtos.ToArray();
        //                vehicleDto.Doors = doorDtos.ToArray();

        //                vehicle = this.vehicleBuilder.import(vehicleDto);

        //            }
        //        }
        //        connection.Close();
        //        return vehicle;
        //    }
        //}

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

            //public IEnumerable<IVehicle> getAll()
            //{
            //    IList<IVehicle> vehicles = new List<IVehicle>();

            //    using (SqlConnection connection = new SqlConnection(this.connectionString))
            //    using (SqlCommand command = new SqlCommand())
            //    {
            //        connection.Open();
            //        command.Connection = connection;
            //        command.CommandText = SELECT_FROM_VEHICLE;
            //        using (SqlDataReader readerVehicle = command.ExecuteReader())
            //        {
            //            while (readerVehicle.Read())
            //            {
            //                VehicleDto vehicleDto = new VehicleDto();

            //                int enrollmentId = (int)readerVehicle["enrollmentId"];
            //                vehicleDto.Color = (CarColor)Convert.ToInt32(readerVehicle["color"]);
            //                vehicleDto.Engine = new EngineDto
            //                {
            //                    HorsePower = Convert.ToInt16(readerVehicle["engineHorsePower"]),
            //                    IsStarted = Convert.ToBoolean(readerVehicle["engineIsStarted"]),
            //                };
            //                vehicleDto.Enrollment = new EnrollmentDto
            //                {
            //                    Serial = readerVehicle["serial"].ToString(),
            //                    Number = Convert.ToInt16(readerVehicle["number"]),
            //                };

            //                List<WheelDto> wheelDtos = readWheels(connection, enrollmentId);
            //                List<DoorDto> doorDtos = readDoors(connection, enrollmentId);

            //                vehicleDto.Wheels = wheelDtos.ToArray();
            //                vehicleDto.Doors = doorDtos.ToArray();

            //                IVehicle vehicle = this.vehicleBuilder.import(vehicleDto);
            //                vehicles.Add(vehicle);
            //            }
            //        }


        //        connection.Close();
        //    }

        //    return vehicles;
        //}


        public void set(IVehicle vehicle)
        {
            string query = $@"
                     SELECT id
                     FROM enrollment
                     WHERE serial = '{vehicle.Enrollment.Serial}'
                     AND number = {vehicle.Enrollment.Number};";

            bool exist = false;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = query;
                command.Connection = connection;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exist = true;
                    }
                }
                connection.Close();
            }
            Asserts.isFalse(exist);

            if (exist)
            {
                updateVehicle(vehicle, this.connectionString, query);
            }
            else
            {
                insertNewVehicle(vehicle, this.connectionString, query);
            }
        }

        private static void updateVehicle(IVehicle vehicle, string connectionString, string queryEnrollment)
        {
            string sentences = "";
            int enrollmentId;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = queryEnrollment;
                command.Connection = connection;

                enrollmentId = (int)command.ExecuteScalar();

                command.CommandText = $@"
                                SELECT *
                                FROM vehicle
                                WHERE enrollmentId = {enrollmentId};";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CarColor colorAux = (CarColor)Convert.ToInt32(reader["color"]);
                        int horsePowerAux = (int)reader["engineHorsePower"];
                        bool startedAux = Convert.ToBoolean(reader["engineIsStarted"]);
                        if (colorAux != vehicle.Color)
                        {
                            sentences += $@"INSERT INTO vehicle (color) 
                                    VALUES ({(int)vehicle.Color});";
                        }
                        if (horsePowerAux != vehicle.Engine.HorsePower)
                        {
                            sentences += $@"INSERT INTO vehicle (engineHorsePower) 
                                    VALUES ({vehicle.Engine.HorsePower});";
                        }
                        if (startedAux != vehicle.Engine.IsStarted)
                        {
                            sentences += $@"INSERT INTO vehicle (engineIsStarted) 
                                    VALUES ({(vehicle.Engine.IsStarted ? 1 : 0)});";
                        }
                    }
                }
                command.CommandText = $@"
                                SELECT *
                                FROM wheel
                                WHERE vehicleId = {enrollmentId};";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double pressureAux;
                        foreach (IWheel wheel in vehicle.Wheels)
                        {
                            pressureAux = Convert.ToDouble(reader["pressure"]);
                            if(pressureAux != wheel.Pressure)
                            {
                                sentences += $@"INSERT INTO wheel (pressure) 
                                                VALUES ({wheel.Pressure});";
                            }                            
                        }
                    }                    
                }
                command.CommandText = $@"
                                SELECT *
                                FROM door
                                WHERE vehicleId = {enrollmentId};";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool openAux;
                        foreach (IDoor door in vehicle.Doors)
                        {
                            openAux = Convert.ToBoolean(reader["isOpen"]);
                            if (openAux != door.IsOpen)
                            {
                                sentences += $@"INSERT INTO door (isOpen) 
                                                VALUES ({(door.IsOpen ? 1 : 0)});";
                            }
                        }
                    }
                }
                connection.Close();
            }
            executeCommand(connectionString, sentences);
        }

        private static void insertNewVehicle(IVehicle vehicle, string connectionString, string query)
        {
            string sentences;
            int enrollmentId;

            sentences = $@"INSERT INTO enrollment (serial, number) 
                    VALUES ('{vehicle.Enrollment.Serial}', {vehicle.Enrollment.Number});";
            executeCommand(connectionString, sentences);

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                connection.Open();
                command.CommandText = query;
                command.Connection = connection;

                enrollmentId = (int)command.ExecuteScalar();

                connection.Close();
            }

            sentences = $@"INSERT INTO vehicle (enrollmentId, color, engineHorsePower, engineIsStarted) 
                    VALUES ({enrollmentId}, {(int)vehicle.Color}, {vehicle.Engine.HorsePower}, {(vehicle.Engine.IsStarted ? 1 : 0)});";
            executeCommand(connectionString, sentences);

            foreach (IWheel wheel in vehicle.Wheels)
            {
                sentences = $@"INSERT INTO wheel (vehicleId, pressure) 
                    VALUES ({enrollmentId}, {wheel.Pressure.ToString().Replace(',', '.')});";
                executeCommand(connectionString, sentences);
            }

            foreach (IDoor door in vehicle.Doors)
            {
                sentences = $@"INSERT INTO door (vehicleId, isOpen) 
                    VALUES ({enrollmentId}, {(door.IsOpen ? 1 : 0)});";
                executeCommand(connectionString, sentences);
            }
        }

        private static List<WheelDto> readWheels(SqlConnection connection, int vehicleId)
        {
            List<WheelDto> wheelsDto = new List<WheelDto>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $@"SELECT * FROM wheel
                                        WHERE vehicleId = {vehicleId};";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        WheelDto wheelDto = new WheelDto();
                        wheelDto.Pressure = Convert.ToDouble(reader["pressure"]);
                        wheelsDto.Add(wheelDto);
                    }
                }
            }
            return wheelsDto;
        }

        private static List<DoorDto> readDoors(SqlConnection connection, int vehicleId)
        {
            List<DoorDto> doorsDto = new List<DoorDto>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $@"SELECT * FROM door
                                        WHERE vehicleId = {vehicleId};";
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoorDto doorDto = new DoorDto();
                        doorDto.IsOpen = Convert.ToBoolean(reader["isOpen"]);
                        doorsDto.Add(doorDto);
                    }
                }
            }
            return doorsDto;
        }

        private static void executeCommand(string connectionString, string sentencies)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            using (IDbCommand command = new SqlCommand())
            {
                command.CommandText = sentencies;
                command.Connection = connection;

                connection.Open();
                int afectedRows = command.ExecuteNonQuery();
                connection.Close();
            }
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
