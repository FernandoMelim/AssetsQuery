using Domain.DTOs;

namespace Application.ApiCalls;

public interface IYahooApiCalls
{
    AssetDataDTO GetDataFromSource(string assetName);
}
