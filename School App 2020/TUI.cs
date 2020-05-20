using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        private static bool[] Trash = GenTrash(1);
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
        static int fps = 1000;
        static public void Paint()
        {
            new Thread(() =>
            {
                while (true)
                {
                    int del = 1000 / fps;
                    Console.CursorVisible = false;
                    var sw = Stopwatch.StartNew();
                    
                    LC.Clear(0x33);
                    PaintTerrain();
                    PaintPlayer();

                    
                    var ms = (int)sw.ElapsedMilliseconds;
                    if (ms < del)
                        Thread.Sleep(del - ms);

                }
            }).Start();
        }


        /// <summary>
        /// This Function is not Enviromenaly Frendly 
        /// </summary>
        /// <param name="onein"></param>
        /// <returns></returns>
        static bool[] GenTrash(int onein)
        {
            Random r = new Random();
            int width = LC.Width - 1;

            List<bool> trash = new List<bool>();

            trash.Add(false);
            for (int i = 0; i < width; i += 2)
            {
                int am = r.Next(onein+1);
                trash.Add(am == 1);
                trash.Add(false);
            }

            return trash.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">start hight 1-9</param>
        /// <param name="tend">to up or down 5 normal</param>
        /// <returns>Terrain</returns>
        static int[] GenTerrain(int start = 5, int tend = 5)
        {
            Random r = new Random();
            int width = LC.Width-1;
            
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
                    if (curser >= LC.Buffer.Length)
                        curser--;
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
