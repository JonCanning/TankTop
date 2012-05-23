using System.Net;

namespace TankTop
{
    public interface IWebClient
    {
        HttpStatusCode StatusCode { get; }
        string Response { get; }
        T Put<T>(string resource, object request);
        void Put(string resource, object request);
        T Get<T>(string resource);
        void Delete(string resource);
        void Delete(string resource, object request);
    }
}