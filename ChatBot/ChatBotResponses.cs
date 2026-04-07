using System;
using System.Collections.Generic;

namespace Chatbot
{
    public class ChatbotResponses
    {
        private readonly Dictionary<string, string> _responses;

        public ChatbotResponses()
        {
            _responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "how are you", "I'm functioning well, thank you! How can I assist you today?" },
                { "purpose", "My purpose is to educate you about cybersecurity risks like phishing, password safety, and safe browsing." },
                { "what can i ask", "You can ask me about password safety, phishing, safe browsing, or just say hello!" },
                { "password", "Use strong, unique passwords. Consider a password manager and enable two‑factor authentication (2FA)." },
                { "phishing", "Phishing emails often have urgent language, suspicious links, or mismatched sender addresses. Never click on unexpected attachments or links." },
                { "safe browsing", "Always check that websites use HTTPS, avoid public Wi‑Fi for sensitive transactions, and keep your browser updated." },
                { "hello", "Hello! I'm your cybersecurity assistant." },
                { "help", "I can answer questions about passwords, phishing, safe browsing, my purpose, or just chat." }
            };
        }

        public string GetResponse(string input)
        {
            foreach (var keyword in _responses.Keys)
            {
                if (input.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    return _responses[keyword];
            }
            return "I didn't quite understand that. Could you rephrase? (Try asking about password safety, phishing, or safe browsing.)";
        }
    }
}