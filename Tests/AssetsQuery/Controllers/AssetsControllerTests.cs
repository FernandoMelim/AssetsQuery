
using Application.ServicesInterfaces;
using AssetsQuery.Controllers;
using Domain.DTOs;
using Moq;

namespace Tests.AssetsQuery.Controllers;

public class AssetsControllerTests
{
    private AutoMocker _autoMocker;

    public AssetsControllerTests()
    {
        _autoMocker = new AutoMocker();
    }

    [Fact]
    public void TestGetEndpoint()
    {
        var testStatusCode = System.Net.HttpStatusCode.NotFound;
        var assetNameTest = "PETR4.SA";
        var returnedDTO = new OperationResultDTO() { StatusCode = testStatusCode };

        _autoMocker
            .Setup<IAssetsService, OperationResultDTO>(x => x.GetAssetDataFromDatabase(It.IsAny<string>()))
            .Callback((string assetName) =>
            {
                returnedDTO.Message = assetName;
            })
            .Returns(returnedDTO);

        var assetsControllerInstance = _autoMocker.CreateInstance<AssetsController>();

        var response = assetsControllerInstance.Get(assetNameTest);

        var responseCasted = (OperationResultDTO)((Microsoft.AspNetCore.Mvc.JsonResult)((Microsoft.AspNetCore.Mvc.ObjectResult)response).Value).Value;

        Assert.NotNull(responseCasted);
        Assert.True(responseCasted.StatusCode == testStatusCode);
        Assert.True(responseCasted.Message == assetNameTest);
    }

    [Fact]
    public void TestPostEndpoint()
    {
        var testStatusCode = System.Net.HttpStatusCode.OK;
        var assetNameTest = "PETR4.SA";
        var returnedDTO = new OperationResultDTO() { StatusCode = testStatusCode };

        _autoMocker
            .Setup<IAssetsService, OperationResultDTO>(x => x.SaveAssetDataFromSource(It.IsAny<string>()))
            .Callback((string assetName) =>
            {
                returnedDTO.Message = assetName;
            })
            .Returns(returnedDTO);

        var assetsControllerInstance = _autoMocker.CreateInstance<AssetsController>();

        var response = assetsControllerInstance.Post(assetNameTest);

        var responseCasted = (OperationResultDTO)((Microsoft.AspNetCore.Mvc.JsonResult)response).Value;

        Assert.NotNull(responseCasted);
        Assert.True(responseCasted.StatusCode == testStatusCode);
        Assert.True(responseCasted.Message == assetNameTest);
    }
}
