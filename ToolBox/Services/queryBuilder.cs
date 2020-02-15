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

        public StringBuilder complexSelect()
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            IList<string> strings = new List<string>();
            foreach (FieldValues fieldvalues in this.iquery.tablesColumns)
            {
                for (int i = 0; i < fieldvalues.values.Count; i++)
                {
                    strings.Add($"{fieldvalues.field[0]}.{fieldvalues.values[i]}");
                }
                counter++;
            }
            query.Insert(query.Length, $"SELECT {string.Join(", ", strings)} FROM ");
            counter = 0;
            strings = new List<string>();
            foreach (FieldValues fieldvalues in this.iquery.tablesColumns)
            {
                strings.Add($"{fieldvalues.field} {fieldvalues.field[0]}");
                counter++;
            }
            query.Insert(query.Length, $"{string.Join(" INNER JOIN ", strings)} ON ");
            counter = 0;
            strings = new List<string>();
            foreach (FieldValues joinCondition in this.iquery.innerValues)
            {
                strings.Add($"{joinCondition.field} = {joinCondition.values[0]}");
                counter++;
            }
            query.Insert(query.Length, $"{string.Join(" AND ", strings)} {where(this.iquery)}");
            return query;
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
            foreach (whereFieldValues wherevalues in iquery.whereValues)
            {
                strings.Add($"{buildCondition(checkCondition(wherevalues.field, iquery.tablesColumns[0].field), wherevalues)}");
                counter++;
            }
            return new StringBuilder(100).Append($" WHERE {string.Join(" AND ", strings)}");
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
    }
}
