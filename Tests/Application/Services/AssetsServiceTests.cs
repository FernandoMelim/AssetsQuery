using Application.ApiCalls;
using Application.Services;
using AssetsQuery.Controllers;
using Domain.DTOs;
using Domain.Models;
using Domain.RepositoriesInterfaces;
using Moq;
using System.Net;

namespace Tests.Application.Services;

public class AssetsServiceTests
{
    private AutoMocker _autoMocker;

    public AssetsServiceTests()
    {
        _autoMocker = new AutoMocker();
    }

    [Fact]
    public void TestWhenSavingExistingAsset()
    {
        var assetNameTest = "PETR4.SA";

        _autoMocker
            .Setup<IAssetsRepository, Task<Asset>>(x => x.GetAsset(It.IsAny<string>()))
            .ReturnsAsync(() => new Asset() { Id = 1, AssetName = assetNameTest });

        var assetsControllerInstance = _autoMocker.CreateInstance<AssetsService>();

        var response = assetsControllerInstance.SaveAssetDataFromSource(assetNameTest);

        Assert.NotNull(response);
        Assert.Equal($"Asset {assetNameTest} já consultado.", response.Message);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public void TestWhenGeetingNewAsset()
    {
        var assetNameTest = "PETR4.SA";

        _autoMocker
            .Setup<IAssetsRepository, Task<Asset>>(x => x.GetAsset(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        _autoMocker
            .Setup<IAssetsRepository, Asset>(x => x.CreateAsset(It.IsAny<string>()))
            .Returns( new Asset()
            {
                Id = 1,
                AssetName = assetNameTest
            });

        _autoMocker
            .Setup<IAssetsRepository>(x => x.CreateAssetData(It.IsAny<IEnumerable<AssetData>>()))
            .Callback(() => { });

        _autoMocker
            .Setup<IYahooApiCalls, AssetDataDTO>(x => x.GetDataFromSource(It.IsAny<string>()))
            .Returns(new AssetDataDTO()
            {
                StatusCode = HttpStatusCode.OK,
                Chart = new AssetDataDTO.ChartData()
                {
                    Error = null,
                    Result = new AssetDataDTO.ResultData[] 
                    { 
                        new AssetDataDTO.ResultData() 
                        { 
                            Timestamp = new long[] { 34534, 1413, 456456, 321654987 },
                            Indicators = new AssetDataDTO.IndicatorsData ()
                            {
                                Quote = new AssetDataDTO.QuoteData[]
                                {
                                    new AssetDataDTO.QuoteData()
                                    {
                                        Open = new decimal?[] { (decimal)12.5, (decimal)15.6, (decimal)13.6, (decimal)13.68 }
                                    }
                                }
                            }
                        } 
                    }
                }
            });

        var assetsControllerInstance = _autoMocker.CreateInstance<AssetsService>();

        var response = assetsControllerInstance.SaveAssetDataFromSource(assetNameTest);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public void TestWhenNotGeetingNewAsset()
    {
        var assetNameTest = "PETR4.SA";

        _autoMocker
            .Setup<IAssetsRepository, Task<Asset>>(x => x.GetAsset(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        _autoMocker
            .Setup<IAssetsRepository, Asset>(x => x.CreateAsset(It.IsAny<string>()))
            .Returns(new Asset()
            {
                Id = 1,
                AssetName = assetNameTest
            });

        _autoMocker
            .Setup<IAssetsRepository>(x => x.CreateAssetData(It.IsAny<IEnumerable<AssetData>>()))
            .Callback(() => { });

        _autoMocker
            .Setup<IYahooApiCalls, AssetDataDTO>(x => x.GetDataFromSource(It.IsAny<string>()))
            .Returns(new AssetDataDTO()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Chart = new AssetDataDTO.ChartData()
                {
                    Error = new ErrorDTO()
                    {
                        Code = "Not Found",
                        Description = "Resource not found"
                    },
                    Result = null
                }
            });

        var assetsControllerInstance = _autoMocker.CreateInstance<AssetsService>();

        var response = assetsControllerInstance.SaveAssetDataFromSource(assetNameTest);

        Assert.NotNull(response);
        Assert.Equal("Resource not found", response.Message);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public void TestWhenNotGettingAssetFromDatabase()
    {
        var assetNameTest = "PETR4.SA";

        _autoMocker
            .Setup<IAssetsRepository, Task<Asset>>(x => x.GetAsset(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var assetsControllerInstance = _autoMocker.CreateInstance<AssetsService>();

        var response = assetsControllerInstance.GetAssetDataFromDatabase(assetNameTest);

        Assert.NotNull(response);
        Assert.Equal("Asset ainda não foi consultado.", response.Message);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public void TestWhenGettingAssetFromDatabase()
    {
        var assetNameTest = "PETR4.SA";
        var assetDataReturned = new AssetData() { Id = 1, AssetId = 1, AssetValue = 10, TradingFloorDate = new DateTime(), VariationForOneDay = 50, VariationSinceFirstDay = 100 };

        _autoMocker
            .Setup<IAssetsRepository, Task<Asset>>(x => x.GetAsset(It.IsAny<string>()))
            .ReturnsAsync(() => new Asset() { Id = 1, AssetName = assetNameTest });

        _autoMocker
            .Setup<IAssetsRepository, IEnumerable<AssetData>>(x => x.GetAssetDataForLastThirtyDays(It.IsAny<int>()))
            .Returns(new List<AssetData>()
            {
                assetDataReturned
            });

        var assetsControllerInstance = _autoMocker.CreateInstance<AssetsService>();

        var response = (assetsControllerInstance.GetAssetDataFromDatabase(assetNameTest));
        var data = ((OperationResultDTO<IEnumerable<Domain.Models.AssetData>>)response).Data.First();

        Assert.NotNull(response);
        Assert.Equal(10, data.AssetValue);
        Assert.Equal(1, data.Id);
        Assert.Equal(1, data.AssetId);
        Assert.Equal(50, data.VariationForOneDay);
        Assert.Equal(100, data.VariationSinceFirstDay);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
