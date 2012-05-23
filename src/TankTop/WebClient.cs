using System;
using System.IO;
using System.Net;
using System.Text;
using ServiceStack.Text;
using TankTop.Extensions;

namespace TankTop
{
    class WebClient : IWebClient
    {
        readonly string baseAddress;
        static WebHeaderCollection webHeaderCollection;

        public WebClient(string baseAddress)
        {
            this.baseAddress = baseAddress;
            var userInfo = new Uri(baseAddress).UserInfo;
            var authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(userInfo));
            webHeaderCollection = new WebHeaderCollection { { HttpRequestHeader.Authorization, "Basic " + authInfo } };
        }

        public HttpStatusCode StatusCode { get; private set; }
        public string Response { get; private set; }

        public T Put<T>(string resource, object request)
        {
            return Send<T>(resource, "PUT", request);
        }

        public void Put(string resource, object request)
        {
            Send(resource, "PUT", request);
        }

        public T Get<T>(string resource)
        {
            return Send<T>(resource, "GET");
        }

        public void Delete(string resource)
        {
            Send(resource, "DELETE");
        }

        public void Delete(string resource, object request)
        {
            Send(resource, "DELETE", request);
        }

        T Send<T>(string resource, string method, object request = null)
        {
            using (var webResponse = HttpWebResponse(resource, method, request))
            {
                var responseStream = webResponse.GetResponseStream();
                var json = new StreamReader(responseStream).ReadToEnd();
                Response = json;
                return JsonSerializer.DeserializeFromString<T>(json);
            }
        }

        void Send(string resource, string method, object request = null)
        {
            using (HttpWebResponse(resource, method, request)) { }
        }

        HttpWebResponse HttpWebResponse(string resource, string method, object request)
        {
            var webRequest = WebRequest.Create(baseAddress + resource).CastTo<HttpWebRequest>();
            webRequest.Method = method;
            webRequest.Headers = webHeaderCollection;
            if (request != null)
            {
                using (var outputStream = webRequest.GetRequestStream())
                {
                    var json = JsonSerializer.SerializeToString(request);
                    var bytes = Encoding.ASCII.GetBytes(json);
                    outputStream.Write(bytes, 0, bytes.Length);
                }
            }
            try
            {
                var webResponse = webRequest.GetResponse().CastTo<HttpWebResponse>();
                StatusCode = webResponse.StatusCode;
                return webResponse;
            }
            catch (WebException e)
            {
                if (e.Response.IsNotNull())
                {
                    var statusCode = e.Response.CastTo<HttpWebResponse>().StatusCode;
                    var responseBody = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                    var exceptionMessage = "{0} : {1} : {2}".FormatWith((int)statusCode, statusCode, responseBody);
                    throw new WebException(exceptionMessage, e.Status);
                }
                throw;
            }
        }
    }
}