using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Neutrino.Client {
    public class NeutrinoClient {
        private Uri _serverAddress;
        private static HttpClient _client = new HttpClient();

        public NeutrinoClient(Uri serverAddress) {
            _serverAddress = serverAddress;
        }

        public async Task CreateTimeSerieAsync(TimeSerieHeader header) {
            var url = UrlBuilderEx.StartWithBaseUri(_serverAddress)
                                  .AddPath("api/TimeSeries")
                                  .AddPath(header.Id)
                                  .Build();
            Console.WriteLine(url);
            var json = JsonConvert.SerializeObject(header);
            await _client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public async Task SaveAsync(string id, IEnumerable<OccurrenceDecimal> occurrences) {
            var url = UrlBuilderEx.StartWithBaseUri(_serverAddress)
                                  .AddPath("api/Occurrences")
                                  .AddPath(id)
                                  .Build();
            Console.WriteLine(url);
            var json = JsonConvert.SerializeObject(occurrences);
            await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public async Task<TimeSerieDecimal> ListDecimalAsync(string id, DateTime startInclusive, DateTime endInclusive) {
            var url = UrlBuilderEx.StartWithBaseUri(_serverAddress)
                                  .AddPath("api/Occurrences")
                                  .AddPath(id)
                                  .AddQueryParameter("start", startInclusive.ToString("s"))
                                  .AddQueryParameter("end", endInclusive.ToString("s"))
                                  .Build();
            Console.WriteLine(url);
            var json = await _client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<TimeSerieDecimal>(json);
        }
    }
}
