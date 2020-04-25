using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        public static StringBuilder setData(params FieldValue[] fields)
        {
            string[] strings = new string[fields.Length];
            int counter = 0;
            foreach (FieldValue fieldValue in fields)
            {
                strings[counter] = $"{$"{fieldValue.field} = {fieldValue.value}"}";
                counter++;
            }
            return new StringBuilder($"{QueryWords.SET} {string.Join(", ", strings)}");
        }
        public static StringBuilder setData(params string[] strings)
        {
            switch (strings.Length)
            {
                case 1:
                    return new StringBuilder($"{QueryWords.VALUES} ({strings[0]})");
                case 2:
                    return new StringBuilder($"({strings[0]}) {QueryWords.VALUES} ({strings[1]})");
                case 3:
                    return new StringBuilder($"({strings[0]}) {strings[1]} {QueryWords.VALUES} ({strings[2]})");
                default:
                    return null;
            }
        }
        public static StringBuilder element(FieldNames column, TableNames table, string type)
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
        public static FieldNames check_tag_name(FieldNames column, TableNames table)
        {
            FieldNames fix;
            if (column == FieldNames.id && table != TableNames.enrollment)
            {
                if (table == TableNames.vehicle)
                {
                    fix = FieldNames.enrollmentId;
                }
                else
                {
                    fix = FieldNames.vehicleId;
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

        public enum QueryWords
        {
            SELECT = 0,
            UPDATE = 1,
            DELETE = 2,
            INSERT = 3,
            ON = 4,
            IN = 5,
            BETWEEN = 6,
            WHERE = 7,
            AND = 8,
            OR = 9,
            NOT = 10,
            SET = 11,
            INNER = 12,
            JOIN = 13,
            INTO = 14,
            VALUES = 15,
            FROM = 16,
            DECLARE = 17,
            OUTPUT = 18,
            USE = 19,
            DROP = 20,
            TRUNCATE = 21,
            COUNT = 22
        }
        public enum TableNames
        {
            enrollment = 0,
            vehicle = 1,
            door = 2,
            wheel = 3
        }
        public enum FieldNames
        {
            id = 0,
            enrollmentId = 1,
            vehicleId = 2,
            serial = 3,
            number = 4,
            color = 5,
            engineIsStarted = 6,
            engineHorsePower = 7,
            isOpen = 8,
            pressure = 9
        }
    }
}
