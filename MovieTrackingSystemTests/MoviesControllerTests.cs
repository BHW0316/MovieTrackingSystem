using Microsoft.AspNetCore.Mvc;
using MovieTrackingSystem.Controllers;
using MovieTrackingSystem.Models;
using MovieTrackingSystem.Services;
using NUnit.Framework;
using NSubstitute;
using NUnit.Framework.Legacy;
namespace MovieTrackingSystemTests
{
    [TestFixture]
    public class MoviesControllerTests
    {
        private MoviesController _moviesController;
        private IFileReaderService _fileReaderService;

        [SetUp]
        public void Setup()
        {
            // 使用 NSubstitute 建立 IFileReaderService 的模擬實例
            _fileReaderService = Substitute.For<IFileReaderService>();

            // 建立被測試的 Controller 實例，注入模擬的服務
            _moviesController = new MoviesController(_fileReaderService);
        }

        [Test]
        public void GetShowtimes_ShouldReturnOk()
        {
            // Arrange
            // 模擬 ReadFileContent 方法的行為
            _fileReaderService.ReadFileContent(Arg.Any<string>()).Returns("Mocked file content");

            // Act
            var result = _moviesController.GetShowtimes() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("Mocked file content"));
        }

        [Test]
        public void GetMovieOverviewByDate_ShouldReturnOk()
        {
            // Arrange
            var mockFileContent = @"{
                ""Showtimes"": [
                    {
                      ""Name"": ""Aquaman 2"",
                      ""Length"": ""2 hours"",
                      ""Time"": ""2023-12-20T12:30:00.00+08:00"",
                      ""Entrance"": ""2F-A""
                    },
                    {
                      ""Name"": ""Wish"",
                      ""Length"": ""1 hours 50 minutes"",
                      ""Time"": ""2023-12-29T17:30:00.00+08:00"",
                      ""Entrance"": ""2F-B""
                    }
                ]
            }";

            // 模擬 ReadFileContent 方法的行為
            _fileReaderService.ReadFileContent(Arg.Any<string>()).Returns(mockFileContent);

            // Act
            var result = _moviesController.GetMovieOverviewByDate(DateTime.Parse("2023-12-20")) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var overview = result.Value as List<Movie>;
            Assert.That(overview, Is.Not.Null);
            Assert.That(overview.Count, Is.EqualTo(1));
            Assert.That(overview[0].Name, Is.EqualTo("Aquaman 2"));
        }

        [Test]
        public void GetMovieOverviewByDate_WhenFileContentIsInvalid_ShouldReturnBadRequest()
        {
            // Arrange
            var invalidMockFileContent = "Invalid JSON Content"; // 假設 JSON 內容不合法

            // 模擬 ReadFileContent 方法的行為
            _fileReaderService.ReadFileContent(Arg.Any<string>()).Returns(invalidMockFileContent);

            // Act
            var result = _moviesController.GetMovieOverviewByDate(DateTime.Now) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.Not.EqualTo(200)); // 驗證狀態碼不是 200
            Assert.That(result.StatusCode, Is.EqualTo(500)); // 如果沒有其他錯誤處理邏輯，可以將預期狀態碼設置為 500
        }

        [Test]
        public void GetShowtimes_WhenFileContentIsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            _fileReaderService.ReadFileContent(Arg.Any<string>()).Returns(string.Empty);

            // Act
            var actionResult = _moviesController.GetShowtimes();

            // Assert
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var result = actionResult as OkObjectResult;
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var showtimes = result.Value as List<Movie>;
            Assert.That(showtimes, Is.Not.Null);
            Assert.That(showtimes, Is.Empty);
        }

        [Test]
        public void GetMovieOverviewByDate_WhenNoMoviesForDate_ShouldReturnEmptyList()
        {
            // Arrange
            var mockFileContent = @"{
        ""Showtimes"": [
            {
                ""Name"": ""Aquaman 2"",
                ""Length"": ""2 hours"",
                ""Time"": ""2023-12-20T12:30:00.00+08:00"",
                ""Entrance"": ""2F-A""
            }]}";

            // 模擬 ReadFileContent 方法的行為
            _fileReaderService.ReadFileContent(Arg.Any<string>()).Returns(mockFileContent);

            // Act
            var result = _moviesController.GetMovieOverviewByDate(DateTime.Parse("2023-12-21")) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            var overview = result.Value as List<Movie>;
            Assert.That(overview, Is.Not.Null);
            Assert.That(overview, Is.Empty);
        }

        [Test]
        public void GetMovieOverviewByDate_ValidDate_ShouldReturnOk()
        {
            // Arrange
            var validDate = new DateTime(2023, 12, 20);

            // Act
            var result = _moviesController.GetMovieOverviewByDate(validDate) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));            
        }
    }
}
