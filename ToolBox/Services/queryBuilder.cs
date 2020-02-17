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
            return selectDelete($"SELECT {this.iquery.tablesColumns[0].values[0]}", this.iquery);
        }
        public StringBuilder delete()
        {
            return selectDelete("DELETE", this.iquery);
        }
        //SELECT [fields(s)1-n] 
        //FROM [table(s)1-n] (INNER JOIN [table_n] ON [condition_n] = [value_n])
        //(optional)WHERE [condition(s) 1-n]
        private string select(IList<string> fields)
        {
            return string.Join(", ", fields);
        }
        private string where()
        {
            //case 1: condition_1 = value1 AND/OR condition_2 = value_2
            //2 list(string) + union type

            //case 2: conditions_group_1/condition_1 AND/OR condition_2/ conditions_group_2
            //2 list(string) + union type

            //case 3: condition BETWEEN value_1 AND value_2
            //2 list string + union type -> list(condition_values object)

            //case 4: condition 1-n IN (value_list) 1-n
            //list(string) + list(list(string)) + union type
            return "";
        }

        public string from(IList<string> tables, IList<Condition_value> conditions = null)
        {
            int counter = 0;
            int counter2 = 0;
            string query = "FROM ";
            foreach (string table in tables)
            {
                query += table;
                if (counter >=1 && counter < tables.Count)
                {
                    query += $" ON {conditions[counter2].condition} = {conditions[counter2].value}";
                    counter2++;
                }
                if (tables.Count > 1 && counter < tables.Count-1)
                {
                    query += " INNER JOIN ";
                }
                counter++;
            }

            return query;
        }


        private static StringBuilder selectDelete(string instruction, iQuery iquery)
        {
            StringBuilder query = new StringBuilder(30);
            query.Insert(query.Length, $"{instruction} FROM {iquery.tablesColumns[0].field}");
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
                    query.Insert(query.Length, $"({string.Join(", ", whereValues.values)})");
                }
                else
                {
                    query.Insert(query.Length, string.Join(" AND ", whereValues.values));
                }
            }
            else
            {
                query.Insert(query.Length, whereValues.values[0]);
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
        public class Condition_value
        {
            public string condition { get; set; }
            public string value { get; set; }
            public string union_type { get; set; }
        }
    }
}
