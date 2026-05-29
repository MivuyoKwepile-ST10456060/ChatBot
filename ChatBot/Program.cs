using System;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Chatbot
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new ChatbotForm());
        }
    }
}