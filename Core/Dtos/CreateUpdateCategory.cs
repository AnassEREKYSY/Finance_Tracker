using System;

namespace Core.Dtos;

public class CreateUpdateCategory
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
}
