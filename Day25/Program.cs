using System;
using System.Collections.Generic;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            PartOne();
            Console.ReadLine();
        }

        static void PartOne()
        {
            var machine = new TuringMachine();
            for (int i = 0; i < 12_861_455; i++)
            {
                machine.Run();
                if(i % 128614 == 0)
                {
                    Console.WriteLine("Working..." + i / 128614 + "%");
                }
            }
            Console.WriteLine(machine.GetTrueCount());
        }
    }

    class TuringMachine
    {
        public List<bool> Tape { get; set; }
        public char State { get; set; }
        public int CurrentPosition { get; set; }

        public TuringMachine()
        {
            Tape = new List<bool>();
            State = 'A';
            CurrentPosition = 0;
        }

        public int GetTapeState()
        {
            if(CurrentPosition < 0)
            {
                CurrentPosition++;
                Tape.Insert(0, false);
            }
            else if(CurrentPosition == Tape.Count)
            {
                Tape.Add(false);
            }
            
            return Tape[CurrentPosition] ? 1 : 0;
        }

        public void SetTapeState(int state)
        {
            Tape[CurrentPosition] = state == 1;
        }

        public void Run()
        {
            switch(State)
            {
                case 'A':
                    RunA();
                    break;
                case 'B':
                    RunB();
                    break;
                case 'C':
                    RunC();
                    break;
                case 'D':
                    RunD();
                    break;
                case 'E':
                    RunE();
                    break;
                case 'F':
                    RunF();
                    break;
            }
        }

        public void MoveRight()
        {
            CurrentPosition++;
        }

        public void MoveLeft()
        {
            CurrentPosition--;
        }

        public void RunA()
        {
            if (GetTapeState() == 0)
            {
                SetTapeState(1);
                MoveRight();
                State = 'B';
            }
            else
            {
                SetTapeState(0);
                MoveLeft();
                State = 'B';
            }
        }

        public void RunB()
        {
            if (GetTapeState() == 0)
            {
                SetTapeState(1);
                MoveLeft();
                State = 'C';
            }
            else
            {
                SetTapeState(0);
                MoveRight();
                State = 'E';
            }
        }

        public void RunC()
        {
            if (GetTapeState() == 0)
            {
                SetTapeState(1);
                MoveRight();
                State = 'E';
            }
            else
            {
                SetTapeState(0);
                MoveLeft();
                State = 'D';
            }
        }

        public void RunD()
        {
            if(GetTapeState() == 0)
            {
                SetTapeState(1);
                MoveLeft();
                State = 'A';
            }
            else
            {
                SetTapeState(1);
                MoveLeft();
                State = 'A';
            }
        }

        public void RunE()
        {
            if (GetTapeState() == 0)
            {
                SetTapeState(0);
                MoveRight();
                State = 'A';
            }
            else
            {
                SetTapeState(0);
                MoveRight();
                State = 'F';
            }
        }

        public void RunF()
        {
            if (GetTapeState() == 0)
            {
                SetTapeState(1);
                CurrentPosition++;
                State = 'E';
            }
            else
            {
                SetTapeState(1);
                CurrentPosition--;
                State = 'A';
            }
        }

        public int GetTrueCount()
        {
            return Tape.Count(c => c);
        }
    }
}
