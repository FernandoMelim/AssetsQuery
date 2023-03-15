using Application.ServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace AssetsQuery.Controllers;

[ApiController]
[Route("[controller]")]
public class AssetsController : ControllerBase
{
    private IAssetsService _assetsService;

    public AssetsController(IAssetsService assetsService) 
    {
        _assetsService = assetsService ?? throw new ArgumentNullException(nameof(assetsService));
    }

    [HttpGet]
    public ActionResult Get(string assetName)
    {
        var operationResultDTO = _assetsService.SaveAssetDataFromSource(assetName);

        var result = new JsonResult(operationResultDTO);
        result.StatusCode = (int)operationResultDTO.StatusCode;

        return result;
    }

    [HttpPost]
    public ActionResult Post(string assetName)
    {
        var operationResultDTO = _assetsService.GetAssetDataFromDatabase(assetName);
        return Ok(operationResultDTO);
    }
}

