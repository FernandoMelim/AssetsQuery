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

    public int CreateAsset(string assetName)
    {
        var query = "INSERT INTO Asset (AssetName) OUTPUT INSERTED.Id VALUES (@assetName)";

        var parameters = new DynamicParameters();
        parameters.Add("assetName", assetName);

        using (var connection = _context.CreateConnection())
        {
            return connection.Query<int>(query, parameters).First();
        }
    }

    public async Task<Asset> GetAsset(string assetName)
    {
        var query = "SELECT * FROM Asset WHERE AssetName = @assetName";

        var parameters = new DynamicParameters();
        parameters.Add("assetName", assetName);

        using (var connection = _context.CreateConnection())
        {
            var asset = await connection.QueryFirstOrDefaultAsync<Asset>(query, parameters);

            return asset;
        }
    }

    public IEnumerable<AssetData> GetAssetDataForLastThirtyDays(int assetId)
    {
        var query = "SELECT TOP 30 * FROM AssetData WHERE AssetId = @assetId ORDER BY TradingFloorDate DESC";

        var parameters = new DynamicParameters();
        parameters.Add("assetId", assetId);

        using (var connection = _context.CreateConnection())
        {
            return connection.Query<AssetData>(query, parameters);
        }
    }
}
