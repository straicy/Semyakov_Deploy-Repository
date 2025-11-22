// Models/UserRole.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
namespace MyWebApi.Models
{
    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        User = 4
    }
}