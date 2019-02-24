using System;

namespace CarManagement.Core.Models
{
    public interface IEngine
    {
        int HorsePower { get; set; }
        bool IsStarted { get; set; }

        void start();
        void stop();
    }
}