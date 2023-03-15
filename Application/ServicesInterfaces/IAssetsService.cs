using Domain.DTOs;

namespace Application.ServicesInterfaces;

public interface IAssetsService
{
    OperationResultDTO SaveAssetDataFromSource(string assetName);

    OperationResultDTO GetAssetDataFromDatabase(string assetName);
}
