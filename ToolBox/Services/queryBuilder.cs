using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        public static FieldNames check_tag_name(FieldNames column, TableNames table)
        {
            if (column.Equals(FieldNames.id) && !table.Equals(TableNames.enrollment))
                if (table.Equals(TableNames.vehicle))
                    return FieldNames.enrollmentId;
                else
                    return FieldNames.vehicleId;
            else
                return column;
        }
        public enum QueryWords
        {
            SELECT,
            UPDATE,
            DELETE,
            INSERT,
            ON,
            IN,
            BETWEEN,
            WHERE,
            AND,
            OR,
            NOT,
            SET,
            INNER,
            JOIN,
            INTO,
            VALUES,
            FROM,
            DECLARE,
            OUTPUT,
            USE,
            DROP,
            TRUNCATE,
            COUNT
        }
        public enum TableNames
        {
            enrollment,
            vehicle,
            door,
            wheel
        }
        public enum FieldNames
        {
            id,
            enrollmentId,
            vehicleId,
            serial,
            number,
            color,
            engineIsStarted,
            engineHorsePower,
            isOpen,
            pressure
        }
    }
}
