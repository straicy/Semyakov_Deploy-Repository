namespace MyWebApi.Models
{
    public class BookingDto
    {
        public string Id { get; set; } = string.Empty;
        public string CarId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}