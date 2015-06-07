using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Neutrino.Core;
using Neutrino.Data;

namespace Neutrino.Api {
    public class TimeSerieController : ApiController {
        private ITimeSerieService _service;

        public TimeSerieController(ITimeSerieService service) {
            _service = service;
        }

        public TimeSerieController() {
            _service = new TimeSerieService(new FileFinder("DataSets"));
        }

        public async Task<TimeSerie> Get(string id, DateTime start, DateTime end) {
            return await _service.List(id, start, end);
        }

        public async Task<string> Put(TimeSerieHeader timeSerieHeader) {
            return await _service.Create(timeSerieHeader);
        }
    }
}