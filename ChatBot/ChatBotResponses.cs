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
                // Greetings
                { "hello", "Hello! I'm your cybersecurity assistant." },
                { "hi", "Hi there! How can I help you stay safe online?" },
                { "hey", "Hey! Need help with cybersecurity tips?" },

                // General Help
                { "help", "I can help with passwords, phishing, malware, safe browsing, social media safety, and more!" },
                { "what can i ask", "Ask me about password safety, phishing, malware, scams, or how to stay safe online." },
                { "purpose", "My purpose is to help you stay safe online by teaching cybersecurity best practices." },

                // Password Safety
                { "password", "Use strong, unique passwords with a mix of letters, numbers, and symbols. Never reuse passwords." },
                { "strong password", "A strong password should be at least 12 characters long and include a mix of uppercase, lowercase, numbers, and symbols." },
                { "2fa", "Enable Two-Factor Authentication (2FA) for extra security. It adds a second layer of protection." },

                // Phishing
                { "phishing", "Phishing scams trick you into giving personal info. Watch for urgent messages, fake links, and unknown senders." },
                { "email scam", "Be cautious of emails asking for personal information. Always verify the sender before clicking links." },

                // Malware
                { "malware", "Malware is harmful software. Install antivirus software and avoid downloading files from untrusted sources." },
                { "virus", "Computer viruses can damage your system. Keep your antivirus updated and scan files regularly." },

                // Safe Browsing
                { "safe browsing", "Always check for HTTPS in websites, avoid suspicious links, and keep your browser updated." },
                { "https", "HTTPS means a website is secure. Avoid entering personal data on sites without it." },

                // Social Media Safety
                { "social media", "Avoid sharing personal information online. Set your profiles to private and be careful who you accept." },
                { "privacy", "Check your privacy settings regularly and limit what others can see about you online." },

                // Scams & Fraud
                { "scam", "Online scams often promise rewards or create urgency. Always verify before taking action." },
                { "fraud", "Never share sensitive information like banking details. Legitimate companies won't ask for it via email." },

                // Public Wi-Fi
                { "wifi", "Avoid using public Wi-Fi for banking or sensitive activities. Use a VPN if necessary." },
                { "public wifi", "Public Wi-Fi is not secure. Hackers can intercept your data." },

                // Updates & Software
                { "update", "Keep your software and apps updated to protect against security vulnerabilities." },
                { "software update", "Updates fix security flaws. Always install them as soon as possible." },

                // Device Safety
                { "device", "Lock your devices with a PIN, password, or fingerprint to prevent unauthorized access." },
                { "phone security", "Enable screen locks and avoid installing apps from unknown sources." },

                // Identity Protection
                { "identity theft", "Protect your personal information and monitor your accounts for suspicious activity." },

                // Backup
                { "backup", "Regularly back up your data to prevent loss from attacks like ransomware." },

                // Default conversational
                { "how are you", "I'm functioning well and ready to help you stay safe online!" }
            };
        }

        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type something so I can assist you.";

            input = input.ToLower();

            foreach (var keyword in _responses.Keys)
            {
                if (input.Contains(keyword))
                    return _responses[keyword];
            }

            return "I didn't quite understand that. Try asking about passwords, phishing, malware, scams, or safe browsing.";
        }
    }
}