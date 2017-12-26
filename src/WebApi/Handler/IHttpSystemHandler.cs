#if !NETSTANDARD2_0
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Yizuan.Service.Api.WebApi
{
    /// <summary>
    /// ϵͳ��HTTP����ӿ�
    /// </summary>
    public interface IHttpSystemHandler
    {
        /// <summary>
        /// ��ʼʱ�Ĵ���
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns>����������ݲ�Ϊ�գ�ֱ�ӷ���,�����Ĵ����ټ���</returns>
        Task<HttpResponseMessage> OnBegin(HttpRequestMessage request, CancellationToken cancellationToken);

        /// <summary>
        /// ����ʱ�Ĵ���
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void OnEnd(HttpRequestMessage request, CancellationToken cancellationToken, HttpResponseMessage response);
    }
}
# endif
