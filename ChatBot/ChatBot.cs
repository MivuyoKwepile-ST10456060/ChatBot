using System;
using System.Collections.Generic;

namespace Chatbot
{
    public class Chatbot
    {
        private readonly UserProfile _user;
        private readonly ChatbotResponses _responses;

        public Chatbot(UserProfile user)
        {
            _user = user;
            _responses = new ChatbotResponses();
        }

        public void StartConversation()
        {
            ConsoleHelper.WriteLineColored($"\nNice to meet you, {_user.Name}!", ConsoleColor.Magenta);
            ConsoleHelper.WriteLineColored("Type 'exit' or 'quit' to end the conversation.\n", ConsoleColor.DarkGray);

            while (true)
            {
                ConsoleHelper.WriteColored($"{_user.Name}: ", ConsoleColor.Yellow);
                string? input = Console.ReadLine()?.Trim();

                // Input validation: empty entry
                if (string.IsNullOrEmpty(input))
                {
                    ConsoleHelper.WriteColored("I didn't catch that. Could you please say something?\n", ConsoleColor.Red);
                    continue;
                }

                // Exit condition
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleHelper.TypewriterEffect($"Goodbye, {_user.Name}! Stay safe online.\n", 40);
                    break;
                }

                // Get response
                string response = _responses.GetResponse(input);
                ConsoleHelper.TypewriterEffect($"Bot: {response}\n", 30);
                _user.Interactions++;
            }
        }
    }
}