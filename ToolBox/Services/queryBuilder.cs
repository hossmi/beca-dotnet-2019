using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
        public static FieldNames check_tag_name(FieldNames column, TableNames table)
        {
            return column.Equals(FieldNames.id) && !table.Equals(TableNames.enrollment) ? (column.Equals(FieldNames.id) && table.Equals(TableNames.vehicle) ? FieldNames.enrollmentId : FieldNames.vehicleId) : column;
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
