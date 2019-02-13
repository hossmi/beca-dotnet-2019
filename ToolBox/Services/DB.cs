using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Services
{
    public class DB<TConnection>
        where TConnection : class, IDbConnection, new()
    {
        private readonly string connectionString;

        public DB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<T> executeQuery<T>(string query, Func<IDataRecord, T> buildDelegate, IDictionary<string, object> parameters = null)
        {
            using (TConnection connection = new TConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                
                if (parameters != null)
                    foreach (var parameter in parameters)
                    command.Parameters[parameter.Key] = parameter.Value;

                command.CommandText = query;
                connection.ConnectionString = this.connectionString;

                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                    while (reader.Read())
                        yield return buildDelegate(reader);

                connection.Close();
            }
        }

        public T executeScalar<T>(string query, Func<object, T> buildDelegate, IDictionary<string, object> parameters = null)
        {
            using (TConnection connection = new TConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                if (parameters != null)
                    foreach (var parameter in parameters)
                    command.Parameters[parameter.Key] = parameter.Value;

                command.CommandText = query;
                connection.ConnectionString = this.connectionString;
                connection.Open();
                object returned = command.ExecuteScalar();
                connection.Close();

                return buildDelegate(returned);
            }
        }

        public int execute(string query, IDictionary<string, object> parameters = null)
        {
            using (TConnection connection = new TConnection())
            using (IDbCommand command = connection.CreateCommand())
            {
                if(parameters != null)
                    foreach (var parameter in parameters)
                        command.Parameters[parameter.Key] = parameter.Value;

                command.CommandText = query;
                connection.ConnectionString = this.connectionString;
                connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                connection.Close();

                return affectedRows;
            }
        }
    }
}
