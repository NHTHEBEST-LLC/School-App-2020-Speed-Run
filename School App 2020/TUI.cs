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

                SetCords(new Cords(4, Terrain[4]+2));
                //LC.WriteChar((char)30, (short)0x2);

                //await Task.Delay(100);
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
            SetCords(Player);
            LC.WriteChar(' ', 0x33);
            int x = Player.X;
            int _new = x + info;
            if (_new >=0 && _new < Console.WindowWidth)
            {
                _Player.X = _new;
            }
            PaintPlayer();
        }


        // us char 30 ▲ for trash
        
        static void PaintPlayer()
        {
            //PrintAT(2, Player, ConsoleColor.Cyan, ConsoleColor.Red);
            SetCords(Player);

            if (DateTime.Now.Second%2==0)
                LC.WriteChar((char)2, 0x34);
            else
                LC.WriteChar((char)1, 0x34);
            

        }

 

       static void PrintAT(string text, Cords cords, int color)
        {
            
            //SetColor(background, forground);
            SetCords(cords);
            //Console.Write(text);


            LC.Write(text, (char)color);
        }

        static void SetCords(Cords cords)
        {
            //Console.SetCursorPosition(cords.X, Console.WindowHeight - cords.Y);
            LC.setCuser(cords.X, LC.Height - cords.Y);
        }



    }
}
