using System.Collections.Generic;
using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        //SELECT
        //1+ fields(not all)
        public static string select(params string[] fields)
        {
            return $"select {string.Join(", ", fields)}";
        }
        //all fields
        public static string select()
        {
            return "SELECT *";
        }
        //FROM
        public static string from(string table_name)
        {
            return $"FROM {table_name}";
        }
        //INNER JOIN
        public static string inner_join(string table_name)
        {
            return $"INNER JOIN {table_name}";
        }
        //ON condition
        public static string on()
        {
            return "ON";
        }
        //condition = value
        public static string condition_value(string condition, string value)
        {
            return $"{condition} = {value}";
        }
        //IN (value_list)
        public static string condition_in_values(string condition, params string[] values)
        {
            return $"{condition} IN ({string.Join(", ", values)})";
        }
        //BETWEEN 
        public static string between(string a, string b)
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
        public string where(string condition, string[] values)
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
        public static string update(string table_name)
        {
            return $"UPDATE {table_name}";
        }
        public static string setData(params FieldValue[] fields)
        {
            string[] strings = new string[fields.Length];
            int counter = 0;
            foreach (FieldValue fieldValue in fields)
            {
                strings[counter] = equal(fieldValue.field, fieldValue.value);
                counter++;
            }
            return $"SET {string.Join(", ", strings)}";
        }
        //INSERT
        public static string insert(string table_name)
        {
            return $"INSERT INTO {table_name}";
        }
        //SET
        public static string setData(string fields, string values)
        {
            return $"({fields}) VALUES ({values})";
        }
        public static string setData(string fields, string output, string values)
        {
            return $"({fields}) {output} VALUES ({values})";
        }
        public static string setData(string values)
        {
            return $"VALUES ({values})";
        }
        public static string fields_values(params string[] fields_values)
        {
            return $"{string.Join(", ", fields_values)}";
        }
        public static StringBuilder element(string column, string table, string type)
        {
            StringBuilder query = new StringBuilder(30);
            if (type != "condition")
            {
                query.Append($"@{column}");
            }
            else
            {
                query.Append(check_tag_name(column, table));
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
        public class FieldValue
        {
            public string field { get; set; }
            public string value { get; set; }
        }
    }
}
