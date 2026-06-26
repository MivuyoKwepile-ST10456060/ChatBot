using System.Collections.Generic;

namespace Chatbot
{
    public class QuizService
    {
        public List<QuizQuestion> Questions { get; } = new()
        {
            new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                Options = ["Reply with your password", "Delete the email", "Report it as phishing", "Ignore it"],
                CorrectIndex = 2,
                Explanation = "Correct. Reporting phishing emails helps prevent scams."
            },
            new QuizQuestion
            {
                Question = "True or False: You should use the same password for all accounts.",
                Options = ["True", "False"],
                CorrectIndex = 1,
                Explanation = "Correct. Reusing passwords is risky because one leaked password can expose many accounts."
            },
            new QuizQuestion
            {
                Question = "What does 2FA help protect?",
                Options = ["Only your photos", "Your online accounts", "Your keyboard", "Your screen brightness"],
                CorrectIndex = 1,
                Explanation = "Correct. Two-factor authentication adds an extra layer of security."
            },
            new QuizQuestion
            {
                Question = "Which password is strongest?",
                Options = ["123456", "password", "Mivuyo123", "L!on#River92@Safe"],
                CorrectIndex = 3,
                Explanation = "Correct. Strong passwords use a mix of letters, numbers, and symbols."
            },
            new QuizQuestion
            {
                Question = "True or False: Public Wi-Fi can be risky.",
                Options = ["True", "False"],
                CorrectIndex = 0,
                Explanation = "Correct. Public Wi-Fi can expose your data if it is not secure."
            },
            new QuizQuestion
            {
                Question = "What is malware?",
                Options = ["A safe update", "Malicious software", "A password manager", "A browser setting"],
                CorrectIndex = 1,
                Explanation = "Correct. Malware is software designed to harm or steal data."
            },
            new QuizQuestion
            {
                Question = "What should you check before clicking a link?",
                Options = ["The URL", "The font size", "The colour", "The email length"],
                CorrectIndex = 0,
                Explanation = "Correct. Suspicious URLs can lead to fake or harmful websites."
            },
            new QuizQuestion
            {
                Question = "True or False: Antivirus software should be updated regularly.",
                Options = ["True", "False"],
                CorrectIndex = 0,
                Explanation = "Correct. Updates help protect against new threats."
            },
            new QuizQuestion
            {
                Question = "What is phishing?",
                Options = ["A scam to steal information", "A safe login method", "A backup tool", "A browser update"],
                CorrectIndex = 0,
                Explanation = "Correct. Phishing tricks users into giving away personal information."
            },
            new QuizQuestion
            {
                Question = "Why are backups important?",
                Options = ["They slow your PC", "They protect files if data is lost", "They delete viruses", "They replace passwords"],
                CorrectIndex = 1,
                Explanation = "Correct. Backups help recover data after loss, theft, or ransomware."
            },
            new QuizQuestion
            {
                Question = "True or False: You should share OTPs with trusted friends.",
                Options = ["True", "False"],
                CorrectIndex = 1,
                Explanation = "Correct. OTPs should never be shared with anyone."
            }
        };
    }
}