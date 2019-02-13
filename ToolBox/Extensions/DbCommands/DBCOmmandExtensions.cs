using System.Collections.Generic;
using System.Data;

namespace ToolBox.Extensions.DbCommands
{
    public static class DBCommandExtensions
    {
        public static void setParameters(this IDbCommand command, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            foreach (var parameter in parameters)
                prv_setParameter(command, parameter.Key, parameter.Value);
        }

        public static void setParameter(this IDbCommand command, string name, object value)
        {
            prv_setParameter(command, name, value);
        }

        private static void prv_setParameter(this IDbCommand command, string name, object value)
        {
            IDbDataParameter commandParameter = command.CreateParameter();
            commandParameter.ParameterName = name;
            commandParameter.Value = value;
            command.Parameters.Add(commandParameter);
        }
    }
}
