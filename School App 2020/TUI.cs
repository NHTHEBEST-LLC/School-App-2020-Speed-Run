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
using School_App_2020.Properties;

namespace School_App_2020
{
    /// <summary>
    /// 
    /// </summary>
    struct Cords
    {
        /// <summary>
        /// 
        /// </summary>
        public int X;
        /// <summary>
        /// 
        /// </summary>
        public int Y;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Cords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    static class TUI
    {
        /// <summary>
        /// 
        /// </summary>
        private static Cords _Player = new Cords(0, 0);
        /// <summary>
        /// 
        /// </summary>
        private static int[] Terrain = GenTerrain(7, 3);
        /// <summary>
        /// 
        /// </summary>
        private static bool[] Trash = GenTrash(1);
        /// <summary>
        /// 
        /// </summary>
        private static int Score = 0;
        /// <summary>
        /// 
        /// </summary>
        private static int NumberofTrash;
        /// <summary>
        /// 
        /// </summary>
        private static int NonChangingNumberofTrash;
        /// <summary>
        /// 
        /// </summary>
        private static Stopwatch timer = new Stopwatch();
        /// <summary>
        /// 
        /// </summary>
        private static bool Alive = true;
        /// <summary>
        /// 
        /// </summary>
        private static bool Win = false;
        /// <summary>
        /// 
        /// </summary>
        /// 
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
        /// <summary>
        /// 
        /// </summary>
        static int fps = 60;
        /// <summary>
        /// 
        /// </summary>
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
                        if (first)
                        {
                            PrintAT("Instructions", new Cords(4, LC.Height - 10), 0x34);
                            PrintAT("Space To Pickup", new Cords(4, LC.Height - 11), 0x34);
                            PrintAT("Right Arrow to Move Right", new Cords(4, LC.Height - 11), 0x34);
                            PrintAT("Left Arrow To move Left", new Cords(4, LC.Height - 12), 0x34);
                            PrintAT("Don't Miss (You Die When you have less than one heart)", new Cords(4, LC.Height - 13), 0x34);
                        }
                        string hs = string.Format("High Score = {0}", Settings.Default.HighScore);
                        PrintAT(hs, new Cords(LC.Width - (4+hs.Length), LC.Height - 4), 0x34);

                        PrintAT(string.Format("Score = {0}/{1}  {2}%", Score, NonChangingNumberofTrash, (Score*100)/NonChangingNumberofTrash), new Cords(4, LC.Height - 3), 0x34);
                        PrintAT(string.Format("Time = {0}", timer.Elapsed.ToString("mm\\:ss\\.ff")), new Cords(4, LC.Height - 4), 0x34);
                        List<int> HARTS = new List<int>();
                        for (int i = -1; i < Score/3; i++)
                        {
                            HARTS.Add(3);
                        }
                        SetCords(new Cords(4, LC.Height - 5));
                        LC.Writeints(HARTS.ToArray(), 0x34);
                        PaintTerrain();

                        PaintTrash();
                        PaintPlayer();

                        if (Win)
                        {
                            timer.Stop();
                            PrintAT(@"You Win Press R to Restart", new Cords(40, LC.Height - (13)), 0x34);
                            int final = (Score * 100) / NonChangingNumberofTrash;
                            final = final * 100;
                            final = final / (int)(timer.ElapsedMilliseconds / 100);
                            PrintAT(string.Format("Total Score = {0}",final), new Cords(40, LC.Height - (14)), 0x34);
                            if (final > Settings.Default.HighScore)
                            {
                                Settings.Default.HighScore = final;
                                Settings.Default.Save();
                            }
                                
                        }
                    }

                    if (!Alive)
                    {
                        int mgsx = 30;
                        int msgy = 13;

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
                        
                    }


                    var ms = (int)sw.ElapsedMilliseconds;
                    if (ms < del)
                        Thread.Sleep(del - ms);

                }
            }).Start();
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Restart()
        {
            timer.Reset();
            timer.Stop();
            first = true;
            Alive = true;
            Win = false;
            Score = 0;
            _Player = new Cords(0, 0);
            Terrain = GenTerrain(7, 3);
            Trash = GenTrash(1);
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

            NumberofTrash = 0;
            foreach (var item in trash)
            {
                if (item)
                    NumberofTrash++;
            }
            NonChangingNumberofTrash = NumberofTrash;
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        public static void Pickup()
        {
            int x = Player.X;
            if (Trash[x])
            {
                Trash[x] = false;
                Score++;
                NumberofTrash--;
                if(NumberofTrash ==0)
                {
                    Win = true;
                }
            }
            else if (!Win)
            {
                Score -= 3;
                // check if neg
                if (Score < 0)
                {
                    Alive = false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        static bool first = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public static void PlayerMove(int info)
        {
            if (first)
            {
                timer.Start();
                first = false;
            }
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


        /// <summary>
        /// 
        /// </summary>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="cords"></param>
        /// <param name="color"></param>
        static void PrintAT(string text, Cords cords, int color)
        {

            //SetColor(background, forground);
            SetCords(cords);
            //Console.Write(text);


            LC.Write(text, (char)color);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cords"></param>
        static void SetCords(Cords cords)
        {
            //Console.SetCursorPosition(cords.X, Console.WindowHeight - cords.Y);
            LC.setCuser(cords.X, LC.Height - cords.Y);
        }



    }
}
