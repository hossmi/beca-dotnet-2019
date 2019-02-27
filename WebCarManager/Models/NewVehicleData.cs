using CarManagement.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace WebCarManager.Models
{
    public class NewVehicleData
    {
        [Required(ErrorMessage ="Numero de ruedas no puede estar vacio.")]
        [Range(1,4,ErrorMessage ="Las ruedas deben estar entre 1 y 4")]
        public int WheelCount{ get; set; }

        [Range(0, 6, ErrorMessage = "Las puertas deben estar entre 0 y 6")]
        public int DoorCount { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La potencia del motor tiene que ser positiva.")]
        public int HorsePower { get; set; }

        public bool IsStarted { get; set; }

        [Required]
        public CarColor Color { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La presion tiene que ser mayor o igual que 1.")]
        public double Pressure { get; set; }
    }
}