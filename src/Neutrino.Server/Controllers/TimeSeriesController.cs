using System.Threading.Tasks;
using Neutrino.Data;
using Microsoft.AspNet.Mvc;

namespace Neutrino.Api {
    [Route("api/[controller]")]

    public class TimeSeriesController {
        private ITimeSerieService _service;

        public TimeSeriesController(ITimeSerieService service) {
            _service = service;
        }

        public async Task<string> Put([FromBody]TimeSerieHeader timeSerieHeader) {
            return await _service.Create(timeSerieHeader);
        }
    }

}