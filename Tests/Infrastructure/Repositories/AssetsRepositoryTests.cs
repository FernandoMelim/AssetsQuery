using Domain.Models;
using Infrastructure.Dapper.Contexts;
using Infrastructure.Repositories;
using Xunit;

namespace Tests.Infrastructure.Repositories;

public class AssetsRepositoryTests
{
    private AutoMocker _autoMocker;

    public AssetsRepositoryTests()
    {
        _autoMocker = new AutoMocker();
    }

    [Fact]
    public void TestingWhenGettingAsset()
    {
        var assetNameTest = "PETR4.SA";
        var connectionFactoryMock = new Mock<DapperContext>();

        var assets = new List<Asset>
        {
            new Asset { Id = 1, AssetName = assetNameTest },
            new Asset { Id = 2, AssetName = "test" }
        };

        var db = new InMemoryDatabase();
        db.Insert(assets);
        connectionFactoryMock.Setup(c => c.CreateConnection()).Returns(db.OpenConnection());

        var instance = new AssetsRepository(connectionFactoryMock.Object);
            
        var response = instance.GetAsset(assetNameTest).Result;

        Assert.NotNull(response);
        Assert.Equal(1, response.Id);
        Assert.Equal(assetNameTest, response.AssetName);
    }

    [Fact]
    public void TestingWhenCreatingAssetsData()
    {
        var assetNameTest = "PETR4.SA";
        var connectionFactoryMock = new Mock<DapperContext>();
        var assetsData = new List<AssetData>()
        {
            new AssetData() { Id = 1, AssetId = 1, AssetValue = 10, TradingFloorDate = new DateTime(), VariationForOneDay = 50, VariationSinceFirstDay = 100 },
            new AssetData() { Id = 2, AssetId = 2, AssetValue = 100, TradingFloorDate = new DateTime(), VariationForOneDay = 50, VariationSinceFirstDay = 100 }
        };

        var db = new InMemoryDatabase();
        db.Insert(assetsData);
        connectionFactoryMock.Setup(c => c.CreateConnection()).Returns(db.OpenConnection());

        var instance = new AssetsRepository(connectionFactoryMock.Object);

        instance.CreateAssetData(assetsData);

        var expectedQuery = @"INSERT INTO ""AssetData"" (""Id"",""AssetId"",""TradingFloorDate"",""AssetValue"",""VariationForOneDay"",""VariationSinceFirstDay"") VALUES (@Id,@AssetId,@TradingFloorDate,@AssetValue,@VariationForOneDay,@VariationSinceFirstDay)";
        var executedQuery = ((ServiceStack.OrmLite.OrmLiteConnection)db.OpenConnection()).LastCommandText;

        Assert.Equal(expectedQuery, executedQuery);
    }

    [Fact]
    public void TestingWhenGettingAssetData()
    {
        var assetNameTest = "PETR4.SA";
        var connectionFactoryMock = new Mock<DapperContext>();
        var assetsData = new List<AssetData>()
        {
            new AssetData() { Id = 1, AssetId = 1, AssetValue = 10, TradingFloorDate = new DateTime(), VariationForOneDay = 50, VariationSinceFirstDay = 100 },
            new AssetData() { Id = 2, AssetId = 1, AssetValue = 100, TradingFloorDate = new DateTime(), VariationForOneDay = 50, VariationSinceFirstDay = 100 }
        };

        var db = new InMemoryDatabase();
        db.Insert(assetsData);
        connectionFactoryMock.Setup(c => c.CreateConnection()).Returns(db.OpenConnection());

        var instance = new AssetsRepository(connectionFactoryMock.Object);

        var assetData = instance.GetAssetDataForLastThirtyDays(1);

        Assert.Equal(2, assetData.Count());
    }
}
