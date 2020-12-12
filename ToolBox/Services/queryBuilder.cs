using System.Text;

namespace ToolBox.Services
{
    public class QueryBuilder
    {
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
