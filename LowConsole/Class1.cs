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
        public readonly static short Width = 120;
        public readonly static short Height = 30;

        static bool first = true;

        static int curser = 0;
        static SafeFileHandle h = lowlevel.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

        public static CharInfo[] Buffer = new CharInfo[Width * Height];
        static lowlevel.SmallRect rect = new lowlevel.SmallRect() { Left = 0, Top = 0, Right = Width, Bottom = Height };

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

        public static void setCuser(int x, int y)
        {
            int c = (y * Width) + x;
            if (c < Width * Height)
                curser = c;
        }

        public static void Write(string text, char color)
        {
            foreach(char c in text)
            {
                WriteChar(c, (short)color);
            }
        }

        public static void WriteChar(char c, short color)
        {
            if (first)
            {
                new Thread(WriteBufferLoop).Start();
                first = false;
            }

            Buffer[curser].Char.UnicodeChar = c;
            Buffer[curser].Attributes = color;
            curser++;
        }

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
