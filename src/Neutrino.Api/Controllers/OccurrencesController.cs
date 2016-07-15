using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neutrino;
using Microsoft.AspNetCore.Mvc;

namespace Neutrino.Api.Controllers {
    [Route("api/[controller]")]

    public class OccurrencesController {
        private ITimeSerieService _service;

        public OccurrencesController(ITimeSerieService service) {
            _service = service;
        }

        [HttpGet]
        public string Get() {
            return "Hello, fella. I'm the Occurrences endpoint.";
        }

        [Route("{*id}")]
        public async Task Post(string id, [FromBody] List<Occurrence> occurrences) {
            await _service.Save(id, occurrences);
        }

        [HttpGet]
        [Route("{*id}")]
        public async Task<TimeSerie> Get(string id, DateTime start, DateTime end) {
            return await _service.List(id, start, end);
        }
    }
}