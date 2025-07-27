using Amazon.ECS;
using Amazon.ECS.Model;

namespace DotNetEcsServiceViewer.Api.Services
{
    /// <summary>
    /// ECSの情報を管理するサービス
    /// </summary>
    /// <param name="ecsClient"><see cref="IAmazonECS"/></param>
    public class EcsInfoControlService(IAmazonECS ecsClient)
    {
        /// <summary>
        /// ECSサービス情報を取得する
        /// </summary>
        /// <param name="clusterName">クラスタ名</param>
        /// <returns><see cref="Service"/>リストを返却する<see cref="Task{TResult}"/></returns>
        public async Task<List<Service>> GetEcsServicesAsync(string clusterName)
        {
            var services = new List<Service>();
            string nextToken = null;

            do
            {
                var request = new ListServicesRequest
                {
                    Cluster = clusterName,
                    NextToken = nextToken
                };

                var response = await ecsClient.ListServicesAsync(request);
                if (response.ServiceArns.Any())
                {
                    var describeServicesRequest = new DescribeServicesRequest
                    {
                        Cluster = clusterName,
                        Services = response.ServiceArns
                    };
                    var describeServicesResponse = await ecsClient.DescribeServicesAsync(describeServicesRequest);
                    services.AddRange(describeServicesResponse.Services);
                }
                nextToken = response.NextToken;

            } while (nextToken != null);

            return services;
        }

        /// <summary>
        /// ECSクラスタ名を取得する
        /// </summary>
        /// <returns>クラスタ名リストを返却する<see cref="Task{TResult}"/></returns>
        public async Task<List<string>> GetEcsClusterNamesAsync()
        {
            var clusterNames = new List<string>();
            string nextToken = null;

            do
            {
                var request = new ListClustersRequest
                {
                    NextToken = nextToken
                };
                var response = await ecsClient.ListClustersAsync(request);
                clusterNames.AddRange(response.ClusterArns.Select(arn => arn.Split('/').Last()));
                nextToken = response.NextToken;
            } while (nextToken != null);

            return clusterNames;
        }

        /// <summary>
        /// ECSサービスのdesiredCountを更新する
        /// </summary>
        /// <param name="clusterName">クラスタ名</param>
        /// <param name="serviceName">サービス名</param>
        /// <param name="desiredCount">設定するdesiredCount</param>
        /// <returns>更新された<see cref="Service"/>を返却する<see cref="Task{TResult}"/></returns>
        public async Task<Service> UpdateEcsServiceDesiredCountAsync(string clusterName, string serviceName, int desiredCount)
        {
            var request = new UpdateServiceRequest
            {
                Cluster = clusterName,
                Service = serviceName,
                DesiredCount = desiredCount
            };

            var response = await ecsClient.UpdateServiceAsync(request);
            return response.Service;
        }
    }
}
