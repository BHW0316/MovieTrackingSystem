namespace MovieTrackingSystem.Services
{
    public class FileReaderService : IFileReaderService
    {
        public string ReadFileContent(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return null;
            }
        }
    }
}
