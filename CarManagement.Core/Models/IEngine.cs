using System;

namespace CarManagement.Core.Models
{
    public interface IEngine
    {
        int HorsePower { get; }
        bool IsStarted { get; }

        void start();
        void stop();
    }
}