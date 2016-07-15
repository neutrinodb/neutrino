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

        public async Task<string> Put([FromBody]TimeSerieHeader timeSerieHeader) {
            return await _service.Create(timeSerieHeader);
        }
    }
}