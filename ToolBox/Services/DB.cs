using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ToolBox.Extensions.DbCommands;
using ToolBox.Models;

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

        public IEnumerable<T> select<T>(string query, Func<IDataRecord, T> buildDelegate, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            parameters = parameters ?? Enumerable.Empty<KeyValuePair<string, object>>();

            using (TConnection connection = new TConnection())
            {
                connection.ConnectionString = this.connectionString;
                connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction())
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = query;
                    command.setParameters(parameters);

                    using (IDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                            yield return buildDelegate(reader);

                    transaction.Commit();
                }

                connection.Close();
            }
        }

        public object selectValue(string query, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            object result;

            parameters = parameters ?? Enumerable.Empty<KeyValuePair<string, object>>();

            using (TConnection connection = new TConnection())
            {
                connection.ConnectionString = this.connectionString;
                connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction())
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = query;
                    command.setParameters(parameters);

                    result = command.ExecuteScalar();
                    transaction.Commit();
                }

                connection.Close();
            }

            return result;
        }

        public int write(string query, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            int affectedRows;

            parameters = parameters ?? Enumerable.Empty<KeyValuePair<string, object>>();

            using (TConnection connection = new TConnection())
            {
                connection.ConnectionString = this.connectionString;
                connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction())
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = query;
                    command.setParameters(parameters);

                    affectedRows = command.ExecuteNonQuery();
                    transaction.Commit();
                }

                connection.Close();
            }

            return affectedRows;
        }

        public void transact(Func<ICommandBuilder, TransactionAction> statementBlockDelegate)
        {
            using (TConnection connection = new TConnection())
            {
                connection.ConnectionString = this.connectionString;
                connection.Open();

                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    ICommandBuilder commandBuilder = new PrvCommandBuilder(transaction);

                    TransactionAction action = statementBlockDelegate(commandBuilder);

                    if (action == TransactionAction.Commit)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }

                connection.Close();
            }
        }

        private class PrvCommandBuilder : ICommandBuilder
        {
            private readonly IDbTransaction transaction;
            private readonly IDictionary<string, object> parameters;
            private string query;

            public PrvCommandBuilder(IDbTransaction transaction)
            {
                this.transaction = transaction;
                this.parameters = new Dictionary<string, object>();
            }

            public IDbCommand build()
            {
                IDbCommand command = this.transaction.Connection.CreateCommand();
                command.Transaction = this.transaction;
                command.CommandText = this.query;
                command.setParameters(this.parameters);

                return command;
            }

            public ICommandBuilder setParameter(string name, object value)
            {
                Asserts.stringIsFilled(name);
                this.parameters[name] = value;
                return this;
            }

            public ICommandBuilder setQuery(string query)
            {
                Asserts.stringIsFilled(query);
                this.query = query;
                return this;
            }
        }
    }
}
