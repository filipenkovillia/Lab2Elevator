using System;
using System.Threading.Tasks;

namespace Lab2Elevator
{
    public class Passenger
    {
        private const int CheckIsArrivedInterval = 100;
        public int ID { get; set; }
        public string Name { get; set; }
        public int Departure { get; set; }
        public int Destination { get; set; }
        public Direction Direction { get; set; }
        public bool Arrived { get; set; }
        public Elevator Elevator { get; set; }
        
        public Passenger(Elevator elevator, int id)
        {
            Elevator = elevator;
            ID = id;
            Name = $"client-{id}";
            Destination = 0;
            Departure = GetRandom();
            Direction = Direction.Up;
            Arrived = true;
            CheckIsArrived();
        }

        public void CheckIsArrived()
        {
            SetInterval(() => 
            {
                if (Arrived)
                    GoToFloor();
            }, new TimeSpan(CheckIsArrivedInterval));
        }

        public void GoToFloor()
        {
            Destination = GetRandom();
            Direction = Departure > Destination ? Direction.Down : Direction.Up;
            Arrived = false;
            Random rand = new Random();
            Task.Delay(rand.Next(5) * 1000).ContinueWith((task) => Elevator.PassengerRequest(this));
        }

        public int GetRandom()
        {
            Random rand = new Random();
            return rand.Next(Elevator.MinFloor, Elevator.MaxFloor + 1);
        }

        public static async Task SetInterval(Action action, TimeSpan timeout)
        {
            await Task.Delay(timeout).ConfigureAwait(false);

            action();

            SetInterval(action, timeout);
        }
    }
}
