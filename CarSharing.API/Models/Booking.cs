using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyWebApi.Models
{
    public enum BookingStatus
    {
        Active,
        Completed,
        Cancelled
    }

    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Car ID is required")]
        [JsonPropertyName("carId")]
        public string CarId { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID is required")]
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(BookingStatus))]
        [BsonRepresentation(BsonType.String)]
        [JsonPropertyName("status")]
        public BookingStatus Status { get; set; }
    }
}