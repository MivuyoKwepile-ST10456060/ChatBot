using System;
using System.Threading;

namespace Chatbot
{
    public static class ConsoleHelper
    {
        public static void WriteColored(string text, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }

        public static void WriteLineColored(string text, ConsoleColor color)
        {
            WriteColored(text + Environment.NewLine, color);
        }

        public static void TypewriterEffect(string text, int delayMs)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMs);
            }
        }

        public static void DisplayHeader(string asciiArt)
        {
            Console.Clear();
            WriteLineColored(asciiArt, ConsoleColor.Cyan);
            DrawLine('=', ConsoleColor.DarkCyan);
        }

        public static void DrawLine(char lineChar, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(new string(lineChar, Console.WindowWidth - 1));
            Console.ForegroundColor = originalColor;
        }
    }
}