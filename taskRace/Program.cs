using System;
using System.Threading;
using System.Threading.Tasks;

namespace threadRace
{
    internal class Program
    {
        private static readonly object lockObject = new object(); 
        private static bool raceWon = false; 
        private static int t1Location = 0, t2Location = 0, t3Location = 0, t4Location = 0, t5Location = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Ready, Set, Go...");

            
            Task t1 = Task.Run(() => RunRace(ref t1Location, "Speedy Gonzales"));
            Task t2 = Task.Run(() => RunRace(ref t2Location, "Road Runner"));
            Task t3 = Task.Run(() => RunRace(ref t3Location, "Flash"));
            Task t4 = Task.Run(() => RunRace(ref t4Location, "Sonic"));
            Task t5 = Task.Run(() => RunRace(ref t5Location, "Quick Silver"));

            
            Task.WaitAny(new[] { t1, t2, t3, t4, t5 });

            Console.WriteLine("Race has ended");
        }

        static void RunRace(ref int location, string runnerName)
        {
            while (location < 100 && !raceWon)
            {
                MoveIt(ref location, runnerName);
                Thread.Sleep(10); 
            }
        }

        static void MoveIt(ref int location, string runnerName)
        {
            lock (lockObject)
            {
                if (!raceWon && location < 100) 
                {
                    location++;
                    Console.WriteLine($"{runnerName} location={location}");
                    if (location >= 100)
                    {
                        raceWon = true; 
                        Console.WriteLine($"{runnerName} is the winner!");
                    }
                }
            }
        }
    }
}
