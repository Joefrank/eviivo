

using TextMatch.Domain.Constants;
using TextMatch.Model.Request;

namespace TextMatch.Api.Tests.MockData
{
    public static class TextMatchingMockData
    {
        public const string MainString = "Polly put the kettle on, polly put the kettle on, polly put the kettle on we’ll all have tea";

        public static TextMatchingRequest DummyRequestWithEmptyMainText => new TextMatchingRequest { SubText = "Dummy Sub Text" };
        public static TextMatchingRequest DummyRequestWithEmptySubText => new TextMatchingRequest { MainText = "Dummy Main Text" };
        public static TextMatchingRequest DummyRequestWithLongerSubText => new TextMatchingRequest {MainText = "Dummy longer ", SubText = "Dummy Sub Text" };

        public static IEnumerable<object[]> GetMatchingTextTestData()
        {
            return new List<object[]> {

                new object[] {MainString, "polly", "1,26,51" },
                new object[] {MainString, "Polly", "1,26,51" },
                new object[] {MainString, "lly", "3,28,53" },
                new object[] {MainString, "ll", "3,28,53,78,82" },
                new object[] {MainString, "kettle", "15,40,65" }
            };

        }

        public static IEnumerable<object[]> GetNonMatchingTextTestData()
        {
            return new List<object[]> {
               new object[] {MainString, "polx", GenericMessages.NoOutputMessage },
                new object[] {MainString, "katle", GenericMessages.NoOutputMessage },
                new object[] {MainString, "ccv", GenericMessages.NoOutputMessage },
                new object[] {MainString, "dolly", GenericMessages.NoOutputMessage }
            };
        }

        public static IEnumerable<object[]> GetErroneousTestData()
        {
            return new List<object[]> {               
                new object[] {"This is a ", "This is a longer subtext", ErrorMessages.SubTextMustBeShorterThanMainTextErrorMsg },
                new object[] {"", "dolly", ErrorMessages.MainTextIsRequiredErrorMsg },
                new object[] {MainString, "", ErrorMessages.SubTextIsRequiredErrorMsg }
            };
        }

    }
}
