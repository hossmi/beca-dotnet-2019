﻿namespace CarManagement.Models
{
    public class Engine
    {
        private string model;
        private int horsePower;
        private bool isStarted;

        public Engine(int horsePower, string model = "standart")
        {
            this.model = model;
            this.horsePower = horsePower;
        }

        public Engine(Engine engine)
        {
            this.model = engine.model;
            this.horsePower = engine.horsePower;
        }

        public void Start()
        {
            this.isStarted = true;
        }

        public string Model { get => model; }
        public int HorsePower { get => horsePower; }
        public bool IsStarted { get => isStarted; }
    }
}