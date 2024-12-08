using System;
using System.Text.Json.Serialization;
using Core.Enums;

namespace Core.Dtos;

public class CreateUpdateTransaction
{
    public int TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("TransactionDate")]
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? UserFirstName { get; set; } = string.Empty;
    public string? UserLastName { get; set; } = string.Empty;
}
