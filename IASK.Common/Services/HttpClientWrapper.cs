using IASK.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.Common.Services
{
    public class HttpClientWrapper
    {
        private readonly HttpClient httpClient;
        private readonly IUrlFactory urlFactory;

        public HttpClientWrapper(IHttpClientFactory httpClientFactory, IUrlFactory urlFactory)
        {
            httpClient = httpClientFactory.CreateClient();
            this.urlFactory = urlFactory;
        }

        public async Task<TResponse> GetResponse<TResponse>(object request, CancellationToken token) where TResponse : class
        {
            {
                string Url = urlFactory.GetUrl<TResponse>();
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Url, stringContent, token);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(content);
                }
                else throw new HttpRequestException("Http request failed! Status code: " + response.StatusCode.ToString() + " " + response.ReasonPhrase);
            }
        }
    }
}
