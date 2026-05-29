namespace Chatbot
{
    public class UserProfile
    {
        public string Name { get; set; } = "User";
        public int Interactions { get; set; } = 0;

        public string FavouriteTopic { get; set; } = "";
        public string LastTopic { get; set; } = "";
        public string LastSentiment { get; set; } = "";
    }
}