using Dapper;
using Domain.Models;
using Domain.RepositoriesInterfaces;
using Infrastructure.Dapper.Contexts;

namespace Infrastructure.Repositories;

public class AssetsRepository : IAssetsRepository
{
    private readonly DapperContext _context;

    public AssetsRepository(DapperContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Asset CreateAsset(string assetName)
    {
        var query = "INSERT INTO Asset (AssetName) OUTPUT INSERTED.* VALUES (@assetName);";

        var parameters = new DynamicParameters();
        parameters.Add("assetName", assetName);

        using (var connection = _context.CreateConnection())
        {
            return connection.Query<Asset>(query, parameters).First();
        }
    }

    public async Task<Asset> GetAsset(string assetName)
    {
        var query = "SELECT * FROM Asset WHERE AssetName = @assetName;";

        var parameters = new DynamicParameters();
        parameters.Add("assetName", assetName);

        using (var connection = _context.CreateConnection())
        {
            var asset = await connection.QueryFirstOrDefaultAsync<Asset>(query, parameters);

            return asset;
        }
    }

    public void CreateAssetData(IEnumerable<AssetData> assetData)
    {
        using (var connection = _context.CreateConnection())
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {

                foreach (var asset in assetData)
                {
                    var variationForOneDay = asset.VariationForOneDay.HasValue ? asset.VariationForOneDay.ToString() : "null";
                    var variationSinceFirstDay = asset.VariationSinceFirstDay.HasValue ? asset.VariationSinceFirstDay.ToString() : "null";

                    var query =
                        @"INSERT INTO AssetData (AssetId, TradingFloorDate, AssetValue, VariationForOneDay, VariationSinceFirstDay)  
                          VALUES (@assetId, @tradingFloorDate, @assetValue, @variationForOneDay, @variationSinceFirstDay);";


                    var parameters = new DynamicParameters();
                    parameters.Add("assetId", asset.AssetId);
                    parameters.Add("tradingFloorDate", asset.TradingFloorDate);
                    parameters.Add("assetValue", asset.AssetValue);
                    parameters.Add("variationForOneDay", asset.VariationForOneDay);
                    parameters.Add("variationSinceFirstDay", asset.VariationSinceFirstDay);

                    connection.Execute(query, parameters, transaction);

                }

                transaction.Commit();

            }
            connection.Close();
        }
    }

    public IEnumerable<AssetData> GetAssetDataForLastThirtyDays(int assetId)
    {
        var query = "SELECT * FROM AssetData WHERE AssetId = @assetId ORDER BY TradingFloorDate DESC;";

        var parameters = new DynamicParameters();
        parameters.Add("assetId", assetId);

        using (var connection = _context.CreateConnection())
        {
            return connection.Query<AssetData>(query, parameters);
        }
    }
}
