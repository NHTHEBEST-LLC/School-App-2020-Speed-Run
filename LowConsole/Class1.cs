using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace LowConsole
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CharUnion
    {
        [FieldOffset(0)] public char UnicodeChar;
        [FieldOffset(0)] public byte AsciiChar;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct CharInfo
    {
        [FieldOffset(0)] public CharUnion Char;
        [FieldOffset(2)] public short Attributes;
    }
    public class LC
    {
        /// <summary>
        /// Width of buffer
        /// </summary>
        public readonly static short Width = 120;
        /// <summary>
        /// Height of buffer
        /// </summary>
        public readonly static short Height = 30;

        
        static bool first = true;

        static int curser = 0;
        static SafeFileHandle h = lowlevel.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

        /// <summary>
        /// Screen Buffer
        /// </summary>
        public static CharInfo[] Buffer = new CharInfo[Width * Height];
        static lowlevel.SmallRect rect = new lowlevel.SmallRect() { Left = 0, Top = 0, Right = Width, Bottom = Height };

        /// <summary>
        /// clears the buffer
        /// </summary>
        /// <param name="color">color to clear to</param>
        public static void Clear(int color)
        {
            if (first)
            {
                new Thread(WriteBufferLoop).Start();
                first = false;
            }
            for (int i = 0; i < Width*Height; i++)
            {
                Buffer[i].Attributes = (short)color;
                Buffer[i].Char.UnicodeChar = (char)0;
            }

        }

        /// <summary>
        /// sets the Cuser for Write and Writechar
        /// </summary>
        /// <param name="x">X Value</param>
        /// <param name="y">Y Vakue</param>
        /// <returns>The element in the buffer array</returns>
        public static int setCuser(int x, int y)
        {
            int c = (y * Width) + x;
            if (c < Width * Height)
                curser = c;
            if (curser>=Buffer.Length)
            {
                throw new IndexOutOfRangeException("X or Y Too Big");
            }
            return curser;
        }

        /// <summary>
        /// Writes a string to the buffer 
        /// </summary>
        /// <param name="text">text to write</param>
        /// <param name="color">color to write in</param>
        public static void Write(string text, char color)
        {
            foreach(char c in text)
            {
                WriteChar(c, (short)color);
            }
        }

        /// <summary>
        /// Writes a string to the buffer (buts from int array)
        /// </summary>
        /// <param name="text">text to write</param>
        /// <param name="color">color to write in</param>
        public static void Writeints(int[] text, int color)
        {
            foreach (int c in text)
            {
                WriteChar((char)c, (short)color);
            }
        }

        /// <summary>
        /// Writes one char to the buffer 
        /// </summary>
        /// <param name="c">char to write</param>
        /// <param name="color">color to write in</param>
        public static void WriteChar(char c, short color)
        {
            if (first)
            {
                new Thread(WriteBufferLoop).Start();
                first = false;
            }
            int xy = curser;
            if (xy >= Buffer.Length)
                xy = Buffer.Length - 1;

            Buffer[xy].Char.UnicodeChar = c;
            Buffer[xy].Attributes = color;
            curser++;
        }

        /// <summary>
        /// loop to update the disply to what the buffer contains
        /// must be run in new thread
        /// </summary>
        private static void WriteBufferLoop()
        {
            while (true)
            {
                //var sw = Stopwatch.StartNew();
                WriteBuffer();
                //var ms = (int)sw.ElapsedMilliseconds;
                //if (ms < 16)
                //    await Task.Delay(16 - ms);
                
            }
        }

        /// <summary>
        /// updates the screen to what the buffer contains
        /// </summary>
        private static void WriteBuffer()
        {
            if (!h.IsInvalid)
            {
                bool b = lowlevel.WriteConsoleOutput(h, Buffer,
                  new lowlevel.Coord() { X = Width, Y = Height },
                  new lowlevel.Coord() { X = 0, Y = 0 },
                  ref rect
                 );
            }
        }
    }
}
