using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neutrino.Data;

namespace Neutrino.Api {

    [Route("api/[controller]")]
    public class OcurrencesController<T>  {
        private ITimeSerieService<T> _service;
        private ITimeSerieHeaderService _timeSerieHeaderService;

        public OcurrencesController(ITimeSerieHeaderService timeSerieHeaderService, ITimeSerieService<T> service) {
            _timeSerieHeaderService = timeSerieHeaderService;
            _service = service;
        }

        [HttpGet]
        public async Task<TimeSerie<T>> Get(string id, DateTime start, DateTime end) {
            return await _service.List(id, start, end);
        }

        [HttpPost]
        [Route("{*id}")]
        public async Task Post(string id, [FromBody] string ocurrences) {
            var header = await _timeSerieHeaderService.Get(id);
            switch(header.OcurrenceType) {
                case OcurrenceKind.Decimal:

            }

            await _service.Add(id, ocurrences);
        }

        private ITimeSerieService<T> GetService(OcurrenceKind ocurrenceKind) {
            
        }
    }
}