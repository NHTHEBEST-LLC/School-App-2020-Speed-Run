using System;

namespace School_App_2020
{
    class Program
    {
        /// <summary>
        /// this is the main funtion thats starts the program
        /// </summary>
        /// <param name="args">command line args (not used)</param>
        static void Main(string[] args)
        {
            // start the painting of the console game
            TUI.Paint();
            
            // forever
            while (true)
            {
                // read key
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                ConsoleKey x = keyInfo.Key;

                // check if key pressed does somthing
                if (x == ConsoleKey.RightArrow)
                    TUI.PlayerMove(1); // move right one
                else if (x == ConsoleKey.LeftArrow)
                    TUI.PlayerMove(-1); // move left one
                else if (x == ConsoleKey.Spacebar)
                    TUI.Pickup(); // try to pick up
                else if (x == ConsoleKey.R)
                    TUI.Restart(); // restart game
                else if (x == ConsoleKey.Escape)
                    TUI.Cheat(true); // cheat the whole game
                else if (x == ConsoleKey.N)
                    TUI.Cheat(false); // cheat to next trash
                else if (x == ConsoleKey.NumPad0)
                    TUI.HSR(); // reset high score
                else if (x == ConsoleKey.F)
                    TUI.Spead(); // set up for spead run
            }
        }
    }
}
