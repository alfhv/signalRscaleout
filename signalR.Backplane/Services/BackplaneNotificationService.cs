using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace signalR.Backplane.Services
{
    public interface IBackplaneNotificationService
    {
        Task Publish(string msgType, string msgContent);
        void AddInstance(string urlInstance);
        List<string> GetInstances();
    }

    public class BackplaneNotificationService : IBackplaneNotificationService
    {
        private List<Uri> _Instances;

        public BackplaneNotificationService()
        {
            _Instances = new List<Uri>();
        }

        public void AddInstance(string urlInstance)
        {
            System.Console.WriteLine($"adding urlInstance: {urlInstance}");       

            urlInstance = urlInstance.TrimEnd('/');
            if (!urlInstance.StartsWith("http://"))
                urlInstance = $"http://{urlInstance}/";

            var parsedUri = new System.Uri(urlInstance);

            if (!_Instances.Contains(parsedUri))
                _Instances.Add(parsedUri);

            System.Console.WriteLine($"latest instance: {_Instances.Last()}");            
        }

        public List<string> GetInstances()
        {
            return _Instances.Select(u => u.ToString()).ToList();
        }

        /// <summary>
        /// Notify all registered instances
        /// </summary>
        /// <param name="message"></param>
        public async Task Publish(string msgType, string msgContent)
        {
            // TODO: check instance is alive calling /ping
            foreach (var instance in _Instances)
            {
                var httpClient = new HttpClient();
                using (var request = new HttpRequestMessage())
                {
                    var requestUri = new Uri(instance, $"notification/notify?msgType={msgType}&msgContent={msgContent}");
                    request.Method = new HttpMethod("POST");
                    request.RequestUri = requestUri;
                    await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                }
            }
        }
    }
}
