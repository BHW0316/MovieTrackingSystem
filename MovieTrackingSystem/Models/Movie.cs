namespace MovieTrackingSystem.Models
{
    public class Movie
    {
        /// <summary>
        /// 電影名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 電影時長(字串表示)
        /// </summary>
        public string Length { get; set; }
        /// <summary>
        /// 上映時間
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 電影撥放地點
        /// </summary>
        public string Entrance { get; set; }
    }
}
