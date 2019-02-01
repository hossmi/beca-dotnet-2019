namespace CarManagement.Core.Models
{
    public interface IDoor
    {
        bool IsOpen { get; }

        void close();
        void open();
    }
}