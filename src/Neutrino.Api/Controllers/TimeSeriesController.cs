using System.Threading.Tasks;
using Neutrino;
using Microsoft.AspNetCore.Mvc;

namespace Neutrino.Api.Controllers {
    [Route("api/[controller]")]

    public class TimeSeriesController {
        private ITimeSerieService _service;

        public TimeSeriesController(ITimeSerieService service) {
            _service = service;
        }

        [HttpGet]
        public string Get() {
            return "Hello, fella. I'm the TimeSeries endpoint.";
        }

        [Route("{*id}")]
        public async Task<string> Put(string id, [FromBody]TimeSerieHeader timeSerieHeader) {
            timeSerieHeader.Id = id;
            return await _service.Create(timeSerieHeader);
        }
    }
}