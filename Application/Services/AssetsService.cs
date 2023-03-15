using Application.ServicesInterfaces;
using Domain.DTOs;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Application.Services;

public class AssetsService : IAssetsService
{
    private static HttpClient client = new HttpClient();
    private readonly IConfiguration _configuration;

    public AssetsService(IConfiguration configuration)
    {
        this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public OperationResultDTO SaveAssetDataFromSource(string assetName)
    {
        var assetData = GetDataFromSource(assetName);

        if (assetData.Chart.Error != null)
            return new OperationResultDTO() { Message = assetData.Chart.Error.Description, StatusCode = assetData.StatusCode };

        InsertAssetDataIntoDatabase(assetName, assetData);

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

    private void InsertAssetDataIntoDatabase(string assetName, AssetDataDTO assetData)
    {

    }

    public OperationResultDTO GetAssetDataFromDatabase(string assetName)
    {
        throw new NotImplementedException();
    }

}
