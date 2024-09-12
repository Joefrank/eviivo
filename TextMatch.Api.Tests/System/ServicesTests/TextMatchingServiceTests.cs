
using FluentAssertions;
using System.Linq;
using TextMatch.Api.Tests.MockData;
using TextMatch.Services.Implementation;

namespace TextMatch.Api.Tests.System.ServicesTests
{
    public class TextMatchingServiceTests
    {
        public static IEnumerable<object[]> _textMatchingSetupValues => TextMatchingMockData.GetMatchingTextTestData();
        public static IEnumerable<object[]> _nonMatchingTextSetupValues => TextMatchingMockData.GetNonMatchingTextTestData();
        public static IEnumerable<object[]> _erroneousSetupValues => TextMatchingMockData.GetErroneousTestData();


        [Theory]
        [MemberData(nameof(_textMatchingSetupValues))]
        public void GetTextMatchingPositions_Returns_Positions_When_MainText_Contains_SubText(string text, string subText, string outputMessage)
        {
            //Arrange            
            var serviceUnderTest = new TextMatchingService();

            //Act
            var matchingResult = serviceUnderTest.GetTextMatchingPositions(text, subText);
            var outputString = string.Join(",", matchingResult.FoundPositions);

            //Assert
            matchingResult.Should().NotBeNull();
            matchingResult.Success.Should().BeTrue();
            outputString.Should().Be(outputMessage);

        }

        [Theory]
        [MemberData(nameof(_nonMatchingTextSetupValues))]
        public void GetTextMatchingPositions_Returns_NoOutput_When_MainText_DoesNotContains_SubText(string text, string subText, string outputMessage)
        {
            //Arrange            
            var serviceUnderTest = new TextMatchingService();

            //Act
            var matchingResult = serviceUnderTest.GetTextMatchingPositions(text, subText);
           
            //Assert
            matchingResult.Should().NotBeNull();
            matchingResult.Success.Should().BeTrue();
            matchingResult.FoundPositions.Should().BeEmpty();//No output

        }

        [Theory]
        [MemberData(nameof(_erroneousSetupValues))]
        public void GetTextMatchingPositions_Returns_ErrorMessage_When_InvalidValues_Passed(string text, string subText, string errorMessage)
        {
            //Arrange            
            var serviceUnderTest = new TextMatchingService();

            //Act
            var matchingResult = serviceUnderTest.GetTextMatchingPositions(text, subText);

            //Assert
            matchingResult.Should().NotBeNull();
            matchingResult.Success.Should().BeFalse();
            matchingResult.ErrorMessage.Should().Be(errorMessage);

        }

    }
}
