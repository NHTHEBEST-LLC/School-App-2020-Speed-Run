using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LowConsole;

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
        private static Cords _Player = new Cords(0, 0);
        private static int[] Terrain = GenTerrain(7, 3);
        public static Cords Player 
        { 
            private set 
            { 
                _Player = value; 
            } 
            get 
            {
                int x = _Player.X;
                int y = Terrain[x] + 1;
                return new Cords(x,y);
            } 
        }
        
        static public async Task Paint()
        {
            while (true)
            {
                Console.CursorVisible = false;

                //SetColor(ConsoleColor.Cyan, ConsoleColor.Cyan);
                //Console.Clear();
                LC.Clear(0x33);
                PaintTerrain();
                PaintPlayer();
                
                await Task.Delay(200);
            }
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
            int width = Console.WindowWidth-1;
            
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
            
            foreach (int my in Terrain)
            {
                for (int y = 1; y <= my; y++)
                {
                    int curser = x+( LC.Height - y)*LC.Width;
                    LC.Buffer[curser].Char.AsciiChar = 219;
                    LC.Buffer[curser].Attributes = 0x2;
                }
                    //printAtUnder(219, new Cords(x,y),  ConsoleColor.Green);
                x++;
            }
        }

        public static void PlayerMove(int info)
        {
            PrintAT(" ", Player, ConsoleColor.Cyan, ConsoleColor.Cyan);
            int x = Player.X;
            int _new = x + info;
            if (_new >=0 && _new < Console.WindowWidth - 2)
            {
                _Player.X = _new;
            }
            PaintPlayer();
        }

        public static void PlayerJump(int info)
        {
            SetCords(Player);
            LC.WriteChar((char)240, 0x3b);
            int x = Player.X;
            int _new = x + info;
            if (_new >= 0 && _new < Console.WindowWidth - 1)
            {
                _Player.X = _new;
            }
            PaintPlayer();
        }

        public static void Log(object text)
        {
            PrintAT(text, new Cords(0, 0), ConsoleColor.Cyan, ConsoleColor.Black);
        }

        static void PaintPlayer()
        {
            //PrintAT(2, Player, ConsoleColor.Cyan, ConsoleColor.Red);
            SetCords(Player);
            LC.WriteChar((char)2, 0x31);
        }

        static void printAtUnder(object text, Cords cords, ConsoleColor color)
        {
            int y = cords.Y;

            for (int i = 0; i <= y; i++)
                PrintAT(text, new Cords(cords.X, i), color, color);
        }

       static void PrintAT(object text, Cords cords, ConsoleColor background, ConsoleColor forground)
        {
            
            //SetColor(background, forground);
            SetCords(cords);
            //Console.Write(text);
            int col = (int)background << 4;
            col += (int)forground;

            LC.Write(text.ToString(), (char)col);
        }
        static void SetColor(ConsoleColor background, ConsoleColor forground)
        {

            Console.BackgroundColor = background;
            Console.ForegroundColor = forground;
        }
        static void SetCords(Cords cords)
        {
            //Console.SetCursorPosition(cords.X, Console.WindowHeight - cords.Y);
            LC.setCuser(cords.X, LC.Height - cords.Y);
        }



    }
}
