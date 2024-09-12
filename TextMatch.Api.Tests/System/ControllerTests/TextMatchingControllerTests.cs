
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Runtime.Serialization;
using TextMatch.Api.Tests.MockData;
using TextMatch.Controllers;
using TextMatch.Domain.Constants;
using TextMatch.Domain.Contract.TextMatch;
using TextMatch.Model.Request;
using TextMatch.Services.Abstract;
using Xunit.Sdk;

namespace TextMatch.Api.Tests.System.ControllerTests
{
    public class TextMatchingControllerTests
    {
        private TextMatchController _controllerUnderTest;
        private TextMatchingRequest _textMatchingRequest;

        public static IEnumerable<object[]> _textMatchingSetupValues => TextMatchingMockData.GetMatchingTextTestData();
        public static IEnumerable<object[]> _nonMatchingTextSetupValues => TextMatchingMockData.GetNonMatchingTextTestData();


        #region utilmethods

        private void ArrangeData(TextMatchingRequest request)
        {
            ArrangeData(request.MainText, request.SubText);
        }

        private void ArrangeData(string text, string subText)
        {
            var textMatchingService = new Mock<ITextMatchingService>();
            var logger = new Mock<ILogger<TextMatchController>>();           
            textMatchingService.Setup(_ => _.GetTextMatchingPositions(text, subText))
                .Returns(new TextMatchingResult { Success = false, ErrorMessage = GenericMessages.NoOutputMessage });
            _controllerUnderTest = new TextMatchController(logger.Object, textMatchingService.Object);
            _textMatchingRequest = new TextMatchingRequest { MainText = text, SubText = subText };
        }

        //Act on SUT
        private BadRequestObjectResult ActOnBadRequest(string errorKey, string errorMessage)
        {
            _controllerUnderTest.ModelState.AddModelError(errorKey, errorMessage);            
            IActionResult actionResult = _controllerUnderTest.ComputeTextMatchingPositions(_textMatchingRequest);
            return actionResult as BadRequestObjectResult; // <-- Cast is before using it.
        }

        private void AssertBadRequestResult(BadRequestObjectResult badRequestObject, string errorMessage)
        {
            //Assert
            badRequestObject.Should().NotBeNull();
            badRequestObject.Value.Should().NotBeNull();
            badRequestObject.Value.Should().Be(errorMessage);
        }

        #endregion

        #region Tests

        [Fact]
        public void ComputeTextMatchingPositions_Produces_ErrorMessage_When_MainText_Is_Empty()
        {
            //Arrange 
            ArrangeData(TextMatchingMockData.DummyRequestWithEmptyMainText);

            //Act
            var badRequestResult = ActOnBadRequest("MainTextErr", ErrorMessages.MainTextIsRequiredErrorMsg);

            //Assert
            AssertBadRequestResult(badRequestResult, ErrorMessages.MainTextIsRequiredErrorMsg);

        }

        [Fact]
        public void ComputeTextMatchingPositions_Produces_ErrorMessage_When_SubText_Is_Empty()
        {
            //Arrange 
            ArrangeData(TextMatchingMockData.DummyRequestWithEmptySubText);

            //Act
            var badRequestResult = ActOnBadRequest("SubTextErr", ErrorMessages.SubTextIsRequiredErrorMsg);

            //Assert
            AssertBadRequestResult(badRequestResult, ErrorMessages.SubTextIsRequiredErrorMsg);
        }

        [Fact]
        public void ComputeTextMatchingPositions_Produces_ErrorMessage_When_SubText_IsLonger_Than_MainText()
        {
            //Arrange 
            ArrangeData(TextMatchingMockData.DummyRequestWithLongerSubText);

            //Act
            var badRequestResult = ActOnBadRequest("SubTextLongerErr", ErrorMessages.SubTextMustBeShorterThanMainTextErrorMsg);

            //Assert
            AssertBadRequestResult(badRequestResult, ErrorMessages.SubTextMustBeShorterThanMainTextErrorMsg);
        }


        [Theory]
        [MemberData(nameof(_textMatchingSetupValues))]
        public void ComputeTextMatchingPositions_Returns_Positions_When_MainText_Contains_SubText(string text, string subText, string outputMessage)
        {
            //Arrange           

            var textMatchingService = new Mock<ITextMatchingService>();
            var logger = new Mock<ILogger<TextMatchController>>();
            var resultingPositions = outputMessage.Split(',').Select(int.Parse).ToList();//this is message that is supposed to be output
            textMatchingService.Setup(_ => _.GetTextMatchingPositions(text, subText))
                .Returns(new TextMatchingResult { Success = true, FoundPositions = resultingPositions });
            _controllerUnderTest = new TextMatchController(logger.Object, textMatchingService.Object);
            _textMatchingRequest = new TextMatchingRequest { MainText = text, SubText = subText };

            //Act
            IActionResult actionResult = _controllerUnderTest.ComputeTextMatchingPositions(_textMatchingRequest);
            var okResponseObject = actionResult as OkObjectResult; // <-- Cast is before using it.

            //Assert
            okResponseObject.Should().NotBeNull();
            okResponseObject.Value.Should().NotBeNull();
            okResponseObject.Value.Should().Be(outputMessage);
        }

        [Theory]
        [MemberData(nameof(_textMatchingSetupValues))]
        public void GetTextMatchingPositions_Returns_Positions_When_MainText_Contains_SubText(string text, string subText, string outputMessage)
        {
            //Arrange           

            var textMatchingService = new Mock<ITextMatchingService>();
            var logger = new Mock<ILogger<TextMatchController>>();
            var resultingPositions = outputMessage.Split(',').Select(int.Parse).ToList();//this is message that is supposed to be output
            textMatchingService.Setup(_ => _.GetTextMatchingPositions(text, subText))
                .Returns(new TextMatchingResult { Success = true, FoundPositions = resultingPositions });
            _controllerUnderTest = new TextMatchController(logger.Object, textMatchingService.Object);
            _textMatchingRequest = new TextMatchingRequest { MainText = text, SubText = subText };

            //Act
            IActionResult actionResult = _controllerUnderTest.GetTextMatchingPositions(text, subText);
            var okResponseObject = actionResult as OkObjectResult; // <-- Cast is before using it.

            //Assert
            okResponseObject.Should().NotBeNull();
            okResponseObject.Value.Should().NotBeNull();
            okResponseObject.Value.Should().Be(outputMessage);
        }


        [Theory]
        [MemberData(nameof(_nonMatchingTextSetupValues))]
        public void ComputeTextMatchingPositions_Returns_NoOutput_When_MainText_DoesNotContain_SubText(string text, string subText, string outputMessage)
        {
            //Arrange           

            var textMatchingService = new Mock<ITextMatchingService>();
            var logger = new Mock<ILogger<TextMatchController>>();
            
            textMatchingService.Setup(_ => _.GetTextMatchingPositions(text, subText))
                .Returns(new TextMatchingResult { Success = true, ErrorMessage = outputMessage });//outputMessage should be no positions found

            _controllerUnderTest = new TextMatchController(logger.Object, textMatchingService.Object);
            _textMatchingRequest = new TextMatchingRequest { MainText = text, SubText = subText };

            //Act
            IActionResult actionResult = _controllerUnderTest.ComputeTextMatchingPositions(_textMatchingRequest);
            var okResponseObject = actionResult as OkObjectResult; // <-- Cast is before using it.

            //Assert
            okResponseObject.Should().NotBeNull();
            okResponseObject.Value.Should().NotBeNull();
            okResponseObject.Value.Should().Be(outputMessage);
        }

        #endregion

    }
}
