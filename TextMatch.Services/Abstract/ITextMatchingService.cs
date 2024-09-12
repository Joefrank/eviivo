
using TextMatch.Domain.Contract.TextMatch;

namespace TextMatch.Services.Abstract
{
    public interface ITextMatchingService
    {
        bool AreTwoCharsEqualCaseInsensitive(char char1, char char2);
        string ValidateTwoStringsForComparison(string text, string subText);
        TextMatchingResult GetTextMatchingPositions(string text, string subText);
    }
}
