using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace School_App_2020
{
    class Program
    {
        static  void Main(string[] args)
        {

            TUI.Paint();
            if (Console.ReadKey().Key == ConsoleKey.R)
                Main(args);
        }
    }
}
