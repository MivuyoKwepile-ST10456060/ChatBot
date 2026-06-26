namespace Chatbot
{
    public class QuizQuestion
    {
        public string Question { get; set; } = "";
        public string[] Options { get; set; } = [];
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; } = "";
    }
}