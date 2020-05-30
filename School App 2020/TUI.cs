using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using LowConsole;
using School_App_2020.Properties;

namespace School_App_2020
{
    /// <summary>
    /// this is the object for the coredents on screen
    /// starts from bottom left
    /// </summary>
    struct Cords
    {
        /// <summary>
        /// the x value
        /// </summary>
        public int X;
        /// <summary>
        /// the y value
        /// </summary>
        public int Y;
        /// <summary>
        /// this make a new cords object
        /// from bottom left
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        public Cords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// the Text User Interface
    /// the main game class
    /// </summary>
    static class TUI
    {
        /// <summary>
        /// the player cords
        /// </summary>
        private static Cords _Player = new Cords(0, 0);
        /// <summary>
        /// the array for the terrain hight
        /// </summary>
        private static int[] Terrain = GenTerrain(7, 3);
        /// <summary>
        /// the array for if the top of this terrain contains trash
        /// </summary>
        private static bool[] Trash = GenTrash(1);
        /// <summary>
        /// the score
        /// </summary>
        private static int Score = 0;
        /// <summary>
        /// number of trash on screen
        /// 
        /// </summary>
        private static int NumberofTrash;
        /// <summary>
        /// number of trash genorated
        /// </summary>
        private static int NonChangingNumberofTrash;
        /// <summary>
        /// the timer for when playing
        /// </summary>
        private static Stopwatch timer = new Stopwatch();
        /// <summary>
        /// defines if you get killed
        /// </summary>
        private static bool Alive = true;
        /// <summary>
        /// defines if you win
        /// </summary>
        private static bool Win = false;

