using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System; // Required for DateTime
using System.ComponentModel.DataAnnotations;

namespace MyWebApi.Models;

public class Car
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("brand")]
    public string Brand { get; set; } = null!;

    [BsonElement("model")]
    public string Model { get; set; } = null!;

    [BsonElement("year")]
    [Range(1886, 9999, ErrorMessage = "The car year must be between 1886 and 9999.")]
    public int Year { get; set; }

    [BsonElement("licensePlate")]
    public string LicensePlate { get; set; } = null!;

   [BsonElement("price")]
   [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be positive.")]
    public decimal Price { get; set; }

    // --- Added properties to fix CS1061 errors ---

    // Fixes the 'CreatedAt' error (Error 1)
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Set a default value

    // Fixes the 'IsAvailable' error (Error 2)
    [BsonElement("isAvailable")]
    public bool IsAvailable { get; set; } = true; // Set a default value
}