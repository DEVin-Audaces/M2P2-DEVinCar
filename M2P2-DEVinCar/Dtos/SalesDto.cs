using System.ComponentModel.DataAnnotations;
namespace M2P2_DEVinCar.Dtos;

public class SalesDto
{
    public int Id { get; set;}

    [StringLength(255)]
    public string NameSeller { get; set; }

    [StringLength(255)]
    public string NameBuyer { get; set; }

    public DateTime? SaleDate { get; set; }

    public List<ItemSaleDto> listSale { get; set; }

}