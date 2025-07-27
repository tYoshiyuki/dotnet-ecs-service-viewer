using Amazon.ECS.Model;
using DotNetEcsServiceViewer.Api.Dto;
using DotNetEcsServiceViewer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetEcsServiceViewer.Api.Controllers
{
    /// <summary>
    /// ECSの情報を管理するコントローラ
    /// </summary>
    /// <param name="ecsInfoControlService"></param>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EcsController(EcsInfoControlService ecsInfoControlService) : Controller
    {
        /// <summary>
        /// ECSサービス情報を取得する
        /// </summary>
        /// <param name="clusterName">クラスタ名</param>
        /// <returns><see cref="Service"/>リストを返却する<see cref="Task{TResult}"/></returns>
        [HttpGet]
        public async Task<IEnumerable<Service>> GetEcsServicesAsync(string clusterName)
        {
            return await ecsInfoControlService.GetEcsServicesAsync(clusterName);
        }

        /// <summary>
        /// ECSクラスタ名を取得する
        /// </summary>
        /// <returns>クラスタ名リストを返却する<see cref="Task{TResult}"/></returns>
        [HttpGet]
        public async Task<IEnumerable<string>> GetEcsClusterNamesAsync()
        {
            return await ecsInfoControlService.GetEcsClusterNamesAsync();
        }

        /// <summary>
        /// ECSサービスのdesiredCountを更新する
        /// </summary>
        /// <param name="request"><see cref="UpdateEcsServiceDesiredCountRequest"/></param>
        /// <returns>更新された<see cref="Service"/>を返却する<see cref="Task{TResult}"/></returns>
        [HttpPost]
        public async Task<Service> UpdateEcsServiceDesiredCountAsync(UpdateEcsServiceDesiredCountRequest request)
        {
            return await ecsInfoControlService.UpdateEcsServiceDesiredCountAsync(request.ClusterName, request.ServiceName, request.DesiredCount);
        }
    }
}
