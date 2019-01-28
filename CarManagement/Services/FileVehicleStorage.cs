using CarManagement.Models;
using System.IO;

namespace CarManagement.Services
{
    public class FileVehicleStorage : IVehicleStorage
    {
        private StreamWriter writerFileVehicleStorage = new StreamWriter("FileVehicleStorage.txt");
        private StreamReader readFileVehicleStorate;

        public int Count { get; }

        public void clear()
        {
            throw new System.NotImplementedException();
        }

        public Vehicle get(IEnrollment defaultEnrollment)
        {
            throw new System.NotImplementedException();
        }

        public void set(Vehicle motoVehicle)
        {
            
        }
    }
}