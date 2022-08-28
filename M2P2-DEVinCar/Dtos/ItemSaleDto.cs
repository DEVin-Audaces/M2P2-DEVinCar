using System.ComponentModel.DataAnnotations;

namespace M2P2_DEVinCar.Dtos;

public class ItemSaleDto
{
    [StringLength(255)]
    public string? NameProduct { get; set; }

    public int Amount { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }
    
}