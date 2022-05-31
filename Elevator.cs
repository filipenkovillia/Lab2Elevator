using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2Elevator
{
    public class Elevator
    {
        public const int MaxPassengersCount = 3;
        public const int MinFloor = 1;
        public const int MaxFloor = 5;
        public List<Passenger> PassengersInElevator = new List<Passenger>();
        public List<Passenger> PassengersQueue = new List<Passenger>();
        public ElevatorState CurrentState = new ElevatorState();

        public void PassengerRequest(Passenger passenger)
        {
            AddPassengerToQueue(passenger);
            if(CurrentState.State == State.Free)
            {
                GoToFloorWithoutPassengers(passenger.Departure);
                if (passenger.Direction == Direction.Up)
                    GoUp();
                if (passenger.Direction == Direction.Down)
                    GoDown();
            }
        }

        private void AddPassengerToQueue(Passenger passenger)
        {
            PassengersQueue.Add(passenger);
        }

        private void GoToFloorWithoutPassengers(int floor)
        {
            CurrentState.CurrentFloor = floor;
        }

        private void GoUp()
        {
            if (CurrentState.CurrentFloor == MaxFloor)
                throw new Exception("You are on the last floor already");
            CurrentState.State = State.Busy;
            CurrentState.Direction = Direction.Up;

            List<Passenger> passengersWithSameDirection = PassengersQueue.Where(p => p.Departure == CurrentState.CurrentFloor && 
                                                                                     p.Direction == Direction.Up)
                                                                         .ToList();

            AddPassengersToElevator(passengersWithSameDirection);

            List<Passenger> arrivedPassengers = PassengersInElevator.Where(p => p.Destination == CurrentState.CurrentFloor).ToList();
            PrintArrivedPassengers(arrivedPassengers);
            RemoveArrivedPassengers();
            PrintElevatorState();
            CurrentState.CurrentFloor++;

            if (CurrentState.CurrentFloor == MaxFloor)
            {
                CurrentState.State = State.Free;
            }
            else
            {
                GoUp();
            }
        }

        private void GoDown()
        {
            if (CurrentState.CurrentFloor == MinFloor)
                throw new Exception("You are on the first floor already");
            CurrentState.State = State.Busy;
            CurrentState.Direction = Direction.Down;
            List<Passenger> passengersWithSameDirection = PassengersQueue.Where(p => p.Departure == CurrentState.CurrentFloor && 
                                                                                     p.Direction == Direction.Down)
                                                                         .ToList();
            AddPassengersToElevator(passengersWithSameDirection);

            List<Passenger> arrivedPassengers = PassengersInElevator.Where(p => p.Destination == CurrentState.CurrentFloor).ToList();
            PrintArrivedPassengers(arrivedPassengers);
            RemoveArrivedPassengers();
            PrintElevatorState();
            CurrentState.CurrentFloor--;
            if(CurrentState.CurrentFloor == MinFloor)
            {
                CurrentState.State = State.Free;
                return;
            }
            else
            {
                GoDown();
            }
        }

        public bool IsFull()
        {
            return PassengersInElevator.Count >= MaxPassengersCount;
        }

        private void AddPassengersToElevator(List<Passenger> passengers)
        {
            int freePlaces = MaxPassengersCount - PassengersInElevator.Count;
            List<Passenger> newPassengers = passengers.Take(freePlaces).ToList();
            PassengersInElevator.AddRange(newPassengers);
            RemovePassengersFromQueue(newPassengers);
        }

        private void RemovePassengersFromQueue(List<Passenger> passengers)
        {
            foreach(Passenger passenger in passengers)
            {
                PassengersQueue.Remove(passenger);
                passenger.Arrived = true;
                passenger.Departure = CurrentState.CurrentFloor;
                PrintStartingPassenger(passenger);
            }
        }

        private void RemoveArrivedPassengers()
        {
            PassengersInElevator = PassengersInElevator.Where(p => p.Destination != CurrentState.CurrentFloor).ToList();
        }

        private void PrintStartingPassenger(Passenger passenger)
        {
            Console.WriteLine($"Starting Passenger {passenger.Name}");
        }

        private void PrintArrivedPassengers(List<Passenger> arrivedPassengers)
        {
            foreach (Passenger passenger in arrivedPassengers)
                Console.WriteLine($"Arrived Passenger {passenger.Name}");
        }

        private void PrintElevatorState()
        {
            Console.WriteLine($"Elevator State: {CurrentState.CurrentFloor}, " +
                                              $"{Enum.GetName(CurrentState.Direction)}, " +
                                              $"{Enum.GetName(CurrentState.State)}");
        }
    }


    public enum State
    {
        Busy,
        Free
    }

    public enum Direction
    {
        Up,
        Down
    }
}
