using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2Elevator
{
    public class ElevatorState
    {
        public State State { get; set; }
        public int CurrentFloor { get; set; }
        public Direction Direction { get; set; }

        public ElevatorState()
        {
            State = State.Free;
            CurrentFloor = 1;
            Direction = Direction.Up;
        }
    }
}
