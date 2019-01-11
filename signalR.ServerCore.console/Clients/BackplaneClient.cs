using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace signalR.ServerCore.Clients
{
    public class BackplaneClient
    {
        private string BackPlaneInstanceUrl = "";
        public BackplaneClient(string backplaneUrl)
        {
            BackPlaneInstanceUrl = backplaneUrl;
        }

        public async Task Notify(string msgType, string msgContent)
        {
            var urlPath = $"/backplane/publish?msgType={msgType}&msgContent={msgContent}";
            await Post(urlPath);
        }

        public async Task AddInstance(string urlInstance)
        {
            var urlPath = $"/backplane/addinstance?urlInstance={urlInstance}";
            await Post(urlPath);
        }

        private async Task Post(string urlPath)
        {
            var requestUri = $"{BackPlaneInstanceUrl.TrimEnd('/')}{urlPath}";
            var httpClient = new HttpClient();
            using (var request = new HttpRequestMessage())
            {
                request.Method = new HttpMethod("POST");
                request.RequestUri = new Uri(requestUri, UriKind.Absolute);
                await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            }
        }
    }
}