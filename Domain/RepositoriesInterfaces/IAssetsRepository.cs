using Domain.Models;

namespace Domain.RepositoriesInterfaces;

public interface IAssetsRepository
{
    int CreateAsset(string assetName);

    Task<Asset> GetAsset(string assetName);

    IEnumerable<AssetData> GetAssetDataForLastThirtyDays(int assetId);
}
