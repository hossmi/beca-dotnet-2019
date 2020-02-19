using System.Collections.Generic;
using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        private readonly iQuery iquery;

        public QueryBuilder(iQuery iquery)
        {
            this.iquery = iquery;
        }
        public QueryBuilder()
        {

        }

        public StringBuilder update()
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"UPDATE {this.iquery.tablesColumns[0].field} SET {addFields(this.iquery.tablesColumns[0], "UPDATE")} {where(this.iquery)}");
            return query;
        }
        public StringBuilder insert()
        {
            StringBuilder query = new StringBuilder(50);
            query.Insert(query.Length, $"INSERT INTO {this.iquery.tablesColumns[0].field} ({addFields(this.iquery.tablesColumns[0], "INSERT", "condition")}) ");
            if (this.iquery.tablesColumns[0].output != null)
            {
                query.Insert(query.Length, this.iquery.tablesColumns[0].output);
            }
            query.Insert(query.Length, $" VALUES({ addFields(this.iquery.tablesColumns[0], "INSERT", "value")})");
            return query;
        }
        public StringBuilder select()
        {
            if (this.iquery.tablesColumns[0].values.Count == 1)
            {
                return selectDelete($"SELECT {this.iquery.tablesColumns[0].values[0]}", this.iquery);
            }
            else
            {
                return select(this.iquery.tablesColumns);
            }
            
        }
        public StringBuilder delete()
        {
            return selectDelete("DELETE", this.iquery);
        }
        public string query_select(Select select)
        {
            return string.Empty;
        }
        private static StringBuilder select(IList<FieldValues> fields)
        {
            IList<string> strings = new List<string>();
            foreach (FieldValues fieldValue in fields)
            {
                strings.Add(string.Join(", ", alias(fieldValue)));
            }
            return new StringBuilder($"SELECT {string.Join(", ", strings)} ");
        }
        public static string from(From from)
        {
            int counter = 0;
            int counter2 = 0;
            string query = "FROM ";
            foreach (string table in from.tables)
            {                
                if (counter == 0)
                {
                    query += table;
                }
                else
                {
                    query += inner_join(table, from.conditions[counter2]);
                    counter2++;
                }
                counter++;
            }

            return query;
        }
        private static string inner_join(string table_name, Condition_values condition_value)
        {
            return $" INNER JOIN {table_name} ON {condition_Value(condition_value)}";
        }
        private static string condition_Value(Condition_values condition_value)
        {
            return $"{condition_value.condition} = {condition_value.values[0]}";
        }
        //case 1: WHERE condition_1 = value_1 AND/OR condition_2 = value_2 AND/OR condition_n = value_n...
        //2 list(string) + union type
        //IDictionary<whereFieldValues>
        //{ condition = "", value = "", union_type = ""}

        //case 2: conditions_group_1/condition_1 AND/OR condition_2/ conditions_group_2
        //2 list(string) + union type

        //case 3: condition BETWEEN value_1 AND value_2
        //2 list string + union type -> list(condition_values object)
        //case 4: condition 1-n IN (value_list) 1-n
        //list(string) + list(list(string)) + union type
        public string where(Where wh3re)
        {
            string query = string.Empty;
            foreach (whereFieldValues whereValue in wh3re.where_fields_values)
            {
                query += string.Join(wh3re.key, where_value(whereValue));
            }
            return query;
        }
        private static string where_value(whereFieldValues condition_value)
        {
            string query = " WHERE ";
            if (condition_value.values.Count > 1)
            {
                if (condition_value.key == "IN")
                {
                    query += $" {string.Join(condition_value.key, new List<string>() { condition_value.field, $"({ string.Join(", ", condition_value.values) })"})}";
                }
                else if (condition_value.key == "BETWEEN")
                {
                    query += $" {string.Join(condition_value.key, new List<string>() {condition_value.field, string.Join( "AND", new List<string>() {condition_value.values[0], condition_value.values[1]})})}";
                }
                return query;
            }
            else
            {
                return $"{query} {condition_value.field} = {condition_value.values[0]}";
            }
            
        }
        private static IList<string> alias(FieldValues fieldValues)
        {
            IList<string> strings = new List<string>();
            foreach (string value in fieldValues.values)
            {
                strings.Add($"{fieldValues.field[0]}.{value}");
            }
            return strings;
        }
        private static StringBuilder selectDelete(string instruction, iQuery iquery)
        {
            StringBuilder query = new StringBuilder(30);
            query.Insert(query.Length, $"{instruction} {from(new From() { tables = new List<string>() { iquery.tablesColumns[0].field } })}");
            if (iquery.whereValues != null)
            {
                query.Insert(query.Length, where(iquery));
            }
            return query;
        }
        private static StringBuilder where(iQuery iquery)
        {
            IList<string> strings = new List<string>();
            int counter = 0;
            if (iquery.whereValues.Count != 0)
            {
                foreach (whereFieldValues wherevalues in iquery.whereValues)
                {
                    strings.Add($"{buildCondition(checkCondition(wherevalues.field, iquery.tablesColumns[0].field), wherevalues)}");
                    counter++;
                }
                return new StringBuilder(100).Append($" WHERE {string.Join(" AND ", strings)}");
            }
            else
            {
                return new StringBuilder();
            }
            
        }
        private static StringBuilder addFields(FieldValues tableColumns, string instruction, string fieldType = null)
        {
            IList<string> strings = new List<string>();
            int counter = 0;
            foreach (string column in tableColumns.values)
            {
                if (instruction.Contains("INSERT"))
                {
                    strings.Add($"{element(column, tableColumns.field, fieldType)}");
                }
                else
                {
                    strings.Add($"{element(column, tableColumns.field, "condition")} = {element(column, tableColumns.field, "value")}");
                }
                counter++;
            }
            return new StringBuilder(100).Append(string.Join(", ", strings));
        }
        private static StringBuilder element(string column, string table, string type)
        {
            StringBuilder query = new StringBuilder(30);
            if (type != "condition")
            {
                query.Insert(query.Length, $"@{column}");
            }
            else
            {
                query.Insert(query.Length, checkCondition(column, table));
            }
            return query;
        }

        private static string checkCondition(string column, string table)
        {
            string fix;
            if (column == "id" && table != "enrollment")
            {
                if (table == "vehicle")
                {
                    fix = "enrollmentId";
                }
                else
                {
                    fix = "vehicleId";
                }
            }
            else
            {
                fix = column;
            }
            return fix;
        }
        private static StringBuilder buildCondition(string whereColumn, whereFieldValues whereValues)
        {
            StringBuilder query = new StringBuilder($"{whereColumn} {whereValues.key} ", 60);
            if (whereValues.values.Count != 1)
            {
                if (whereValues.key == "IN")
                {
                    query.Append($"({string.Join(", ", whereValues.values)})");
                }
                else
                {
                    query.Append(string.Join(" AND ", whereValues.values));
                }
            }
            else
            {
                query.Append(whereValues.values[0]);
            }
            return query;
        }

        public class iQuery
        {
            public IList<FieldValues> tablesColumns { get; set; }
            public IList<whereFieldValues> whereValues { get; set; }
            public IList<FieldValues> innerValues { get; set; }
        }
        public class FieldValues
        {
            public string field { get; set; }
            public IList<string> values { get; set; }
            public string output { get; set; }
        }
        public class whereFieldValues : FieldValues
        {
            public string key { get; set; }
        }
        public class Condition_values
        {
            public string condition { get; set; }
            public IList<string> values { get; set; }
            public string union_type { get; set; }
        }

        public class Where
        {
            public IList<whereFieldValues> where_fields_values { get; set; }
            public string key { get; set; }
        }
        public class From
        {
            public IList<string> tables { get; set; }
            public IList<Condition_values> conditions { get; set; }
        }
        public class Select
        {
            public IList<FieldValues> fields { get; set; }

            public Where where { get; set; }

            public From from { get; set; }
        }
    }
}
