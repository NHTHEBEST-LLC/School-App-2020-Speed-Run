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
        public Cords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    static class TUI
    {
        
        public static Cords Player { private set; get; }
        static public void Paint()
        {
            Console.CursorVisible = false;
            Player = new Cords(0, 3);
            SetColor(ConsoleColor.Cyan, ConsoleColor.Cyan);
            Console.Clear();
            PaintPlayer();
            PaintTerrain();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">start hight 1-9</param>
        /// <param name="tend">to up or down 5 normal</param>
        /// <returns></returns>
        static int[] GenTerrain(int start = 5, int tend = 5)
        {
            Random r = new Random();
            int width = Console.WindowWidth-2;
            
            List<int> terrain = new List<int>();
            terrain.Add(start);
            for (int i = 0; i<width;i+=2)
            {
                int last = terrain[i];
                int min = last*10 - 10 +tend;
                int max = last*10 + 10  +tend;
                if (min < 0)
                    min = 0;
                if (max >= 100)
                    max = 100;
                int next = r.Next(min, max + 1)/10;

                terrain.Add(next); terrain.Add(next);
            }
            return terrain.ToArray();
        }
        static void PaintTerrain()
        {
            int x = 0;
            int[] Terrain = GenTerrain(5, 2);
            foreach (int y in Terrain)
            {
                printAtUnder("_", new Cords(x,y),  ConsoleColor.Green);
                x++;
            }
        }

        

        static void PaintPlayer()
        {
            PrintAT(Convert.ToChar(946), Player, Console.BackgroundColor, ConsoleColor.Blue);
        }

        static void printAtUnder(object text, Cords cords, ConsoleColor color)
        {
            int y = cords.Y;

            for (int i = 0; i <= y; i++)
                PrintAT(text, new Cords(cords.X, i), color, color);
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
