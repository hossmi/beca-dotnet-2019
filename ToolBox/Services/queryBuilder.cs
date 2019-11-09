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
            query.Insert(query.Length, "SELECT ");
            int counter = 0;
            foreach (FieldValues fieldvalues in this.iquery.tablesColumns)
            {
                for (int i = 0; i < fieldvalues.values.Count; i++)
                {
                    query.Insert(query.Length, $"{fieldvalues.field[0]}.{fieldvalues.values[i]}");
                    if (i < fieldvalues.values.Count - 1)
                        query.Insert(query.Length, ", ");
                }
                if (counter < this.iquery.tablesColumns.Count - 1)
                    query.Insert(query.Length, ", ");
                counter++;
            }
            query.Insert(query.Length, " FROM ");
            counter = 0;
            foreach (FieldValues fieldvalues in this.iquery.tablesColumns)
            {
                query.Insert(query.Length, $"{fieldvalues.field} {fieldvalues.field[0]}");
                if (counter < this.iquery.tablesColumns.Count - 1)
                    query.Insert(query.Length, " INNER JOIN ");
                counter++;
            }
            query.Insert(query.Length, " ON ");
            counter = 0;
            foreach (FieldValues joinCondition in this.iquery.innerValues)
            {
                query.Insert(query.Length, $"{joinCondition.field} = {joinCondition.values[0]}");
                if (counter < this.iquery.innerValues.Count - 1)
                    query.Insert(query.Length, " AND ");
                counter++;
            }
            query.Insert(query.Length, where(this.iquery));
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
            query.Insert(query.Length, $"INSERT INTO {this.iquery.tablesColumns[0].field} ({addFields(this.iquery.tablesColumns[0], "INSERT", "condition")}) VALUES ({addFields(this.iquery.tablesColumns[0], "INSERT", "value")})");
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
                query.Insert(query.Length, where(iquery));
            return query;
        }
        private static StringBuilder where(iQuery iquery)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (whereFieldValues wherevalues in iquery.whereValues)
            {
                if (counter == 0 || iquery.whereValues.Count == 1)
                    query.Insert(query.Length, " WHERE ");
                else if (counter > 0)
                    query.Insert(query.Length, " AND ");
                query.Insert(query.Length, buildCondition(checkCondition(wherevalues.field, iquery.tablesColumns[0].field), wherevalues));
                counter++;
            }
            return query;
        }

        private static StringBuilder addFields(FieldValues tableColumns, string instruction, string fieldType = null)
        {
            StringBuilder query = new StringBuilder(100);
            int counter = 0;
            foreach (string column in tableColumns.values)
            {
                if (instruction.Contains("INSERT"))
                    query.Insert(query.Length, element(column, tableColumns.field, fieldType));
                else
                    query.Insert(query.Length, $"{element(column, tableColumns.field, "condition")} = {element(column, tableColumns.field, "value")}");
                if (counter < tableColumns.values.Count - 1)
                    query.Insert(query.Length, ", ");
                counter++;
            }
            return query;
        }
        private static StringBuilder element(string column, string table, string type)
        {
            StringBuilder query = new StringBuilder(30);
            if (type != "condition")
                query.Insert(query.Length, $"@{column}");
            else
                query.Insert(query.Length, checkCondition(column, table));
            return query;
        }

        private static string checkCondition(string column, string table)
        {
            string fix;
            if (column == "id" && table != "enrollment")
            {
                if (table == "vehicle")
                    fix = "enrollmentId";
                else
                    fix = "vehicleId";
            }
            else
                fix = column;
            return fix;
        }
        private static StringBuilder buildCondition(string whereColumn, whereFieldValues whereValues)
        {
            StringBuilder query = new StringBuilder($"{whereColumn} {whereValues.key} ", 60);
            if (whereValues.values.Count != 1)
            {
                for (int i = 0; i < whereValues.values.Count; i++)
                {
                    if (whereValues.key == "IN")
                    {
                        query.Insert(query.Length, $"({whereValues.values[i]} ,");
                        if (i == whereValues.values.Count - 1)
                            query.Insert(query.Length, $")");
                    }
                    else
                    {
                        query.Insert(query.Length, whereValues.values[i]);
                        if (i == 0)
                            query.Insert(query.Length, $" AND ");
                    }
                }
            }
            else
                query.Insert(query.Length, whereValues.values[0]);
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
        }
        public class whereFieldValues : FieldValues
        {
            public string key { get; set; }
        }
    }
}