        /// <summary>
        /// the public player cords
        /// </summary>
        public static Cords Player
        {
            private set
            {
                _Player = value; // sets the private vaubale 
            }
            get
            {
                // gets the player cords
                int x = _Player.X; 
                int y = Terrain[x] + 1;
                return new Cords(x, y);
            }
        }
        /// <summary>
        /// the max frames per second
        /// </summary>
        static int fps = 60;
        /// <summary>
        /// the paint function
        /// start a thread that controles the screen
        /// shuld only be called once
        /// </summary>
        static public void Paint()
        {
            // make new thread
            new Thread(() =>
            {
                // foever
                while (true)
                {
                    // make shure the console is correct
                    Console.WindowHeight = LC.Height;
                    Console.WindowWidth = LC.Width;
                    Console.CursorVisible = false;

                    // check fps
                    int del = 1000 / fps;
                    var sw = Stopwatch.StartNew();

                    // check if alive
                    if (Alive)
                        LC.Clear(0x33); // print sky
                    else
                        LC.Clear(0x44); // print red

                    

                    // check if alive
                    if (Alive)
                    {
                        // check if first time
                        if (first)
                        {
                            // print instructions
                            PrintAT("Instructions", new Cords(4, LC.Height - 10), 0x34);
                            PrintAT("Space To Pickup", new Cords(4, LC.Height - 11), 0x34);
                            PrintAT("Right Arrow to Move Right", new Cords(4, LC.Height - 11), 0x34);
                            PrintAT("Left Arrow To move Left", new Cords(4, LC.Height - 12), 0x34);
                            PrintAT("Don't Miss (You Die When you have less than one heart)", new Cords(4, LC.Height - 13), 0x34);
                        }

                        // print high score
                        string hs = string.Format("High Score = {0}", Settings.Default.HighScore);
                        PrintAT(hs, new Cords(LC.Width - (4+hs.Length), LC.Height - 4), 0x34);

                        // print score and time
                        PrintAT(string.Format("Score = {0}/{1}  {2}%", Score, NonChangingNumberofTrash, (Score*100)/NonChangingNumberofTrash), new Cords(4, LC.Height - 3), 0x34);
                        PrintAT(string.Format("Time = {0}", timer.Elapsed.ToString("mm\\:ss\\.ff")), new Cords(4, LC.Height - 4), 0x34);
                        // print harts 
                        List<int> HARTS = new List<int>();
                        for (int i = -1; i < Score/3; i++)
                        {
                            HARTS.Add(3);
                        }
                        SetCords(new Cords(4, LC.Height - 5));
                        LC.Writeints(HARTS.ToArray(), 0x34);
                        
                        // paint terrain
                        PaintTerrain();
                        // paint trash
                        PaintTrash();
                        // paint player
                        PaintPlayer();

                        // if you win
                        if (Win)
                        {
                            // stop game timer
                            timer.Stop();
                            // print win msg
                            PrintAT(@"You Win Press R to Restart", new Cords(40, LC.Height - (13)), 0x34);
                            // calcualet final score
                            int final = (Score * 100) / NonChangingNumberofTrash;
                            final = final * 100;
                            final = final / (int)(timer.ElapsedMilliseconds / 100);
                            // print score
                            PrintAT(string.Format("Total Score = {0}",final), new Cords(40, LC.Height - (14)), 0x34);
                            // check if grater than high score
                            if (final > Settings.Default.HighScore)
                            {
                                // save new high score
                                Settings.Default.HighScore = final;
                                Settings.Default.Save();
                            }
                                
                        }
                    }

                    // if dead
                    if (!Alive)
                    {
                        // declare msg location
                        int mgsx = 30;
                        int msgy = 13;

                        //██╗░░░██╗░█████╗░██╗░░░██╗  ██████╗░██╗███████╗██████╗░
                        //╚██╗░██╔╝██╔══██╗██║░░░██║  ██╔══██╗██║██╔════╝██╔══██╗
                        //░╚████╔╝░██║░░██║██║░░░██║  ██║░░██║██║█████╗░░██║░░██║
                        //░░╚██╔╝░░██║░░██║██║░░░██║  ██║░░██║██║██╔══╝░░██║░░██║
                        //░░░██║░░░╚█████╔╝╚██████╔╝  ██████╔╝██║███████╗██████╔╝
                        //░░░╚═╝░░░░╚════╝░░╚═════╝░  ╚═════╝░╚═╝╚══════╝╚═════╝░
                        
                        // text ubove but in prinable format
                        int[] l1 = new int[] { 219, 219, 187, 176, 176, 176, 219, 219, 187, 176, 219, 219, 219, 219, 219, 187, 176, 219, 219, 187, 176, 176, 176, 219, 219, 187, 32, 32, 219, 219, 219, 219, 219, 219, 187, 176, 219, 219, 187, 219, 219, 219, 219, 219, 219, 219, 187, 219, 219, 219, 219, 219, 219, 187, 176 };
                        int[] l2 = new int[] { 200, 219, 219, 187, 176, 219, 219, 201, 188, 219, 219, 201, 205, 205, 219, 219, 187, 219, 219, 186, 176, 176, 176, 219, 219, 186, 32, 32, 219, 219, 201, 205, 205, 219, 219, 187, 219, 219, 186, 219, 219, 201, 205, 205, 205, 205, 188, 219, 219, 201, 205, 205, 219, 219, 187 };
                        int[] l3 = new int[] { 176, 200, 219, 219, 219, 219, 201, 188, 176, 219, 219, 186, 176, 176, 219, 219, 186, 219, 219, 186, 176, 176, 176, 219, 219, 186, 32, 32, 219, 219, 186, 176, 176, 219, 219, 186, 219, 219, 186, 219, 219, 219, 219, 219, 187, 176, 176, 219, 219, 186, 176, 176, 219, 219, 186 };
                        int[] l4 = new int[] { 176, 176, 200, 219, 219, 201, 188, 176, 176, 219, 219, 186, 176, 176, 219, 219, 186, 219, 219, 186, 176, 176, 176, 219, 219, 186, 32, 32, 219, 219, 186, 176, 176, 219, 219, 186, 219, 219, 186, 219, 219, 201, 205, 205, 188, 176, 176, 219, 219, 186, 176, 176, 219, 219, 186 };
                        int[] l5 = new int[] { 176, 176, 176, 219, 219, 186, 176, 176, 176, 200, 219, 219, 219, 219, 219, 201, 188, 200, 219, 219, 219, 219, 219, 219, 201, 188, 32, 32, 219, 219, 219, 219, 219, 219, 201, 188, 219, 219, 186, 219, 219, 219, 219, 219, 219, 219, 187, 219, 219, 219, 219, 219, 219, 201, 188 };
                        int[] l6 = new int[] { 176, 176, 176, 200, 205, 188, 176, 176, 176, 176, 200, 205, 205, 205, 205, 188, 176, 176, 200, 205, 205, 205, 205, 205, 188, 176, 32, 32, 200, 205, 205, 205, 205, 205, 188, 176, 200, 205, 188, 200, 205, 205, 205, 205, 205, 205, 188, 200, 205, 205, 205, 205, 205, 188, 176 };

                        // prinst each line of msg
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

                        // print instruction
                        PrintAT(@"                    Press R To Restart                 ", new Cords(mgsx, LC.Height - (msgy + 7)), 0x40);
                        
                    }

                    // wait as to not glich the screen
                    var ms = (int)sw.ElapsedMilliseconds;
                    if (ms < del)
                        Thread.Sleep(del - ms);

                }
                // start the thread
            }).Start();
        }
        /// <summary>
        /// restarts the game
        /// </summary>
        public static void Restart()
        {
            timer.Reset(); // reset timer
            timer.Stop();
            first = true; // reset globals
            Alive = true;
            Win = false;
            Score = 0;
            _Player = new Cords(0, 0);
            Terrain = GenTerrain(7, 3); // get new terrain
            Trash = GenTrash(1);       // get new trash
        }

