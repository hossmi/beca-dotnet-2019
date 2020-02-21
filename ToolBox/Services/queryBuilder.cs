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

        //SELECT
        //2+ fields(not all)
        public static string select(IList<string> fields)
        {
            return $"select {string.Join(", ", fields)}";
        }
        //all fields
        public static string select()
        {
            return "SELECT *";
        }
        //1 field
        public static string select(string field)
        {
            return $"SELECT {field}";
        }
        //FROM
        public static string from(string table_name)
        {
            return $"FROM {table_name}";
        }
        //INNER JOIN
        public string inner_join(string table_name)
        {
            return $"INNER JOIN {table_name}";
        }
        //ON condition
        public string on()
        {
            return "ON";
        }
        //condition = value
        public string condition_value(string condition, string value)
        {
            return $"{condition} = {value}";
        }
        //IN (value_list)
        public string condition_in_values(string condition, IList<string> values)
        {
            return $"{condition} IN ({string.Join(", ", values)})";
        }
        //BETWEEN 
        public string between(string a, string b)
        {
            return $"BETWEEN {a} AND {b}";
        }
        public static string where()
        {
            return "WHERE";
        }
        //condition = value
        public static string equal(string condition, string value)
        {
            return $"{condition} = {value}";
        }
        //WHERE condition IN (value_list)
        public string where(string condition, IList<string> values)
        {
            return $"WHERE {condition_in_values(condition, values)}";
        }
        //WHERE condition_value and/or condition_value
        public string where_group(string conditions1, string instruction, string conditions2)
        {
            return $"WHERE {and_or(conditions1, instruction, conditions2)}";
        }
        //a and/or b
        public static string and_or(string a, string instruction, string b)
        {
            return $"{a} {instruction} {b}";
        }

        //UPDATE
        //soon

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
        public StringBuilder delete()
        {
            return selectDelete("DELETE", this.iquery);
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
                    strings.Add($"{buildCondition(check_tag_name(wherevalues.field, iquery.tablesColumns[0].field), wherevalues)}");
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
                query.Insert(query.Length, check_tag_name(column, table));
            }
            return query;
        }

        public static string check_tag_name(string column, string table)
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

        public class From
        {
            public IList<string> tables { get; set; }
            public IList<Condition_values> conditions { get; set; }
        }
    }
}
