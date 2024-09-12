namespace TextMatch.Domain.Contract.TextMatch
{
    public class TextMatchingResult
    {
        public bool Success {  get; set; } = false;
        public List<int> FoundPositions { get; set; } = new List<int>();
        public string ErrorMessage { get; set; }=string.Empty;
    }
}
