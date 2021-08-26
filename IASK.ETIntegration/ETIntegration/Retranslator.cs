using ETALib.Models;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IASK.ETIntegration.Services
{
    public class Retranslator
    {
        private readonly HttpClient client;

        public Retranslator(HttpClient httpClient)
        {
            this.client = httpClient;
        }

        public async Task<Alert> TryRetranslate(string uri,object submit)
        {
            StringContent stringContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(submit),Encoding.UTF8, "application/json");
            HttpResponseMessage resp = await client.PostAsync(uri, stringContent);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Alert>(await resp.Content.ReadAsStringAsync()); ;
        }

        public async Task<string> Retranslate(string uri, string data,string requestType,CancellationToken cancellationToken, string mediaType=null)
        {
            switch (requestType)
            {
                case "POST":
                    {
                        StringContent stringContent = new StringContent(data, Encoding.UTF8, mediaType);
                        HttpResponseMessage resp = await client.PostAsync(uri, stringContent, cancellationToken);
                        return await resp.Content.ReadAsStringAsync(); ;
                    }
                case "GET":
                    {
                        HttpResponseMessage resp = await client.GetAsync(uri, cancellationToken);
                        return await resp.Content.ReadAsStringAsync(); ;
                    }
                default:
                    return "Unimplementated type of request!";
            }
        }
    }
}
