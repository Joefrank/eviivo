using TextMatch.Domain.Constants;
using TextMatch.Domain.Contract.TextMatch;
using TextMatch.Services.Abstract;

namespace TextMatch.Services.Implementation
{
    public class TextMatchingService : ITextMatchingService
    {

        public bool AreTwoCharsEqualCaseInsensitive(char char1, char char2)
        {
            // Check if the characters are alphabetic (A-Z or a-z)
            if (char1 >= 'A' && char1 <= 'Z')
            {
                // Convert char1 to lowercase by adding the ASCII difference between 'A' and 'a'
                char1 = (char)(char1 + ('a' - 'A'));
            }

            if (char2 >= 'A' && char2 <= 'Z')
            {
                // Convert char2 to lowercase by adding the ASCII difference between 'A' and 'a'
                char2 = (char)(char2 + ('a' - 'A'));
            }

            // Compare the resulting characters
            return char1 == char2;
        }

        public string ValidateTwoStringsForComparison(string text, string subText)
        {
            //validate text here
            if (string.IsNullOrEmpty(text)) {
                return ErrorMessages.MainTextIsRequiredErrorMsg;
            }
            //validate subText
            else if (string.IsNullOrEmpty(subText)) {
                return ErrorMessages.SubTextIsRequiredErrorMsg; 
            }
            //make sure subText is shorter or same length as text to avoid indexing errors.
            else if (text.Length < subText.Length)
            {
                return ErrorMessages.SubTextMustBeShorterThanMainTextErrorMsg;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a list of position of subText in text. validation assumes that text and subText are not null.
        /// And also that subText is smaller or equal to text in length
        /// </summary>
        /// <param name="text"></param>
        /// <param name="subText"></param>
        /// <returns></returns>
        public TextMatchingResult GetTextMatchingPositions(string text, string subText)
        {
            var errorMessage = ValidateTwoStringsForComparison(text, subText);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new() { ErrorMessage = errorMessage };
            }

            //get the lengths for our calculations
            int mainLength = text.Length;
            int searchLength = subText.Length;
            var retList = new List<int>();

            for (int i = 0; i <= mainLength - searchLength; i++)
            {
                bool found = true;
                for (int j = 0; j < searchLength; j++)
                {
                    if (!AreTwoCharsEqualCaseInsensitive(text[i + j], subText[j]))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    retList.Add(i + 1);
                }
            }

            return new() { Success = true, FoundPositions = retList };
        }       

    }
}
