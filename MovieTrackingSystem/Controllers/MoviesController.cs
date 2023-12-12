using Microsoft.AspNetCore.Mvc;
using MovieTrackingSystem.Models;
using MovieTrackingSystem.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieTrackingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IFileReaderService _fileReaderService;

        public MoviesController(IFileReaderService fileReaderService)
        {
            _fileReaderService = fileReaderService;
        }

        [HttpGet("showtimes")]
        public IActionResult GetShowtimes()
        {
            try
            {
                // read JSON file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "input.json");

                var fileContent = _fileReaderService.ReadFileContent(filePath);
                if (string.IsNullOrWhiteSpace(fileContent))
                {
                    // 如果文件內容是空的，返回空清單
                    return Ok(new List<Movie>());
                }

                return Ok(fileContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in API: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }      

        [HttpGet("overview/{date}")]
        public IActionResult GetMovieOverviewByDate(DateTime date)
        {
            try
            {
                // 讀取 JSON
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "input.json");

                // 使用服務讀取檔案內容
                var fileContent = _fileReaderService.ReadFileContent(filePath);

                // 取得特定日期的電影時刻總覽
                var overview = GetMovieOverview(fileContent, date);

                return Ok(overview);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in API: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        private IEnumerable<Movie> GetMovieOverview(string fileContent, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(fileContent))
            {
                // 處理文件內容為空的情況
                return Enumerable.Empty<Movie>();
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(fileContent);

            if (jsonObject != null && jsonObject.TryGetValue("Showtimes", out JToken showtimesToken))
            {
                var showtimes = showtimesToken.ToObject<List<Movie>>();

                // 篩選特定日期的電影時刻
                var overview = showtimes.Where(m => m.Time.Date == date.Date).ToList();
                return overview;
            }

            return Enumerable.Empty<Movie>();
        }
    }
}
