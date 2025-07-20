using Amazon.ECS;
using Amazon.ECS.Model;
using DotNetEcsServiceViewer.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetEcsServiceViewer.Web.Pages
{
    /// <summary>
    /// IndexModel
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly EcsInfoControlService _ecsInfoControlService;
        private readonly MessageService _messageService;

        public List<string> ClusterNames { get; set; }
        public List<Service> EcsServices { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SelectedCluster { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ecsInfoControlService"><see cref="EcsInfoControlService"/></param>
        /// <param name="messageService"><see cref="MessageService"/></param>
        public IndexModel(EcsInfoControlService ecsInfoControlService, MessageService messageService)
        {
            _ecsInfoControlService = ecsInfoControlService;
            _messageService = messageService;
        }

        /// <summary>
        /// Getメソッドハンドラー
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task OnGetAsync()
        {
            ClusterNames = await _ecsInfoControlService.GetEcsClusterNamesAsync();

            if (!string.IsNullOrEmpty(SelectedCluster))
            {
                EcsServices = await _ecsInfoControlService.GetEcsServicesAsync(SelectedCluster);
            }
            else
            {
                EcsServices = [];
            }
        }

        /// <summary>
        /// サービス停止ボタンのハンドラー
        /// </summary>
        /// <param name="clusterName">クラスタ名</param>
        /// <param name="serviceName">サービス名</param>
        /// <returns><see cref="IActionResult"/></returns>
        public async Task<IActionResult> OnPostStopServiceAsync(string clusterName, string serviceName)
        {
            if (string.IsNullOrEmpty(clusterName) || string.IsNullOrEmpty(serviceName))
            {
                // エラー処理
                _messageService.Error = "クラスタ名、サービス名が正しく設定されていません。";
                return RedirectToPage(new { SelectedCluster });
            }

            try
            {
                await _ecsInfoControlService.UpdateEcsServiceDesiredCountAsync(clusterName, serviceName, 0);
                _messageService.Success = $"ECSサービスを停止しました。サービス名: {serviceName} クラスタ名: {clusterName}";
            }
            catch (AmazonECSException ex)
            {
                _messageService.Error = $"ECSサービスの停止に失敗しました。サービス名: {serviceName}: {ex.Message}";
            }
            catch (Exception ex)
            {
                _messageService.Error = $"想定外のエラーが発生しました: {ex.Message}";
            }

            // データの再ロードとページのリダイレクト
            return RedirectToPage(new { SelectedCluster = clusterName });
        }

        /// <summary>
        /// サービス起動ボタンのハンドラー
        /// </summary>
        /// <param name="clusterName">クラスタ名</param>
        /// <param name="serviceName">サービス名</param>
        /// <returns><see cref="IActionResult"/></returns>
        public async Task<IActionResult> OnPostStartServiceAsync(string clusterName, string serviceName)
        {
            if (string.IsNullOrEmpty(clusterName) || string.IsNullOrEmpty(serviceName))
            {
                _messageService.Error = "クラスタ名、サービス名が正しく設定されていません。";
                return RedirectToPage(new { SelectedCluster });
            }

            try
            {
                await _ecsInfoControlService.UpdateEcsServiceDesiredCountAsync(clusterName, serviceName, 1);
                _messageService.Success = $"ECSサービスが起動しました。サービス名: {serviceName} クラスタ名: {clusterName}";
            }
            catch (AmazonECSException ex)
            {
                _messageService.Error = $"ECSサービスの起動に失敗しました。サービス名: {serviceName}: {ex.Message}";
            }
            catch (Exception ex)
            {
                _messageService.Error = $"想定外のエラーが発生しました: {ex.Message}";
            }

            return RedirectToPage(new { SelectedCluster = clusterName });
        }
    }
}
