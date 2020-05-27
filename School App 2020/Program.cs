﻿using System;
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
            // start the painting of the console game
            // this function is does not return 
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
                else if (x == ConsoleKey.Spacebar)
                    TUI.Pickup();
                else if (x == ConsoleKey.R)
                    TUI.Restart();

            }
        }
    }
}
