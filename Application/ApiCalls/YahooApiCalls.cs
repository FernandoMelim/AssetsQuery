using Domain.DTOs;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Application.ApiCalls;

public class YahooApiCalls : IYahooApiCalls
{
    private static HttpClient client = new HttpClient();
    private readonly IConfiguration _configuration;

    public YahooApiCalls(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public AssetDataDTO GetDataFromSource(string assetName)
    {

        var yahooSettings = _configuration.GetSection("YahooSettings");

        var requestUrl = yahooSettings.GetSection("YahooFinanceApiBaseUrl").Value
                        + yahooSettings.GetSection("YahooFinanceApiVersion").Value
                        + yahooSettings.GetSection("YahooFinanceApiAssetBasePath").Value
                        + assetName
                        + yahooSettings.GetSection("MonthFilter").Value;

        HttpResponseMessage response = client.GetAsync(requestUrl).Result;

        var assetData = response.Content.ReadFromJsonAsync<AssetDataDTO>().Result;

        assetData.StatusCode = response.StatusCode;

        return assetData;
    }
}
