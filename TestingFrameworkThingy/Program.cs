using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestingFrameworkThingy
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new TestRunner();
            runner.Run();
            Console.ReadLine();
        }

    } 
}
