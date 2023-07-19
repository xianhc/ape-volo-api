using System.ComponentModel;
using System.Threading.Tasks;
using ApeVolo.Api.Controllers.Base;
using ApeVolo.Common.Extention;
using ApeVolo.Common.Helper;
using ApeVolo.Common.Model;
using ApeVolo.IBusiness.Dto.System;
using ApeVolo.IBusiness.Interface.System;
using ApeVolo.IBusiness.QueryModel;
using ApeVolo.IBusiness.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace ApeVolo.Api.Controllers.System;

/// <summary>
/// 全局设置管理
/// </summary>
[Area("System")]
[Route("/api/appSecret")]
public class AppSecretController : BaseApiController
{
    #region 字段

    private readonly IAppSecretService _appSecretService;

    #endregion

    #region 构造函数

    public AppSecretController(IAppSecretService appSecretService)
    {
        _appSecretService = appSecretService;
    }

    #endregion

    #region 内部接口

    /// <summary>
    /// 新增设置
    /// </summary>
    /// <param name="createUpdateAppSecretDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    [Description("Add")]
    public async Task<ActionResult<object>> Create(
        [FromBody] CreateUpdateAppSecretDto createUpdateAppSecretDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        await _appSecretService.CreateAsync(createUpdateAppSecretDto);
        return Create();
    }

    /// <summary>
    /// 更新设置
    /// </summary>
    /// <param name="createUpdateAppSecretDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("edit")]
    [Description("Edit")]
    public async Task<ActionResult<object>> Update(
        [FromBody] CreateUpdateAppSecretDto createUpdateAppSecretDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        await _appSecretService.UpdateAsync(createUpdateAppSecretDto);
        return NoContent();
    }

    /// <summary>
    /// 删除设置
    /// </summary>
    /// <param name="idCollection"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    [Description("Delete")]
    public async Task<ActionResult<object>> Delete([FromBody] IdCollection idCollection)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        await _appSecretService.DeleteAsync(idCollection.IdArray);
        return Success();
    }

    /// <summary>
    /// 获取设置列表
    /// </summary>
    /// <param name="appsecretQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("List")]
    public async Task<ActionResult<object>> Query(AppsecretQueryCriteria appsecretQueryCriteria,
        Pagination pagination)
    {
        var appSecretList = await _appSecretService.QueryAsync(appsecretQueryCriteria, pagination);

        return new ActionResultVm<AppSecretDto>
        {
            Content = appSecretList,
            TotalElements = pagination.TotalElements
        }.ToJson();
    }


    /// <summary>
    /// 导出设置
    /// </summary>
    /// <param name="appsecretQueryCriteria"></param>
    /// <returns></returns>
    [HttpGet]
    [Description("Export")]
    [Route("download")]
    public async Task<ActionResult<object>> Download(AppsecretQueryCriteria appsecretQueryCriteria)
    {
        var appSecretExports = await _appSecretService.DownloadAsync(appsecretQueryCriteria);
        var data = new ExcelHelper().GenerateExcel(appSecretExports, out var mimeType);
        return File(data, mimeType);
    }

    #endregion
}