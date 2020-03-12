using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School_App_2020
{
    struct Cords
    {
        public int X;
        public int Y;
    }
    static class TUI
    {
        public static Cords Player { private set; get; }
        static public void Paint()
        {
            PaintPlayer();
        }

        static void PaintPlayer()
        {
            PrintAT(Convert.ToChar(946), Player, ConsoleColor.Black, ConsoleColor.Blue);
        }


       static void PrintAT(object text, Cords cords, ConsoleColor background, ConsoleColor forground)
        {
            SetColor(background, forground);
            SetCords(cords);
            Console.Write(text);
        }
        static void SetColor(ConsoleColor background, ConsoleColor forground)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = forground;
        }
        static void SetCords(Cords cords)
        {
            Console.SetCursorPosition(cords.X, Console.WindowHeight - cords.Y);
        }
    }
}