        /// <summary>
        /// This Function is not Enviromenaly Frendly 
        /// </summary>
        /// <param name="onein"></param>
        /// <returns></returns>
        static bool[] GenTrash(int onein)
        {
            Random r = new Random(); // new random number gen
            int width = LC.Width - 1; // get the max width

            List<bool> trash = new List<bool>(); // new bool list

            trash.Add(false);// start with no trash

            // for each or the collums 
            for (int i = 0; i < width; i += 2)
            {
                int am = r.Next(onein + 1); // get randome number
                trash.Add(am == 1); // add true if = true or false if false
                trash.Add(false); // dont have 2 trash next to eachother
            }

            // clear the trash varubale
            NumberofTrash = 0;
            // loop on the trash array
            foreach (var item in trash)
            {
                if (item) // trash
                    NumberofTrash++; // incriment
            }
            NonChangingNumberofTrash = NumberofTrash; // set the genorated number to the new number
            return trash.ToArray(); // reteurn array
        }

        /// <summary>
        /// genorates the terrain
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
        /// paints the terrain
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
        /// paints the trash
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
        /// trys to pickup the trash 
        /// reduces lives if failed
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
        /// first time moveing in game
        /// </summary>
        static bool first = true;
        /// <summary>
        /// moves the player
        /// </summary>
        /// <param name="info">how mutch - is left </param>
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
        /// resets the high score
        /// </summary>
        public static void HSR()
        {
            Settings.Default.HighScore = 0;

            Settings.Default.Save();
        }
        /// <summary>
        /// cheats the game
        /// </summary>
        /// <param name="all">if true the whole game gets completed</param>
        public static void Cheat(bool all)
        {
            _Player.X = 0;
            foreach (var item in Trash)
            {
                if (item)
                    if (all)
                        Pickup();
                    else
                        break;
                PlayerMove(1);
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// paints the player on screen
        /// </summary>
        static void PaintPlayer()
        {
            SetCords(Player);
            if (Alive)
                if (DateTime.Now.Second % 2 == 0)
                    LC.WriteChar((char)2, 0x34);
                else
                    LC.WriteChar((char)1, 0x34);


        }


        /// <summary>
        /// prints text at a location
        /// </summary>
        /// <param name="text">text to print</param>
        /// <param name="cords">place</param>
        /// <param name="color">color</param>
        static void PrintAT(string text, Cords cords, int color)
        {
            SetCords(cords);
            LC.Write(text, (char)color);
        }
        /// <summary>
        /// sets the cuser
        /// </summary>
        /// <param name="cords">place</param>
        static void SetCords(Cords cords)
        {
            LC.setCuser(cords.X, LC.Height - cords.Y);
        }



    }
}
