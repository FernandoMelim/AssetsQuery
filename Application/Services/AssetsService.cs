using Application.ServicesInterfaces;
using Domain.DTOs;
using Domain.Models;
using Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;

namespace Application.Services;

public class AssetsService : IAssetsService
{
    private static HttpClient client = new HttpClient();
    private readonly IConfiguration _configuration;
    private readonly IAssetsRepository _assetsRepository;

    public AssetsService(IConfiguration configuration, IAssetsRepository assetsRepository)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _assetsRepository = assetsRepository ?? throw new ArgumentNullException(nameof(assetsRepository));
    }

    public OperationResultDTO SaveAssetDataFromSource(string assetName)
    {
        var asset = _assetsRepository.GetAsset(assetName).Result;
        if(asset != null)
            return new OperationResultDTO() { Message = $"Asset {assetName} já consultado.", StatusCode = HttpStatusCode.BadRequest };

        asset = _assetsRepository.CreateAsset(assetName);

        var assetData = GetDataFromSource(assetName);

        if (assetData.Chart.Error != null)
            return new OperationResultDTO() { Message = assetData.Chart.Error.Description, StatusCode = assetData.StatusCode };

        var calculatedAssetsData = GetAssetsDataCalculated(asset, assetData);
        _assetsRepository.CreateAssetData(calculatedAssetsData);

        return new OperationResultDTO() { StatusCode = assetData.StatusCode };
    }

    private AssetDataDTO GetDataFromSource(string assetName)
    {

        var yahooSettings = _configuration.GetSection("YahooSettings");

        var requestUrl = yahooSettings.GetSection("YahooFinanceApiBaseUrl").Value
                        + yahooSettings.GetSection("YahooFinanceApiVersion").Value
                        + yahooSettings.GetSection("YahooFinanceApiAssetBasePath").Value
                        + assetName;

        HttpResponseMessage response = client.GetAsync(requestUrl).Result;

        var assetData = response.Content.ReadFromJsonAsync<AssetDataDTO>().Result;

        assetData.StatusCode = response.StatusCode;

        return assetData;
    }

    private List<AssetData> GetAssetsDataCalculated(Asset asset, AssetDataDTO assetData)
    {
        var results = assetData.Chart.Result.First();
        var timestamps = results.Timestamp;
        var openValues = results.Indicators.Quote.First().Open;

        var newAssets = new List<AssetData>();
        var firstValue = openValues[0];

        for (var valueIndex = 0; valueIndex < timestamps.Length; valueIndex++) 
        {
            long? timestamp = timestamps[valueIndex];
            var openValue = openValues[valueIndex] ?? 0;

            decimal? variationForOneDay = null;
            decimal? variationSinceFirstDay = null;

            if (valueIndex != 0)
            {
                var previousValue = openValues[valueIndex - 1];
                var minorValue = previousValue < openValue ? previousValue : openValue;
                var majorValue = previousValue > openValue ? previousValue : openValue;

                variationForOneDay = minorValue != 0
                    ? ((majorValue - minorValue) / minorValue) * 100
                    : majorValue;

                minorValue = firstValue < openValue ? firstValue : openValue;
                majorValue = firstValue > openValue ? firstValue : openValue;

                variationSinceFirstDay = minorValue != 0
                    ? ((majorValue - minorValue) / minorValue) * 100
                    : majorValue;
            }

            newAssets.Add(new AssetData() { 
                AssetId = asset.Id, 
                AssetValue = openValue, 
                TradingFloorDate = timestamp != null ? new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp.Value) : null, 
                VariationForOneDay = variationForOneDay, 
                VariationSinceFirstDay = variationSinceFirstDay });
        }

        return newAssets;
    }

    public OperationResultDTO GetAssetDataFromDatabase(string assetName)
    {
        var asset = _assetsRepository.GetAsset(assetName).Result;

        if(asset == null)
            return new OperationResultDTO() { StatusCode = HttpStatusCode.NotFound, Message = "Asset não encontrado." };

        var assetsData = _assetsRepository.GetAssetDataForLastThirtyDays(asset.Id).OrderBy(x => x.TradingFloorDate);

        return new OperationResultDTO<IEnumerable<AssetData>>() { Data = assetsData, StatusCode = HttpStatusCode.OK };
    }

}
