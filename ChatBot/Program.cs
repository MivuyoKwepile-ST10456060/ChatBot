using System;

namespace Chatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";

            // 1. Play voice greeting
            SoundPlayerHelper.PlayGreeting("welcome.wav");

            // 2. Display ASCII header (using your original art + a themed header)
            ConsoleHelper.DisplayHeader(AsciiArt.GetFullHeader());

            // 3. Text greeting and name collection
            ConsoleHelper.WriteColored("Welcome to the ", ConsoleColor.Cyan);
            ConsoleHelper.WriteLineColored("Cybersecurity Awareness Chatbot!", ConsoleColor.Green);
            ConsoleHelper.TypewriterEffect("I'm here to help you stay safe online.\n", 30);

            Console.Write("\nWhat is your name? ");
            string name = (Console.ReadLine() ?? string.Empty).Trim();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Name cannot be empty. Please enter your name: ");
                name = (Console.ReadLine() ?? string.Empty).Trim();
            }

            // 4. Create user profile and start chatbot
            var user = new UserProfile { Name = name.Trim() };
            var chatbot = new Chatbot(user);
            chatbot.StartConversation();
        }
    }
}