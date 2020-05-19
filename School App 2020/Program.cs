using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace School_App_2020
{
    class Program
    {
        static  void Main(string[] args)
        {

          




            TUI.Paint();
            
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                ConsoleKey x = keyInfo.Key;
                if (x == ConsoleKey.RightArrow)
                    TUI.PlayerMove(1);
                //else if (x == ConsoleKey.Spacebar)
                else if (x == ConsoleKey.LeftArrow)
                    TUI.PlayerMove(-1);

            }


            if (Console.ReadKey().Key == ConsoleKey.R)
                Main(args);
        }
    }
}
