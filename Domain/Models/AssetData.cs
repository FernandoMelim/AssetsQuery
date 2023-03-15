namespace Domain.Models;

public class AssetData
{
    public int Id { get; set; }

    public int AssetId { get; set; }

    public DateTime TradingFloorDate { get; set; }

    public decimal AssetValue { get; set; }

    public decimal VariationForOneDay { get; set; }

    public decimal VariationSinceFirstDay { get; set; }
}
