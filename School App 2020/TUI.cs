using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
        private static int Score = 0;
        private static Stopwatch timer = new Stopwatch();
        private static bool Alive = true;
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
                return new Cords(x, y);
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

                    if (Alive)
                        LC.Clear(0x33);
                    else
                        LC.Clear(0x44);



                    if (Alive)
                    {
                        PrintAT(string.Format("Score = {0}", Score), new Cords(4, LC.Height - 3), 0x04);
                        PrintAT(string.Format("Time = {0}", timer.Elapsed.ToString("mm\\:ss\\.ff")), new Cords(4, LC.Height - 4), 0x04);
                        PaintTerrain();

                        PaintTrash();
                        PaintPlayer();
                    }

                    if (!Alive)
                    {
                        int mgsx = 20;
                        int msgy = 10;

                        //PrintAT(@"██╗░░░██╗░█████╗░██╗░░░██╗  ██████╗░██╗███████╗██████╗░", new Cords(mgsx, LC.Height - (msgy + 0)), 0x40);
                        //PrintAT(@"╚██╗░██╔╝██╔══██╗██║░░░██║  ██╔══██╗██║██╔════╝██╔══██╗", new Cords(mgsx, LC.Height - (msgy + 1)), 0x40);
                        //PrintAT(@"░╚████╔╝░██║░░██║██║░░░██║  ██║░░██║██║█████╗░░██║░░██║", new Cords(mgsx, LC.Height - (msgy + 2)), 0x40);
                        //PrintAT(@"░░╚██╔╝░░██║░░██║██║░░░██║  ██║░░██║██║██╔══╝░░██║░░██║", new Cords(mgsx, LC.Height - (msgy + 3)), 0x40);
                        //PrintAT(@"░░░██║░░░╚█████╔╝╚██████╔╝  ██████╔╝██║███████╗██████╔╝", new Cords(mgsx, LC.Height - (msgy + 4)), 0x40);
                        //PrintAT(@"░░░╚═╝░░░░╚════╝░░╚═════╝░  ╚═════╝░╚═╝╚══════╝╚═════╝░", new Cords(mgsx, LC.Height - (msgy + 5)), 0x40);

                        int[] l1 = new int[] { 219, 219, 187, 176, 176, 176, 219, 219, 187, 176, 219, 219, 219, 219, 219, 187, 176, 219, 219, 187, 176, 176, 176, 219, 219, 187, 32, 32, 219, 219, 219, 219, 219, 219, 187, 176, 219, 219, 187, 219, 219, 219, 219, 219, 219, 219, 187, 219, 219, 219, 219, 219, 219, 187, 176 };
                        int[] l2 = new int[] { 200, 219, 219, 187, 176, 219, 219, 201, 188, 219, 219, 201, 205, 205, 219, 219, 187, 219, 219, 186, 176, 176, 176, 219, 219, 186, 32, 32, 219, 219, 201, 205, 205, 219, 219, 187, 219, 219, 186, 219, 219, 201, 205, 205, 205, 205, 188, 219, 219, 201, 205, 205, 219, 219, 187 };
                        int[] l3 = new int[] { 176, 200, 219, 219, 219, 219, 201, 188, 176, 219, 219, 186, 176, 176, 219, 219, 186, 219, 219, 186, 176, 176, 176, 219, 219, 186, 32, 32, 219, 219, 186, 176, 176, 219, 219, 186, 219, 219, 186, 219, 219, 219, 219, 219, 187, 176, 176, 219, 219, 186, 176, 176, 219, 219, 186 };
                        int[] l4 = new int[] { 176, 176, 200, 219, 219, 201, 188, 176, 176, 219, 219, 186, 176, 176, 219, 219, 186, 219, 219, 186, 176, 176, 176, 219, 219, 186, 32, 32, 219, 219, 186, 176, 176, 219, 219, 186, 219, 219, 186, 219, 219, 201, 205, 205, 188, 176, 176, 219, 219, 186, 176, 176, 219, 219, 186 };
                        int[] l5 = new int[] { 176, 176, 176, 219, 219, 186, 176, 176, 176, 200, 219, 219, 219, 219, 219, 201, 188, 200, 219, 219, 219, 219, 219, 219, 201, 188, 32, 32, 219, 219, 219, 219, 219, 219, 201, 188, 219, 219, 186, 219, 219, 219, 219, 219, 219, 219, 187, 219, 219, 219, 219, 219, 219, 201, 188 };
                        int[] l6 = new int[] { 176, 176, 176, 200, 205, 188, 176, 176, 176, 176, 200, 205, 205, 205, 205, 188, 176, 176, 200, 205, 205, 205, 205, 205, 188, 176, 32, 32, 200, 205, 205, 205, 205, 205, 188, 176, 200, 205, 188, 200, 205, 205, 205, 205, 205, 205, 188, 200, 205, 205, 205, 205, 205, 188, 176 };

                        SetCords(new Cords(mgsx, LC.Height - (msgy + 0)));
                        LC.Writeints(l1, 0x40);
                        SetCords(new Cords(mgsx, LC.Height - (msgy + 1)));
                        LC.Writeints(l2, 0x40);
                        SetCords(new Cords(mgsx, LC.Height - (msgy + 2)));
                        LC.Writeints(l3, 0x40);
                        SetCords(new Cords(mgsx, LC.Height - (msgy + 3)));
                        LC.Writeints(l4, 0x40);
                        SetCords(new Cords(mgsx, LC.Height - (msgy + 4)));
                        LC.Writeints(l5, 0x40);
                        SetCords(new Cords(mgsx, LC.Height - (msgy + 5)));
                        LC.Writeints(l6, 0x40);

                        PrintAT(@"                    Press R To Restart                 ", new Cords(mgsx, LC.Height - (msgy + 7)), 0x40);
                        bool notrestart = true;
                        while (notrestart)
                        {
                            if (Console.ReadKey().Key == ConsoleKey.R)
                            {
                                timer.Reset();
                                timer.Stop();
                                first = true;
                                Alive = true;
                                Score = 0;
                                _Player = new Cords(0, 0);
                                Terrain = GenTerrain(7, 3);
                                Trash = GenTrash(1);
                                notrestart = false;
                            }
                        }
                    }


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
                int am = r.Next(onein + 1);
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
            int width = LC.Width - 1;

            List<int> terrain = new List<int>();
            terrain.Add(start);
            for (int i = 0; i < width; i += 2)
            {
                int last = terrain[i];
                int min = last * 10 - 10 + tend;
                int max = last * 10 + 10 + tend;
                if (min < 0)
                    min = 0;
                if (max >= 100)
                    max = 100;
                int next = r.Next(min, max + 1) / 10;
                if (next <= 0)
                    next = 1;
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

                    int curser = x + (LC.Height - y) * LC.Width;
                    if (curser >= LC.Buffer.Length)
                        curser--;
                    LC.Buffer[curser].Char.AsciiChar = 219;
                    LC.Buffer[curser].Attributes = 0x2;
                }
                //printAtUnder(219, new Cords(x,y),  ConsoleColor.Green);
                x++;
            }
        }

        static void PaintTrash()
        {
            int x = 0;
            CharInfo @char = new CharInfo();
            @char.Char.AsciiChar = 31;
            @char.Attributes = 0x25;
            foreach (int my in Terrain)
            {
                int curser = x + (LC.Height - my) * LC.Width;
                if (curser >= LC.Buffer.Length)
                    curser = LC.Buffer.Length - 1;

                if (Trash[x])
                    LC.Buffer[curser] = @char;
                x++;
            }
        }
        public static void Pickup()
        {
            int x = Player.X;
            if (Trash[x])
            {
                Trash[x] = false;
                Score++;
            }
            else
            {
                Score -= 3;
                // check if neg
                if (Score < 0)
                {
                    Alive = false;
                }
            }
        }
        static bool first = true;
        public static void PlayerMove(int info)
        {
            if (first)
                timer.Start();
            SetCords(Player);
            if (Alive)
                LC.WriteChar(' ', 0x33);
            int x = Player.X;
            int _new = x + info;
            if (_new >= 0 && _new < Console.WindowWidth)
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
            if (Alive)
                if (DateTime.Now.Second % 2 == 0)
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
