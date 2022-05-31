using System;

namespace Lab2Elevator
{
    class Program
    {
        static void Main(string[] args)
        {
            Elevator elevator = new Elevator();
            for (int i = 0; i < 10; i++)
            {
                Passenger passenger = new Passenger(elevator, i);
            }
        }
    }
}
