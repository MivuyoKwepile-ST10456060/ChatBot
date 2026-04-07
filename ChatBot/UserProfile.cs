namespace Chatbot
{
    public class UserProfile
    {
        // Automatic properties
        public required string Name { get; set; }
        public int Interactions { get; set; } = 0;
    }
}