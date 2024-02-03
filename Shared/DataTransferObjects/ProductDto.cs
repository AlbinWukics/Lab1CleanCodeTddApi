﻿namespace Shared.DataTransferObjects;

public class ProductDto
{
    public Guid Id { get; set; }


    public string Name { get; set; } = string.Empty;


    public string Description { get; set; } = string.Empty;


    public decimal UnitPrice { get; set; }


    public DateTime CreatedDate { get; set; }


    public DateTime LastUpdatedDate { get; set; }
}