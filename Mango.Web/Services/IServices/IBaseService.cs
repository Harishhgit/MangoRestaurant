using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IBaseService
    {
        ResponseDto responseModel { get; set; }

        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
