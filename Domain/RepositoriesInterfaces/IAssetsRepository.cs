using Domain.Models;

namespace Domain.RepositoriesInterfaces;

public interface IAssetsRepository
{
    Asset CreateAsset(string assetName);

    Task<Asset> GetAsset(string assetName);

    void CreateAssetData(IEnumerable<AssetData> assetData);

    IEnumerable<AssetData> GetAssetDataForLastThirtyDays(int assetId);
}
