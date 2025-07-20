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
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="ecsInfoControlService"><see cref="EcsInfoControlService"/></param>
        /// <param name="messageService"><see cref="MessageService"/></param>
        public IndexModel(EcsInfoControlService ecsInfoControlService, MessageService messageService)
        {
            _ecsInfoControlService = ecsInfoControlService;
            _messageService = messageService;
        }

        /// <summary>
        /// Get���\�b�h�n���h���[
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
        /// �T�[�r�X��~�{�^���̃n���h���[
        /// </summary>
        /// <param name="clusterName">�N���X�^��</param>
        /// <param name="serviceName">�T�[�r�X��</param>
        /// <returns><see cref="IActionResult"/></returns>
        public async Task<IActionResult> OnPostStopServiceAsync(string clusterName, string serviceName)
        {
            if (string.IsNullOrEmpty(clusterName) || string.IsNullOrEmpty(serviceName))
            {
                // �G���[����
                _messageService.Error = "�N���X�^���A�T�[�r�X�����������ݒ肳��Ă��܂���B";
                return RedirectToPage(new { SelectedCluster });
            }

            try
            {
                await _ecsInfoControlService.UpdateEcsServiceDesiredCountAsync(clusterName, serviceName, 0);
                _messageService.Success = $"ECS�T�[�r�X���~���܂����B�T�[�r�X��: {serviceName} �N���X�^��: {clusterName}";
            }
            catch (AmazonECSException ex)
            {
                _messageService.Error = $"ECS�T�[�r�X�̒�~�Ɏ��s���܂����B�T�[�r�X��: {serviceName}: {ex.Message}";
            }
            catch (Exception ex)
            {
                _messageService.Error = $"�z��O�̃G���[���������܂���: {ex.Message}";
            }

            // �f�[�^�̍ă��[�h�ƃy�[�W�̃��_�C���N�g
            return RedirectToPage(new { SelectedCluster = clusterName });
        }

        /// <summary>
        /// �T�[�r�X�N���{�^���̃n���h���[
        /// </summary>
        /// <param name="clusterName">�N���X�^��</param>
        /// <param name="serviceName">�T�[�r�X��</param>
        /// <returns><see cref="IActionResult"/></returns>
        public async Task<IActionResult> OnPostStartServiceAsync(string clusterName, string serviceName)
        {
            if (string.IsNullOrEmpty(clusterName) || string.IsNullOrEmpty(serviceName))
            {
                _messageService.Error = "�N���X�^���A�T�[�r�X�����������ݒ肳��Ă��܂���B";
                return RedirectToPage(new { SelectedCluster });
            }

            try
            {
                await _ecsInfoControlService.UpdateEcsServiceDesiredCountAsync(clusterName, serviceName, 1);
                _messageService.Success = $"ECS�T�[�r�X���N�����܂����B�T�[�r�X��: {serviceName} �N���X�^��: {clusterName}";
            }
            catch (AmazonECSException ex)
            {
                _messageService.Error = $"ECS�T�[�r�X�̋N���Ɏ��s���܂����B�T�[�r�X��: {serviceName}: {ex.Message}";
            }
            catch (Exception ex)
            {
                _messageService.Error = $"�z��O�̃G���[���������܂���: {ex.Message}";
            }

            return RedirectToPage(new { SelectedCluster = clusterName });
        }
    }
}
