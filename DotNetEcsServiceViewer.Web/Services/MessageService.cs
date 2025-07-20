using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DotNetEcsServiceViewer.Web.Services
{
    /// <summary>
    /// メッセージサービス
    /// </summary>
    public class MessageService
    {
        private readonly ITempDataDictionary _tempData;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tempDataDictionaryFactory"><see cref="ITempDataDictionaryFactory"/></param>
        /// <param name="httpContextAccessor"><see cref="IHttpContextAccessor"/></param>
        public MessageService(ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContextAccessor)
        {
             _tempData = tempDataDictionaryFactory.GetTempData(httpContextAccessor.HttpContext);
        }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string Error
        {
            get
            {
                return _tempData["ErrorMessage"]?.ToString();
            }

            set
            {
                _tempData["ErrorMessage"] = value;
            }
        }

        /// <summary>
        /// 処理成功メッセージ
        /// </summary>
        public string Success
        {
            get
            {
                return _tempData["SuccessMessage"]?.ToString();
            }

            set
            {
                _tempData["SuccessMessage"] = value;
            }
        }
    }
}
