using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Neutrino.Api.Controllers {

    [Route("api/[controller]")]
    public class OcurrencesController  {
        private ITimeSerieService _service;

        public OcurrencesController(ITimeSerieService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<TimeSerie> Get(string id, DateTime start, DateTime end) {
            return await _service.List(id, start, end);
        }

        [HttpPost]
        [Route("{*id}")]
        public async Task Post(string id, [FromBody] List<Occurrence> ocurrences) {
            await _service.Save(id, ocurrences);
        }
    }
}