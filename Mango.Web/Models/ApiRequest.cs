using static Mango.Web.SD;

namespace Mango.Web.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; }  //by using namespace - static Mango.Web.SD;
        public string Url { get; set; }
        public object Data { get; set; }

        public string AccessToken { get; set; }  //string object for token ---require tokens to be passed to each request so that they can be validated for the request. 

    }
}
