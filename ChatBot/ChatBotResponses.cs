using System;
using System.Collections.Generic;
using System.Linq;

namespace Chatbot
{
    public class ChatBotResponses
    {
        private readonly Random _random = new Random();
        private readonly UserProfile _user;

        public delegate string SentimentDelegate(string sentiment, string topic);

        private readonly Dictionary<string, List<string>> _responses;

        public ChatBotResponses(UserProfile user)
        {
            _user = user;

            _responses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "password",
                    new List<string>
                    {
                        "Use strong, unique passwords for every account. Avoid using your name, birthday, or simple words.",
                        "A strong password should be at least 12 characters long and include uppercase letters, lowercase letters, numbers, and symbols.",
                        "Never reuse the same password across different accounts. If one account is hacked, the others could be at risk too.",
                        "Consider using a password manager to safely store complex passwords."
                    }
                },
                {
                    "phishing",
                    new List<string>
                    {
                        "Phishing messages often create panic or urgency. Always check the sender before clicking any link.",
                        "Do not enter your login details through a link sent by email or SMS unless you are sure it is legitimate.",
                        "Look out for spelling mistakes, strange email addresses, and suspicious attachments.",
                        "When in doubt, go directly to the official website instead of clicking the link in the message."
                    }
                },
                {
                    "scam",
                    new List<string>
                    {
                        "Online scams often promise prizes, money, or urgent help. Always verify before responding.",
                        "Never send money or personal information to someone you only know online.",
                        "Scammers may pretend to be banks, delivery companies, or government departments. Contact the organisation directly to confirm.",
                        "If something sounds too good to be true, it is probably a scam."
                    }
                },
                {
                    "privacy",
                    new List<string>
                    {
                        "Protect your privacy by checking your app permissions regularly.",
                        "Limit how much personal information you share on social media.",
                        "Use privacy settings to control who can see your posts, photos, and contact details.",
                        "Avoid sharing your ID number, address, banking details, or passwords online."
                    }
                },
                {
                    "malware",
                    new List<string>
                    {
                        "Malware is harmful software that can steal information or damage your device.",
                        "Avoid downloading files from unknown websites or suspicious links.",
                        "Keep your antivirus software updated and scan your device regularly.",
                        "Do not open unexpected attachments, especially from unknown senders."
                    }
                },
                {
                    "wifi",
                    new List<string>
                    {
                        "Avoid using public Wi-Fi for banking or shopping unless you are using a trusted VPN.",
                        "Public Wi-Fi can be risky because attackers may intercept your data.",
                        "Do not enter sensitive information while connected to unknown public Wi-Fi networks."
                    }
                },
                {
                    "vpn",
                    new List<string>
                    {
                        "A VPN can help protect your connection, especially when using public Wi-Fi.",
                        "A VPN improves privacy, but you should still avoid suspicious websites.",
                        "Choose a trusted VPN provider and avoid unknown free VPN services."
                    }
                },
                {
                    "backup",
                    new List<string>
                    {
                        "Back up your important files regularly to protect yourself from ransomware and device failure.",
                        "Use cloud storage or an external drive to keep copies of important documents.",
                        "A good backup can help you recover your work if your device is stolen, damaged, or infected."
                    }
                },
                {
                    "update",
                    new List<string>
                    {
                        "Keep your software updated because updates often fix security weaknesses.",
                        "Turn on automatic updates for your operating system, browser, and apps.",
                        "Ignoring updates can leave your device vulnerable to cyberattacks."
                    }
                },
                {
                    "social media",
                    new List<string>
                    {
                        "Be careful about what you post online because strangers can use personal details to target you.",
                        "Set your social media accounts to private and only accept people you know.",
                        "Avoid posting your location, school, workplace, or daily routine publicly."
                    }
                }
            };
        }

        public string GetResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Please type something so I can assist you.";
            }

            string lowerInput = input.ToLower();

            SaveName(lowerInput);
            SaveFavouriteTopic(lowerInput);

            string sentiment = DetectSentiment(lowerInput);

            if (IsGreeting(lowerInput))
            {
                return $"Hello {_user.Name}! I am your Cybersecurity Awareness Chatbot. You can ask me about passwords, phishing, scams, privacy, malware, public Wi-Fi, VPNs, backups, or updates.";
            }

            if (lowerInput.Contains("what do you remember") || lowerInput.Contains("remember about me"))
            {
                return RecallMemory();
            }

            if (IsFollowUp(lowerInput))
            {
                return HandleFollowUp(sentiment);
            }

            string? detectedTopic = DetectTopic(lowerInput);

            if (detectedTopic != null)
            {
                _user.LastTopic = detectedTopic;
                _user.LastSentiment = sentiment;

                string response = GetRandomResponse(detectedTopic);

                if (!string.IsNullOrWhiteSpace(_user.FavouriteTopic) &&
                    detectedTopic.Equals(_user.FavouriteTopic, StringComparison.OrdinalIgnoreCase))
                {
                    response += $"\n\nSince you are interested in {_user.FavouriteTopic}, this is a good topic to keep learning about.";
                }

                if (!string.IsNullOrWhiteSpace(sentiment))
                {
                    SentimentDelegate sentimentHandler = RespondToSentiment;
                    response = sentimentHandler(sentiment, detectedTopic) + "\n\n" + response;
                }

                return response;
            }

            if (lowerInput.Contains("help") || lowerInput.Contains("what can i ask"))
            {
                return "You can ask me about password safety, phishing, scams, privacy, malware, VPNs, public Wi-Fi, backups, updates, and social media safety.";
            }

            return "I did not fully understand that. Try asking about passwords, phishing, scams, privacy, malware, VPNs, backups, updates, or social media safety.";
        }

        private string? DetectTopic(string input)
        {
            foreach (string topic in _responses.Keys)
            {
                if (input.Contains(topic))
                {
                    return topic;
                }
            }

            if (input.Contains("2fa") || input.Contains("two factor"))
            {
                return "password";
            }

            if (input.Contains("email scam"))
            {
                return "phishing";
            }

            if (input.Contains("public wifi") || input.Contains("public wi-fi"))
            {
                return "wifi";
            }

            if (input.Contains("virus"))
            {
                return "malware";
            }

            return null;
        }

        private string GetRandomResponse(string topic)
        {
            List<string> possibleResponses = _responses[topic];
            int index = _random.Next(possibleResponses.Count);
            return possibleResponses[index];
        }

        private bool IsGreeting(string input)
        {
            return input == "hi" ||
                   input == "hello" ||
                   input == "hey" ||
                   input.Contains("good morning") ||
                   input.Contains("good afternoon") ||
                   input.Contains("good evening");
        }

        private bool IsFollowUp(string input)
        {
            return input.Contains("tell me more") ||
                   input.Contains("another tip") ||
                   input.Contains("explain more") ||
                   input.Contains("more info") ||
                   input.Contains("continue");
        }

        private string HandleFollowUp(string sentiment)
        {
            if (string.IsNullOrWhiteSpace(_user.LastTopic))
            {
                return "Please ask me about a cybersecurity topic first, such as passwords, phishing, scams, privacy, malware, VPNs, or public Wi-Fi.";
            }

            string response = GetRandomResponse(_user.LastTopic);

            if (!string.IsNullOrWhiteSpace(sentiment))
            {
                SentimentDelegate sentimentHandler = RespondToSentiment;
                response = sentimentHandler(sentiment, _user.LastTopic) + "\n\n" + response;
            }

            return response;
        }

        private void SaveName(string input)
        {
            if (input.Contains("my name is"))
            {
                string name = input.Replace("my name is", "").Trim();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    _user.Name = ToTitleCase(name);
                }
            }
        }

        private void SaveFavouriteTopic(string input)
        {
            if (input.Contains("interested in") || input.Contains("favourite topic is") || input.Contains("favorite topic is"))
            {
                string? topic = DetectTopic(input);

                if (topic != null)
                {
                    _user.FavouriteTopic = topic;
                }
            }
        }

        private string RecallMemory()
        {
            string memory = "Here is what I remember:\n";

            memory += $"- Your name is {_user.Name}.\n";

            if (!string.IsNullOrWhiteSpace(_user.FavouriteTopic))
            {
                memory += $"- You are interested in {_user.FavouriteTopic}.\n";
            }
            else
            {
                memory += "- You have not told me your favourite cybersecurity topic yet.\n";
            }

            if (!string.IsNullOrWhiteSpace(_user.LastTopic))
            {
                memory += $"- The last topic we discussed was {_user.LastTopic}.\n";
            }

            memory += $"- You have interacted with me {_user.Interactions} time(s).";

            return memory;
        }

        private string DetectSentiment(string input)
        {
            if (input.Contains("worried") ||
                input.Contains("scared") ||
                input.Contains("afraid") ||
                input.Contains("nervous") ||
                input.Contains("anxious"))
            {
                return "worried";
            }

            if (input.Contains("curious") ||
                input.Contains("interested") ||
                input.Contains("want to learn"))
            {
                return "curious";
            }

            if (input.Contains("frustrated") ||
                input.Contains("confused") ||
                input.Contains("angry") ||
                input.Contains("annoyed"))
            {
                return "frustrated";
            }

            return "";
        }

        private string RespondToSentiment(string sentiment, string topic)
        {
            if (sentiment == "worried")
            {
                return $"It is completely understandable to feel worried about {topic}. Learning about it is already a strong first step toward staying safe.";
            }

            if (sentiment == "curious")
            {
                return $"That is a great thing to be curious about. Understanding {topic} can help you make safer decisions online.";
            }

            if (sentiment == "frustrated")
            {
                return $"I understand that {topic} can feel confusing or frustrating. Let me make it simple and practical.";
            }

            return "";
        }

        private string ToTitleCase(string value)
        {
            string[] words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }

            return string.Join(" ", words);
        }
    }
}